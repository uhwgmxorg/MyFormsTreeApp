using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace MyStoreOnWebService
{
    public class WebServiceManagementStore
    {
        private string? _id;
        public string? Id
        {
            get { return _id; }
            set
            {
                Match match;
                // Parse the Id from the dataSource string
                if (!string.IsNullOrWhiteSpace(value))
                {
                    match = Regex.Match(value, @"Id:\s*(\d+)");
                    if (match.Success)
                        _id = match.Groups[1].Value;
                    else
                        throw new FormatException("The value does not contain a valid Id.");
                }
                else
                    _id = value;
            }
        }
        private string? _name;
        public string? Name
        {
            get { return _name; }
            set
            {
                Match match;
                // Parse the Name from the dataSource string
                if (!string.IsNullOrWhiteSpace(value))
                {
                    match = Regex.Match(value, @"Name:\s*(.*)");
                    if (match.Success)
                        _name = match.Groups[1].Value;
                    else
                        throw new FormatException("The value does not contain a valid Name.");
                }
                else
                    _name = value;
            }
        }

        public System.Windows.Forms.Form? mainForm;
        public System.Windows.Forms.TreeView treeView;

        private bool showMessageBoxes;
        private string webServiceURL;
        private string dataSource = "";

        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Constructor
        /// </summary>
        public WebServiceManagementStore(System.Windows.Forms.TreeView treeView, bool showMessageBoxes = true, string webServiceURL = "http://localhost:3000/api", string dataSource = "")
        {
            this.treeView = treeView;
            this.webServiceURL = webServiceURL.TrimEnd('/');
            this.showMessageBoxes = showMessageBoxes;
            this.dataSource = dataSource;
            Id = dataSource;
            Name = dataSource;

            mainForm = this.treeView.FindForm();
        }

        #region Public Interface Methods
        /// <summary>
        /// SaveTree
        /// </summary>
        /// <param dataSource="dataSource"></param>
        public void SaveTree(string? dataSource)
        {
            if (string.IsNullOrEmpty(dataSource))
            {
                MessageBox.Show("Name cannot be empty.", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string xmlData = SaveTreeViewDataToXmlString();

            if (string.IsNullOrEmpty(Id) || !int.TryParse(Id, out int parsedId) || parsedId <= 0)
            {
                MessageBox.Show("Invalid ID. Save failed.", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                _ = UpdateXmlToWebService(Int32.Parse(Id), xmlData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while saving: {ex.Message}", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// SaveAsTree
        /// </summary>
        /// <param dataSource="dataSource"></param>
        public string SaveAsTree(string? dataSource)
        {
            XmlSelectBoxForm loadForm = new XmlSelectBoxForm(this, webServiceURL, showMessageBoxes, true, this.dataSource);
            string? xmlName = "";

            // Set the gap between the main form and the dialog
            int gap = 0;

            // Get the position and size of the main form
            Point mainFormPosition;
            Size mainFormSize;
            if (mainForm != null)
            {
                mainFormPosition = mainForm.Location;
                mainFormSize = mainForm.Size;

                // Calculate the position for the dialog (bottom-right with a gap from the main form)
                int dialogX = mainFormPosition.X + mainFormSize.Width + gap;
                int dialogY = mainFormPosition.Y + mainFormSize.Height - loadForm.Height;

                // Set the position of the dialog
                loadForm.StartPosition = FormStartPosition.Manual;
                loadForm.Location = new Point(dialogX, dialogY);

                // Show the dialog
                if (loadForm.ShowDialog() == DialogResult.OK)
                {
                    int xmlId = loadForm.SelectedXmlId;
                    xmlName = $"Id: {xmlId} Name: {loadForm.SelectedXmlName}";

                    if (string.IsNullOrWhiteSpace(xmlName))
                    {
                        if (showMessageBoxes)
                            MessageBox.Show("Please enter a valid name for the XML file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return xmlName;
                    }
                }
            }
            return xmlName;
        }

        /// <summary>
        /// LoadTree
        /// </summary>
        /// <param dataSource="mainForm"></param>
        /// <returns></returns>
        public string? LoadTree(Form mainForm)
        {
            XmlSelectBoxForm loadForm = new XmlSelectBoxForm(this, webServiceURL, showMessageBoxes, false, dataSource);
            string? xmlName = null;

            // Set the gap between the main form and the dialog
            int gap = 0;

            // Get the position and size of the main form
            var mainFormPosition = mainForm.Location;
            var mainFormSize = mainForm.Size;

            // Calculate the position for the dialog (top-right with a gap from the main form)
            int dialogX = mainFormPosition.X + mainFormSize.Width + gap;
            int dialogY = mainFormPosition.Y;  // Align the top edges

            // Set the position of the dialog
            loadForm.StartPosition = FormStartPosition.Manual;
            loadForm.Location = new Point(dialogX, dialogY);

            // Show the dialog
            if (loadForm.ShowDialog() == DialogResult.OK)
            {
                int xmlId = loadForm.SelectedXmlId;
                GetXmlById(xmlId);
                xmlName = $"Id: {xmlId} Name: {loadForm.SelectedXmlName}";
            }

            return xmlName;
        }
        #endregion

        #region Public Async Methods uesd by the XmlSelectBoxForm
        /// <summary>
        /// GetAllXmlFileInfoAsync
        /// </summary>
        /// <returns></returns>
        public async Task<List<XmlFileInfo>> GetAllXmlFileInfoAsync()
        {
            try
            {
                string url = $"{webServiceURL}/get_all_xml_info";
                var response = await client.GetStringAsync(url);

                // Debug output to verify the retrieved JSON string
                Debug.WriteLine($"Response: {response}");

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var xmlFileInfos = JsonSerializer.Deserialize<List<XmlFileInfo>>(response, options);
                return xmlFileInfos ?? new List<XmlFileInfo>();
            }
            catch (Exception ex)
            {
                if (showMessageBoxes)
                    MessageBox.Show($"An error occurred: {ex.Message}");
                return new List<XmlFileInfo>();
            }
        }

        /// <summary>
        /// GetXmlById
        /// </summary>
        /// <param dataSource="xmlId"></param>
        public async void GetXmlById(int xmlId)
        {
            try
            {
                string url = $"{webServiceURL}/get_xml_by_id/{xmlId}";
                var response = await client.GetStringAsync(url);

                // Loading the XML data into the TreeView
                LoadXmlIntoTreeView(response);
            }
            catch (Exception ex)
            {
                if (showMessageBoxes)
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// CreateNewXmlAsync
        /// </summary>
        /// <param dataSource="dataSource"></param>
        /// <param dataSource="xmlData"></param>
        /// <returns></returns>
        public async Task<bool> CreateNewXmlAsync(string name, string xmlData)
        {
            try
            {
                string url = $"{webServiceURL}/create_new_xml";
                var content = new StringContent(JsonSerializer.Serialize(new { name, xmlData }), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    if (showMessageBoxes)
                        MessageBox.Show("XML saved successfully.", "Save Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    if (showMessageBoxes)
                        MessageBox.Show($"Error saving XML: {response.ReasonPhrase}", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (showMessageBoxes)
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// UpdateXmlToWebService
        /// </summary>
        /// <param dataSource="id"></param>
        /// <param dataSource="xmlData"></param>
        /// <returns></returns>
        public async Task<bool> UpdateXmlToWebService(int id, string xmlData)
        {
            try
            {
                // Set the request URL
                string url = $"{webServiceURL}/update_xml_by_id";

                // Create the JSON content
                var content = new StringContent(JsonSerializer.Serialize(new { id, xmlData }), Encoding.UTF8, "application/json");

                // Send the PUT request
                HttpResponseMessage response = await client.PutAsync(url, content);

                // Check if the response indicates success
                if (response.IsSuccessStatusCode)
                {
                    if (showMessageBoxes)
                        MessageBox.Show("XML updated successfully.", "Update Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    if (showMessageBoxes)
                        MessageBox.Show($"Error updating XML: {response.ReasonPhrase}", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (showMessageBoxes)
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// DeleteXmlById
        /// </summary>
        /// <param dataSource="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteXmlById(int id)
        {
            try
            {
                string url = $"{webServiceURL}/delete_xml_by_id/{id}";
                HttpResponseMessage response = await client.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    if (showMessageBoxes)
                        MessageBox.Show("XML deleted successfully.", "Delete Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    if (showMessageBoxes)
                        MessageBox.Show($"Error deleting XML: {response.ReasonPhrase}", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (showMessageBoxes)
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// GetNextXmlIdAsync
        /// </summary>
        /// <returns></returns>
        public async Task<int?> GetNextXmlIdAsync()
        {
            try
            {
                string url = $"{webServiceURL}/get_next_xml_id";
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseBody);

                    if (jsonResponse.TryGetProperty("nextId", out JsonElement nextIdElement) && nextIdElement.TryGetInt32(out int nextId))
                    {
                        if (showMessageBoxes)
                            MessageBox.Show($"Next XML ID: {nextId}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return nextId;
                    }
                    else
                    {
                        if (showMessageBoxes)
                            MessageBox.Show("Invalid response format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return null;
                    }
                }
                else
                {
                    if (showMessageBoxes)
                        MessageBox.Show($"Error fetching next XML ID: {response.ReasonPhrase}", "Fetch Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            catch (Exception ex)
            {
                if (showMessageBoxes)
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        /// <summary>
        /// UpdateXmlNameByIdAsync
        /// </summary>
        /// <param dataSource="id"></param>
        /// <param dataSource="dataSource"></param>
        /// <returns></returns>
        public async Task<bool> UpdateXmlNameByIdAsync(int id, string name)
        {
            try
            {
                // Set the request URL
                string url = $"{webServiceURL}/update_xml_name_by_id";

                // Create the JSON content
                var content = new StringContent(JsonSerializer.Serialize(new { id, name }), Encoding.UTF8, "application/json");

                // Send the PUT request
                HttpResponseMessage response = await client.PutAsync(url, content);

                // Check if the response indicates success
                if (response.IsSuccessStatusCode)
                {
                    if (showMessageBoxes)
                        MessageBox.Show("The XML name was updated successfully.", "Update successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    if (showMessageBoxes)
                        MessageBox.Show($"Error updating XML name: {response.ReasonPhrase}", "Update error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (showMessageBoxes)
                    MessageBox.Show($"An error has occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// SaveTreeViewDataToXmlString
        /// </summary>
        /// <returns></returns>
        public string SaveTreeViewDataToXmlString()
        {
            // Create the root element
            XElement rootElement = new XElement("TreeView");

            // Populate the XML with the TreeView data
            WriteTreeViewDataToXml(rootElement, treeView.Nodes);

            // Return the XML as a string
            return rootElement.ToString();
        }
        private void WriteTreeViewDataToXml(XElement parentElement, TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                // Create an element for the node
                XElement nodeElement = new XElement("Node", new XAttribute("Text", node.Text));

                // Check if the node has child nodes and write them
                if (node.Nodes.Count > 0)
                    WriteTreeViewDataToXml(nodeElement, node.Nodes);

                // Add the node element to the parent element
                parentElement.Add(nodeElement);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// SaveTree
        /// </summary>
        private async Task<string?> SaveTreeAsync()
        {
            XmlSelectBoxForm loadForm = new XmlSelectBoxForm(this, webServiceURL, showMessageBoxes, true, dataSource);
            string? xmlName = "";

            // Set the gap between the main form and the dialog
            int gap = 0;

            // Get the position and size of the main form
            Point mainFormPosition;
            Size mainFormSize;
            if (mainForm != null)
            {
                mainFormPosition = mainForm.Location;
                mainFormSize = mainForm.Size;

                // Calculate the position for the dialog (bottom-right with a gap from the main form)
                int dialogX = mainFormPosition.X + mainFormSize.Width + gap;
                int dialogY = mainFormPosition.Y + mainFormSize.Height - loadForm.Height;

                // Set the position of the dialog
                loadForm.StartPosition = FormStartPosition.Manual;
                loadForm.Location = new Point(dialogX, dialogY);

                // Show the dialog
                if (loadForm.ShowDialog() == DialogResult.OK)
                {
                    int xmlId = loadForm.SelectedXmlId;
                    xmlName = loadForm.SelectedXmlName;

                    if (string.IsNullOrWhiteSpace(xmlName))
                    {
                        if (showMessageBoxes)
                            MessageBox.Show("Please enter a valid name for the XML file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return xmlName;
                    }

                    string xmlData = SaveTreeViewDataToXmlString();
                    await CreateNewXmlAsync(xmlName, xmlData);
                    xmlName = $"Id: {xmlId} Name: {loadForm.SelectedXmlName}";
                }
            }
            return xmlName;
        }

        /// <summary>
        /// GetXmlById
        /// </summary>
        /// <param dataSource="xmlName"></param>
        private void LoadTreeViewDataFromWebService(string xmlName)
        {
            treeView.Nodes.Clear(); // Clear existing nodes

            try
            {
                // Load the XML document
                XDocument xDocument = XDocument.Load(xmlName);

                // Start with the root element
                foreach (XElement rootNode in xDocument.Elements())
                {
                    foreach (XElement nodeElement in rootNode.Elements())
                    {
                        // Parse the XML element into a TreeNode and add it to the TreeView
                        TreeNode newNode = ParseElementToTreeNode(nodeElement);
                        treeView.Nodes.Add(newNode);
                    }
                }

                if (showMessageBoxes)
                    MessageBox.Show("Tree view data loaded from Web-Service.", "Load Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                if (showMessageBoxes)
                    MessageBox.Show($"Error loading tree view data: {ex.Message}", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    Debug.WriteLine($"Error loading tree view data: {ex.ToString()}");
            }
        }

        /// <summary>
        /// ParseElementToTreeNode
        /// </summary>
        /// <param dataSource="element"></param>
        /// <returns></returns>
        private TreeNode ParseElementToTreeNode(XElement element)
        {
            TreeNode node = new TreeNode(element.Attribute("Text")?.Value ?? string.Empty);

            foreach (XElement childElement in element.Elements())
                node.Nodes.Add(ParseElementToTreeNode(childElement));

            return node;
        }

        private void LoadXmlIntoTreeView(string xmlData)
        {
            var xDocument = XDocument.Parse(xmlData);
            var rootElement = xDocument.Root;
            if (rootElement != null)
            {
                treeView.Nodes.Clear();
                foreach (var nodeElement in rootElement.Elements())
                {
                    var treeNode = new TreeNode(nodeElement.Attribute("Text")?.Value ?? "Node");
                    AddChildNodes(treeNode, nodeElement);
                    treeView.Nodes.Add(treeNode);
                }
            }
        }
        private void AddChildNodes(TreeNode treeNode, XElement element)
        {
            foreach (var childElement in element.Elements())
            {
                var childNode = new TreeNode(childElement.Attribute("Text")?.Value ?? "Node");
                treeNode.Nodes.Add(childNode);
                AddChildNodes(childNode, childElement);
            }
        }
        #endregion
    }

    public class XmlFileInfo
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}

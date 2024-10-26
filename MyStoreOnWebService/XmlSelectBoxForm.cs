using System.Collections;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MyStoreOnWebService
{
    public partial class XmlSelectBoxForm : Form
    {
        public bool ShowMessageBoxes { get; set; }
        public bool SaveAsMode { get; set; } = false;

        public int SelectedXmlId { get; private set; }
        public string? SelectedXmlName { get; private set; }
        public string? SaveXmlName { get; private set; }

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
                        _id = value;
                }
                else
                    _id = value;
            }
        }
        private string? _name;
        public new string? Name
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
                        _name = value;
                }
                else
                    _name = value;
            }
        }

        private WebServiceManagementStore webServiceManagementStore;

        /// <summary>
        /// Constructor for the XmlSelectBoxForm
        /// </summary>
        public XmlSelectBoxForm(WebServiceManagementStore webServiceManagementStore, string url, bool showMessageBoxes, bool saveAs, string saveAsName)
        {
            InitializeComponent();

            this.webServiceManagementStore = webServiceManagementStore;
            ShowMessageBoxes = showMessageBoxes;

            button_Cancel.Hide();

            // DataGridView setup
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { Name = "ID", HeaderText = "ID", Width = 50, ReadOnly = true });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Name", HeaderText = "Name", Width = 180, ReadOnly = false });
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridView1.Columns["ID"].SortMode = DataGridViewColumnSortMode.Automatic;
            dataGridView1.Columns["Name"].SortMode = DataGridViewColumnSortMode.Automatic;

            // Adding the CellClick event.
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
            // Adding the KeyDown event.
            dataGridView1.KeyDown += dataGridView1_KeyDown;

            SaveAsMode = saveAs;
            textBox_SaveAs.Enabled = SaveAsMode;
            button_Save.Enabled = SaveAsMode;
            button_Cancel.Enabled = SaveAsMode;
            if (SaveAsMode)
            {
                button_Save.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
                button_Load.Visible = false;
            }

            textBox_SaveAs.Text = webServiceManagementStore.Name;
            Id = saveAsName;
        }

        /******************************/
        /*       Button Events        */
        /******************************/
        #region Button Events

        /// <summary>
        /// button_Save_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param="e"></param>
        private async void button_Save_Click(object sender, EventArgs e)
        {

            string xmlData = webServiceManagementStore.SaveTreeViewDataToXmlString();
            await webServiceManagementStore.CreateNewXmlAsync(textBox_SaveAs.Text, xmlData);
            await LoadXmlFileInfos();

            int? nextId = await webServiceManagementStore.GetNextXmlIdAsync();
            if (nextId.HasValue)
                SelectAndScrollToRow(dataGridView1.Rows.Count);

            int xmlId = SelectedXmlId;
            textBox_SaveAs.Text = $"Id: {xmlId} Name: {SelectedXmlName}";

            return;
        }
        private void SelectAndScrollToRow(int rowIndex)
        {
            rowIndex--;
            if (rowIndex >= 0 && rowIndex <= dataGridView1.Rows.Count)
            {
                dataGridView1.ClearSelection();
                dataGridView1.Rows[rowIndex].Selected = true;

                dataGridView1.FirstDisplayedScrollingRowIndex = rowIndex;

                // Get the selected row
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                // Set SelectedXmlId and SelectedXmlName from the cells
                SelectedXmlId = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                SelectedXmlName = selectedRow.Cells["Name"].Value?.ToString();
                textBox_SaveAs.Text = $"Id: {SelectedXmlId} Name: {SelectedXmlName}";
                Id = SelectedXmlId.ToString();
                Name = SelectedXmlName;
            }
            else
            {
                MessageBox.Show("Row index is out of range.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// button_Cancel_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// button_Reload_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param="e"></param>
        private async void button_Reload_Click(object sender, EventArgs e)
        {
            await LoadXmlFileInfos();
        }

        /// <summary>
        /// button_Delete_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param="e"></param>
        private async void button_Delete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];
                if (selectedRow.Cells["ID"].Value != null)
                {
                    int id = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                    bool deleteConfirmed = true;

                    if (ShowMessageBoxes)
                    {
                        var result = MessageBox.Show($"Are you sure you want to use the XML with the ID {id} löschen möchten?", "Confirm deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        deleteConfirmed = (result == DialogResult.Yes);
                    }

                    if (deleteConfirmed)
                    {
                        bool success = await webServiceManagementStore.DeleteXmlById(id);
                        if (success)
                            await LoadXmlFileInfos();
                    }
                }
                else
                {
                    if (ShowMessageBoxes)
                        MessageBox.Show("The selected item does not have a valid ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (ShowMessageBoxes)
                    MessageBox.Show("Please select an item to delete.", "No item selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// button_Load_Click
        /// </summary>
        /// <param="sender"></param>
        /// <param="e"></param>
        private void button_Load_Click(object sender, EventArgs e)
        {
            LoadXml();
        }

        /// <summary>
        /// button_Close_Click
        /// </summary>
        /// <param="sender"></param>
        /// <param="e"></param>
        private void button_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion
        /******************************/
        /*      Other Events          */
        /******************************/
        #region Other Events

        /// <summary>
        /// XmlSelectBoxForm_Load
        /// </summary>
        /// <param="sender"></param>
        /// <param="e"></param>
        private async void XmlSelectBoxForm_Load(object sender, EventArgs e)
        {
            // Load data and fill ListView
            await LoadXmlFileInfos();
            ScrollToCurrentID(Id);
        }
        private void ScrollToCurrentID(string? id)
        {
            int selectedId = -1;

            if (!string.IsNullOrWhiteSpace(id))
                selectedId = Int32.Parse(id);

            // Loop through all rows in the DataGridView
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Check if the cell in the "Id" column contains the desired value
                if (row.Cells["Id"].Value != null && (int)row.Cells["Id"].Value == selectedId)
                {
                    // Select the line
                    row.Selected = true;
                    // Optional: Scroll to the selected row
                    dataGridView1.FirstDisplayedScrollingRowIndex = row.Index;
                    break;
                }
            }
        }

        /// <summary>
        /// dataGridView1_ColumnHeaderMouseClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param="e"></param>
        private void dataGridView1_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            // Ensure that the clicked cell is valid
            if (e.RowIndex >= 0)
            {
                // Get the clicked row
                var selectedRow = dataGridView1.Rows[e.RowIndex];

                // Set SelectedXmlId and SelectedXmlName from the cells
                SelectedXmlId = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                SelectedXmlName = selectedRow.Cells["Name"].Value?.ToString();

                // Update the Save As textbox with the selected name
                textBox_SaveAs.Text = SelectedXmlName;

                Debug.WriteLine($"Selected ID: {SelectedXmlId}, Name: {SelectedXmlName}");
            }
        }

        /// <summary>
        /// dataGridView1_ColumnHeaderMouseClick
        /// </summary>
        /// <param="sender"></param>
        /// <param="e"></param>
        private void dataGridView1_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            // If SaveAsMode is active no loading is allowed
            if (SaveAsMode)
                return;

            if (e.RowIndex >= 0)
            {
                var row = dataGridView1.Rows[e.RowIndex];
                SelectedXmlId = Convert.ToInt32(row.Cells["ID"].Value);
                SelectedXmlName = row.Cells["Name"].Value?.ToString();

                // Loading the XML and updating the TreeView
                LoadXml();
            }
        }

        /// <summary>
        /// dataGridView1_ColumnHeaderMouseClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param="e"></param>
        private async void dataGridView1_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Name"].Index)
            {
                var row = dataGridView1.Rows[e.RowIndex];
                string? newName = row.Cells["Name"].Value?.ToString();
                if (string.IsNullOrWhiteSpace(newName))
                {
                    if (ShowMessageBoxes)
                        MessageBox.Show("Please enter a valid name for the XML file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Retrieve the ID from the current row
                int id = Convert.ToInt32(row.Cells["ID"].Value);

                var success = await webServiceManagementStore.UpdateXmlNameByIdAsync(id, newName);
                if (!success)
                {
                    if (ShowMessageBoxes)
                        MessageBox.Show("Failed to update the name on the server.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // Reload the data to update the UI
                    await LoadXmlFileInfos();
                }
            }
        }

        /// <summary>
        /// dataGridView1_KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void dataGridView1_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                await DeleteSelectedRowAsync();
            else if (e.KeyCode == Keys.F2)
                EditSelectedRowName();
        }
        private async Task DeleteSelectedRowAsync()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];
                if (selectedRow.Cells["ID"].Value != null)
                {
                    int id = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                    bool deleteConfirmed = true;

                    if (ShowMessageBoxes)
                    {
                        var result = MessageBox.Show($"Are you sure you want to delete the XML with ID {id}?", "Confirm deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        deleteConfirmed = (result == DialogResult.Yes);
                    }

                    if (deleteConfirmed)
                    {
                        bool success = await webServiceManagementStore.DeleteXmlById(id);
                        if (success)
                        {
                            await LoadXmlFileInfos();
                        }
                    }
                }
                else
                {
                    if (ShowMessageBoxes)
                    {
                        MessageBox.Show("The selected item does not have a valid ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                if (ShowMessageBoxes)
                {
                    MessageBox.Show("Please select an item to delete.", "No item selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private void EditSelectedRowName()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];

                // Check if the Name column exists
                if (dataGridView1.Columns["Name"] != null && selectedRow.Cells["Name"] != null)
                {
                    // Set focus to the cell and start edit mode
                    dataGridView1.CurrentCell = selectedRow.Cells["Name"];
                    dataGridView1.BeginEdit(true);
                }
            }
        }

        #endregion
        /******************************/
        /*      Other Functions       */
        /******************************/
        #region Other Functions

        /// <summary>
        /// LoadXmlFileInfos
        /// This method fills the DataGridView in the XmlSelectBoxForm dialog
        /// </summary>
        /// <returns></returns>
        private async Task LoadXmlFileInfos()
        {
            var xmlFileInfos = await webServiceManagementStore.GetAllXmlFileInfoAsync();
            PopulateDataGridView(xmlFileInfos);
        }

        /// <summary>
        /// LoadXml
        /// </summary>
        private void LoadXml()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];
                SelectedXmlId = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                SelectedXmlName = selectedRow.Cells["Name"].Value.ToString();
                textBox_SaveAs.Text = SelectedXmlName;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                if (ShowMessageBoxes)
                    MessageBox.Show("Please select an item to load.", "No item selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// DeleteXmlById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task DeleteXmlByIdAsync(int id)
        {
            var result = await webServiceManagementStore.DeleteXmlById(id);
            if (result)
            {
                await LoadXmlFileInfos();
            }
        }

        /// <summary>
        /// PopulateDataGridView
        /// </summary>
        /// <param name="xmlFileInfos"></param>
        private void PopulateDataGridView(List<XmlFileInfo> xmlFileInfos)
        {
            dataGridView1.Rows.Clear();

            foreach (var xmlFileInfo in xmlFileInfos)
            {
                // Debug output to verify the data
                Debug.WriteLine($"Id: {xmlFileInfo.Id}, Name: {xmlFileInfo.Name}");

                int rowIndex = dataGridView1.Rows.Add(xmlFileInfo.Id, xmlFileInfo.Name);
                DataGridViewRow row = dataGridView1.Rows[rowIndex];
                row.Tag = xmlFileInfo.Id; // Save the ID for future use
            }
        }

        /// <summary>
        /// GetXmlFromTreeView
        /// </summary>
        /// <param name="treeView"></param>
        /// <returns></returns>
        private string GetXmlFromTreeView(System.Windows.Forms.TreeView treeView)
        {
            // Convert the TreeView nodes to XML string
            XElement rootElement = new XElement("TreeView");
            foreach (TreeNode node in treeView.Nodes)
            {
                XElement nodeElement = ConvertTreeNodeToXElement(node);
                rootElement.Add(nodeElement);
            }
            return rootElement.ToString();
        }

        /// <summary>
        /// ConvertTreeNodeToXElement
        /// </summary>
        /// <param name="treeNode"></param>
        /// <returns></returns>
        private XElement ConvertTreeNodeToXElement(TreeNode treeNode)
        {
            XElement element = new XElement("Node", new XAttribute("Text", treeNode.Text));
            foreach (TreeNode childNode in treeNode.Nodes)
            {
                element.Add(ConvertTreeNodeToXElement(childNode));
            }
            return element;
        }
    }
    #endregion
}
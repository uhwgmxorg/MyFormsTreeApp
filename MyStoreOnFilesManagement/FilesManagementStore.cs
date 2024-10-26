using System.Diagnostics;
using System.Windows.Forms;
using System.Xml.Linq;

namespace MyStoreOnFilesManagement
{
    public class FilesManagementStore
    {
        private System.Windows.Forms.TreeView myTreeView;
        private bool showMessageBoxes = true;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="treeView"></param>
        public FilesManagementStore(System.Windows.Forms.TreeView treeView, bool showMessageBoxes = true)
        {
            myTreeView = treeView;
            this.showMessageBoxes = showMessageBoxes;
        }

        #region Public Interface Methods
        /// <summary>
        /// SaveTree
        /// </summary>
        /// <param name="lastUsedPath"></param>
        public void SaveTree(string? fileName)
        {
            if (!String.IsNullOrEmpty(fileName))
                SaveTreeViewDataToFile(fileName);
            else
                MessageBox.Show("Please provide a file name to save the tree view data.", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// SaveAsTree
        /// </summary>
        /// <param name="fileName"></param>
        public void SaveAsTree(ref string? lastUsedPath)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            saveFileDialog.DefaultExt = "xml";
            // Use the last saved path
            saveFileDialog.InitialDirectory = lastUsedPath;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog.FileName;
                // Save the path for next use
                lastUsedPath = fileName;
                SaveTreeViewDataToFile(fileName);
            }
        }

        /// <summary>
        /// LoadTree
        /// </summary>
        /// <param name="lastUsedPath"></param>
        /// <returns></returns>
        public string LoadTree(ref string? lastUsedPath)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            // Use the last saved path
            openFileDialog.InitialDirectory = lastUsedPath;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog.FileName;
                // Save the path for next use
                lastUsedPath = Path.GetDirectoryName(fileName);
                LoadTreeViewDataFromFile(fileName);
                return fileName;
            }

            return string.Empty;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// SaveTreeViewDataToFile
        /// </summary>
        /// <param name="fileName"></param>
        private void SaveTreeViewDataToFile(string fileName)
        {
            // Create the root element
            XElement rootElement = new XElement("TreeView");

            // Populate the XML with the TreeView data
            WriteTreeViewDataToXml(rootElement, myTreeView.Nodes);

            // Save the XML document to the file
            rootElement.Save(fileName);

            if (showMessageBoxes)
                MessageBox.Show("Tree view data saved to file.", "Save Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// WriteTreeViewDataToXml
        /// </summary>
        /// <param name="parentElement"></param>
        /// <param name="nodes"></param>
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

        /// <summary>
        /// LoadTreeViewDataFromFile
        /// </summary>
        /// <param name="fileName"></param>
        private void LoadTreeViewDataFromFile(string fileName)
        {
            myTreeView.Nodes.Clear(); // Clear existing nodes

            try
            {
                // Load the XML document
                XDocument xDocument = XDocument.Load(fileName);

                // Start with the root element
                foreach (XElement rootNode in xDocument.Elements())
                {
                    foreach (XElement nodeElement in rootNode.Elements())
                    {
                        // Parse the XML element into a TreeNode and add it to the TreeView
                        TreeNode newNode = ParseElementToTreeNode(nodeElement);
                        myTreeView.Nodes.Add(newNode);
                    }
                }

                if (showMessageBoxes)
                    MessageBox.Show("Tree view data loaded from file.", "Load Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// <param name="element"></param>
        /// <returns></returns>
        private TreeNode ParseElementToTreeNode(XElement element)
        {
            TreeNode node = new TreeNode(element.Attribute("Text")?.Value ?? string.Empty);

            foreach (XElement childElement in element.Elements())
                node.Nodes.Add(ParseElementToTreeNode(childElement));

            return node;
        }
        #endregion
    }
}

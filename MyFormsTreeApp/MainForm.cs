using MyStoreOnFilesManagement;
using MyStoreOnWebService;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace MyFormsTreeApp
{
    public partial class MainForm : Form
    {

        public enum DataSource
        {
            Files,
            WebService
        }
        private DataSource currentDataSource;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            // Create context menu strip
            treeContextMenu = new ContextMenuStrip();

            #region TreeView Drag and Drop
            // Enable label editing
            myTreeView.LabelEdit = true;
            // Enable drag and drop
            myTreeView.AllowDrop = true;
            // Subscribe to drag and drop events
            myTreeView.ItemDrag += myTreeView_ItemDrag;
            myTreeView.DragEnter += myTreeView_DragEnter;
            myTreeView.DragOver += myTreeView_DragOver;
            myTreeView.DragDrop += myTreeView_DragDrop;
            #endregion

            #region TreeView Contex-Menu Items
            // Create menu item for adding nodes
            ToolStripMenuItem addNodeItem = new ToolStripMenuItem("Add Node");
            addNodeItem.Click += AddNodeItem_Click; // Subscribe to click event
            treeContextMenu.Items.Add(addNodeItem); // Add menu item to context menu strip

            // Create menu item for deleting nodes
            ToolStripMenuItem deleteNodeItem = new ToolStripMenuItem("Delete Node");
            deleteNodeItem.Click += DeleteNodeItem_Click; // Subscribe to click event
            treeContextMenu.Items.Add(deleteNodeItem); // Add menu item to context menu strip

            // Create a menu separator
            treeContextMenu.Items.Add(new ToolStripSeparator());

            // Create menu item for delete all nodes
            ToolStripMenuItem deleteAllNodesItem = new ToolStripMenuItem("Delete All Nodes");
            deleteAllNodesItem.Click += DeleteAllNodesItem_Click; // Subscribe to click event
            treeContextMenu.Items.Add(deleteAllNodesItem); // Add menu item to context menu strip

            // Create a menu separator
            treeContextMenu.Items.Add(new ToolStripSeparator());

            // Create menu item for loading tree view data
            ToolStripMenuItem loadDataItem = new ToolStripMenuItem("Load Data");
            loadDataItem.Click += (sender, e) => LoadTreeViewFromDataSourse(); // Subscribe to click event
            treeContextMenu.Items.Add(loadDataItem); // Add menu item to context menu strip

            // Create menu item for saving tree view data
            ToolStripMenuItem saveDataItem = new ToolStripMenuItem("Save Data");
            saveDataItem.Click += (sender, e) => SaveTreeViewToDataSource(); // Subscribe to click event
            treeContextMenu.Items.Add(saveDataItem); // Add menu item to context menu strip

            // Create menu item for saving as tree view data
            ToolStripMenuItem saveAsDataItem = new ToolStripMenuItem("Save As Data");
            saveAsDataItem.Click += (sender, e) => SaveAsTreeViewToDataSource(); // Subscribe to click event
            treeContextMenu.Items.Add(saveAsDataItem); // Add menu item to context menu strip
            #endregion

            // Assign context menu strip to the tree view
            myTreeView.ContextMenuStrip = treeContextMenu;

            // MouseDown Event-Handler hinzufügen
            myTreeView.MouseDown += MyTreeView_MouseDown;

            // Enable label editing
            myTreeView.LabelEdit = true;

            // Add event handler for AfterLabelEdit
            myTreeView.AfterLabelEdit += MyTreeView_AfterLabelEdit;

            // Subscribe to KeyDown event
            myTreeView.KeyDown += MyTreeView_KeyDown;

            // Set the initial notice text
            textBoxNotice.Text = "With this small Windows Forms application you can " +
                "create nodes and sub-nodes in the left TreeView, change the text, " +
                "delete them, save them in an XML file and load them from there.\r\n\r\n" +
                "Use the Context-Menu for action";

            // Initialize the radio buttons
            radioButtonFiles.CheckedChanged += RadioButtonFiles_CheckedChanged;
            radioButtonWebService.CheckedChanged += RadioButtonWebService_CheckedChanged;
            radioButtonFiles.Checked = true;

            // Set the initial value if necessary
            if (radioButtonFiles.Checked)
                currentDataSource = DataSource.Files;
            else if (radioButtonWebService.Checked)
                currentDataSource = DataSource.WebService;

            // Hide/Show Debug Buttons
            //button1.Hide();
            //button2.Hide();
            //button3.Hide();

            // Add tooltips to the buttons
            toolTip1.SetToolTip(button1, "Check the DataSource");
            toolTip1.SetToolTip(button2, "...");
            toolTip1.SetToolTip(button3, "Example how to use GetNextXmlIdFromWebService");
            toolTip1.SetToolTip(button_Close, "Close the application");

            // Initialize the CheckBox based on the saved setting
            checkBoxShowMessageBoxes.Checked = Properties.Settings.Default.ShowMessageBoxes;

            // Add the event handler for the CheckBox
            checkBoxShowMessageBoxes.CheckedChanged += CheckBoxShowMessageBoxes_CheckedChanged;

            // Bind the TextBox textBoxWebServiceURL to the application setting Properties.Settings.Default.WebServiceURL
            textBoxWebServiceURL.DataBindings.Add("Text", Properties.Settings.Default, "WebServiceURL");

            // Set the initial value of the RadioButton based on the saved setting
            currentDataSource = (DataSource)Properties.Settings.Default.CurrentDataSource;
            switch (currentDataSource)
            {
                case DataSource.Files:
                    radioButtonFiles.Checked = true;
                    break;
                case DataSource.WebService:
                    radioButtonWebService.Checked = true;
                    break;
                default:
                    break;
            }
        }

        /******************************/
        /*       Button Events        */
        /******************************/
        #region Button Events

        /// <summary>
        /// button1_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Button 1 clicked");

            Debug.WriteLine("Current DataSource: " + currentDataSource);

            Properties.Settings.Default.ShowMessageBoxes = !Properties.Settings.Default.ShowMessageBoxes;
            checkBoxShowMessageBoxes.Checked = Properties.Settings.Default.ShowMessageBoxes;
            Debug.WriteLine("ShowMessageBoxes: " + Properties.Settings.Default.ShowMessageBoxes);
        }

        /// <summary>
        /// button2_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Button 2 clicked");
        }

        /// <summary>
        /// button3_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button3_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Button 3 clicked");

            // Example how to use GetNextXmlIdAsync
            WebServiceManagementStore webServiceManagementStore = new WebServiceManagementStore(myTreeView, Properties.Settings.Default.ShowMessageBoxes);
            int? nextId = await webServiceManagementStore.GetNextXmlIdAsync(); // Use await since it's an async method
            if (nextId != null)
                Debug.WriteLine($"Next XML ID: {nextId.Value}");
            else
                Debug.WriteLine("Failed to fetch the next XML ID.");
        }

        /// <summary>
        /// button_Close_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion
        /******************************/
        /*      Menu Events          */
        /******************************/
        #region Menu Events

        /// <summary>
        /// AddNodeItem_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNodeItem_Click(object? sender, EventArgs e)
        {
            AddNodeToTreeView();
        }

        /// <summary>
        /// DeleteNodeItem_Click
        /// Event handler for deleting the selected node when the context menu item is clicked
        /// </summary>
        private void DeleteNodeItem_Click(object? sender, EventArgs e)
        {
            DeleteSelectedNode();
        }

        /// <summary>
        /// DeleteAllNodesItem_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteAllNodesItem_Click(object? sender, EventArgs e)
        {
            if (Properties.Settings.Default.ShowMessageBoxes)
            {
                // Confirm before deleting
                if (MessageBox.Show("Are you sure you want to delete all nodes?", "Delete All Nodes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Remove all nodes
                    myTreeView.Nodes.Clear();
                    textBox_DataSource.Text = "";
                }
            }
            else
            {
                // Remove all nodes
                myTreeView.Nodes.Clear();
                textBox_DataSource.Text = "";
            }
        }

        /// <summary>
        /// LoadTreeViewFromDataSourse
        /// </summary>
        private void LoadTreeViewFromDataSourse()
        {
            switch (currentDataSource)
            {
                case DataSource.Files:
                    LoadTreeFromFile();
                    break;
                case DataSource.WebService:
                    LoadTreeFromWebService();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// SaveTreeViewToDataSource
        /// </summary>
        private void SaveTreeViewToDataSource()
        {
            switch (currentDataSource)
            {
                case DataSource.Files:
                    SaveTreeToFile();
                    break;
                case DataSource.WebService:
                    SaveTreeToWebService();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// SaveAsTreeViewToDataSource
        /// </summary>
        private void SaveAsTreeViewToDataSource()
        {
            switch (currentDataSource)
            {
                case DataSource.Files:
                    SaveAsTreeToFile();
                    break;
                case DataSource.WebService:
                    SaveAsTreeToWebService();
                    break;
                default:
                    break;
            }
        }

        #endregion
        /******************************/
        /*      Other Events          */
        /******************************/
        #region Other Events

        /// <summary>
        /// MainForm_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            Location = Properties.Settings.Default.LastWindowLocation;
            Size = Properties.Settings.Default.LastWindowSize;

            // Get the version string safely
            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Version not set";

            string mode = "";
#if DEBUG
            mode = "Debug";
#else
    mode = "Release";
#endif

            Text += $" - {mode} Version {version}";
        }

        /// <summary>
        /// myTreeView_ItemDrag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myTreeView_ItemDrag(object? sender, ItemDragEventArgs e)
        {
            // Check if e.Item is not null
            if (e.Item != null)
            {
                // Begin dragging the selected node
                DoDragDrop(e.Item, DragDropEffects.Move);
            }
        }

        /// <summary>
        /// myTreeView_DragEnter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myTreeView_DragEnter(object? sender, DragEventArgs e)
        {
            // Check if e.Data is not null
            if (e.Data != null && e.Data.GetDataPresent(typeof(TreeNode)))
            {
                // Allow move effect
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        /// <summary>
        /// myTreeView_DragOver
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myTreeView_DragOver(object? sender, DragEventArgs e)
        {
            // Ensure the sender is not null and is of type TreeView
            if (sender is TreeView treeView)
            {
                // Get the node at the current mouse position
                Point targetPoint = treeView.PointToClient(new Point(e.X, e.Y));
                TreeNode? targetNode = treeView.GetNodeAt(targetPoint);

                // Check if targetNode is not null
                if (targetNode != null)
                {
                    // Select the node over which the cursor is hovering
                    treeView.SelectedNode = targetNode;
                }
            }
        }

        /// <summary>
        /// myTreeView_DragDrop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myTreeView_DragDrop(object? sender, DragEventArgs e)
        {
            // Ensure the sender is not null and is of type TreeView
            if (sender is TreeView treeView)
            {
                // Check if e.Data is not null
                if (e.Data != null)
                {
                    // Get the node that was dragged
                    object? data = e.Data.GetData(typeof(TreeNode));
                    TreeNode? draggedNode = data as TreeNode;

                    // Handle the case where draggedNode is null
                    if (draggedNode == null)
                        return;

                    // Get the drop location
                    Point targetPoint = treeView.PointToClient(new Point(e.X, e.Y));
                    TreeNode? targetNode = treeView.GetNodeAt(targetPoint);

                    // Confirm that the dragged node is not being dropped on itself or its descendants
                    if (targetNode == null || (!draggedNode.Equals(targetNode) && !ContainsNode(draggedNode, targetNode)))
                    {
                        // Remove the node from its current location
                        draggedNode.Remove();

                        if (targetNode == null)
                        {
                            // If the target node is null, add the dragged node to the root level
                            treeView.Nodes.Add(draggedNode);
                        }
                        else
                        {
                            // Add the node to the target node
                            targetNode.Nodes.Add(draggedNode);
                            // Expand the target node
                            targetNode.Expand();
                        }
                    }
                }
            }
        }
        private bool ContainsNode(TreeNode? node1, TreeNode node2)
        {
            // Check if node2 is a descendant of node1
            while (node2 != null)
            {
                if (node2.Equals(node1))
                    return true;
                node2 = node2.Parent;
            }
            return false;
        }

        /// <summary>
        /// MyTreeView_MouseDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyTreeView_MouseDown(object? sender, MouseEventArgs e)
        {
            // Check if the click was on a node
            if (myTreeView.GetNodeAt(e.X, e.Y) == null)
            {
                // No node at the click position, clear selection
                myTreeView.SelectedNode = null;
            }
        }

        /// <summary>
        /// Handles the KeyDown event for the TreeView to enable editing with F2 key.
        /// </summary>
        private void MyTreeView_KeyDown(object? sender, KeyEventArgs e)
        {
            // Check if the F2 key was pressed and a node is selected
            if (e.KeyCode == Keys.F2 && myTreeView.SelectedNode != null)
            {
                // Begin editing the selected node's label
                myTreeView.SelectedNode.BeginEdit();
                e.Handled = true; // Mark the event as handled
            }
        }

        /// MyTreeView_AfterLabelEdit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyTreeView_AfterLabelEdit(object? sender, NodeLabelEditEventArgs e)
        {
            // Check if the new label text is not null or empty
            if (!string.IsNullOrWhiteSpace(e.Label))
            {
                // Check if the node object is not null
                if (e.Node != null)
                {
                    // Update the node text with the new label
                    e.Node.Text = e.Label;
                }
            }
            else
            {
                // Cancel the label edit action, do not update the node text
                e.CancelEdit = true;
            }
        }

        /// <summary>
        /// RadioButtonFiles_CheckedChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButtonFiles_CheckedChanged(object? sender, EventArgs e)
        {
            if (radioButtonFiles.Checked)
                currentDataSource = DataSource.Files;
        }

        /// <summary>
        /// RadioButtonWebService_CheckedChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButtonWebService_CheckedChanged(object? sender, EventArgs e)
        {
            if (radioButtonWebService.Checked)
            {
                if(currentDataSource == DataSource.Files)
                    textBox_DataSource.Text = "";
                currentDataSource = DataSource.WebService;
            }
        }

        /// <summary>
        /// CheckBoxShowMessageBoxes_CheckedChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBoxShowMessageBoxes_CheckedChanged(object? sender, EventArgs e)
        {
            // Update the setting based on the state of the CheckBox
            Properties.Settings.Default.ShowMessageBoxes = checkBoxShowMessageBoxes.Checked;
            Properties.Settings.Default.Save(); // Saves the changes in the settings
        }

        /// <summary>
        /// MainForm_FormClosing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.CurrentDataSource = (int)currentDataSource;
            Properties.Settings.Default.Save();

            // Save the current window location and size
            Properties.Settings.Default.LastWindowLocation = this.Location;
            Properties.Settings.Default.LastWindowSize = this.Size;
            Properties.Settings.Default.Save();
        }

        #endregion
        /******************************/
        /*      Other Functions       */
        /******************************/
        #region Other Functions

        /// <summary>
        /// AddNodeToTreeView
        /// Adds a new node to the tree view. If no node is selected, the new node is added at the top level.
        /// If a node is selected, the new node is added as a child of the selected node.
        /// </summary>
        private void AddNodeToTreeView()
        {
            // Create a new node
            TreeNode newNode = new TreeNode("New Node");

            // Check if a node is selected
            if (myTreeView.SelectedNode != null)
            {
                // Add the new node as a child of the selected node
                myTreeView.SelectedNode.Nodes.Add(newNode);
                myTreeView.SelectedNode.Expand(); // Optionally expand the selected node to show the new child
            }
            else
            {
                // Add the new node at the top level
                myTreeView.Nodes.Add(newNode);
                // Clear the selection only when adding a new top-level node
                myTreeView.SelectedNode = null;
            }

            // Optionally select the newly added node
            //treeView.SelectedNode = newNode;
        }

        /// <summary>
        /// DeleteSelectedNode
        /// Deletes the currently selected node from the tree view
        /// </summary>
        private void DeleteSelectedNode()
        {
            if (myTreeView.SelectedNode != null)
            {
                if (Properties.Settings.Default.ShowMessageBoxes)
                {
                    // Confirm before deleting
                    if (MessageBox.Show("Are you sure you want to delete this node?", "Delete Node", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        // Remove the selected node
                        myTreeView.Nodes.Remove(myTreeView.SelectedNode);
                    }
                }
                else
                {
                    // Remove the selected node
                    myTreeView.Nodes.Remove(myTreeView.SelectedNode);
                }
            }
            else
            {
                MessageBox.Show("Please select a node to delete.", "Select Node", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// SaveTreeToFile
        /// </summary>
        private void SaveTreeToFile()
        {
            if (myTreeView.Nodes.Count == 0)
            {
                MessageBox.Show("There is no data to save.", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox_DataSource.Text))
            {
                SaveAsTreeToFile();
                return;
            }
            FilesManagementStore filesManagementStore = new FilesManagementStore(myTreeView, Properties.Settings.Default.ShowMessageBoxes);
            string? lastUsedPath = Properties.Settings.Default.LastUsedPath;
            filesManagementStore.SaveTree(textBox_DataSource.Text);
        }

        /// <summary>
        /// SaveAsTreeToFile
        /// </summary>
        private void SaveAsTreeToFile()
        {
            if (myTreeView.Nodes.Count == 0)
            {
                MessageBox.Show("There is no data to save.", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FilesManagementStore filesManagementStore = new FilesManagementStore(myTreeView, Properties.Settings.Default.ShowMessageBoxes);
            string? lastDataSource = textBox_DataSource.Text;
            filesManagementStore.SaveAsTree(ref lastDataSource);
            textBox_DataSource.Text = lastDataSource;
        }

        /// <summary>
        /// LoadTreeFromFile
        /// </summary>
        private void LoadTreeFromFile()
        {
            FilesManagementStore filesManagementStore = new FilesManagementStore(myTreeView, Properties.Settings.Default.ShowMessageBoxes);
            string? lastUsedPath = Properties.Settings.Default.LastUsedPath;
            textBox_DataSource.Text = filesManagementStore.LoadTree(ref lastUsedPath);
            Properties.Settings.Default.LastUsedPath = lastUsedPath;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// SaveTreeToWebService
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void SaveTreeToWebService()
        {
            if (myTreeView.Nodes.Count == 0)
            {
                MessageBox.Show("There is no data to save.", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox_DataSource.Text))
            {
                SaveAsTreeToWebService();
                return;
            }
            WebServiceManagementStore webServiceManagementStore = new WebServiceManagementStore(myTreeView, Properties.Settings.Default.ShowMessageBoxes, Properties.Settings.Default.WebServiceURL, textBox_DataSource.Text);
            webServiceManagementStore.SaveTree(textBox_DataSource.Text);
        }

        /// <summary>
        /// SaveAsTreeToWebService
        /// </summary>
        private void SaveAsTreeToWebService()
        {
            if (myTreeView.Nodes.Count == 0)
            {
                MessageBox.Show("There is no data to save.", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            WebServiceManagementStore webServiceManagementStore = new WebServiceManagementStore(myTreeView, Properties.Settings.Default.ShowMessageBoxes, Properties.Settings.Default.WebServiceURL, textBox_DataSource.Text);
            textBox_DataSource.Text = webServiceManagementStore.SaveAsTree(textBox_DataSource.Text);
        }

        /// <summary>
        /// LoadTreeFromWebService
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void LoadTreeFromWebService()
        {
            WebServiceManagementStore webServiceManagementStore = new WebServiceManagementStore(myTreeView, Properties.Settings.Default.ShowMessageBoxes, Properties.Settings.Default.WebServiceURL, textBox_DataSource.Text);
            textBox_DataSource.Text = webServiceManagementStore.LoadTree(this);
        }

        #endregion
    }
}
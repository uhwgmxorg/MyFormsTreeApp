namespace MyFormsTreeApp
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            button_Close = new Button();
            myTreeView = new TreeView();
            treeContextMenu = new ContextMenuStrip(components);
            textBoxNotice = new TextBox();
            label1 = new Label();
            radioButtonFiles = new RadioButton();
            radioButtonWebService = new RadioButton();
            dataSource = new GroupBox();
            button1 = new Button();
            toolTip1 = new ToolTip(components);
            checkBoxShowMessageBoxes = new CheckBox();
            label2 = new Label();
            textBoxWebServiceURL = new TextBox();
            button2 = new Button();
            button3 = new Button();
            label3 = new Label();
            textBox_DataSource = new TextBox();
            dataSource.SuspendLayout();
            SuspendLayout();
            // 
            // button_Close
            // 
            button_Close.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button_Close.Location = new Point(713, 415);
            button_Close.Name = "button_Close";
            button_Close.Size = new Size(75, 23);
            button_Close.TabIndex = 8;
            button_Close.Text = "Close";
            button_Close.UseVisualStyleBackColor = true;
            button_Close.Click += button_Close_Click;
            // 
            // treeView
            // 
            myTreeView.AllowDrop = true;
            myTreeView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            myTreeView.HideSelection = false;
            myTreeView.Location = new Point(12, 12);
            myTreeView.Name = "myTreeView";
            myTreeView.Size = new Size(243, 426);
            myTreeView.TabIndex = 1;
            // 
            // treeContextMenu
            // 
            treeContextMenu.ImageScalingSize = new Size(20, 20);
            treeContextMenu.Name = "contextMenuStrip1";
            treeContextMenu.Size = new Size(61, 4);
            // 
            // textBoxNotice
            // 
            textBoxNotice.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxNotice.Font = new Font("Segoe UI", 12F);
            textBoxNotice.Location = new Point(298, 122);
            textBoxNotice.Multiline = true;
            textBoxNotice.Name = "textBoxNotice";
            textBoxNotice.ReadOnly = true;
            textBoxNotice.Size = new Size(456, 178);
            textBoxNotice.TabIndex = 20;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            label1.Location = new Point(298, 79);
            label1.Name = "label1";
            label1.Size = new Size(170, 25);
            label1.TabIndex = 30;
            label1.Text = "MyFormsTreeApp";
            // 
            // radioButtonFiles
            // 
            radioButtonFiles.AutoSize = true;
            radioButtonFiles.Location = new Point(6, 28);
            radioButtonFiles.Name = "radioButtonFiles";
            radioButtonFiles.Size = new Size(48, 19);
            radioButtonFiles.TabIndex = 4;
            radioButtonFiles.TabStop = true;
            radioButtonFiles.Text = "Files";
            radioButtonFiles.UseVisualStyleBackColor = true;
            // 
            // radioButtonWebService
            // 
            radioButtonWebService.AutoSize = true;
            radioButtonWebService.Location = new Point(60, 28);
            radioButtonWebService.Name = "radioButtonWebService";
            radioButtonWebService.Size = new Size(89, 19);
            radioButtonWebService.TabIndex = 5;
            radioButtonWebService.TabStop = true;
            radioButtonWebService.Text = "Web Service";
            radioButtonWebService.UseVisualStyleBackColor = true;
            // 
            // dataSource
            // 
            dataSource.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            dataSource.Controls.Add(radioButtonFiles);
            dataSource.Controls.Add(radioButtonWebService);
            dataSource.Location = new Point(597, 334);
            dataSource.Name = "dataSource";
            dataSource.Size = new Size(157, 63);
            dataSource.TabIndex = 6;
            dataSource.TabStop = false;
            dataSource.Text = "Choose the data source";
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button1.Location = new Point(470, 415);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 5;
            button1.Text = "#1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // checkBoxShowMessageBoxes
            // 
            checkBoxShowMessageBoxes.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            checkBoxShowMessageBoxes.AutoSize = true;
            checkBoxShowMessageBoxes.Location = new Point(298, 333);
            checkBoxShowMessageBoxes.Name = "checkBoxShowMessageBoxes";
            checkBoxShowMessageBoxes.Size = new Size(138, 19);
            checkBoxShowMessageBoxes.TabIndex = 2;
            checkBoxShowMessageBoxes.Text = "Show Message Boxes";
            checkBoxShowMessageBoxes.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(298, 356);
            label2.Name = "label2";
            label2.Size = new Size(95, 15);
            label2.TabIndex = 9;
            label2.Text = "Web Service URL";
            // 
            // textBoxWebServiceURL
            // 
            textBoxWebServiceURL.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            textBoxWebServiceURL.Location = new Point(298, 374);
            textBoxWebServiceURL.Name = "textBoxWebServiceURL";
            textBoxWebServiceURL.Size = new Size(284, 23);
            textBoxWebServiceURL.TabIndex = 3;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button2.Location = new Point(551, 415);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 6;
            button2.Text = "#2";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button3.Location = new Point(632, 415);
            button3.Name = "button3";
            button3.Size = new Size(75, 23);
            button3.TabIndex = 7;
            button3.Text = "#3";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(298, 32);
            label3.Name = "label3";
            label3.Size = new Size(150, 15);
            label3.TabIndex = 31;
            label3.Text = "Data Source File/Database:";
            // 
            // textBox_DataSource
            // 
            textBox_DataSource.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBox_DataSource.Location = new Point(454, 29);
            textBox_DataSource.Name = "textBox_DataSource";
            textBox_DataSource.ReadOnly = true;
            textBox_DataSource.Size = new Size(334, 23);
            textBox_DataSource.TabIndex = 32;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(textBox_DataSource);
            Controls.Add(label3);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(textBoxWebServiceURL);
            Controls.Add(label2);
            Controls.Add(checkBoxShowMessageBoxes);
            Controls.Add(button1);
            Controls.Add(dataSource);
            Controls.Add(label1);
            Controls.Add(textBoxNotice);
            Controls.Add(myTreeView);
            Controls.Add(button_Close);
            Name = "MainForm";
            Text = "MyFormsTreeApp C#";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            dataSource.ResumeLayout(false);
            dataSource.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button_Close;
        private TreeView myTreeView;
        private ContextMenuStrip treeContextMenu;
        private TextBox textBoxNotice;
        private Label label1;
        private RadioButton radioButtonFiles;
        private RadioButton radioButtonWebService;
        private GroupBox dataSource;
        private Button button1;
        private ToolTip toolTip1;
        private CheckBox checkBoxShowMessageBoxes;
        private Label label2;
        private TextBox textBoxWebServiceURL;
        private Button button2;
        private Button button3;
        private Label label3;
        private TextBox textBox_DataSource;
    }
}

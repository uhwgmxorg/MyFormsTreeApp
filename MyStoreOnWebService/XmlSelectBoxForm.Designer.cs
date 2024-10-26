namespace MyStoreOnWebService
{
    partial class XmlSelectBoxForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button_Load = new Button();
            button_Delete = new Button();
            label1 = new Label();
            button_Reload = new Button();
            button_Close = new Button();
            label2 = new Label();
            textBox_SaveAs = new TextBox();
            button_Save = new Button();
            dataGridView1 = new DataGridView();
            button_Cancel = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // button_Load
            // 
            button_Load.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button_Load.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button_Load.Location = new Point(328, 247);
            button_Load.Name = "button_Load";
            button_Load.Size = new Size(75, 23);
            button_Load.TabIndex = 12;
            button_Load.Text = "Load";
            button_Load.UseVisualStyleBackColor = true;
            button_Load.Click += button_Load_Click;
            // 
            // button_Delete
            // 
            button_Delete.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button_Delete.Location = new Point(328, 218);
            button_Delete.Name = "button_Delete";
            button_Delete.Size = new Size(75, 23);
            button_Delete.TabIndex = 11;
            button_Delete.Text = "Delete";
            button_Delete.UseVisualStyleBackColor = true;
            button_Delete.Click += button_Delete_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(10, 51);
            label1.Name = "label1";
            label1.Size = new Size(74, 15);
            label1.TabIndex = 4;
            label1.Text = "Select a Xml:";
            // 
            // button_Reload
            // 
            button_Reload.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button_Reload.Location = new Point(328, 189);
            button_Reload.Name = "button_Reload";
            button_Reload.Size = new Size(75, 23);
            button_Reload.TabIndex = 10;
            button_Reload.Text = "Reload";
            button_Reload.UseVisualStyleBackColor = true;
            button_Reload.Click += button_Reload_Click;
            // 
            // button_Close
            // 
            button_Close.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button_Close.Location = new Point(328, 275);
            button_Close.Margin = new Padding(3, 2, 3, 2);
            button_Close.Name = "button_Close";
            button_Close.Size = new Size(75, 23);
            button_Close.TabIndex = 13;
            button_Close.Text = "Close";
            button_Close.UseVisualStyleBackColor = true;
            button_Close.Click += button_Close_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(10, 9);
            label2.Name = "label2";
            label2.Size = new Size(113, 15);
            label2.TabIndex = 6;
            label2.Text = "Save or Save Xml as:";
            // 
            // textBox_SaveAs
            // 
            textBox_SaveAs.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBox_SaveAs.Location = new Point(10, 26);
            textBox_SaveAs.Margin = new Padding(3, 2, 3, 2);
            textBox_SaveAs.Name = "textBox_SaveAs";
            textBox_SaveAs.Size = new Size(308, 23);
            textBox_SaveAs.TabIndex = 1;
            // 
            // button_Save
            // 
            button_Save.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button_Save.Location = new Point(328, 26);
            button_Save.Margin = new Padding(3, 2, 3, 2);
            button_Save.Name = "button_Save";
            button_Save.Size = new Size(75, 23);
            button_Save.TabIndex = 8;
            button_Save.Text = "Save/As";
            button_Save.UseVisualStyleBackColor = true;
            button_Save.Click += button_Save_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(10, 69);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(308, 227);
            dataGridView1.TabIndex = 2;
            // 
            // button_Cancel
            // 
            button_Cancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button_Cancel.Location = new Point(328, 53);
            button_Cancel.Margin = new Padding(3, 2, 3, 2);
            button_Cancel.Name = "button_Cancel";
            button_Cancel.Size = new Size(75, 23);
            button_Cancel.TabIndex = 9;
            button_Cancel.Text = "Cancel";
            button_Cancel.UseVisualStyleBackColor = true;
            button_Cancel.Click += button_Cancel_Click;
            // 
            // XmlSelectBoxForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(414, 308);
            Controls.Add(button_Cancel);
            Controls.Add(dataGridView1);
            Controls.Add(button_Save);
            Controls.Add(textBox_SaveAs);
            Controls.Add(label2);
            Controls.Add(button_Close);
            Controls.Add(button_Reload);
            Controls.Add(label1);
            Controls.Add(button_Delete);
            Controls.Add(button_Load);
            Name = "XmlSelectBoxForm";
            Text = "XmlSelectBoxForm";
            Load += XmlSelectBoxForm_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button button_Load;
        private Button button_Delete;
        private Label label1;
        private Button button_Reload;
        private Button button_Close;
        private Label label2;
        private TextBox textBox_SaveAs;
        private Button button_Save;
        private DataGridView dataGridView1;
        private Button button_Cancel;
    }
}
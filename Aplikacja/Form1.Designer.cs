namespace Aplikacja
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            LoadButton = new Button();
            TransformButton = new Button();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            SaveToFile = new Button();
            textBox1 = new TextBox();
            label1 = new Label();
            toolStrip1 = new ToolStrip();
            toolStripDropDownButton1 = new ToolStripDropDownButton();
            categorizeGrainsToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // LoadButton
            // 
            LoadButton.Anchor = AnchorStyles.Top;
            LoadButton.Cursor = Cursors.Hand;
            LoadButton.Location = new Point(343, 28);
            LoadButton.Name = "LoadButton";
            LoadButton.Size = new Size(114, 50);
            LoadButton.TabIndex = 0;
            LoadButton.Text = "Load";
            LoadButton.UseVisualStyleBackColor = true;
            LoadButton.Click += LoadButton_Click;
            // 
            // TransformButton
            // 
            TransformButton.Anchor = AnchorStyles.Top;
            TransformButton.Cursor = Cursors.Hand;
            TransformButton.Location = new Point(343, 84);
            TransformButton.Name = "TransformButton";
            TransformButton.Size = new Size(114, 50);
            TransformButton.TabIndex = 1;
            TransformButton.Text = "Transform";
            TransformButton.UseVisualStyleBackColor = true;
            TransformButton.Click += TransformButton_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.Location = new Point(12, 28);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(325, 410);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            pictureBox2.BorderStyle = BorderStyle.FixedSingle;
            pictureBox2.Location = new Point(463, 28);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(325, 410);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 3;
            pictureBox2.TabStop = false;
            // 
            // SaveToFile
            // 
            SaveToFile.Anchor = AnchorStyles.Top;
            SaveToFile.Cursor = Cursors.Hand;
            SaveToFile.Location = new Point(343, 140);
            SaveToFile.Name = "SaveToFile";
            SaveToFile.Size = new Size(114, 50);
            SaveToFile.TabIndex = 6;
            SaveToFile.Text = "Save sample";
            SaveToFile.UseVisualStyleBackColor = true;
            SaveToFile.Click += SaveToFile_Click;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBox1.Location = new Point(343, 243);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(114, 23);
            textBox1.TabIndex = 7;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label1.Location = new Point(343, 193);
            label1.Name = "label1";
            label1.Size = new Size(114, 47);
            label1.TabIndex = 11;
            label1.Text = "Write the rice type (do not leave this label empty!)";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            label1.Click += label1_Click;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripDropDownButton1 });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(800, 25);
            toolStrip1.TabIndex = 12;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            toolStripDropDownButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripDropDownButton1.DropDownItems.AddRange(new ToolStripItem[] { categorizeGrainsToolStripMenuItem });
            toolStripDropDownButton1.Image = (Image)resources.GetObject("toolStripDropDownButton1.Image");
            toolStripDropDownButton1.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            toolStripDropDownButton1.Size = new Size(29, 22);
            toolStripDropDownButton1.Text = "toolStripDropDownButton1";
            // 
            // categorizeGrainsToolStripMenuItem
            // 
            categorizeGrainsToolStripMenuItem.Name = "categorizeGrainsToolStripMenuItem";
            categorizeGrainsToolStripMenuItem.Size = new Size(165, 22);
            categorizeGrainsToolStripMenuItem.Text = "Categorize grains";
            categorizeGrainsToolStripMenuItem.Click += categorizeGrainsToolStripMenuItem_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(toolStrip1);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Controls.Add(SaveToFile);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Controls.Add(TransformButton);
            Controls.Add(LoadButton);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button LoadButton;
        private Button TransformButton;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private Button SaveToFile;
        private TextBox textBox1;
        private Label label1;
        private ToolStrip toolStrip1;
        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStripMenuItem categorizeGrainsToolStripMenuItem;
    }
}
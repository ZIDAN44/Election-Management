namespace ElectionApp.Nominee
{
    partial class Election_List
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lbIMessage1 = new Label();
            button1 = new Button();
            lbIMessage2 = new Label();
            panel1 = new Panel();
            button2 = new Button();
            lbIMessage3 = new Label();
            lbIMessage4 = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // lbIMessage1
            // 
            lbIMessage1.AutoEllipsis = true;
            lbIMessage1.Font = new Font("Times New Roman", 10.8F, FontStyle.Regular, GraphicsUnit.Point);
            lbIMessage1.Location = new Point(172, 10);
            lbIMessage1.Name = "lbIMessage1";
            lbIMessage1.Size = new Size(141, 32);
            lbIMessage1.TabIndex = 12;
            lbIMessage1.Text = "ID";
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(11, 116, 30);
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Times New Roman", 12F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point);
            button1.ForeColor = Color.Cyan;
            button1.Location = new Point(373, 38);
            button1.Name = "button1";
            button1.Size = new Size(90, 90);
            button1.TabIndex = 15;
            button1.Text = "Join";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // lbIMessage2
            // 
            lbIMessage2.AutoEllipsis = true;
            lbIMessage2.Font = new Font("Times New Roman", 10.8F, FontStyle.Regular, GraphicsUnit.Point);
            lbIMessage2.Location = new Point(172, 45);
            lbIMessage2.Name = "lbIMessage2";
            lbIMessage2.Size = new Size(164, 32);
            lbIMessage2.TabIndex = 14;
            lbIMessage2.Text = "Name";
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ActiveCaption;
            panel1.Controls.Add(button2);
            panel1.Location = new Point(-1, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(167, 168);
            panel1.TabIndex = 13;
            // 
            // button2
            // 
            button2.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
            button2.Location = new Point(28, 28);
            button2.Name = "button2";
            button2.Size = new Size(105, 110);
            button2.TabIndex = 0;
            button2.Text = "Rules DOC";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // lbIMessage3
            // 
            lbIMessage3.AutoEllipsis = true;
            lbIMessage3.Font = new Font("Times New Roman", 10.8F, FontStyle.Regular, GraphicsUnit.Point);
            lbIMessage3.Location = new Point(172, 86);
            lbIMessage3.Name = "lbIMessage3";
            lbIMessage3.Size = new Size(164, 32);
            lbIMessage3.TabIndex = 16;
            lbIMessage3.Text = "S_DATE";
            // 
            // lbIMessage4
            // 
            lbIMessage4.AutoEllipsis = true;
            lbIMessage4.Font = new Font("Times New Roman", 10.8F, FontStyle.Regular, GraphicsUnit.Point);
            lbIMessage4.Location = new Point(172, 128);
            lbIMessage4.Name = "lbIMessage4";
            lbIMessage4.Size = new Size(164, 32);
            lbIMessage4.TabIndex = 17;
            lbIMessage4.Text = "E_DATE";
            // 
            // Election_List
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lbIMessage4);
            Controls.Add(lbIMessage3);
            Controls.Add(lbIMessage1);
            Controls.Add(button1);
            Controls.Add(lbIMessage2);
            Controls.Add(panel1);
            Name = "Election_List";
            Size = new Size(513, 168);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Label lbIMessage1;
        private Button button1;
        private Label lbIMessage2;
        private Panel panel1;
        private Label lbIMessage3;
        private Label lbIMessage4;
        private Button button2;
    }
}

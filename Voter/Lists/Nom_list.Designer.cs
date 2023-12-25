namespace ElectionApp.Voter
{
    partial class Nom_list
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
            pictureBox1 = new PictureBox();
            button1 = new Button();
            lbIMessage2 = new Label();
            panel1 = new Panel();
            lbIMessage3 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // lbIMessage1
            // 
            lbIMessage1.AutoEllipsis = true;
            lbIMessage1.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
            lbIMessage1.Location = new Point(173, 29);
            lbIMessage1.Name = "lbIMessage1";
            lbIMessage1.Size = new Size(141, 32);
            lbIMessage1.TabIndex = 12;
            lbIMessage1.Text = "ID";
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(29, 28);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(105, 110);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(11, 116, 30);
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point);
            button1.ForeColor = Color.MintCream;
            button1.Location = new Point(374, 38);
            button1.Name = "button1";
            button1.Size = new Size(90, 90);
            button1.TabIndex = 15;
            button1.Text = "VOTE";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // lbIMessage2
            // 
            lbIMessage2.AutoEllipsis = true;
            lbIMessage2.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
            lbIMessage2.Location = new Point(173, 71);
            lbIMessage2.Name = "lbIMessage2";
            lbIMessage2.Size = new Size(164, 32);
            lbIMessage2.TabIndex = 14;
            lbIMessage2.Text = "Name";
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ActiveCaption;
            panel1.Controls.Add(pictureBox1);
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(167, 168);
            panel1.TabIndex = 13;
            // 
            // lbIMessage3
            // 
            lbIMessage3.AutoEllipsis = true;
            lbIMessage3.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
            lbIMessage3.Location = new Point(173, 115);
            lbIMessage3.Name = "lbIMessage3";
            lbIMessage3.Size = new Size(164, 32);
            lbIMessage3.TabIndex = 16;
            lbIMessage3.Text = "Party Name";
            // 
            // Nom_list
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lbIMessage3);
            Controls.Add(lbIMessage1);
            Controls.Add(button1);
            Controls.Add(lbIMessage2);
            Controls.Add(panel1);
            Name = "Nom_list";
            Size = new Size(513, 168);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Label lbIMessage1;
        private PictureBox pictureBox1;
        private Button button1;
        private Label lbIMessage2;
        private Panel panel1;
        private Label lbIMessage3;
    }
}

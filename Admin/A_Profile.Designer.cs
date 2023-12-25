namespace ElectionApp.Admin
{
    partial class A_Profile
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
            panel1 = new Panel();
            pictureBox3 = new PictureBox();
            label4 = new Label();
            panelAdminProfile = new Panel();
            label3 = new Label();
            label7 = new Label();
            label2 = new Label();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            panelAdminProfile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(pictureBox3);
            panel1.Controls.Add(label4);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1088, 57);
            panel1.TabIndex = 27;
            // 
            // pictureBox3
            // 
            pictureBox3.Image = Properties.Resources.profile_settings;
            pictureBox3.Location = new Point(1004, 6);
            pictureBox3.Margin = new Padding(0);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(41, 45);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 22;
            pictureBox3.TabStop = false;
            pictureBox3.Click += pictureBox3_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Times New Roman", 16.2F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new Point(672, 12);
            label4.Name = "label4";
            label4.Size = new Size(197, 33);
            label4.TabIndex = 21;
            label4.Text = "Welcome Admin";
            // 
            // panelAdminProfile
            // 
            panelAdminProfile.Controls.Add(label3);
            panelAdminProfile.Controls.Add(label7);
            panelAdminProfile.Controls.Add(label2);
            panelAdminProfile.Controls.Add(label1);
            panelAdminProfile.Controls.Add(pictureBox1);
            panelAdminProfile.Dock = DockStyle.Fill;
            panelAdminProfile.Location = new Point(0, 0);
            panelAdminProfile.Name = "panelAdminProfile";
            panelAdminProfile.Size = new Size(1088, 839);
            panelAdminProfile.TabIndex = 28;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(136, 554);
            label3.Name = "label3";
            label3.Size = new Size(80, 25);
            label3.TabIndex = 29;
            label3.Text = "Phone: ";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label7.Location = new Point(136, 503);
            label7.Name = "label7";
            label7.Size = new Size(71, 25);
            label7.TabIndex = 28;
            label7.Text = "Email: ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(136, 447);
            label2.Name = "label2";
            label2.Size = new Size(75, 25);
            label2.TabIndex = 27;
            label2.Text = "Name: ";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(136, 392);
            label1.Name = "label1";
            label1.Size = new Size(88, 25);
            label1.TabIndex = 26;
            label1.Text = "User ID: ";
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(136, 158);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(195, 195);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 25;
            pictureBox1.TabStop = false;
            // 
            // A_Profile
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel1);
            Controls.Add(panelAdminProfile);
            Name = "A_Profile";
            Size = new Size(1088, 839);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            panelAdminProfile.ResumeLayout(false);
            panelAdminProfile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private PictureBox pictureBox3;
        private Label label4;
        private Panel panelAdminProfile;
        private Label label7;
        private Label label2;
        private Label label1;
        private PictureBox pictureBox1;
        private Label label3;
    }
}

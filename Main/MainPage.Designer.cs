namespace ElectionApp.Main
{
    partial class MainPage
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainPage));
            panel1 = new Panel();
            linkLabel1 = new LinkLabel();
            pictureBox3 = new PictureBox();
            label4 = new Label();
            textpassword = new TextBox();
            textUserName = new TextBox();
            button1 = new Button();
            panel3 = new Panel();
            pictureBox2 = new PictureBox();
            panel2 = new Panel();
            pictureBox1 = new PictureBox();
            guna2GradientPanel1 = new Guna.UI2.WinForms.Guna2GradientPanel();
            guna2CircleButton2 = new Guna.UI2.WinForms.Guna2CircleButton();
            guna2CircleButton1 = new Guna.UI2.WinForms.Guna2CircleButton();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            guna2GradientPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackgroundImage = Properties.Resources.Login;
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
            panel1.Controls.Add(linkLabel1);
            panel1.Controls.Add(pictureBox3);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(textpassword);
            panel1.Controls.Add(textUserName);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(pictureBox2);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(pictureBox1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 50);
            panel1.Name = "panel1";
            panel1.Size = new Size(1088, 789);
            panel1.TabIndex = 1;
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.BackColor = Color.Transparent;
            linkLabel1.Font = new Font("Times New Roman", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            linkLabel1.LinkColor = Color.DeepSkyBlue;
            linkLabel1.Location = new Point(565, 613);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(133, 26);
            linkLabel1.TabIndex = 11;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "Register here";
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // pictureBox3
            // 
            pictureBox3.BackColor = Color.Transparent;
            pictureBox3.Image = Properties.Resources.eye_white;
            pictureBox3.Location = new Point(750, 426);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(32, 32);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 7;
            pictureBox3.TabStop = false;
            pictureBox3.Click += pictureBox3_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Bahnschrift SemiCondensed", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            label4.ForeColor = SystemColors.AppWorkspace;
            label4.Location = new Point(449, 610);
            label4.Name = "label4";
            label4.Size = new Size(106, 28);
            label4.TabIndex = 10;
            label4.Text = "New user?";
            // 
            // textpassword
            // 
            textpassword.BackColor = Color.FromArgb(1, 38, 40);
            textpassword.BorderStyle = BorderStyle.None;
            textpassword.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point);
            textpassword.ForeColor = Color.White;
            textpassword.Location = new Point(410, 424);
            textpassword.Multiline = true;
            textpassword.Name = "textpassword";
            textpassword.PasswordChar = '*';
            textpassword.Size = new Size(334, 34);
            textpassword.TabIndex = 6;
            textpassword.KeyDown += textpassword_KeyDown;
            // 
            // textUserName
            // 
            textUserName.BackColor = Color.FromArgb(1, 38, 40);
            textUserName.BorderStyle = BorderStyle.None;
            textUserName.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point);
            textUserName.ForeColor = Color.White;
            textUserName.Location = new Point(410, 329);
            textUserName.Multiline = true;
            textUserName.Name = "textUserName";
            textUserName.Size = new Size(334, 34);
            textUserName.TabIndex = 5;
            textUserName.KeyDown += textUserName_KeyDown;
            // 
            // button1
            // 
            button1.BackColor = Color.Silver;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Bahnschrift", 13.8F, FontStyle.Bold, GraphicsUnit.Point);
            button1.ForeColor = Color.Black;
            button1.Location = new Point(372, 494);
            button1.Name = "button1";
            button1.Size = new Size(372, 45);
            button1.TabIndex = 4;
            button1.Text = "LOGIN";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // panel3
            // 
            panel3.BackColor = Color.ForestGreen;
            panel3.Location = new Point(372, 464);
            panel3.Name = "panel3";
            panel3.Size = new Size(372, 1);
            panel3.TabIndex = 3;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.Transparent;
            pictureBox2.Image = Properties.Resources.lock_white;
            pictureBox2.Location = new Point(372, 426);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(32, 32);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 2;
            pictureBox2.TabStop = false;
            // 
            // panel2
            // 
            panel2.BackColor = Color.ForestGreen;
            panel2.Location = new Point(372, 367);
            panel2.Name = "panel2";
            panel2.Size = new Size(372, 1);
            panel2.TabIndex = 1;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = Properties.Resources.user_white;
            pictureBox1.Location = new Point(372, 329);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(32, 32);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // guna2GradientPanel1
            // 
            guna2GradientPanel1.Controls.Add(guna2CircleButton2);
            guna2GradientPanel1.Controls.Add(guna2CircleButton1);
            guna2GradientPanel1.CustomizableEdges = customizableEdges3;
            guna2GradientPanel1.Dock = DockStyle.Top;
            guna2GradientPanel1.FillColor = Color.Silver;
            guna2GradientPanel1.FillColor2 = Color.DimGray;
            guna2GradientPanel1.Location = new Point(0, 0);
            guna2GradientPanel1.Name = "guna2GradientPanel1";
            guna2GradientPanel1.ShadowDecoration.CustomizableEdges = customizableEdges4;
            guna2GradientPanel1.Size = new Size(1088, 50);
            guna2GradientPanel1.TabIndex = 0;
            guna2GradientPanel1.MouseDown += guna2GradientPanel1_MouseDown;
            // 
            // guna2CircleButton2
            // 
            guna2CircleButton2.BackColor = Color.Transparent;
            guna2CircleButton2.BackgroundImage = Properties.Resources.Minus;
            guna2CircleButton2.BackgroundImageLayout = ImageLayout.Zoom;
            guna2CircleButton2.BorderColor = Color.Transparent;
            guna2CircleButton2.CheckedState.BorderColor = Color.Transparent;
            guna2CircleButton2.CheckedState.CustomBorderColor = Color.Transparent;
            guna2CircleButton2.CheckedState.FillColor = Color.Transparent;
            guna2CircleButton2.CheckedState.ForeColor = Color.Transparent;
            guna2CircleButton2.CustomBorderColor = Color.Transparent;
            guna2CircleButton2.CustomBorderThickness = new Padding(0, 0, 0, 3);
            guna2CircleButton2.DisabledState.BorderColor = Color.DarkGray;
            guna2CircleButton2.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2CircleButton2.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2CircleButton2.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2CircleButton2.FillColor = Color.Transparent;
            guna2CircleButton2.FocusedColor = Color.Transparent;
            guna2CircleButton2.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            guna2CircleButton2.ForeColor = Color.Transparent;
            guna2CircleButton2.HoverState.BorderColor = Color.Transparent;
            guna2CircleButton2.HoverState.CustomBorderColor = Color.Transparent;
            guna2CircleButton2.HoverState.FillColor = Color.Transparent;
            guna2CircleButton2.HoverState.ForeColor = Color.Transparent;
            guna2CircleButton2.Location = new Point(987, 0);
            guna2CircleButton2.Name = "guna2CircleButton2";
            guna2CircleButton2.PressedColor = Color.White;
            guna2CircleButton2.PressedDepth = 5;
            guna2CircleButton2.ShadowDecoration.BorderRadius = 0;
            guna2CircleButton2.ShadowDecoration.Color = Color.Transparent;
            guna2CircleButton2.ShadowDecoration.CustomizableEdges = customizableEdges1;
            guna2CircleButton2.ShadowDecoration.Depth = 0;
            guna2CircleButton2.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            guna2CircleButton2.Size = new Size(50, 50);
            guna2CircleButton2.TabIndex = 3;
            guna2CircleButton2.Click += guna2CircleButton2_Click;
            // 
            // guna2CircleButton1
            // 
            guna2CircleButton1.BackColor = Color.Transparent;
            guna2CircleButton1.BackgroundImage = Properties.Resources.Cancel;
            guna2CircleButton1.BackgroundImageLayout = ImageLayout.Zoom;
            guna2CircleButton1.BorderColor = Color.Transparent;
            guna2CircleButton1.CheckedState.BorderColor = Color.Transparent;
            guna2CircleButton1.CheckedState.CustomBorderColor = Color.Transparent;
            guna2CircleButton1.CheckedState.FillColor = Color.Transparent;
            guna2CircleButton1.CheckedState.ForeColor = Color.Transparent;
            guna2CircleButton1.CustomBorderColor = Color.Transparent;
            guna2CircleButton1.CustomBorderThickness = new Padding(0, 0, 0, 3);
            guna2CircleButton1.DisabledState.BorderColor = Color.DarkGray;
            guna2CircleButton1.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2CircleButton1.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2CircleButton1.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2CircleButton1.FillColor = Color.Transparent;
            guna2CircleButton1.FocusedColor = Color.Transparent;
            guna2CircleButton1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            guna2CircleButton1.ForeColor = Color.Transparent;
            guna2CircleButton1.HoverState.BorderColor = Color.Transparent;
            guna2CircleButton1.HoverState.CustomBorderColor = Color.Transparent;
            guna2CircleButton1.HoverState.FillColor = Color.Transparent;
            guna2CircleButton1.HoverState.ForeColor = Color.Transparent;
            guna2CircleButton1.Location = new Point(1032, 0);
            guna2CircleButton1.Name = "guna2CircleButton1";
            guna2CircleButton1.PressedColor = Color.White;
            guna2CircleButton1.PressedDepth = 5;
            guna2CircleButton1.ShadowDecoration.BorderRadius = 0;
            guna2CircleButton1.ShadowDecoration.Color = Color.Transparent;
            guna2CircleButton1.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2CircleButton1.ShadowDecoration.Depth = 0;
            guna2CircleButton1.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            guna2CircleButton1.Size = new Size(50, 50);
            guna2CircleButton1.TabIndex = 2;
            guna2CircleButton1.Click += guna2CircleButton1_Click;
            // 
            // MainPage
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1088, 839);
            Controls.Add(panel1);
            Controls.Add(guna2GradientPanel1);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainPage";
            StartPosition = FormStartPosition.CenterScreen;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            guna2GradientPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Panel panel1;
        private PictureBox pictureBox1;
        private Panel panel2;
        private Panel panel3;
        private PictureBox pictureBox2;
        private Button button1;
        private TextBox textUserName;
        private TextBox textpassword;
        private PictureBox pictureBox3;
        private Guna.UI2.WinForms.Guna2GradientPanel guna2GradientPanel1;
        private Guna.UI2.WinForms.Guna2CircleButton guna2CircleButton2;
        private Guna.UI2.WinForms.Guna2CircleButton guna2CircleButton1;
        private LinkLabel linkLabel1;
        private Label label4;
    }
}
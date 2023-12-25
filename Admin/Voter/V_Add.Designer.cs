namespace ElectionApp.Admin.Voter
{
    partial class V_Add
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
            button3 = new Button();
            textBox4 = new TextBox();
            textBox1 = new TextBox();
            button1 = new Button();
            label4 = new Label();
            label1 = new Label();
            label7 = new Label();
            textBox6 = new TextBox();
            SuspendLayout();
            // 
            // button3
            // 
            button3.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            button3.Location = new Point(483, 475);
            button3.Name = "button3";
            button3.Size = new Size(168, 47);
            button3.TabIndex = 36;
            button3.Text = "Confirm Adding";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // textBox4
            // 
            textBox4.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            textBox4.Location = new Point(358, 235);
            textBox4.Multiline = true;
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(270, 40);
            textBox4.TabIndex = 35;
            textBox4.KeyDown += textBox4_KeyDown;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            textBox1.Location = new Point(358, 138);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(270, 40);
            textBox1.TabIndex = 32;
            textBox1.KeyDown += textBox1_KeyDown;
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            button1.Location = new Point(309, 475);
            button1.Name = "button1";
            button1.Size = new Size(168, 47);
            button1.TabIndex = 31;
            button1.Text = "Upload Pic";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Century Gothic", 20F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(89, 235);
            label4.Name = "label4";
            label4.Size = new Size(115, 40);
            label4.TabIndex = 30;
            label4.Text = "Email:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Century Gothic", 20F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(89, 138);
            label1.Name = "label1";
            label1.Size = new Size(138, 40);
            label1.TabIndex = 27;
            label1.Text = "Name: ";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Century Gothic", 20F, FontStyle.Bold, GraphicsUnit.Point);
            label7.Location = new Point(89, 332);
            label7.Name = "label7";
            label7.Size = new Size(180, 40);
            label7.TabIndex = 41;
            label7.Text = "Password:";
            // 
            // textBox6
            // 
            textBox6.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            textBox6.Location = new Point(358, 332);
            textBox6.Multiline = true;
            textBox6.Name = "textBox6";
            textBox6.Size = new Size(270, 40);
            textBox6.TabIndex = 42;
            // 
            // V_Add
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(textBox6);
            Controls.Add(label7);
            Controls.Add(button3);
            Controls.Add(textBox4);
            Controls.Add(textBox1);
            Controls.Add(button1);
            Controls.Add(label4);
            Controls.Add(label1);
            Name = "V_Add";
            Size = new Size(1088, 839);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button button3;
        private TextBox textBox4;
        private TextBox textBox1;
        private Button button1;
        private Label label4;
        private Label label1;
        private Label label7;
        private TextBox textBox6;
    }
}

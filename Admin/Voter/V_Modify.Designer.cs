namespace ElectionApp.Admin.Voter
{
    partial class V_Modify
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
            SuspendLayout();
            // 
            // button3
            // 
            button3.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            button3.Location = new Point(401, 501);
            button3.Name = "button3";
            button3.Size = new Size(168, 47);
            button3.TabIndex = 22;
            button3.Text = "Confirm Update";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // textBox4
            // 
            textBox4.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            textBox4.Location = new Point(364, 301);
            textBox4.Multiline = true;
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(270, 40);
            textBox4.TabIndex = 21;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            textBox1.Location = new Point(364, 200);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(270, 40);
            textBox1.TabIndex = 18;
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            button1.Location = new Point(740, 252);
            button1.Name = "button1";
            button1.Size = new Size(138, 42);
            button1.TabIndex = 16;
            button1.Text = "Update Pic";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Century Gothic", 20F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(170, 301);
            label4.Name = "label4";
            label4.Size = new Size(115, 40);
            label4.TabIndex = 15;
            label4.Text = "Email:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Century Gothic", 20F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(170, 200);
            label1.Name = "label1";
            label1.Size = new Size(138, 40);
            label1.TabIndex = 12;
            label1.Text = "Name: ";
            // 
            // V_Modify
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(button3);
            Controls.Add(textBox4);
            Controls.Add(textBox1);
            Controls.Add(button1);
            Controls.Add(label4);
            Controls.Add(label1);
            Name = "V_Modify";
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
    }
}

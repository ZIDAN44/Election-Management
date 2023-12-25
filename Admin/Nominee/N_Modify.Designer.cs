namespace ElectionApp.Admin.Nominee
{
    partial class N_Modify
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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            button2 = new Button();
            textBox1 = new TextBox();
            textBox3 = new TextBox();
            button3 = new Button();
            label5 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Century Gothic", 20F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(52, 91);
            label1.Name = "label1";
            label1.Size = new Size(138, 40);
            label1.TabIndex = 1;
            label1.Text = "Name: ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Century Gothic", 20F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(52, 166);
            label2.Name = "label2";
            label2.Size = new Size(109, 40);
            label2.TabIndex = 2;
            label2.Text = "Party:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Century Gothic", 20F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(52, 245);
            label3.Name = "label3";
            label3.Size = new Size(115, 40);
            label3.TabIndex = 3;
            label3.Text = "Email:";
            // 
            // button2
            // 
            button2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            button2.Location = new Point(809, 104);
            button2.Name = "button2";
            button2.Size = new Size(138, 42);
            button2.TabIndex = 6;
            button2.Text = "Update Logo";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            textBox1.Location = new Point(278, 91);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(270, 40);
            textBox1.TabIndex = 7;
            // 
            // textBox3
            // 
            textBox3.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            textBox3.Location = new Point(278, 245);
            textBox3.Multiline = true;
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(270, 40);
            textBox3.TabIndex = 9;
            // 
            // button3
            // 
            button3.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            button3.Location = new Point(436, 491);
            button3.Name = "button3";
            button3.Size = new Size(168, 51);
            button3.TabIndex = 11;
            button3.Text = "Confirm Update";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Century Gothic", 19.8000011F, FontStyle.Italic, GraphicsUnit.Point);
            label5.Location = new Point(278, 166);
            label5.Name = "label5";
            label5.Size = new Size(249, 40);
            label5.TabIndex = 26;
            label5.Text = "User will select";
            // 
            // N_Modify
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(label5);
            Controls.Add(button3);
            Controls.Add(textBox3);
            Controls.Add(textBox1);
            Controls.Add(button2);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "N_Modify";
            Size = new Size(1088, 839);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Button button2;
        private TextBox textBox1;
        private TextBox textBox3;
        private Button button3;
        private Label label5;
    }
}

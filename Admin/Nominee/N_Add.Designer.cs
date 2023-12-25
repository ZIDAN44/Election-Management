namespace ElectionApp.Admin.Nominee
{
    partial class N_Add
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
            textBox3 = new TextBox();
            textBox1 = new TextBox();
            button2 = new Button();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            textBox5 = new TextBox();
            label5 = new Label();
            label4 = new Label();
            SuspendLayout();
            // 
            // button3
            // 
            button3.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            button3.Location = new Point(436, 431);
            button3.Name = "button3";
            button3.Size = new Size(168, 52);
            button3.TabIndex = 22;
            button3.Text = "Confirm Add";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // textBox3
            // 
            textBox3.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            textBox3.Location = new Point(280, 260);
            textBox3.Multiline = true;
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(270, 40);
            textBox3.TabIndex = 20;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            textBox1.Location = new Point(280, 88);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(270, 40);
            textBox1.TabIndex = 18;
            // 
            // button2
            // 
            button2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            button2.Location = new Point(696, 121);
            button2.Name = "button2";
            button2.Size = new Size(138, 42);
            button2.TabIndex = 17;
            button2.Text = "Upload Logo";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Century Gothic", 20F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(54, 260);
            label3.Name = "label3";
            label3.Size = new Size(115, 40);
            label3.TabIndex = 14;
            label3.Text = "Email:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Century Gothic", 20F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(54, 174);
            label2.Name = "label2";
            label2.Size = new Size(109, 40);
            label2.TabIndex = 13;
            label2.Text = "Party:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Century Gothic", 20F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(54, 88);
            label1.Name = "label1";
            label1.Size = new Size(138, 40);
            label1.TabIndex = 12;
            label1.Text = "Name: ";
            // 
            // textBox5
            // 
            textBox5.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            textBox5.Location = new Point(280, 344);
            textBox5.Multiline = true;
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(270, 40);
            textBox5.TabIndex = 24;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Century Gothic", 20F, FontStyle.Bold, GraphicsUnit.Point);
            label5.Location = new Point(54, 344);
            label5.Name = "label5";
            label5.Size = new Size(180, 40);
            label5.TabIndex = 23;
            label5.Text = "Password:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Century Gothic", 19.8000011F, FontStyle.Italic, GraphicsUnit.Point);
            label4.Location = new Point(280, 174);
            label4.Name = "label4";
            label4.Size = new Size(249, 40);
            label4.TabIndex = 25;
            label4.Text = "User will select";
            // 
            // N_Add
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(label4);
            Controls.Add(textBox5);
            Controls.Add(label5);
            Controls.Add(button3);
            Controls.Add(textBox3);
            Controls.Add(textBox1);
            Controls.Add(button2);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "N_Add";
            Size = new Size(1088, 839);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button3;
        private TextBox textBox3;
        private TextBox textBox1;
        private Button button2;
        private Label label3;
        private Label label2;
        private Label label1;
        private TextBox textBox5;
        private Label label5;
        private Label label4;
    }
}

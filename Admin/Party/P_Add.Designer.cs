namespace ElectionApp.Admin.Party
{
    partial class P_Add
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
            textBox1 = new TextBox();
            button2 = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // button3
            // 
            button3.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            button3.Location = new Point(451, 436);
            button3.Name = "button3";
            button3.Size = new Size(168, 52);
            button3.TabIndex = 32;
            button3.Text = "Confirm Add";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            textBox1.Location = new Point(530, 200);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(270, 40);
            textBox1.TabIndex = 30;
            // 
            // button2
            // 
            button2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            button2.Location = new Point(277, 285);
            button2.Name = "button2";
            button2.Size = new Size(523, 42);
            button2.TabIndex = 29;
            button2.Text = "Upload Logo";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Century Gothic", 20F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(277, 200);
            label1.Name = "label1";
            label1.Size = new Size(230, 40);
            label1.TabIndex = 26;
            label1.Text = "Party Name: ";
            // 
            // P_Add
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(button3);
            Controls.Add(textBox1);
            Controls.Add(button2);
            Controls.Add(label1);
            Name = "P_Add";
            Size = new Size(1088, 839);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button3;
        private TextBox textBox1;
        private Button button2;
        private Label label1;
    }
}

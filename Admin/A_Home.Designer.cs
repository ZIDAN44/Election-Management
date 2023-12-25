namespace ElectionApp.Admin.Nominee
{
    partial class A_Home
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
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Century Gothic", 42F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(342, 252);
            label1.Name = "label1";
            label1.Size = new Size(378, 83);
            label1.TabIndex = 0;
            label1.Text = "WELCOME";
            // 
            // A_Home
            // 
            AutoScaleMode = AutoScaleMode.None;
            Controls.Add(label1);
            Font = new Font("Century Gothic", 10.8F, FontStyle.Regular, GraphicsUnit.Point);
            Name = "A_Home";
            Size = new Size(1070, 630);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
    }
}

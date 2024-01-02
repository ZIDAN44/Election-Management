using System.ComponentModel;

namespace ElectionApp.Common
{
    public partial class User_Notification : UserControl
    {
        public User_Notification()
        {
            InitializeComponent();
        }

        #region Properties
        private string _message1;
        private Image _icon1;

        [Category("Custom Props")]
        public string Message1
        {
            get { return _message1; }
            set { _message1 = value; lbIMessage1.Text = value; }
        }

        [Category("Custom Props")]
        public Image Image1
        {
            get { return _icon1; }
            set { _icon1 = value; pictureBox1.Image = value; }
        }
        #endregion
    }
}

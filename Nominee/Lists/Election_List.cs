using System.ComponentModel;

namespace ElectionApp.Nominee
{
    public partial class Election_List : UserControl
    {
        public Election_List()
        {
            InitializeComponent();
        }

        #region Properties

        private string _message1;
        private string _message2;
        private string _message3;
        private string _message4;

        private void button1_Click(object sender, EventArgs e)
        {
            JoinButtonClicked?.Invoke(this, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OnPdfButtonClicked?.Invoke(this, e);
        }

        [Category("Custom Props")]
        public string Message1
        {
            get { return _message1; }
            set { _message1 = value; lbIMessage1.Text = value; }
        }

        [Category("Custom Props")]
        public string Message2
        {
            get { return _message2; }
            set { _message2 = value; lbIMessage2.Text = value; }
        }

        [Category("Custom Props")]
        public string Message3
        {
            get { return _message3; }
            set { _message3 = value; lbIMessage3.Text = value; }
        }

        [Category("Custom Props")]
        public string Message4
        {
            get { return _message4; }
            set { _message4 = value; lbIMessage4.Text = value; }
        }

        [Category("Custom Props")]
        public Button Button1 { get; set; }

        [Category("Custom Props")]
        public Button Button2 { get; set; }

        [Category("Custom Props")]
        public event EventHandler JoinButtonClicked;

        [Category("Custom Props")]
        public event EventHandler OnPdfButtonClicked;

        #endregion
    }
}

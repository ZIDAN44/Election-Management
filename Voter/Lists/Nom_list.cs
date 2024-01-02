using System.ComponentModel;

namespace ElectionApp.Voter
{
    public partial class Nom_list : UserControl
    {
        public Nom_list()
        {
            InitializeComponent();
        }

        #region Properties

        private string _message1;
        private string _message2;
        private string _message3;
        private Image _icon1;

        private void button1_Click(object sender, EventArgs e)
        {
            // Trigger the event when the button is clicked
            VoteButtonClicked?.Invoke(this, e);
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
        public Image Image1
        {
            get { return _icon1; }
            set { _icon1 = value; pictureBox1.Image = value; }
        }


        [Category("Custom Props")]
        public Button Button1 { get; set; }

        [Category("Custom Props")]
        public event EventHandler VoteButtonClicked;

        #endregion
    }
}

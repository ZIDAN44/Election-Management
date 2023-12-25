using ElectionApp.Admin;
using ElectionApp.Admin.Election;
using ElectionApp.Admin.Nominee;
using ElectionApp.Admin.Party;
using ElectionApp.Admin.Voter;
using ElectionApp.Main;
using System.Runtime.InteropServices;

namespace ElectionApp
{
    public partial class A_Dashboard : Form
    {
        private string adminID;
        private string connectionString;
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        public A_Dashboard(string adminID, string connectionString)
        {
            AdminID = adminID;
            ConnectionString = connectionString;
            InitializeComponent();

            A_Home aHome = new A_Home();
            adduserControl(aHome);
        }

        private string AdminID
        {
            get { return adminID; }
            set { adminID = value; }
        }

        private string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        private void adduserControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            panelContainer.Controls.Clear();
            panelContainer.Controls.Add(userControl);
            userControl.BringToFront();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MainPage mainPage = new MainPage(connectionString);
            mainPage.Show();
            Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            A_Profile a_Profile = new A_Profile(adminID, connectionString);
            adduserControl(a_Profile);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            A_Home aHome = new A_Home();
            adduserControl(aHome);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            M_Voter manageVoter = new M_Voter(adminID, ConnectionString);
            adduserControl(manageVoter);
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            M_Party m_Party = new M_Party(adminID, ConnectionString);
            adduserControl(m_Party);
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            M_Nominee manageNominee = new M_Nominee(adminID, connectionString);
            adduserControl(manageNominee);
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            M_Election manageElection = new M_Election(adminID, connectionString);
            adduserControl(manageElection);
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2CircleButton2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2GradientPanel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}

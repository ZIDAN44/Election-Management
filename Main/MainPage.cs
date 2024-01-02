using ElectionApp.Nominee;
using ElectionApp.Voter;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace ElectionApp.Main
{
    public partial class MainPage : Form
    {
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        public MainPage(string connectionString)
        {
            ConnectionString = connectionString;
            InitializeComponent();
            textUserName.Select();
        }

        private string ConnectionString { get; set; }

        private bool ValidateLogin(string givenID, string password, out string role)
        {
            role = string.Empty;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = "SELECT COUNT(*) FROM LOGIN WHERE UID = @ID AND PASSWORD = @Password";
                string roleQuery = "SELECT ROLE FROM LOGIN WHERE UID = @ID";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlCommand roleCommand = new SqlCommand(roleQuery, connection))
                {
                    command.Parameters.AddWithValue("@ID", givenID);
                    command.Parameters.AddWithValue("@Password", password);

                    roleCommand.Parameters.AddWithValue("@ID", givenID);

                    try
                    {
                        connection.Open();
                        int count = Convert.ToInt32(command.ExecuteScalar());

                        if (count > 0)
                        {
                            role = roleCommand.ExecuteScalar()?.ToString();
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
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

        private void button1_Click(object sender, EventArgs e)
        {
            string givenID = textUserName.Text;
            string password = textpassword.Text;

            if (string.IsNullOrEmpty(givenID) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Pls Enter ID & Password!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (ValidateLogin(givenID, password, out string role))
            {
                // Convert the role to lowercase (or uppercase) for case-insensitive comparison
                string lowerCaseRole = role.ToLower();

                switch (lowerCaseRole)
                {
                    case "admin":
                        A_Dashboard adminDashboard = new A_Dashboard(givenID, ConnectionString);
                        adminDashboard.Show();
                        Hide();
                        break;
                    case "voter":
                        V_Dashboard voterDashboard = new V_Dashboard(givenID, ConnectionString);
                        voterDashboard.Show();
                        Hide();
                        break;
                    case "voter_temp":
                        VT_Dashboard voterTempDashboard = new VT_Dashboard(givenID, ConnectionString);
                        voterTempDashboard.Show();
                        Hide();
                        break;
                    case "nominee":
                        N_Dashboard nomineeDashboard = new N_Dashboard(givenID, ConnectionString);
                        nomineeDashboard.Show();
                        Hide();
                        break;
                    case "nominee_temp":
                        NT_Dashboard nomineeTempDashboard = new NT_Dashboard(givenID, ConnectionString);
                        nomineeTempDashboard.Show();
                        Hide();
                        break;
                    default:
                        // Unknown role OR role error!!!
                        MessageBox.Show("Unknown role. Please contact support.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
                textUserName.Clear();
                textpassword.Clear();
            }
            else
            {
                // Invalid credentials
                MessageBox.Show("Invalid credentials. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textpassword.Clear();
            }
        }

        private void textUserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textpassword.Focus();
                e.SuppressKeyPress = true;
            }
        }

        private void textpassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
                e.SuppressKeyPress = true;
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            textpassword.PasswordChar = (textpassword.PasswordChar == '\0') ? '*' : '\0';
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Registration registration = new Registration(ConnectionString);
            registration.Show();
            Hide();
        }
    }
}

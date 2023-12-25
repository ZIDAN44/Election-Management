using ElectionApp.Nominee;
using ElectionApp.Voter;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace ElectionApp.Main
{
    public partial class MainPage : Form
    {
        private string connectionString;
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

        private string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        private bool ValidateLogin(string givenID, string password, out string role)
        {
            role = string.Empty;

            using (SqlConnection connection = new SqlConnection(connectionString))
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

                            if (role.ToLower() == "nominee")
                            {
                                // Check NOMINEE_TEMP table for APRV
                                string aprvQuery = "SELECT APRV FROM NOMINEE_TEMP WHERE APRV_NOM_ID = @GivenID";

                                using (SqlCommand aprvCommand = new SqlCommand(aprvQuery, connection))
                                {
                                    aprvCommand.Parameters.AddWithValue("@GivenID", givenID);

                                    object aprvResult = aprvCommand.ExecuteScalar();
                                    if (aprvResult != null || aprvResult != DBNull.Value)
                                    {
                                        // If APRV is NULL, fetch reasons from REJECTIONS table
                                        string reasonQuery = "SELECT REASONS FROM REJECTIONS WHERE UID = @GivenID";
                                        using (SqlCommand reasonCommand = new SqlCommand(reasonQuery, connection))
                                        {
                                            reasonCommand.Parameters.AddWithValue("@GivenID", givenID);
                                            object reasonResult = reasonCommand.ExecuteScalar();
                                            if (reasonResult != null)
                                            {
                                                string reasons = reasonResult.ToString();
                                                MessageBox.Show($"Your account has been banded!!\nReasons: {reasons}\nPlease contact supports.", "Rejected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                return false; // Don't let the user login
                                            }
                                        }
                                    }
                                }
                            }

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
                        A_Dashboard adminDashboard = new A_Dashboard(givenID, connectionString);
                        adminDashboard.Show();
                        Hide();
                        break;
                    case "voter":
                        V_Dashboard voterDashboard = new V_Dashboard(givenID, connectionString);
                        voterDashboard.Show();
                        Hide();
                        break;
                    case "voter_temp":
                        VT_Dashboard voterTempDashboard = new VT_Dashboard(givenID, connectionString);
                        voterTempDashboard.Show();
                        Hide();
                        break;
                    case "nominee":
                        N_Dashboard nomineeDashboard = new N_Dashboard(givenID, connectionString);
                        nomineeDashboard.Show();
                        Hide();
                        break;
                    case "nominee_temp":
                        NT_Dashboard nomineeTempDashboard = new NT_Dashboard(givenID, connectionString);
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
            Registration registration = new Registration(connectionString);
            registration.Show();
            Hide();
        }
    }
}

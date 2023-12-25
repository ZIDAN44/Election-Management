using ElectionApp.Common;
using ElectionApp.Main;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace ElectionApp.Voter
{
    public partial class VT_Dashboard : Form
    {
        private string givenID;
        private string connectionString;
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private UserControl currentVoterControl;
        private User_Settings n_SettingsControl;

        public VT_Dashboard(string givenID, string connectionString)
        {
            GivenID = givenID;
            ConnectionString = connectionString;
            InitializeComponent();
            FetchVoterTDetails();
            CheckForAdminApproval();
        }

        private string GivenID
        {
            get { return givenID; }
            set { givenID = value; }
        }

        private string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        private void adduserControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;

            // Check if there's an existing control in panelVoter
            if (currentVoterControl != null && panelVoter.Controls.Contains(currentVoterControl))
            {
                currentVoterControl.Hide(); // Hide the current control instead of clearing it
            }

            // Check if the new control is N_Settings
            if (userControl is User_Settings)
            {
                panelVoter.Controls.Add(userControl);
                userControl.BringToFront();
            }
            else
            {
                // Display the new control
                panelVoter.Controls.Add(userControl);
                userControl.BringToFront();
                currentVoterControl = userControl;
            }
        }

        private void FetchVoterTDetails()
        {
            ElectionApp.Entity.Voter voter = new ElectionApp.Entity.Voter();

            // Fetch data from VOTER_TEMP using the provided GivenID
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT V_IDENTIFIER, V_NAME, V_EMAIL, PIC " +
                                   "FROM VOTER_TEMP WHERE V_IDENTIFIER = @givenID";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@givenID", GivenID);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        // Populate the Voter object properties
                        voter.V_IDENTIFIER = reader["V_IDENTIFIER"].ToString();
                        voter.V_NAME = reader["V_NAME"].ToString();
                        voter.V_EMAIL = reader["V_EMAIL"].ToString();

                        if (reader["PIC"] != DBNull.Value)
                        {
                            voter.PIC = (byte[])reader["PIC"];
                        }

                        // Update UI with Voter object data
                        label1.Text = "Temp ID: " + voter.V_IDENTIFIER;
                        label2.Text = "Name: " + voter.V_NAME;
                        label7.Text = "Email: " + voter.V_EMAIL;
                        label4.Text = "Welcome, " + voter.V_NAME;

                        if (voter.PIC != null)
                        {
                            using (MemoryStream ms = new MemoryStream(voter.PIC))
                            {
                                pictureBox1.Image = Image.FromStream(ms);
                            }
                        }
                        else
                        {
                            // Handle the case where no image is available
                            // pictureBox1.Image = YourDefaultImage;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Voter data not found.");
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void CheckForAdminApproval()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Check if the voter's registration has been approved by the admin
                    string query = "SELECT APRV, APRV_NID FROM VOTER_TEMP WHERE V_IDENTIFIER = @GivenID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@GivenID", givenID);

                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            bool isApproved = Convert.ToBoolean(reader["APRV"]);
                            if (isApproved)
                            {
                                string approvalStatus = "Your registration has been approved!";
                                labelApprovalStatus.Text = approvalStatus;
                                labelApprovalStatus.BackColor = System.Drawing.Color.LightGreen;

                                string aprvVoterId = reader["APRV_NID"].ToString();
                                reader.Close();

                                string loginQuery = "SELECT UID, PASSWORD FROM LOGIN WHERE UID = @GivenID";
                                using (SqlCommand loginCommand = new SqlCommand(loginQuery, connection))
                                {
                                    loginCommand.Parameters.AddWithValue("@GivenID", givenID);
                                    SqlDataReader loginReader = loginCommand.ExecuteReader();
                                    if (loginReader.Read())
                                    {
                                        string uid = loginReader["UID"].ToString();
                                        string password = loginReader["PASSWORD"].ToString();
                                        string approvedMessage = $"{approvalStatus}\n" +
                                            $"Your new NID: {aprvVoterId} and Password: {password}";

                                        labelApprovalStatus.Text = approvedMessage;
                                        labelApprovalStatus.BackColor = System.Drawing.Color.LightGreen;

                                        // Allow copying the APRV_NID and PASSWORD
                                        labelApprovalStatus.Click += (s, ev) =>
                                        {
                                            Clipboard.SetText($"NID: {aprvVoterId}\nPassword: {password}");
                                            MessageBox.Show("NID and Password copied to clipboard!");
                                        };
                                    }
                                    else
                                    {
                                        labelApprovalStatus.Text = "No login details found for the approved voter.";
                                    }
                                    loginReader.Close();
                                }
                            }
                            else
                            {
                                labelApprovalStatus.Text = "Your registration is pending approval.";
                                labelApprovalStatus.BackColor = System.Drawing.Color.Yellow;
                                reader.Close();
                            }
                        }
                        else
                        {
                            labelApprovalStatus.Text = "Your registration has been Rejected.";
                            labelApprovalStatus.BackColor = System.Drawing.Color.Red;
                            reader.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    connection.Close();
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            MainPage mainPage = new MainPage(connectionString);
            mainPage.Show();
            Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (n_SettingsControl == null || n_SettingsControl.IsDisposed)
            {
                n_SettingsControl = new User_Settings(givenID, connectionString);
                adduserControl(n_SettingsControl);
            }
            else
            {
                if (n_SettingsControl.Visible)
                {
                    n_SettingsControl.Hide();
                }
                else
                {
                    n_SettingsControl.Show();
                    n_SettingsControl.BringToFront();
                }
            }
        }
    }
}

using ElectionApp.Common;
using ElectionApp.Main;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace ElectionApp.Nominee
{
    public partial class N_Dashboard : Form
    {
        private string givenID;
        private string connectionString;
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        ElectionApp.Entity.Nominee nominee = new ElectionApp.Entity.Nominee();
        private Election_Select electionSelect;
        private Party_Select partySelect;
        private User_Settings n_SettingsControl;
        private UserControl currentNomineeControl;

        public N_Dashboard(string givenID, string connectionString)
        {
            GivenID = givenID;
            ConnectionString = connectionString;
            InitializeComponent();
            FetchNomineeDetails();
            CheckForPartyStatus();
            CheckForElectionStatus();
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

        private void adduserControl1(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            panelSelecter.Controls.Clear();
            panelSelecter.Controls.Add(userControl);
            userControl.BringToFront();
        }

        private void adduserControl2(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;

            // Check if there's an existing control in panelNominee
            if (currentNomineeControl != null && panelNominee.Controls.Contains(currentNomineeControl))
            {
                currentNomineeControl.Hide(); // Hide the current control instead of clearing it
            }

            // Check if the new control is N_Settings
            if (userControl is User_Settings)
            {
                panelNominee.Controls.Add(userControl);
                userControl.BringToFront();
            }
            else
            {
                // Display the new control
                panelNominee.Controls.Add(userControl);
                userControl.BringToFront();
                currentNomineeControl = userControl;
            }
        }

        private void FetchNomineeDetails()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT N_IDENTIFIER, N_NAME, N_EMAIL, LOGO, VCOUNT " +
                                   "FROM NOMINEE WHERE N_IDENTIFIER = @givenID";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@givenID", GivenID);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        // Populate the Nominee object properties
                        nominee.N_IDENTIFIER = reader["N_IDENTIFIER"].ToString();
                        nominee.N_NAME = reader["N_NAME"].ToString();
                        nominee.N_EMAIL = reader["N_EMAIL"].ToString();

                        // Check for DBNull before assigning VCOUNT
                        if (reader["VCOUNT"] != DBNull.Value)
                        {
                            nominee.VCOUNT = Convert.ToInt32(reader["VCOUNT"]);
                        }

                        if (reader["LOGO"] != DBNull.Value)
                        {
                            nominee.LOGO = (byte[])reader["LOGO"];
                        }

                        // Update UI with Nominee object data
                        label1.Text = "Temp ID: " + nominee.N_IDENTIFIER;
                        label2.Text = "Name: " + nominee.N_NAME;
                        label7.Text = "Email: " + nominee.N_EMAIL;
                        label5.Text = (nominee.VCOUNT != 0) ? "Vote Count: " + nominee.VCOUNT : "Vote Count: N/A";
                        label4.Text = "Welcome, " + nominee.N_NAME;

                        if (nominee.LOGO != null)
                        {
                            using (MemoryStream ms = new MemoryStream(nominee.LOGO))
                            {
                                pictureBox1.Image = Image.FromStream(ms);
                            }
                        }
                        else
                        {
                            // pictureBox1.Image = DefaultImage;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Nominee data not found!!");
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

        private void CheckForPartyStatus()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT P_NAME FROM NOMINEE WHERE N_IDENTIFIER = @GivenID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@GivenID", givenID);

                        object partyName = command.ExecuteScalar();
                        if (partyName != null || partyName != DBNull.Value)
                        {
                            string party = partyName.ToString();
                            nominee.P_NAME = party;

                            if (string.IsNullOrEmpty(party))
                            {
                                label3.Text = "Not Joined Any Party Yet!!";
                                label3.BackColor = System.Drawing.Color.Yellow;
                            }
                            else
                            {
                                label3.Text = "Party: " + nominee.P_NAME;
                            }
                        }
                        else
                        {
                            label3.Text = "Error: Invalid Nominee Not Found.";
                            label3.BackColor = System.Drawing.Color.Red;
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

        private void CheckForElectionStatus()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string participationQuery = "SELECT E_NAME, S_DATE, E_DATE, APRV " +
                                                "FROM ELECTION e INNER JOIN PARTICIPATES p ON e.E_IDENTIFIER = p.ELECTION_ID " +
                                                "WHERE p.NOMINEE_ID = @givenID";

                    SqlCommand participationCommand = new SqlCommand(participationQuery, connection);
                    participationCommand.Parameters.AddWithValue("@givenID", givenID);

                    SqlDataReader participationReader = participationCommand.ExecuteReader();
                    if (participationReader.Read())
                    {
                        bool? approvalStatus = participationReader["APRV"] as bool?;
                        if (approvalStatus == null)
                        {
                            label9.Text = "Rejected! Please Contak Admins.";
                            label9.BackColor = System.Drawing.Color.Red;
                        }
                        else if (approvalStatus == false)
                        {
                            label9.Text = "Selection is pending approval!";
                            label9.BackColor = System.Drawing.Color.Yellow;
                        }
                        else if (approvalStatus == true)
                        {
                            label9.Text = $"Status: {participationReader["E_NAME"]} " +
                                          $"({((DateTime)participationReader["S_DATE"]).ToShortDateString()} " +
                                          $"To {((DateTime)participationReader["E_DATE"]).ToShortDateString()})";
                        }
                    }
                    else
                    {
                        label9.Text = "Not participated in any election yet!";
                        label9.BackColor = System.Drawing.Color.LightYellow;
                    }

                    participationReader.Close();
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

        public void UpdatePartyStatusLabel(string partyName)
        {
            label3.Text = "Party: " + partyName;
            nominee.P_NAME = partyName;
        }

        public void UpdateElectionStatusLabel(string status)
        {
            label9.Text = status;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            MainPage mainPage = new MainPage(connectionString);
            mainPage.Show();
            Hide();
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
            // Check if the nominee is already in a party
            if (!string.IsNullOrEmpty(nominee.P_NAME) && !string.IsNullOrEmpty(label3.Text) && label3.Text.Contains("Party:"))
            {
                MessageBox.Show("You are already in a party!");
                return;
            }

            // If Election_Select control is visible, hide it first
            if (electionSelect != null && !electionSelect.IsDisposed && electionSelect.Visible)
            {
                electionSelect.Hide();
            }

            // Proceed with showing/hiding Party_Select control
            if (partySelect == null || partySelect.IsDisposed)
            {
                partySelect = new Party_Select(givenID, connectionString);
                partySelect.VisibleChanged += (s, args) =>
                {
                    if (!partySelect.Visible)
                    {
                        partySelect = null; // Reset the instance when it is hidden
                    }
                };
                adduserControl1(partySelect);
            }
            else
            {
                if (partySelect.Visible)
                {
                    partySelect.Hide(); // If visible, hide it
                }
                else
                {
                    partySelect.Show(); // If hidden, show it
                    partySelect.BringToFront();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Check if the nominee is already in an election or if the selection is pending approval
            if (!string.IsNullOrEmpty(label9.Text) && (label9.Text.Contains("Status:") || label9.Text.Contains("Selection is pending approval!")))
            {
                MessageBox.Show("You are already in an election or your selection is pending approval!");
                return;
            }

            // If Party_Select control is visible, hide it first
            if (partySelect != null && !partySelect.IsDisposed && partySelect.Visible)
            {
                partySelect.Hide();
            }

            // Proceed with showing/hiding Election_Select control
            if (electionSelect == null || electionSelect.IsDisposed)
            {
                electionSelect = new Election_Select(givenID, connectionString);
                electionSelect.VisibleChanged += (s, args) =>
                {
                    if (!electionSelect.Visible)
                    {
                        electionSelect = null; // Reset the instance when it is hidden
                    }
                };
                adduserControl1(electionSelect);
            }
            else
            {
                if (electionSelect.Visible)
                {
                    electionSelect.Hide(); // If visible, hide it
                }
                else
                {
                    electionSelect.Show(); // If hidden, show it
                    electionSelect.BringToFront();
                }
            }
        }


        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (n_SettingsControl == null || n_SettingsControl.IsDisposed)
            {
                n_SettingsControl = new User_Settings(givenID, connectionString);
                adduserControl2(n_SettingsControl);
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

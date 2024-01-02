using ElectionApp.Common;
using ElectionApp.Main;
using ElectionApp.Voter.Selects;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace ElectionApp.Voter
{
    public partial class V_Dashboard : Form
    {
        private string givenID;
        private string connectionString;
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        ElectionApp.Entity.Voter voter = new ElectionApp.Entity.Voter();
        private UserControl currentVoterControl;
        private User_Settings n_SettingsControl;
        private Notification_Panel n_Notification;

        public V_Dashboard(string givenID, string connectionString)
        {
            GivenID = givenID;
            ConnectionString = connectionString;
            InitializeComponent();
            FetchVoterDetails();
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

        private void FetchVoterDetails()
        {
            // Fetch data from VOTER using the provided GivenID
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT V_IDENTIFIER, V_NAME, V_EMAIL, PIC, HAS_VOTE " +
                                   "FROM VOTER WHERE V_IDENTIFIER = @givenID";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@givenID", GivenID);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        // Populate the Voter object properties
                        voter.V_IDENTIFIER = reader["V_IDENTIFIER"].ToString();
                        voter.V_NAME = reader["V_NAME"].ToString();
                        voter.V_EMAIL = reader["V_EMAIL"].ToString();
                        voter.HAS_VOTE = Convert.ToBoolean(reader["HAS_VOTE"]);

                        if (reader["PIC"] != DBNull.Value)
                        {
                            voter.PIC = (byte[])reader["PIC"];
                        }

                        // Update UI with Voter object data
                        label1.Text = "NID: " + voter.V_IDENTIFIER;
                        label2.Text = "Name: " + voter.V_NAME;
                        label7.Text = "Email: " + voter.V_EMAIL;
                        label4.Text = "Welcome, " + voter.V_NAME;

                        if (!voter.HAS_VOTE)
                        {
                            labelVoteStatus.Text = "Vote Status: N/A";
                        }
                        else
                        {
                            labelVoteStatus.Text = "Vote Status: Voted";
                            labelVoteStatus.BackColor = System.Drawing.Color.LightGreen;
                        }

                        if (voter.PIC != null)
                        {
                            using (MemoryStream ms = new MemoryStream(voter.PIC))
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

        public void UpdateApprovalStatus(bool hasVoted)
        {
            if (hasVoted)
            {
                labelVoteStatus.Text = "Vote Status: Voted";
                labelVoteStatus.BackColor = System.Drawing.Color.LightGreen;
            }
        }

        public void DisableVotingPanel()
        {
            panelSelecter.Controls.Clear();
            panelSelecter.Enabled = false;
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (voter.HAS_VOTE)
            {
                MessageBox.Show("You have already voted!");
                return;
            }

            if (panelSelecter.Controls.Count > 0 && panelSelecter.Controls[0] is Vote_Select)
            {
                // Toggle visibility of the Vote_Select control
                panelSelecter.Controls[0].Visible = !panelSelecter.Controls[0].Visible;
            }
            else
            {
                // If no Vote_Select control is found, create and show it
                Vote_Select vote_Select = new Vote_Select(GivenID, connectionString);
                adduserControl1(vote_Select);
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (n_Notification == null || n_Notification.IsDisposed)
            {
                n_Notification = new Notification_Panel(givenID, connectionString, "voter");
                adduserControl2(n_Notification);
            }
            else
            {
                if (n_Notification.Visible)
                {
                    n_Notification.Hide();
                }
                else
                {
                    n_Notification.Show();
                    n_Notification.BringToFront();
                }
            }
        }
    }
}

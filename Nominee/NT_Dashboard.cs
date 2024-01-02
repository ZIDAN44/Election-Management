using ElectionApp.Common;
using ElectionApp.Main;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace ElectionApp.Nominee
{
    public partial class NT_Dashboard : Form
    {
        private string givenID;
        private string connectionString;
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private UserControl currentNomineeControl;
        private User_Settings n_SettingsControl;
        private Notification_Panel n_Notification;

        public NT_Dashboard(string givenID, string connectionString)
        {
            GivenID = givenID;
            ConnectionString = connectionString;
            InitializeComponent();
            FetchNomineeTDetails();
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

        private void FetchNomineeTDetails()
        {
            ElectionApp.Entity.Nominee nominee = new ElectionApp.Entity.Nominee();

            // Fetch data from NOMINEE using the provided GivenID
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT N_IDENTIFIER, N_NAME, N_EMAIL, LOGO " +
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

                        if (reader["LOGO"] != DBNull.Value)
                        {
                            nominee.LOGO = (byte[])reader["LOGO"];
                        }

                        // Update UI with Nominee object data
                        label1.Text = "User ID: " + nominee.N_IDENTIFIER;
                        label2.Text = "Name: " + nominee.N_NAME;
                        label7.Text = "Email: " + nominee.N_EMAIL;
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
                            // Handle the case where no image is available
                            // pictureBox1.Image = YourDefaultImage;
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

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (n_Notification == null || n_Notification.IsDisposed)
            {
                n_Notification = new Notification_Panel(givenID, connectionString, "nominee");
                adduserControl(n_Notification);
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

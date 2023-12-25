using ElectionApp.Common;
using System.Data.SqlClient;

namespace ElectionApp.Admin
{
    public partial class A_Profile : UserControl
    {
        private string adminID;
        private string connectionString;

        private UserControl currentAdminControl;
        private User_Settings n_SettingsControl;

        public A_Profile(string adminID, string connectionString)
        {
            AdminID = adminID;
            ConnectionString = connectionString;
            InitializeComponent();
            FetchAdminDetails();
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

            // Check if there's an existing control in panelAdminProfile
            if (currentAdminControl != null && panelAdminProfile.Controls.Contains(currentAdminControl))
            {
                currentAdminControl.Hide(); // Hide the current control instead of clearing it
            }

            // Check if the new control is N_Settings
            if (userControl is User_Settings)
            {
                panelAdminProfile.Controls.Add(userControl);
                userControl.BringToFront();
            }
            else
            {
                // Display the new control
                panelAdminProfile.Controls.Add(userControl);
                userControl.BringToFront();
                currentAdminControl = userControl;
            }
        }

        private void FetchAdminDetails()
        {
            ElectionApp.Entity.Admin admin = new ElectionApp.Entity.Admin();

            // Fetch data from ADMIN using the provided GivenID
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT A_IDENTIFIER, A_NAME, EMAIL, PHONE " +
                                   "FROM ADMIN WHERE A_IDENTIFIER = @givenID";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@givenID", adminID);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        // Populate the Admin object properties
                        admin.A_IDENTIFIER = reader["A_IDENTIFIER"].ToString();
                        admin.A_NAME = reader["A_NAME"].ToString();
                        admin.EMAIL = reader["EMAIL"].ToString();
                        admin.PHONE = reader["PHONE"].ToString();

                        // Update UI with Nominee object data
                        label1.Text = "User ID: " + admin.A_IDENTIFIER;
                        label2.Text = "Name: " + admin.A_NAME;
                        label7.Text = "Email: " + admin.EMAIL;
                        label3.Text = "Phone: " + admin.PHONE;
                        label4.Text = "Welcome, " + admin.A_NAME;
                    }
                    else
                    {
                        MessageBox.Show("Admin data not found!!");
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

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (n_SettingsControl == null || n_SettingsControl.IsDisposed)
            {
                n_SettingsControl = new User_Settings(adminID, connectionString);
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

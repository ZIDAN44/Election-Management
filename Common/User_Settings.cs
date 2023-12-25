using System.ComponentModel;
using System.Data.SqlClient;

namespace ElectionApp.Common
{
    public partial class User_Settings : UserControl
    {
        private string givenID;
        private string connectionString;

        public User_Settings(string givenID, string connectionString)
        {
            GivenID = givenID;
            ConnectionString = connectionString;
            InitializeComponent();
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

        // Fetch the current password associated with givenID from the LOGIN table
        private string GetCurrentPassword(string userID)
        {
            string currentPassword = string.Empty;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string selectPasswordQuery = "SELECT PASSWORD FROM LOGIN WHERE UID = @UserID";
                    SqlCommand command = new SqlCommand(selectPasswordQuery, connection);
                    command.Parameters.AddWithValue("@UserID", userID);

                    // Execute the query and get the password
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        currentPassword = result.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching current password: " + ex.Message);
            }

            return currentPassword;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string currentPassword = GetCurrentPassword(GivenID);

            if (textBox1.Text != currentPassword)
            {
                errorProvider1.SetError(textBox1, "Incorrect current password.");
            }
            else
            {
                errorProvider1.Clear();

                if (textBox2.Text != textBox3.Text)
                {
                    errorProvider2.SetError(textBox3, "Passwords do not match.");
                }
                else
                {
                    errorProvider2.Clear();

                    // Update the password in the LOGIN table
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(ConnectionString))
                        {
                            connection.Open();

                            string updatePasswordQuery = "UPDATE LOGIN SET PASSWORD = @NewPassword WHERE UID = @UserID";
                            SqlCommand command = new SqlCommand(updatePasswordQuery, connection);
                            command.Parameters.AddWithValue("@NewPassword", textBox2.Text);
                            command.Parameters.AddWithValue("@UserID", GivenID);
                            command.ExecuteNonQuery();

                            MessageBox.Show("Password updated successfully!");

                            textBox1.Clear();
                            textBox2.Clear();
                            textBox3.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error updating password: " + ex.Message);
                    }
                }
            }
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            string currentPassword = GetCurrentPassword(GivenID);

            if (textBox1.Text != currentPassword)
            {
                errorProvider1.SetError(textBox1, "Incorrect current password.");
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void textBox3_Validating(object sender, CancelEventArgs e)
        {
            if (textBox2.Text != textBox3.Text)
            {
                errorProvider2.SetError(textBox3, "Passwords do not match.");
            }
            else
            {
                errorProvider2.Clear();
            }
        }
    }
}

using System.Data.SqlClient;

namespace ElectionApp.Admin.Nominee
{
    public partial class N_Add : UserControl
    {
        private string adminID;
        private string connectionString;
        private byte[] Logo;

        public N_Add(string adminID, string connectionString)
        {
            AdminID = adminID;
            ConnectionString = connectionString;
            InitializeComponent();
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

        private void button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select Logo";
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Read the selected image file as bytes and store it in logo variable
                        string filePath = openFileDialog.FileName;
                        Logo = File.ReadAllBytes(filePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Retrieve the values from the textboxes
                string nomineeName = textBox1.Text;
                string email = textBox3.Text;

                // Check if all fields are filled before proceeding
                if (string.IsNullOrWhiteSpace(nomineeName) || string.IsNullOrWhiteSpace(email) || Logo == null)
                {
                    MessageBox.Show("Please fill all fields and upload both document and logo.");
                    return;
                }

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO NOMINEE_TEMP (N_NAME, N_EMAIL, LOGO) " +
                                   "VALUES (@NomineeName, @Email, @Logo)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@NomineeName", nomineeName);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Logo", Logo);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Nominee add successfully.");
                    }
                    else
                    {
                        MessageBox.Show("No data added. Something went wrong.");
                    }
                }

                // After uploading data to NOMINEE_TEMP table, retrieve N_IDENTIFIER
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Get the last inserted N_IDENTIFIER
                    string getIdQuery = "SELECT TOP 1 N_IDENTIFIER FROM NOMINEE_TEMP ORDER BY TEMP_NOM_ID DESC";
                    SqlCommand getIdCommand = new SqlCommand(getIdQuery, connection);
                    string nomineeId = getIdCommand.ExecuteScalar()?.ToString();

                    // Insert N_IDENTIFIER into the LOGIN table
                    if (!string.IsNullOrEmpty(nomineeId))
                    {
                        string password = textBox5.Text;
                        string role = "nominee_temp";

                        string insertLoginQuery = "INSERT INTO LOGIN (UID, PASSWORD, ROLE) VALUES (@uid, @password, @role)";
                        SqlCommand insertLoginCommand = new SqlCommand(insertLoginQuery, connection);
                        insertLoginCommand.Parameters.AddWithValue("@uid", nomineeId);
                        insertLoginCommand.Parameters.AddWithValue("@password", password);
                        insertLoginCommand.Parameters.AddWithValue("@role", role);
                        insertLoginCommand.ExecuteNonQuery();

                        string successMessage = $"Login data inserted successfully.\nNominee ID: {nomineeId}\nPassword: {password}";
                        MessageBox.Show(successMessage);
                        Clipboard.SetText(successMessage);
                        MessageBox.Show("ID and Password copied to clipboard!");
                    }
                    else
                    {
                        MessageBox.Show("No nominee identifier found.");
                    }
                }

                textBox1.Clear();
                textBox3.Clear();
                textBox5.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}

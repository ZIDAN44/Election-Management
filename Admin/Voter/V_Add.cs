using System.Data.SqlClient;

namespace ElectionApp.Admin.Voter
{
    public partial class V_Add : UserControl
    {
        private string adminID;
        private string connectionString;
        private byte[] Pic;

        public V_Add(string adminID, string connectionString)
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

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select Pic";
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Read the selected image file as bytes and store it in pic variable
                        string filePath = openFileDialog.FileName;
                        Pic = File.ReadAllBytes(filePath);
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
                string voterName = textBox1.Text;
                string EmailName = textBox4.Text;

                // Check if all fields are filled before proceeding
                if (string.IsNullOrWhiteSpace(voterName) || string.IsNullOrWhiteSpace(EmailName) || Pic == null)
                {
                    MessageBox.Show("Please fill all fields and upload Pic!!");
                    return;
                }

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO VOTER_TEMP (V_NAME, V_EMAIL, PIC) " +
                                   "VALUES (@VoterName, @Email, @Pic)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@VoterName", voterName);
                    command.Parameters.AddWithValue("@Email", EmailName);
                    command.Parameters.AddWithValue("@Pic", Pic);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data added successfully!");
                        // Optionally, refresh or reload the DataGridView after the update
                    }
                    else
                    {
                        MessageBox.Show("No data added. Something went wrong.");
                    }
                }

                // After uploading data to VOTER_TEMP table, retrieve V_IDENTIFIER
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Get the last inserted V_IDENTIFIER
                    string getIdQuery = "SELECT TOP 1 V_IDENTIFIER FROM VOTER_TEMP ORDER BY TEMP_NID DESC";
                    SqlCommand getIdCommand = new SqlCommand(getIdQuery, connection);
                    string voterId = getIdCommand.ExecuteScalar()?.ToString();

                    // Insert V_IDENTIFIER into the LOGIN table
                    if (!string.IsNullOrEmpty(voterId))
                    {
                        string password = textBox6.Text;
                        string role = "voter_temp";

                        string insertLoginQuery = "INSERT INTO LOGIN (UID, PASSWORD, ROLE) VALUES (@uid, @password, @role)";
                        SqlCommand insertLoginCommand = new SqlCommand(insertLoginQuery, connection);
                        insertLoginCommand.Parameters.AddWithValue("@uid", voterId);
                        insertLoginCommand.Parameters.AddWithValue("@password", password);
                        insertLoginCommand.Parameters.AddWithValue("@role", role);
                        insertLoginCommand.ExecuteNonQuery();

                        string successMessage = $"Login data inserted successfully.\nVoter ID: {voterId}\nPassword: {password}";
                        MessageBox.Show(successMessage);
                        Clipboard.SetText(successMessage);
                        MessageBox.Show("ID and Password copied to clipboard!");
                    }
                    else
                    {
                        MessageBox.Show("No voter identifier found.");
                    }
                }

                textBox1.Clear();
                textBox4.Clear();
                textBox6.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox4.Focus();
                e.SuppressKeyPress = true;
            }
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox6.Focus();
                e.SuppressKeyPress = true;
            }
        }
    }
}

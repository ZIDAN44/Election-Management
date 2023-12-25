using System.Data.SqlClient;

namespace ElectionApp.Admin.Election
{
    public partial class E_Add : UserControl
    {
        private string adminID;
        private string connectionString;
        private byte[] Doc;

        public E_Add(string adminID, string connectionString)
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
                openFileDialog.Title = "Select Document";
                openFileDialog.Filter = "All Files|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Read the selected file as bytes and store it in rDoc variable
                        string filePath = openFileDialog.FileName;
                        Doc = File.ReadAllBytes(filePath);
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
                string ElectionName = textBox1.Text;
                string Type = textBox2.Text;
                DateTime StartingDate = guna2DateTimePicker1.Value;
                DateTime EndingDate = guna2DateTimePicker2.Value;

                // Check if all fields are filled before proceeding
                if (string.IsNullOrWhiteSpace(ElectionName) || string.IsNullOrWhiteSpace(Type) || Doc == null)
                {
                    MessageBox.Show("Please fill all fields and upload document!");
                    return;
                }

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO ELECTION (E_NAME, TYPE, S_DATE, E_DATE, R_DOC) " +
                                   "VALUES (@ElectionName, @Type, @StartingDate, @EndingDate, @Doc)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ElectionName", ElectionName);
                    command.Parameters.AddWithValue("@Type", Type);
                    command.Parameters.AddWithValue("@StartingDate", StartingDate);
                    command.Parameters.AddWithValue("@EndingDate", EndingDate);
                    command.Parameters.AddWithValue("@Doc", (object)Doc ?? DBNull.Value);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data added successfully!");
                    }
                    else
                    {
                        MessageBox.Show("No data added. Something went wrong.");
                    }
                }

                // After uploading data to ELECTION table, retrieve E_IDENTIFIER
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Get the last inserted E_IDENTIFIER
                    string getEIdentifierQuery = "SELECT TOP 1 E_IDENTIFIER FROM ELECTION ORDER BY E_ID DESC";
                    SqlCommand getEIdentifierCommand = new SqlCommand(getEIdentifierQuery, connection);
                    string eIdentifier = getEIdentifierCommand.ExecuteScalar()?.ToString();

                    if (!string.IsNullOrEmpty(eIdentifier))
                    {
                        // Insert into REGULATES table
                        string regulatesInsertQuery = "INSERT INTO REGULATES (ELECTION_ID, ADMIN_ID) " +
                                                      "VALUES (@eIdentifier, @adminId)";
                        SqlCommand regulatesCommand = new SqlCommand(regulatesInsertQuery, connection);
                        regulatesCommand.Parameters.AddWithValue("@eIdentifier", eIdentifier);
                        regulatesCommand.Parameters.AddWithValue("@adminId", AdminID);
                        regulatesCommand.ExecuteNonQuery();
                    }
                    else
                    {
                        MessageBox.Show("No Election identifier found.");
                    }
                }

                textBox1.Clear();
                textBox2.Clear();
                guna2DateTimePicker1.Value = DateTime.Now;
                guna2DateTimePicker2.Value = DateTime.Now;
                Doc = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}

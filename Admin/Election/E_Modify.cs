using System.Data.SqlClient;

namespace ElectionApp.Admin.Election
{
    public partial class E_Modify : UserControl
    {
        private string adminID;
        private string connectionString;
        private string eIdentifier;
        private byte[] Doc;

        public E_Modify(string adminID, string connectionString)
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

        public void ReceiveSelectedRowData(DataGridViewRow selectedRow)
        {
            // Retrieve data from the selected row
            eIdentifier = selectedRow.Cells["E_IDENTIFIER"].Value.ToString();
            string electionName = selectedRow.Cells["E_NAME"].Value.ToString();
            string type = selectedRow.Cells["TYPE"].Value.ToString();
            DateTime startingDate = (DateTime)selectedRow.Cells["S_DATE"].Value;
            DateTime endingDate = (DateTime)selectedRow.Cells["E_DATE"].Value;
            Doc = (byte[])selectedRow.Cells["R_DOC"].Value;

            // Display the retrieved data in the respective textboxes
            textBox1.Text = electionName;
            textBox2.Text = type;
            guna2DateTimePicker1.Value = startingDate;
            guna2DateTimePicker2.Value = endingDate;
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
                // Retrieve the updated values from the textboxes
                string updatedElectionName = textBox1.Text;
                string updatedType = textBox2.Text;
                DateTime updateStartingDate = guna2DateTimePicker1.Value;
                DateTime updateEndingDate = guna2DateTimePicker2.Value;

                // Check if all fields are filled before proceeding
                if (string.IsNullOrWhiteSpace(updatedElectionName) || string.IsNullOrWhiteSpace(updatedType))
                {
                    MessageBox.Show("Please fill all fields!!");
                    return;
                }

                // Use the retrieved identifier to update the ELECTION table
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "UPDATE ELECTION SET E_NAME = @ElectionName, TYPE = @Type, S_DATE = @StartingDate, E_DATE = @EndingDate, R_DOC = @Doc WHERE E_IDENTIFIER = @EIdentifier";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ElectionName", updatedElectionName);
                    command.Parameters.AddWithValue("@Type", updatedType);
                    command.Parameters.AddWithValue("@StartingDate", updateStartingDate);
                    command.Parameters.AddWithValue("@EndingDate", updateEndingDate);
                    command.Parameters.AddWithValue("@EIdentifier", eIdentifier);
                    command.Parameters.AddWithValue("@Doc", (object)Doc ?? DBNull.Value);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data updated successfully!");
                        // Optionally, refresh or reload the DataGridView after the update
                    }
                    else
                    {
                        MessageBox.Show("No rows updated. Something went wrong.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            textBox1.Clear();
            textBox2.Clear();
            guna2DateTimePicker1.Value = DateTime.Now;
            guna2DateTimePicker2.Value = DateTime.Now;
        }
    }
}

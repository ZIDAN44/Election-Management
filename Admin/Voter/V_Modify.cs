using System.Data.SqlClient;

namespace ElectionApp.Admin.Voter
{
    public partial class V_Modify : UserControl
    {
        private string vIdentifier;
        private byte[] Pic;

        public V_Modify(string adminID, string connectionString)
        {
            AdminID = adminID;
            ConnectionString = connectionString;
            InitializeComponent();
        }

        private string AdminID { get; set; }

        private string ConnectionString { get; set; }

        public void ReceiveSelectedRowData(DataGridViewRow selectedRow)
        {
            // Retrieve data from the selected row
            vIdentifier = selectedRow.Cells["V_IDENTIFIER"].Value.ToString();
            string voterName = selectedRow.Cells["V_NAME"].Value.ToString();
            string emailName = selectedRow.Cells["V_EMAIL"].Value.ToString();

            // Display the retrieved data in the respective textboxes
            textBox1.Text = voterName;
            textBox4.Text = emailName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select Picture";
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
                // Retrieve the updated values from the textboxes
                string updatedVoterName = textBox1.Text;
                string updatedEmailName = textBox4.Text;

                // Check if all fields are filled before proceeding
                if (string.IsNullOrWhiteSpace(updatedVoterName) || string.IsNullOrWhiteSpace(updatedEmailName))
                {
                    MessageBox.Show("Please fill all fields!!");
                    return;
                }

                // Use the retrieved identifier to update the VOTER table
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "UPDATE VOTER SET V_NAME = @VoterName, V_EMAIL = @Email, PIC = @Pic WHERE V_IDENTIFIER = @VIdentifier";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@VoterName", updatedVoterName);
                    command.Parameters.AddWithValue("@Email", updatedEmailName);
                    command.Parameters.AddWithValue("@VIdentifier", vIdentifier);
                    command.Parameters.AddWithValue("@Pic", (object)Pic ?? DBNull.Value);

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
            textBox4.Clear();
        }
    }
}

using System.Data.SqlClient;

namespace ElectionApp.Admin.Nominee
{
    public partial class N_Modify : UserControl
    {
        private string nIdentifier;
        private byte[] Logo;

        public N_Modify(string adminID, string connectionString)
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
            nIdentifier = selectedRow.Cells["N_IDENTIFIER"].Value.ToString();
            string nomineeName = selectedRow.Cells["N_NAME"].Value.ToString();
            string email = selectedRow.Cells["N_EMAIL"].Value.ToString();
            Logo = (byte[])selectedRow.Cells["Logo"].Value;

            // Display the retrieved data in the respective textboxes
            textBox1.Text = nomineeName;
            textBox3.Text = email;
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
                // Retrieve the updated values from the textboxes
                string updatedNomineeName = textBox1.Text;
                string updatedEmail = textBox3.Text;

                // Check if all fields are filled before proceeding
                if (string.IsNullOrWhiteSpace(updatedNomineeName) || string.IsNullOrWhiteSpace(updatedEmail))
                {
                    MessageBox.Show("Please fill all fields!!");
                    return;
                }

                // Use the retrieved identifier to update the NOMINEE table
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "UPDATE NOMINEE SET N_NAME = @NomineeName, N_EMAIL = @Email, LOGO = @Logo WHERE N_IDENTIFIER = @NIdentifier";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@NomineeName", updatedNomineeName);
                    command.Parameters.AddWithValue("@Email", updatedEmail);
                    command.Parameters.AddWithValue("@NIdentifier", nIdentifier);
                    command.Parameters.AddWithValue("@Logo", Logo);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Nominee updated successfully.");
                    }
                    else
                    {
                        MessageBox.Show("No rows updated. Something went wrong!!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}

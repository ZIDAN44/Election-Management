using System.Data.SqlClient;

namespace ElectionApp.Admin.Party
{
    public partial class P_Modify : UserControl
    {
        private string adminID;
        private string connectionString;
        private string pIdentifier;
        private byte[] Logo;

        public P_Modify(string adminID, string connectionString)
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
            pIdentifier = selectedRow.Cells["P_IDENTIFIER"].Value.ToString();
            string partyName = selectedRow.Cells["P_NAME"].Value.ToString();
            Logo = (byte[])selectedRow.Cells["Logo"].Value;

            // Display the retrieved data in the respective textboxes
            textBox1.Text = partyName;
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
                string updatedPartyName = textBox1.Text;

                // Check if all fields are filled before proceeding
                if (string.IsNullOrWhiteSpace(updatedPartyName))
                {
                    MessageBox.Show("Please fill all fields!!");
                    return;
                }

                // Use the retrieved identifier to update the PARTY table
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "UPDATE PARTY SET P_NAME = @PartyName, LOGO = @Logo WHERE P_IDENTIFIER = @PIdentifier";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@PartyName", updatedPartyName);
                    command.Parameters.AddWithValue("@PIdentifier", pIdentifier);
                    command.Parameters.AddWithValue("@Logo", Logo);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data updated successfully!");
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
        }
    }
}

using System.Data.SqlClient;

namespace ElectionApp.Admin.Party
{
    public partial class P_Add : UserControl
    {
        private byte[] Logo;
        public P_Add(string adminID, string connectionString)
        {
            AdminID = adminID;
            ConnectionString = connectionString;
            InitializeComponent();
        }

        private string AdminID { get; set; }

        private string ConnectionString { get; set; }

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
                string partyName = textBox1.Text;

                // Check if all fields are filled before proceeding
                if (string.IsNullOrWhiteSpace(partyName) || Logo == null)
                {
                    MessageBox.Show("Please fill name and upload the logo.");
                    return;
                }

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO PARTY (P_NAME, LOGO) " +
                                   "VALUES (@PartyName, @Logo)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@PartyName", partyName);
                    command.Parameters.AddWithValue("@Logo", Logo);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Party added successfully!");
                    }
                    else
                    {
                        MessageBox.Show("No data added. Something went wrong!!");
                    }
                }

                textBox1.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}

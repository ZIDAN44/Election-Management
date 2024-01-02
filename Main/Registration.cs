using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace ElectionApp.Main
{
    public partial class Registration : Form
    {
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        private byte[] Pic;

        public Registration(string connectionString)
        {
            ConnectionString = connectionString;
            InitializeComponent();
            textBox1.Select();
        }

        private string ConnectionString { get; set; }

        private void PerformVoterRegistration()
        {
            try
            {
                // Push data to database
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO VOTER (V_NAME, V_EMAIL, PIC) " +
                                   "VALUES (@VName, @Email, @Pic)";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@VName", textBox1.Text);
                    command.Parameters.AddWithValue("@Email", textBox2.Text);
                    command.Parameters.AddWithValue("@Pic", Pic);

                    command.ExecuteNonQuery();

                    MessageBox.Show("Data uploaded successfully.");
                }

                // After uploading data to VOTER table, retrieve V_IDENTIFIER
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Get the last inserted V_IDENTIFIER
                    string getIdQuery = "SELECT TOP 1 V_IDENTIFIER FROM VOTER ORDER BY NID DESC";
                    SqlCommand getIdCommand = new SqlCommand(getIdQuery, connection);
                    string voterId = getIdCommand.ExecuteScalar()?.ToString();

                    // Insert V_IDENTIFIER into the LOGIN table
                    if (!string.IsNullOrEmpty(voterId))
                    {
                        string password = textBox3.Text;
                        string role = "voter_temp";

                        string insertLoginQuery = "INSERT INTO LOGIN (UID, PASSWORD, ROLE) VALUES (@uid, @password, @role)";
                        SqlCommand insertLoginCommand = new SqlCommand(insertLoginQuery, connection);
                        insertLoginCommand.Parameters.AddWithValue("@uid", voterId);
                        insertLoginCommand.Parameters.AddWithValue("@password", password);
                        insertLoginCommand.Parameters.AddWithValue("@role", role);
                        insertLoginCommand.ExecuteNonQuery();

                        MessageBox.Show($"Login data inserted successfully.\nVoter ID: {voterId}\nPassword: {password}");
                    }
                    else
                    {
                        MessageBox.Show("No voter identifier found.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void PerformNomineeRegistration()
        {
            try
            {
                // Push data to database using the provided query
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO NOMINEE (N_NAME, N_EMAIL, LOGO) " +
                                   "VALUES (@nName, @nEmail, @logo)";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@nName", textBox1.Text);
                    command.Parameters.AddWithValue("@nEmail", textBox2.Text);
                    command.Parameters.AddWithValue("@logo", Pic);

                    command.ExecuteNonQuery();

                    MessageBox.Show("Data uploaded successfully.");
                }

                // After uploading data to NOMINEE table, retrieve N_IDENTIFIER
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Get the last inserted N_IDENTIFIER
                    string getIdQuery = "SELECT TOP 1 N_IDENTIFIER FROM NOMINEE ORDER BY NOM_ID DESC";
                    SqlCommand getIdCommand = new SqlCommand(getIdQuery, connection);
                    string nomineeId = getIdCommand.ExecuteScalar()?.ToString();

                    // Insert N_IDENTIFIER into the LOGIN table
                    if (!string.IsNullOrEmpty(nomineeId))
                    {
                        string password = textBox3.Text;
                        string role = "nominee_temp";

                        string insertLoginQuery = "INSERT INTO LOGIN (UID, PASSWORD, ROLE) VALUES (@uid, @password, @role)";
                        SqlCommand insertLoginCommand = new SqlCommand(insertLoginQuery, connection);
                        insertLoginCommand.Parameters.AddWithValue("@uid", nomineeId);
                        insertLoginCommand.Parameters.AddWithValue("@password", password);
                        insertLoginCommand.Parameters.AddWithValue("@role", role);
                        insertLoginCommand.ExecuteNonQuery();

                        MessageBox.Show($"Login data inserted successfully.\nNominee ID: {nomineeId}\nPassword: {password}");
                    }
                    else
                    {
                        MessageBox.Show("No nominee identifier found.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2CircleButton2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2GradientPanel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Username")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Username";
                textBox1.ForeColor = Color.Silver;
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "Email")
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black;
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = "Email";
                textBox2.ForeColor = Color.Silver;
            }
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            if (textBox3.Text == "Password")
            {
                textBox3.Text = "";
                textBox3.ForeColor = Color.Black;
            }
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                textBox3.Text = "Password";
                textBox3.ForeColor = Color.Silver;
            }
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            if (textBox4.Text == "Confirm Password")
            {
                textBox4.Text = "";
                textBox4.ForeColor = Color.Black;
            }
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            if (textBox4.Text == "")
            {
                textBox4.Text = "Confirm Password";
                textBox4.ForeColor = Color.Silver;
            }
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

        private void button2_Click(object sender, EventArgs e)
        {
            // Check if any of the text boxes are empty or contain placeholder text
            if (string.IsNullOrWhiteSpace(textBox1.Text) || textBox1.Text == "Username" ||
                string.IsNullOrWhiteSpace(textBox2.Text) || textBox2.Text == "Email" ||
                string.IsNullOrWhiteSpace(textBox3.Text) || textBox3.Text == "Password" ||
                string.IsNullOrWhiteSpace(textBox4.Text) || textBox4.Text == "Confirm Password")
            {
                MessageBox.Show("Please fill in all the required fields!!");
                return;
            }

            // Check if passwords match
            if (textBox3.Text != textBox4.Text)
            {
                MessageBox.Show("Passwords do not match!!");
                return;
            }

            if (Pic == null)
            {
                MessageBox.Show("Please upload picture!!");
                return;
            }

            if (guna2ComboBox1.SelectedItem != null)
            {
                string role = guna2ComboBox1.SelectedItem.ToString().ToLower();

                if (role == "voter")
                {
                    PerformVoterRegistration();
                }
                else if (role == "nominee")
                {
                    PerformNomineeRegistration();
                }
            }
            else
            {
                MessageBox.Show("Please select a role (Voter or Nominee)!!");
            }
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            Pic = null;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainPage mainPage = new MainPage(ConnectionString);
            mainPage.Show();
            Hide();
        }
    }
}

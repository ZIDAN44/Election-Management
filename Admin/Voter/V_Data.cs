using ElectionApp.Common;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ElectionApp.Admin.Voter
{
    public partial class V_Data : UserControl
    {
        private string adminID;
        private string connectionString;
        private SqlDataAdapter dataAdapter;
        private DataSet dataSet;

        public V_Data(string adminID, string connectionString)
        {
            AdminID = adminID;
            ConnectionString = connectionString;
            InitializeComponent();
            LoadVoterData();
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

        private void LoadVoterData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT V_IDENTIFIER, V_NAME, V_EMAIL, HAS_VOTE FROM VOTER";
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Remove any existing columns before binding data to avoid duplicates
                        dataGridView1.Columns.Clear();

                        // Bind the DataTable to the DataGridView
                        dataGridView1.DataSource = dataTable;

                        // Adjust column headers and other configurations as needed
                        dataGridView1.Columns["V_IDENTIFIER"].HeaderText = "NID";
                        dataGridView1.Columns["V_NAME"].HeaderText = "Voter Name";
                        dataGridView1.Columns["V_EMAIL"].HeaderText = "Email";

                        // Handle "Click to Show" in the PIC column
                        DataGridViewButtonColumn picColumn = new DataGridViewButtonColumn();
                        picColumn.Name = "PIC";
                        picColumn.HeaderText = "Picture";
                        picColumn.Text = "Click to Show";
                        picColumn.UseColumnTextForButtonValue = true;
                        dataGridView1.Columns.Insert(3, picColumn);

                        dataGridView1.Columns["HAS_VOTE"].DisplayIndex = 4;
                        dataGridView1.Columns["HAS_VOTE"].HeaderText = "Voting Status";

                        // Event handler for CellClick event to handle opening documents
                        dataGridView1.CellClick += dataGridView1_CellClick;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked cell is in the PIC column and it's a "Click to Show" text
            if (e.ColumnIndex == dataGridView1.Columns["PIC"].Index && e.RowIndex >= 0 &&
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Click to Show")
            {
                string vIdentifier = dataGridView1.Rows[e.RowIndex].Cells["V_IDENTIFIER"].Value.ToString();
                ShowPicture(vIdentifier);
            }
        }

        private void ShowPicture(string vIdentifier)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT PIC FROM VOTER WHERE V_IDENTIFIER = @VIdentifier";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@VIdentifier", vIdentifier);

                        object result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            byte[] imageData = (byte[])result;

                            // Save the image to a temporary file
                            string tempImagePath = Path.GetTempFileName() + ".jpg";
                            File.WriteAllBytes(tempImagePath, imageData);

                            // Open the image in a separate window using the associated application
                            ProcessStartInfo startInfo = new ProcessStartInfo(tempImagePath);
                            startInfo.UseShellExecute = true;
                            Process.Start(startInfo);
                        }
                        else
                        {
                            MessageBox.Show("No picture found for this voter.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void SearchVoter(string searchTerm)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT V_IDENTIFIER, V_NAME, V_EMAIL " +
                                   "FROM VOTER " +
                                   "WHERE V_NAME LIKE @searchTerm OR " +
                                   "V_EMAIL LIKE @searchTerm OR " +
                                   "V_IDENTIFIER LIKE @searchTerm";

                    dataAdapter = new SqlDataAdapter(query, connection);
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");

                    dataSet = new DataSet();
                    dataAdapter.Fill(dataSet, "VOTER");
                    dataGridView1.DataSource = dataSet.Tables["VOTER"];
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void UpdateVoterTemp(string vIdentifier)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    string updateQuery = "UPDATE VOTER_TEMP SET APRV = NULL WHERE APRV_NID = @vIdentifier";
                    SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@vIdentifier", vIdentifier);
                    updateCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating VOTER_TEMP: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void RemoveVoter(string vID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    string removeRegulatesQuery = "DELETE FROM VOTER WHERE V_IDENTIFIER = @vID";
                    SqlCommand removeRegulatesCommand = new SqlCommand(removeRegulatesQuery, connection);
                    removeRegulatesCommand.Parameters.AddWithValue("@vID", vID);
                    removeRegulatesCommand.ExecuteNonQuery();

                    LoadVoterData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private bool CheckUserExistsInVoterTemp(string vIdentifier)
        {
            bool userExists = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT TOP 1 1 FROM VOTER_TEMP WHERE APRV_NID = @vIdentifier";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@vIdentifier", vIdentifier);

                        object result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            userExists = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error checking VOTER_TEMP: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            return userExists;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string searchTerm = textBox1.Text.Trim();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                SearchVoter(searchTerm);
            }
            else
            {
                LoadVoterData();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Prompt user for removal confirmation and reasons
                DialogResult result = MessageBox.Show("Are you sure you want to remove the user?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                if (result == DialogResult.OK)
                {
                    string reason = Reject.PromptReason();

                    if (!string.IsNullOrEmpty(reason))
                    {
                        try
                        {
                            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                            string vIdentifier = selectedRow.Cells["V_IDENTIFIER"].Value.ToString();

                            // Insert data into REJECTIONS table
                            Reject.InsertIntoRejections(vIdentifier, reason, adminID, ConnectionString);

                            // Check if the user exists in VOTER_TEMP table
                            bool userExistsInVoterTemp = CheckUserExistsInVoterTemp(vIdentifier);

                            if (userExistsInVoterTemp)
                            {
                                // Update VOTER_TEMP table
                                UpdateVoterTemp(vIdentifier);
                            }

                            // Remove user from VOTER table
                            RemoveVoter(vIdentifier);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please provide a reason for removal.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a row to remove.");
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string searchTerm = textBox1.Text.Trim();
                SearchVoter(searchTerm);
            }
        }
    }
}

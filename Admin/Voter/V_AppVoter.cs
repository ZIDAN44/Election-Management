using ElectionApp.Common;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ElectionApp.Admin.Voter
{
    public partial class V_AppVoter : UserControl
    {
        private SqlDataAdapter dataAdapter;
        private DataSet dataSet;

        public V_AppVoter(string adminID, string connectionString)
        {
            AdminID = adminID;
            ConnectionString = connectionString;
            InitializeComponent();
            LoadVoterTempData();
        }

        private string AdminID { get; set; }

        private string ConnectionString { get; set; }

        private void LoadVoterTempData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT V_IDENTIFIER, V_NAME, V_EMAIL, IS_APROV FROM VOTER";
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Remove any existing columns before binding data to avoid duplicates
                        dataGridView1.Columns.Clear();

                        // Bind the DataTable to the DataGridView
                        dataGridView1.DataSource = dataTable;

                        // Adjust column headers and other configurations as needed
                        dataGridView1.Columns["V_IDENTIFIER"].HeaderText = "Voter ID";
                        dataGridView1.Columns["V_NAME"].HeaderText = "Voter Name";
                        dataGridView1.Columns["V_EMAIL"].HeaderText = "Email";

                        // Handle "Click to Show" in the PIC column
                        DataGridViewButtonColumn picColumn = new DataGridViewButtonColumn();
                        picColumn.Name = "PIC";
                        picColumn.HeaderText = "Picture";
                        picColumn.Text = "Click to Show";
                        picColumn.UseColumnTextForButtonValue = true;
                        dataGridView1.Columns.Insert(3, picColumn);

                        // Update the Approval Status and NID columns' display indices
                        dataGridView1.Columns["IS_APROV"].DisplayIndex = 4;

                        dataGridView1.Columns["IS_APROV"].HeaderText = "Approval Status";

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

        private void UpdateVoterApprovalRoleAndInsertRegistrar()
        {
            // Check if there's a selected row in dataGridView1
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the V_IDENTIFIER from the selected row in dataGridView1
                string vIdentifier = dataGridView1.SelectedRows[0].Cells["V_IDENTIFIER"].Value.ToString();

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    try
                    {
                        connection.Open();

                        // Update VOTER's IS_APROV to 1 for the fetched V_IDENTIFIER
                        string updateIS_APROVQuery = "UPDATE VOTER SET IS_APROV = 1 WHERE V_IDENTIFIER = @VIdentifier";
                        using (SqlCommand updateIS_APROVCommand = new SqlCommand(updateIS_APROVQuery, connection))
                        {
                            updateIS_APROVCommand.Parameters.AddWithValue("@VIdentifier", vIdentifier);
                            updateIS_APROVCommand.ExecuteNonQuery();
                        }

                        // Update ROLE to 'voter' in LOGIN table for the fetched V_IDENTIFIER
                        string updateRoleQuery = "UPDATE LOGIN SET ROLE = 'voter' WHERE UID = @VIdentifier";
                        using (SqlCommand updateRoleCommand = new SqlCommand(updateRoleQuery, connection))
                        {
                            updateRoleCommand.Parameters.AddWithValue("@VIdentifier", vIdentifier);
                            updateRoleCommand.ExecuteNonQuery();
                        }

                        // Insert into REGISTRARS table
                        string insertRegistrarQuery = "INSERT INTO REGISTRARS (USER_ID, ADMIN_ID, ROLE) " +
                                                      "VALUES(@VoterID, @AdminID, @Role)";

                        using (SqlCommand insertRegistrarCommand = new SqlCommand(insertRegistrarQuery, connection))
                        {
                            insertRegistrarCommand.Parameters.AddWithValue("@VoterID", vIdentifier);
                            insertRegistrarCommand.Parameters.AddWithValue("@AdminID", AdminID); // Assuming AdminID is defined elsewhere
                            insertRegistrarCommand.Parameters.AddWithValue("@Role", "voter");
                            insertRegistrarCommand.ExecuteNonQuery();
                        }

                        MessageBox.Show("Voter approval & role updated successfully!");
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
            else
            {
                MessageBox.Show("Please select a row to update voter approval & role");
            }
        }

        private void ApproveRegistration()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                int selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
                string vIdentifier = dataGridView1.Rows[selectedRowIndex].Cells["V_IDENTIFIER"].Value.ToString();
                object cellValue = dataGridView1.Rows[selectedRowIndex].Cells["IS_APROV"].Value;
                bool isApproved = cellValue != DBNull.Value && Convert.ToBoolean(cellValue);

                // Check if the selected entry is already approved
                if (isApproved)
                {
                    MessageBox.Show("This registration is already approved.");
                    return;
                }

                // Update Voter & Login table also insert into REGISTRARS table
                UpdateVoterApprovalRoleAndInsertRegistrar();

                // Refresh the DataGridView
                LoadVoterTempData();
            }
        }

        private void RejectRegistration()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
                string vIdentifier = dataGridView1.Rows[selectedRowIndex].Cells["V_IDENTIFIER"].Value.ToString();

                object cellValue = dataGridView1.Rows[selectedRowIndex].Cells["IS_APROV"].Value;
                bool isRejected = (cellValue == DBNull.Value);

                if (isRejected)
                {
                    MessageBox.Show("This registration is already rejected.");
                    return;
                }

                var confirmResult = MessageBox.Show("Are you sure you want to reject this registration?", "Confirm Rejection", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    try
                    {
                        string reason = Reject.PromptReason();

                        if (!string.IsNullOrEmpty(reason))
                        {
                            // Call Reject.InsertIntoRejections for REJECTIONS table insertion
                            Reject.InsertIntoRejections(vIdentifier, reason, AdminID, ConnectionString);

                            using (SqlConnection connection = new SqlConnection(ConnectionString))
                            {
                                connection.Open();
                                string updateAPRVQuery = "UPDATE VOTER SET IS_APROV = NULL WHERE V_IDENTIFIER = @VIdentifier";
                                using (SqlCommand updateAPRVCommand = new SqlCommand(updateAPRVQuery, connection))
                                {
                                    updateAPRVCommand.Parameters.AddWithValue("@VIdentifier", vIdentifier);
                                    updateAPRVCommand.ExecuteNonQuery();

                                    // Refresh the DataGridView
                                    LoadVoterTempData();
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please provide a reason for rejection.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a row to reject.");
            }
        }

        private void SearchTVoter(string searchTerm)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT V_IDENTIFIER, V_NAME, V_EMAIL, IS_APROV " +
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

        private void button1_Click(object sender, EventArgs e)
        {
            ApproveRegistration();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RejectRegistration();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string searchTerm = textBox1.Text.Trim();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                SearchTVoter(searchTerm);
            }
            else
            {
                LoadVoterTempData();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string searchTerm = textBox1.Text.Trim();
                SearchTVoter(searchTerm);
            }
        }
    }
}

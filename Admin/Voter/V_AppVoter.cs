using ElectionApp.Common;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ElectionApp.Admin.Voter
{
    public partial class V_AppVoter : UserControl
    {
        private string adminID;
        private string connectionString;
        private SqlDataAdapter dataAdapter;
        private DataSet dataSet;

        public V_AppVoter(string adminID, string connectionString)
        {
            AdminID = adminID;
            ConnectionString = connectionString;
            InitializeComponent();
            LoadVoterTempData();
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

        private void LoadVoterTempData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT V_IDENTIFIER, V_NAME, V_EMAIL, APRV, APRV_NID FROM VOTER_TEMP";
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Remove any existing columns before binding data to avoid duplicates
                        dataGridView1.Columns.Clear();

                        // Bind the DataTable to the DataGridView
                        dataGridView1.DataSource = dataTable;

                        // Adjust column headers and other configurations as needed
                        dataGridView1.Columns["V_IDENTIFIER"].HeaderText = "TEMP ID";
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
                        dataGridView1.Columns["APRV"].DisplayIndex = 4;
                        dataGridView1.Columns["APRV_NID"].DisplayIndex = 5;

                        dataGridView1.Columns["APRV"].HeaderText = "Approval Status";
                        dataGridView1.Columns["APRV_NID"].HeaderText = "New ID";

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

                    string query = "SELECT PIC FROM VOTER_TEMP WHERE V_IDENTIFIER = @VIdentifier";
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

        private void ApproveRegistration()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                int selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
                string vIdentifier = dataGridView1.Rows[selectedRowIndex].Cells["V_IDENTIFIER"].Value.ToString();
                object cellValue = dataGridView1.Rows[selectedRowIndex].Cells["APRV"].Value;
                bool isApproved = cellValue != DBNull.Value && Convert.ToBoolean(cellValue);

                // Check if the selected entry is already approved
                if (isApproved)
                {
                    MessageBox.Show("This registration is already approved.");
                    return;
                }

                // Check if the selected entry was banned before but is pending for approval again
                string checkBannedQuery = "SELECT APRV_NID FROM VOTER_TEMP WHERE V_IDENTIFIER = @VIdentifier AND APRV IS NULL";
                using (SqlCommand checkBannedCommand = new SqlCommand(checkBannedQuery, connection))
                {
                    checkBannedCommand.Parameters.AddWithValue("@VIdentifier", vIdentifier);
                    object bannedResult = checkBannedCommand.ExecuteScalar();
                    string bannedVoterID = (bannedResult != null) ? bannedResult.ToString() : null;

                    if (!string.IsNullOrEmpty(bannedVoterID))
                    {
                        var confirmResult = MessageBox.Show("This user was banned before! Approving again will generate a new ID. Proceed?", "Confirm Reset Approval", MessageBoxButtons.YesNo);
                        if (confirmResult == DialogResult.Yes)
                        {
                            // Reset the approval status by inserting into VOTER table
                            string insertVoterQuery = "INSERT INTO VOTER (V_NAME, V_EMAIL, PIC) " +
                                                        "OUTPUT INSERTED.V_IDENTIFIER " +
                                                        "SELECT V_NAME, V_EMAIL, PIC FROM VOTER_TEMP WHERE V_IDENTIFIER = @VIdentifier";

                            using (SqlCommand insertVoterCommand = new SqlCommand(insertVoterQuery, connection))
                            {
                                insertVoterCommand.Parameters.AddWithValue("@VIdentifier", vIdentifier);
                                var result = insertVoterCommand.ExecuteScalar();
                                string newVoterID = (result != null) ? result.ToString() : null;

                                if (!string.IsNullOrEmpty(newVoterID))
                                {
                                    // Update VOTER_TEMP's APRV_NID with the newly created V_IDENTIFIER (VT-*)
                                    string updateAPRVNIDQuery = "UPDATE VOTER_TEMP SET APRV = 1, APRV_NID = @NewVoterID WHERE V_IDENTIFIER = @VIdentifier";

                                    using (SqlCommand updateAPRVNIDCommand = new SqlCommand(updateAPRVNIDQuery, connection))
                                    {
                                        updateAPRVNIDCommand.Parameters.AddWithValue("@NewVoterID", newVoterID);
                                        updateAPRVNIDCommand.Parameters.AddWithValue("@VIdentifier", vIdentifier);
                                        updateAPRVNIDCommand.ExecuteNonQuery();
                                    }

                                    // Retrieve the password associated with the TVT-* ID from LOGIN table
                                    string getPasswordQuery = "SELECT PASSWORD FROM LOGIN WHERE UID = @UID";
                                    using (SqlCommand getPasswordCommand = new SqlCommand(getPasswordQuery, connection))
                                    {
                                        getPasswordCommand.Parameters.AddWithValue("@UID", vIdentifier);
                                        object passwordResult = getPasswordCommand.ExecuteScalar();
                                        string password = (passwordResult != null) ? passwordResult.ToString() : null;

                                        // Insert into LOGIN table using newVoterID (VT-*) as UID and retrieved password
                                        string insertLoginQuery = "INSERT INTO LOGIN (UID, PASSWORD, ROLE) " +
                                                                  "VALUES (@UID, @Password, @Role)";

                                        using (SqlCommand insertLoginCommand = new SqlCommand(insertLoginQuery, connection))
                                        {
                                            insertLoginCommand.Parameters.AddWithValue("@UID", newVoterID);
                                            insertLoginCommand.Parameters.AddWithValue("@Password", password);
                                            insertLoginCommand.Parameters.AddWithValue("@Role", "voter");
                                            insertLoginCommand.ExecuteNonQuery();
                                        }
                                    }

                                    // Refresh the DataGridView
                                    LoadVoterTempData();
                                }
                            }
                        }
                        else
                        {
                            // If the user chooses 'No', stop the process
                            return;
                        }
                    }
                    else
                    {
                        // Copy data to VOTER table and retrieve the newly inserted V_IDENTIFIER (VT-*)
                        string insertVoterQuery = "INSERT INTO VOTER (V_NAME, V_EMAIL, PIC) " +
                                                  "OUTPUT INSERTED.V_IDENTIFIER " +
                                                  "SELECT V_NAME, V_EMAIL, PIC FROM VOTER_TEMP WHERE V_IDENTIFIER = @VIdentifier";

                        using (SqlCommand insertVoterCommand = new SqlCommand(insertVoterQuery, connection))
                        {
                            insertVoterCommand.Parameters.AddWithValue("@VIdentifier", vIdentifier);
                            var result = insertVoterCommand.ExecuteScalar();
                            string newVoterID = (result != null) ? result.ToString() : null;

                            if (!string.IsNullOrEmpty(newVoterID))
                            {
                                // Update VOTER_TEMP's APRV_NID with the newly created V_IDENTIFIER (VT-*)
                                string updateAPRVNIDQuery = "UPDATE VOTER_TEMP SET APRV = 1, APRV_NID = @NewVoterID WHERE V_IDENTIFIER = @VIdentifier";

                                using (SqlCommand updateAPRVNIDCommand = new SqlCommand(updateAPRVNIDQuery, connection))
                                {
                                    updateAPRVNIDCommand.Parameters.AddWithValue("@NewVoterID", newVoterID);
                                    updateAPRVNIDCommand.Parameters.AddWithValue("@VIdentifier", vIdentifier);
                                    updateAPRVNIDCommand.ExecuteNonQuery();
                                }

                                // Retrieve the password associated with the TVT-* ID from LOGIN table
                                string getPasswordQuery = "SELECT PASSWORD FROM LOGIN WHERE UID = @UID";
                                using (SqlCommand getPasswordCommand = new SqlCommand(getPasswordQuery, connection))
                                {
                                    getPasswordCommand.Parameters.AddWithValue("@UID", vIdentifier);
                                    object passwordResult = getPasswordCommand.ExecuteScalar();
                                    string password = (passwordResult != null) ? passwordResult.ToString() : null;

                                    // Insert into LOGIN table using newVoterID (VT-*) as UID and retrieved password
                                    string insertLoginQuery = "INSERT INTO LOGIN (UID, PASSWORD, ROLE) " +
                                                              "VALUES (@UID, @Password, @Role)";

                                    using (SqlCommand insertLoginCommand = new SqlCommand(insertLoginQuery, connection))
                                    {
                                        insertLoginCommand.Parameters.AddWithValue("@UID", newVoterID);
                                        insertLoginCommand.Parameters.AddWithValue("@Password", password);
                                        insertLoginCommand.Parameters.AddWithValue("@Role", "voter");
                                        insertLoginCommand.ExecuteNonQuery();
                                    }
                                }

                                // Insert into REGISTRARS table
                                string insertRegistrarQuery = "INSERT INTO REGISTRARS (USER_ID, ADMIN_ID, ROLE) " +
                                                              "SELECT @VoterID, @AdminID, @Role " +
                                                              "FROM VOTER_TEMP WHERE V_IDENTIFIER = @VIdentifier";

                                using (SqlCommand insertRegistrarCommand = new SqlCommand(insertRegistrarQuery, connection))
                                {
                                    insertRegistrarCommand.Parameters.AddWithValue("@VoterID", newVoterID);
                                    insertRegistrarCommand.Parameters.AddWithValue("@AdminID", AdminID);
                                    insertRegistrarCommand.Parameters.AddWithValue("@Role", "voter");
                                    insertRegistrarCommand.Parameters.AddWithValue("@VIdentifier", vIdentifier);
                                    insertRegistrarCommand.ExecuteNonQuery();
                                }
                            }
                        }

                        // Refresh the DataGridView
                        LoadVoterTempData();
                    }
                }
            }
        }

        private void RejectRegistration()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
                string vIdentifier = dataGridView1.Rows[selectedRowIndex].Cells["V_IDENTIFIER"].Value.ToString();

                object cellValue = dataGridView1.Rows[selectedRowIndex].Cells["APRV"].Value;
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
                            Reject.InsertIntoRejections(vIdentifier, reason, adminID, ConnectionString);

                            using (SqlConnection connection = new SqlConnection(ConnectionString))
                            {
                                connection.Open();
                                string updateAPRVQuery = "UPDATE VOTER_TEMP SET APRV = NULL WHERE V_IDENTIFIER = @VIdentifier";
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
                    string query = "SELECT V_IDENTIFIER, V_NAME, V_EMAIL, APRV, APRV_NID " +
                                   "FROM VOTER_TEMP " +
                                   "WHERE V_NAME LIKE @searchTerm OR " +
                                   "V_EMAIL LIKE @searchTerm OR " +
                                   "APRV_NID LIKE @searchTerm OR " +
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

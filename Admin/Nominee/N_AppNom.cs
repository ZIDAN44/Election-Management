using ElectionApp.Common;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ElectionApp.Admin.Nominee
{
    public partial class N_AppNom : UserControl
    {
        private string adminID;
        private string connectionString;
        private SqlDataAdapter dataAdapter;
        private DataSet dataSet;

        public N_AppNom(string adminID, string connectionString)
        {
            AdminID = adminID;
            ConnectionString = connectionString;
            InitializeComponent();
            LoadNomineeTempData();
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

        private void LoadNomineeTempData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT N_IDENTIFIER, 'Click to Show' AS LogoLink, LOGO, N_NAME, N_EMAIL, APRV, APRV_NOM_ID FROM NOMINEE_TEMP";
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Handle "Click to Show" in the LOGO column
                        DataGridViewButtonColumn picColumn = new DataGridViewButtonColumn();
                        picColumn.HeaderText = "LOGO";
                        picColumn.Name = "LOGO Link";
                        picColumn.DataPropertyName = "LOGO Link";
                        picColumn.UseColumnTextForButtonValue = true;
                        dataGridView1.Columns.Add(picColumn);

                        // Remove any existing columns before binding data to avoid duplicates
                        dataGridView1.Columns.Clear();

                        // Bind the DataTable to the DataGridView
                        dataGridView1.DataSource = dataTable;

                        // Adjust column headers and other configurations as needed
                        dataGridView1.Columns["N_IDENTIFIER"].HeaderText = "TEMP ID";
                        dataGridView1.Columns["N_NAME"].HeaderText = "Nominee Name";
                        dataGridView1.Columns["N_EMAIL"].HeaderText = "Email";
                        dataGridView1.Columns["APRV"].HeaderText = "Approval Status";
                        dataGridView1.Columns["APRV_NOM_ID"].HeaderText = "NOMID";

                        // Hide column (optional)
                        dataGridView1.Columns["LOGO"].Visible = false;

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
            // Check if the clicked cell is in the LOGO column and it's a "LogoLink" text
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["LogoLink"].Index)
            {
                string nIdentifier = dataGridView1.Rows[e.RowIndex].Cells["N_IDENTIFIER"].Value.ToString();
                ShowPicture(nIdentifier);
            }
        }

        private void ShowPicture(string nIdentifier)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT LOGO FROM NOMINEE_TEMP WHERE N_IDENTIFIER = @NIdentifier";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NIdentifier", nIdentifier);

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
                            MessageBox.Show("No logo found for this nominee.");
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
                string nIdentifier = dataGridView1.Rows[selectedRowIndex].Cells["N_IDENTIFIER"].Value.ToString();
                object cellValue = dataGridView1.Rows[selectedRowIndex].Cells["APRV"].Value;
                bool isApproved = cellValue != DBNull.Value && Convert.ToBoolean(cellValue);

                // Check if the selected entry is already approved
                if (isApproved)
                {
                    MessageBox.Show("This registration is already approved.");
                    return;
                }

                // Check if the selected entry was banned before but is pending for approval again
                string checkBannedQuery = "SELECT APRV_NOM_ID FROM NOMINEE_TEMP WHERE N_IDENTIFIER = @NIdentifier AND APRV IS NULL";
                using (SqlCommand checkBannedCommand = new SqlCommand(checkBannedQuery, connection))
                {
                    checkBannedCommand.Parameters.AddWithValue("@NIdentifier", nIdentifier);
                    object bannedResult = checkBannedCommand.ExecuteScalar();
                    string bannedNomineeID = (bannedResult != null) ? bannedResult.ToString() : null;

                    if (!string.IsNullOrEmpty(bannedNomineeID))
                    {
                        var confirmResult = MessageBox.Show("This user was banned before! Approving again will generate a new ID. Proceed?", "Confirm Reset Approval", MessageBoxButtons.YesNo);
                        if (confirmResult == DialogResult.Yes)
                        {
                            // Reset the approval status by inserting into NOMINEE table
                            string insertNomineeQuery = "INSERT INTO NOMINEE (N_NAME, N_EMAIL, LOGO) " +
                                                        "OUTPUT INSERTED.N_IDENTIFIER " +
                                                        "SELECT N_NAME, N_EMAIL, LOGO FROM NOMINEE_TEMP WHERE N_IDENTIFIER = @NIdentifier";

                            using (SqlCommand insertNomineeCommand = new SqlCommand(insertNomineeQuery, connection))
                            {
                                insertNomineeCommand.Parameters.AddWithValue("@NIdentifier", nIdentifier);
                                var result = insertNomineeCommand.ExecuteScalar();
                                string newNomineeID = (result != null) ? result.ToString() : null;

                                if (!string.IsNullOrEmpty(newNomineeID))
                                {
                                    // Update NOMINEE_TEMP's APRV_NOM_ID with the newly created N_IDENTIFIER (NT-*)
                                    string updateAPRVNOMIDQuery = "UPDATE NOMINEE_TEMP SET APRV = 1, APRV_NOM_ID = @NewNomineeID WHERE N_IDENTIFIER = @NIdentifier";

                                    using (SqlCommand updateAPRVNOMIDCommand = new SqlCommand(updateAPRVNOMIDQuery, connection))
                                    {
                                        updateAPRVNOMIDCommand.Parameters.AddWithValue("@NewNomineeID", newNomineeID);
                                        updateAPRVNOMIDCommand.Parameters.AddWithValue("@NIdentifier", nIdentifier);
                                        updateAPRVNOMIDCommand.ExecuteNonQuery();
                                    }

                                    // Retrieve the password associated with the TNT-* ID from LOGIN table
                                    string getPasswordQuery = "SELECT PASSWORD FROM LOGIN WHERE UID = @UID";
                                    using (SqlCommand getPasswordCommand = new SqlCommand(getPasswordQuery, connection))
                                    {
                                        getPasswordCommand.Parameters.AddWithValue("@UID", nIdentifier);
                                        object passwordResult = getPasswordCommand.ExecuteScalar();
                                        string password = (passwordResult != null) ? passwordResult.ToString() : null;

                                        // Insert into LOGIN table using newNomineeID (NT-*) as UID and retrieved password
                                        string insertLoginQuery = "INSERT INTO LOGIN (UID, PASSWORD, ROLE) " +
                                                                  "VALUES (@UID, @Password, @Role)";

                                        using (SqlCommand insertLoginCommand = new SqlCommand(insertLoginQuery, connection))
                                        {
                                            insertLoginCommand.Parameters.AddWithValue("@UID", newNomineeID);
                                            insertLoginCommand.Parameters.AddWithValue("@Password", password);
                                            insertLoginCommand.Parameters.AddWithValue("@Role", "nominee");
                                            insertLoginCommand.ExecuteNonQuery();
                                        }
                                    }

                                    // Refresh the DataGridView
                                    LoadNomineeTempData();
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
                        // Copy data to NOMINEE table and retrieve the newly inserted N_IDENTIFIER (NT-*)
                        string insertNomineeQuery = "INSERT INTO NOMINEE (N_NAME, N_EMAIL, LOGO) " +
                                                  "OUTPUT INSERTED.N_IDENTIFIER " +
                                                  "SELECT N_NAME, N_EMAIL, LOGO FROM NOMINEE_TEMP WHERE N_IDENTIFIER = @NIdentifier";

                        using (SqlCommand insertNomineeCommand = new SqlCommand(insertNomineeQuery, connection))
                        {
                            insertNomineeCommand.Parameters.AddWithValue("@NIdentifier", nIdentifier);
                            var result = insertNomineeCommand.ExecuteScalar();
                            string newNomineeID = (result != null) ? result.ToString() : null;

                            if (!string.IsNullOrEmpty(newNomineeID))
                            {
                                // Update NOMINEE_TEMP's APRV_NOM_ID with the newly created N_IDENTIFIER (NT-*)
                                string updateAPRVNOMIDQuery = "UPDATE NOMINEE_TEMP SET APRV = 1, APRV_NOM_ID = @NewNomineeID WHERE N_IDENTIFIER = @NIdentifier";

                                using (SqlCommand updateAPRVNOMIDCommand = new SqlCommand(updateAPRVNOMIDQuery, connection))
                                {
                                    updateAPRVNOMIDCommand.Parameters.AddWithValue("@NewNomineeID", newNomineeID);
                                    updateAPRVNOMIDCommand.Parameters.AddWithValue("@NIdentifier", nIdentifier);
                                    updateAPRVNOMIDCommand.ExecuteNonQuery();
                                }

                                // Retrieve the password associated with the TNT-* ID from LOGIN table
                                string getPasswordQuery = "SELECT PASSWORD FROM LOGIN WHERE UID = @UID";
                                using (SqlCommand getPasswordCommand = new SqlCommand(getPasswordQuery, connection))
                                {
                                    getPasswordCommand.Parameters.AddWithValue("@UID", nIdentifier);
                                    object passwordResult = getPasswordCommand.ExecuteScalar();
                                    string password = (passwordResult != null) ? passwordResult.ToString() : null;

                                    // Insert into LOGIN table using newNomineeID (NT-*) as UID and retrieved password
                                    string insertLoginQuery = "INSERT INTO LOGIN (UID, PASSWORD, ROLE) " +
                                                              "VALUES (@UID, @Password, @Role)";

                                    using (SqlCommand insertLoginCommand = new SqlCommand(insertLoginQuery, connection))
                                    {
                                        insertLoginCommand.Parameters.AddWithValue("@UID", newNomineeID);
                                        insertLoginCommand.Parameters.AddWithValue("@Password", password);
                                        insertLoginCommand.Parameters.AddWithValue("@Role", "nominee");
                                        insertLoginCommand.ExecuteNonQuery();
                                    }
                                }

                                // Insert into REGISTRARS table
                                string insertRegistrarQuery = "INSERT INTO REGISTRARS (USER_ID, ADMIN_ID, ROLE) " +
                                                              "SELECT @NomineeID, @AdminID, @Role " +
                                                              "FROM NOMINEE_TEMP WHERE N_IDENTIFIER = @NIdentifier";

                                using (SqlCommand insertRegistrarCommand = new SqlCommand(insertRegistrarQuery, connection))
                                {
                                    insertRegistrarCommand.Parameters.AddWithValue("@NomineeID", newNomineeID);
                                    insertRegistrarCommand.Parameters.AddWithValue("@AdminID", AdminID);
                                    insertRegistrarCommand.Parameters.AddWithValue("@Role", "nominee");
                                    insertRegistrarCommand.Parameters.AddWithValue("@NIdentifier", nIdentifier);
                                    insertRegistrarCommand.ExecuteNonQuery();
                                }
                            }
                        }

                        // Refresh the DataGridView
                        LoadNomineeTempData();
                    }
                }
            }
        }

        private void RejectRegistration()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
                string nIdentifier = dataGridView1.Rows[selectedRowIndex].Cells["N_IDENTIFIER"].Value.ToString();

                object cellValue = dataGridView1.Rows[selectedRowIndex].Cells["APRV"].Value;
                bool isRejected = (cellValue == DBNull.Value);

                if (isRejected)
                {
                    MessageBox.Show("This registration is already rejected.");
                    return;
                }

                // Prompt user for rejection confirmation and reasons
                var confirmResult = MessageBox.Show("Are you sure you want to reject this registration?", "Confirm Rejection", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    try
                    {
                        string reason = Reject.PromptReason();

                        if (!string.IsNullOrEmpty(reason))
                        {
                            // Call Reject.InsertIntoRejections for REJECTIONS table insertion
                            Reject.InsertIntoRejections(nIdentifier, reason, adminID, ConnectionString);

                            // Update NOMINEE_TEMP table
                            using (SqlConnection connection = new SqlConnection(ConnectionString))
                            {
                                connection.Open();
                                string updateAPRVQuery = "UPDATE NOMINEE_TEMP SET APRV = NULL WHERE N_IDENTIFIER = @NIdentifier";
                                using (SqlCommand updateAPRVCommand = new SqlCommand(updateAPRVQuery, connection))
                                {
                                    updateAPRVCommand.Parameters.AddWithValue("@NIdentifier", nIdentifier);
                                    updateAPRVCommand.ExecuteNonQuery();

                                    // Refresh the DataGridView
                                    LoadNomineeTempData();
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

        private void SearchTNominee(string searchTerm)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT N_IDENTIFIER, 'Click to Show' AS LogoLink, LOGO, N_NAME, N_EMAIL, APRV, APRV_NOM_ID " +
                                   "FROM NOMINEE_TEMP " +
                                   "WHERE N_NAME LIKE @searchTerm OR " +
                                   "N_EMAIL LIKE @searchTerm OR " +
                                   "N_IDENTIFIER LIKE @searchTerm";

                    dataAdapter = new SqlDataAdapter(query, connection);
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");

                    dataSet = new DataSet();
                    dataAdapter.Fill(dataSet, "NOMINEE");
                    dataGridView1.DataSource = dataSet.Tables["NOMINEE"];
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
                SearchTNominee(searchTerm);
            }
            else
            {
                LoadNomineeTempData();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string searchTerm = textBox1.Text.Trim();
                SearchTNominee(searchTerm);
            }
        }
    }
}

using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ElectionApp.Admin.Election
{
    public partial class E_AppCan : UserControl
    {
        private SqlDataAdapter dataAdapter;
        private DataSet dataSet;

        public E_AppCan(string adminID, string connectionString)
        {
            AdminID = adminID;
            ConnectionString = connectionString;
            InitializeComponent();
            LoadNomineeData();
        }

        private string AdminID { get; set; }

        private string ConnectionString { get; set; }

        private void LoadNomineeData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Fetch NOMINEE_ID from PARTICIPATES table
                    string nomineeIdsQuery = "SELECT DISTINCT NOMINEE_ID FROM PARTICIPATES";
                    SqlCommand nomineeIdsCommand = new SqlCommand(nomineeIdsQuery, connection);
                    SqlDataReader nomineeIdsReader = nomineeIdsCommand.ExecuteReader();

                    List<string> nomineeIds = new List<string>();
                    while (nomineeIdsReader.Read())
                    {
                        nomineeIds.Add(nomineeIdsReader["NOMINEE_ID"].ToString());
                    }
                    nomineeIdsReader.Close();

                    // Create a new DataTable to store the merged data
                    DataTable dataTable = new DataTable();

                    foreach (string nomineeId in nomineeIds)
                    {
                        // Fetch data from NOMINEE, ELECTION, and PARTICIPATES tables based on NOMINEE_IDs and ELECTION_IDs
                        string nomineeQuery = "SELECT N.N_IDENTIFIER AS NOMINEE_ID, N.N_NAME, N.P_NAME, N.N_EMAIL, P.R_DOC, P.APRV, " +
                                               "E.E_IDENTIFIER, E.E_NAME, E.TYPE, E.S_DATE, E.E_DATE " +
                                               "FROM NOMINEE N " +
                                               "INNER JOIN PARTICIPATES P ON N.N_IDENTIFIER = P.NOMINEE_ID " +
                                               "INNER JOIN ELECTION E ON E.E_IDENTIFIER = P.ELECTION_ID " +
                                               "WHERE N.N_IDENTIFIER = @NomineeId";

                        SqlDataAdapter adapter = new SqlDataAdapter(nomineeQuery, connection);
                        adapter.SelectCommand.Parameters.AddWithValue("@NomineeId", nomineeId);
                        DataTable tempTable = new DataTable();
                        adapter.Fill(tempTable);

                        // Merge the retrieved data into the main DataTable
                        dataTable.Merge(tempTable);
                    }

                    // Clear existing columns before adding new ones to avoid duplicates
                    dataGridView1.Columns.Clear();

                    // Display the merged data in the DataGridView
                    dataGridView1.DataSource = dataTable;

                    // Adjust column headers and other configurations as needed
                    dataGridView1.Columns["E_IDENTIFIER"].HeaderText = "ELE ID";
                    dataGridView1.Columns["E_NAME"].HeaderText = "Election Name";
                    dataGridView1.Columns["TYPE"].HeaderText = "Type";
                    dataGridView1.Columns["S_DATE"].HeaderText = "Starting Date";
                    dataGridView1.Columns["E_DATE"].HeaderText = "Ending Date";
                    dataGridView1.Columns["NOMINEE_ID"].HeaderText = "NOM ID";
                    dataGridView1.Columns["N_NAME"].HeaderText = "Nominee Name";
                    dataGridView1.Columns["P_NAME"].HeaderText = "Party";
                    dataGridView1.Columns["APRV"].HeaderText = "Approval Status";

                    // Hide columns that might not need to be shown
                    dataGridView1.Columns["R_DOC"].Visible = false; // Hide R_DOC if it's not necessary to display

                    // Add the DataGridViewLinkColumn for the "DocLink" column
                    DataGridViewLinkColumn docLinkColumn = new DataGridViewLinkColumn();
                    docLinkColumn.HeaderText = "DOC";
                    docLinkColumn.Name = "DocLink";
                    docLinkColumn.Text = "Click";
                    docLinkColumn.UseColumnTextForLinkValue = true;
                    dataGridView1.Columns.Add(docLinkColumn);

                    // Rearrange columns to position APRV after DOC column
                    dataGridView1.Columns["APRV"].DisplayIndex = dataGridView1.Columns.Count - 1;

                    // Event handler for CellClick event to handle opening documents
                    dataGridView1.CellClick += dataGridView1_CellClick;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["DocLink"].Index)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        connection.Open();

                        string nIdentifier = dataGridView1.Rows[e.RowIndex].Cells["NOMINEE_ID"].Value.ToString();

                        string query = "SELECT R_DOC FROM PARTICIPATES WHERE NOMINEE_ID = @nIdentifier";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@nIdentifier", nIdentifier);

                        object result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            byte[] fileData = (byte[])result;

                            // Determine the file type by checking the magic number or other metadata
                            string fileExtension = IsPdfFile(fileData) ? ".pdf" : ".docx";

                            // Save the file to a temporary path
                            string tempFilePath = Path.GetTempFileName() + fileExtension;
                            File.WriteAllBytes(tempFilePath, fileData);

                            // Open the file using the associated application
                            ProcessStartInfo startInfo = new ProcessStartInfo();
                            startInfo.FileName = tempFilePath;
                            startInfo.UseShellExecute = true;
                            Process.Start(startInfo);
                        }
                        else
                        {
                            MessageBox.Show("No document found for the nominee.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private bool IsPdfFile(byte[] data)
        {
            // Check for the PDF signature in the first few bytes
            if (data.Length > 4 &&
                data[0] == 0x25 && data[1] == 0x50 && data[2] == 0x44 && data[3] == 0x46) // '%PDF'
            {
                return true;
            }
            return false;
        }

        private void SearchNominee(string searchTerm)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT N.N_IDENTIFIER AS NOMINEE_ID, N.N_NAME, N.P_NAME, N.N_EMAIL, P.R_DOC, P.APRV, " +
                                   "E.E_IDENTIFIER, E.E_NAME, E.TYPE, E.S_DATE, E.E_DATE " +
                                   "FROM NOMINEE N " +
                                   "INNER JOIN PARTICIPATES P ON N.N_IDENTIFIER = P.NOMINEE_ID " +
                                   "INNER JOIN ELECTION E ON E.E_IDENTIFIER = P.ELECTION_ID " +
                                   "WHERE N.N_NAME LIKE @searchTerm OR " +
                                   "N.P_NAME LIKE @searchTerm OR " +
                                   "N.N_EMAIL LIKE @searchTerm OR " +
                                   "N.N_IDENTIFIER LIKE @searchTerm OR " +
                                   "E.E_IDENTIFIER LIKE @searchTerm OR " +
                                   "E.E_NAME LIKE @searchTerm OR " +
                                   "E.TYPE LIKE @searchTerm";

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
            // Check if a row is selected in the DataGridView
            if (dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        connection.Open();

                        // Get the selected NOMINEE_ID from the DataGridView
                        string nomineeId = dataGridView1.SelectedRows[0].Cells["NOMINEE_ID"].Value.ToString();

                        // Check if the APRV is already 1 for the selected NOMINEE_ID
                        string checkQuery = "SELECT APRV FROM PARTICIPATES WHERE NOMINEE_ID = @NomineeId";
                        SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                        checkCommand.Parameters.AddWithValue("@NomineeId", nomineeId);

                        object result = checkCommand.ExecuteScalar();

                        if (result != null && result != DBNull.Value && Convert.ToInt32(result) == 1)
                        {
                            MessageBox.Show("This registration is already approved.");
                        }
                        else
                        {
                            // Check if the previous APRV was NULL (indicating rejection)
                            string checkNullQuery = "SELECT COUNT(*) FROM PARTICIPATES WHERE NOMINEE_ID = @NomineeId AND APRV IS NULL";
                            SqlCommand checkNullCommand = new SqlCommand(checkNullQuery, connection);
                            checkNullCommand.Parameters.AddWithValue("@NomineeId", nomineeId);

                            int nullCount = Convert.ToInt32(checkNullCommand.ExecuteScalar());

                            // Check if the previous APRV was 0 (indicating waiting status)
                            string checkWaitingQuery = "SELECT COUNT(*) FROM PARTICIPATES WHERE NOMINEE_ID = @NomineeId AND APRV = 0";
                            SqlCommand checkWaitingCommand = new SqlCommand(checkWaitingQuery, connection);
                            checkWaitingCommand.Parameters.AddWithValue("@NomineeId", nomineeId);

                            int waitingCount = Convert.ToInt32(checkWaitingCommand.ExecuteScalar());

                            if (nullCount > 0)
                            {
                                DialogResult dialogResult = MessageBox.Show("This registration was previously rejected!! Are you sure you want to approve it?", "Confirmation", MessageBoxButtons.YesNo);
                                if (dialogResult == DialogResult.No)
                                {
                                    // User chose not to approve, return without updating APRV
                                    return;
                                }
                                // Update the APRV to 1 for the selected NOMINEE_ID where APRV is NULL
                                string updateQuery = "UPDATE PARTICIPATES SET APRV = 1 WHERE NOMINEE_ID = @NomineeId AND APRV IS NULL";
                                SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                                updateCommand.Parameters.AddWithValue("@NomineeId", nomineeId);

                                int rowsAffected = updateCommand.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("The registration has been approved.");
                                    // Refresh or reload the data to reflect the changes in the DataGridView
                                    LoadNomineeData();
                                }
                                else
                                {
                                    MessageBox.Show("Failed to update APRV.");
                                }
                            }
                            else if (waitingCount > 0)
                            {
                                DialogResult dialogResult = MessageBox.Show("This registration was waiting for approval. Approve it now?", "Confirmation", MessageBoxButtons.YesNo);
                                if (dialogResult == DialogResult.Yes)
                                {
                                    // Update the APRV to 1 for the selected NOMINEE_ID where APRV is 0
                                    string updateQuery = "UPDATE PARTICIPATES SET APRV = 1 WHERE NOMINEE_ID = @NomineeId AND APRV = 0";
                                    SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                                    updateCommand.Parameters.AddWithValue("@NomineeId", nomineeId);

                                    int rowsAffected = updateCommand.ExecuteNonQuery();

                                    if (rowsAffected > 0)
                                    {
                                        MessageBox.Show("The registration has been approved.");
                                        // Refresh or reload the data to reflect the changes in the DataGridView
                                        LoadNomineeData();
                                    }
                                    else
                                    {
                                        MessageBox.Show("Failed to update approve status.");
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("ERROR: Approve is already set or this registration was not rejected or waiting previously.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select a row!!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Check if a row is selected in the DataGridView
            if (dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        connection.Open();

                        // Get the selected NOMINEE_ID from the DataGridView
                        string nomineeId = dataGridView1.SelectedRows[0].Cells["NOMINEE_ID"].Value.ToString();

                        // Check if the current APRV status is NULL (previously rejected)
                        string checkQuery = "SELECT APRV FROM PARTICIPATES WHERE NOMINEE_ID = @NomineeId";
                        SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                        checkCommand.Parameters.AddWithValue("@NomineeId", nomineeId);

                        object result = checkCommand.ExecuteScalar();

                        if (result != null && result == DBNull.Value)
                        {
                            MessageBox.Show("This registration was previously rejected.");
                            // Return as the status is already NULL (rejected)
                            return;
                        }

                        // Confirmation message for rejecting a previously approved nominee
                        DialogResult dialogResult = MessageBox.Show("This registration was previously approved!! Are you sure you want to reject it now?", "Confirmation", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.No)
                        {
                            // User chose not to reject, return without updating APRV
                            return;
                        }

                        // Update the APRV to NULL for the selected NOMINEE_ID
                        string updateQuery = "UPDATE PARTICIPATES SET APRV = NULL WHERE NOMINEE_ID = @NomineeId";
                        SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                        updateCommand.Parameters.AddWithValue("@NomineeId", nomineeId);

                        int rowsAffected = updateCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("The registration rejected!!");
                            // Refresh or reload the data to reflect the changes in the DataGridView
                            LoadNomineeData();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update APRV.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select a row!!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string searchTerm = textBox1.Text.Trim();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                SearchNominee(searchTerm);
            }
            else
            {
                LoadNomineeData();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string searchTerm = textBox1.Text.Trim();
                SearchNominee(searchTerm);
            }
        }
    }
}

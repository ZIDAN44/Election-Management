using ElectionApp.Common;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ElectionApp.Admin.Election
{
    public partial class E_Data : UserControl
    {
        private string adminID;
        private string connectionString;
        private SqlDataAdapter dataAdapter;
        private DataSet dataSet;

        public E_Data(string adminID, string connectionString)
        {
            AdminID = adminID;
            ConnectionString = connectionString;
            InitializeComponent();
            LoadElectionData();
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

        private void LoadElectionData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT E_IDENTIFIER, E_NAME, TYPE, S_DATE, E_DATE, R_DOC, 'Click to Show' AS DocLink FROM ELECTION";
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Add the DataGridViewLinkColumn for the "R_DOC" column
                        DataGridViewLinkColumn docColumn = new DataGridViewLinkColumn();
                        docColumn.HeaderText = "R_DOC";
                        docColumn.Name = "Doc Link";
                        docColumn.DataPropertyName = "Doc Link";
                        docColumn.UseColumnTextForLinkValue = true;
                        dataGridView1.Columns.Add(docColumn);

                        // Remove any existing columns before binding data to avoid duplicates
                        dataGridView1.Columns.Clear();

                        // Bind the DataTable to the DataGridView
                        dataGridView1.DataSource = dataTable;

                        // Adjust column headers and other configurations as needed
                        dataGridView1.Columns["E_IDENTIFIER"].HeaderText = "Election ID";
                        dataGridView1.Columns["E_NAME"].HeaderText = "Election Name";
                        dataGridView1.Columns["TYPE"].HeaderText = "Type";
                        dataGridView1.Columns["S_DATE"].HeaderText = "Starting Date";
                        dataGridView1.Columns["E_DATE"].HeaderText = "Ending Date";

                        // Hide the R_DOC column (optional)
                        dataGridView1.Columns["R_DOC"].Visible = false;

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
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["DocLink"].Index)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        connection.Open();

                        string eIdentifier = dataGridView1.Rows[e.RowIndex].Cells["E_IDENTIFIER"].Value.ToString();

                        string query = "SELECT R_DOC FROM ELECTION WHERE E_IDENTIFIER = @givenID";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@givenID", eIdentifier);

                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            byte[] fileData = (byte[])reader["R_DOC"];

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

                        reader.Close();
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

        private void SearchElection(string searchTerm)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM ELECTION WHERE E_NAME LIKE @searchTerm OR TYPE LIKE @searchTerm OR " +
                                   "CAST(S_DATE AS NVARCHAR(50)) LIKE @searchTerm OR CAST(E_DATE AS NVARCHAR(50)) LIKE @searchTerm OR " +
                                   "E_IDENTIFIER LIKE @searchTerm OR " +
                                   "CONVERT(NVARCHAR(10), S_DATE, 103) LIKE @dateFormatTerm OR CONVERT(NVARCHAR(10), E_DATE, 103) LIKE @dateFormatTerm";

                    dataAdapter = new SqlDataAdapter(query, connection);
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");

                    // Check for the date format "dd/MM/yyyy"
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@dateFormatTerm", searchTerm);

                    dataSet = new DataSet();
                    dataAdapter.Fill(dataSet, "ELECTION");
                    dataGridView1.DataSource = dataSet.Tables["ELECTION"];
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

        private void RemoveElection(string eID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    // Remove from REGULATES table
                    string removeRegulatesQuery = "DELETE FROM REGULATES WHERE ELECTION_ID = @eID";
                    SqlCommand removeRegulatesCommand = new SqlCommand(removeRegulatesQuery, connection);
                    removeRegulatesCommand.Parameters.AddWithValue("@eID", eID);
                    removeRegulatesCommand.ExecuteNonQuery();

                    // Remove from ELECTION table
                    string removeElectionQuery = "DELETE FROM ELECTION WHERE E_IDENTIFIER = @eID";
                    SqlCommand removeElectionCommand = new SqlCommand(removeElectionQuery, connection);
                    removeElectionCommand.Parameters.AddWithValue("@eID", eID);
                    removeElectionCommand.ExecuteNonQuery();

                    LoadElectionData();
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
            string searchTerm = textBox1.Text.Trim();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                SearchElection(searchTerm);
            }
            else
            {
                LoadElectionData();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Prompt user for removal confirmation and reasons
                DialogResult result = MessageBox.Show("Are you sure you want to remove?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                if (result == DialogResult.OK)
                {
                    string reason = Reject.PromptReason();

                    if (!string.IsNullOrEmpty(reason))
                    {
                        try
                        {
                            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                            string eIdentifier = selectedRow.Cells["E_IDENTIFIER"].Value.ToString();

                            // Insert data into REJECTIONS table
                            Reject.InsertIntoRejections(eIdentifier, reason, adminID, ConnectionString);

                            // Remove from ELECTION table
                            RemoveElection(eIdentifier);
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
                SearchElection(searchTerm);
            }
        }
    }
}

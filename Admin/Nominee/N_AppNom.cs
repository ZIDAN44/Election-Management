using ElectionApp.Common;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ElectionApp.Admin.Nominee
{
    public partial class N_AppNom : UserControl
    {
        private SqlDataAdapter dataAdapter;
        private DataSet dataSet;

        public N_AppNom(string adminID, string connectionString)
        {
            AdminID = adminID;
            ConnectionString = connectionString;
            InitializeComponent();
            LoadNomineeTempData();
        }

        private string AdminID { get; set; }

        private string ConnectionString { get; set; }

        private void LoadNomineeTempData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT N_IDENTIFIER, 'Click to Show' AS LogoLink, LOGO, N_NAME, N_EMAIL, IS_APROV FROM NOMINEE";
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
                        dataGridView1.Columns["N_IDENTIFIER"].HeaderText = "Nominee ID";
                        dataGridView1.Columns["N_NAME"].HeaderText = "Nominee Name";
                        dataGridView1.Columns["N_EMAIL"].HeaderText = "Email";
                        dataGridView1.Columns["IS_APROV"].HeaderText = "Approval Status";

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

                    string query = "SELECT LOGO FROM NOMINEE WHERE N_IDENTIFIER = @NIdentifier";
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

        private void UpdateNomineeApprovalRoleAndInsertRegistrar()
        {
            // Check if there's a selected row in dataGridView1
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the N_IDENTIFIER from the selected row in dataGridView1
                string nIdentifier = dataGridView1.SelectedRows[0].Cells["N_IDENTIFIER"].Value.ToString();

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    try
                    {
                        connection.Open();

                        // Update NOMINEE's IS_APROV to 1 for the fetched N_IDENTIFIER
                        string updateIS_APROVQuery = "UPDATE NOMINEE SET IS_APROV = 1 WHERE N_IDENTIFIER = @NIdentifier";
                        using (SqlCommand updateIS_APROVCommand = new SqlCommand(updateIS_APROVQuery, connection))
                        {
                            updateIS_APROVCommand.Parameters.AddWithValue("@NIdentifier", nIdentifier);
                            updateIS_APROVCommand.ExecuteNonQuery();
                        }

                        // Update ROLE to 'nominee' in LOGIN table for the fetched N_IDENTIFIER
                        string updateRoleQuery = "UPDATE LOGIN SET ROLE = 'nominee' WHERE UID = @NIdentifier";
                        using (SqlCommand updateRoleCommand = new SqlCommand(updateRoleQuery, connection))
                        {
                            updateRoleCommand.Parameters.AddWithValue("@NIdentifier", nIdentifier);
                            updateRoleCommand.ExecuteNonQuery();
                        }

                        // Insert into REGISTRARS table
                        string insertRegistrarQuery = "INSERT INTO REGISTRARS (USER_ID, ADMIN_ID, ROLE) " +
                                                      "VALUES(@NomineeID, @AdminID, @Role)";

                        using (SqlCommand insertRegistrarCommand = new SqlCommand(insertRegistrarQuery, connection))
                        {
                            insertRegistrarCommand.Parameters.AddWithValue("@NomineeID", nIdentifier);
                            insertRegistrarCommand.Parameters.AddWithValue("@AdminID", AdminID);
                            insertRegistrarCommand.Parameters.AddWithValue("@Role", "nominee");
                            insertRegistrarCommand.ExecuteNonQuery();
                        }

                        MessageBox.Show("Nominee approval & role updated successfully!");
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
                MessageBox.Show("Please select a row to update nominee approval & role");
            }
        }

        private void ApproveRegistration()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                int selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
                string nIdentifier = dataGridView1.Rows[selectedRowIndex].Cells["N_IDENTIFIER"].Value.ToString();
                object cellValue = dataGridView1.Rows[selectedRowIndex].Cells["IS_APROV"].Value;
                bool isApproved = cellValue != DBNull.Value && Convert.ToBoolean(cellValue);

                // Check if the selected entry is already approved
                if (isApproved)
                {
                    MessageBox.Show("This registration is already approved.");
                    return;
                }

                // Update Nominee & Login table also insert into REGISTRARS table
                UpdateNomineeApprovalRoleAndInsertRegistrar();

                // Refresh the DataGridView
                LoadNomineeTempData();
            }
        }

        private void RejectRegistration()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
                string nIdentifier = dataGridView1.Rows[selectedRowIndex].Cells["N_IDENTIFIER"].Value.ToString();

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
                            Reject.InsertIntoRejections(nIdentifier, reason, AdminID, ConnectionString);

                            using (SqlConnection connection = new SqlConnection(ConnectionString))
                            {
                                connection.Open();
                                string updateAPRVQuery = "UPDATE NOMINEE SET IS_APROV = NULL WHERE N_IDENTIFIER = @NIdentifier";
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
                    string query = "SELECT N_IDENTIFIER, 'Click to Show' AS LogoLink, LOGO, N_NAME, N_EMAIL, IS_APROV " +
                                   "FROM NOMINEE " +
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

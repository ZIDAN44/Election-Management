using ElectionApp.Common;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ElectionApp.Admin.Nominee
{
    public partial class N_Data : UserControl
    {
        private string adminID;
        private string connectionString;
        private SqlDataAdapter dataAdapter;
        private DataSet dataSet;

        public N_Data(string adminID, string connectionString)
        {
            AdminID = adminID;
            ConnectionString = connectionString;
            InitializeComponent();
            LoadNomineeData();
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

        private void LoadNomineeData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT N_IDENTIFIER, 'Click to Show' AS LogoLink, LOGO, N_NAME, P_NAME, N_EMAIL, VCOUNT FROM NOMINEE WHERE IS_APROV = 1";
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
                        dataGridView1.Columns["N_IDENTIFIER"].HeaderText = "NOM ID";
                        dataGridView1.Columns["N_NAME"].HeaderText = "Nominee Name";
                        dataGridView1.Columns["P_NAME"].HeaderText = "Party Name";
                        dataGridView1.Columns["N_EMAIL"].HeaderText = "Email";
                        dataGridView1.Columns["VCOUNT"].HeaderText = "Vote Count";

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

        private void SearchNominee(string searchTerm)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT N_IDENTIFIER, 'Click to Show' AS LogoLink, N_NAME, P_NAME, N_EMAIL, VCOUNT " +
                                   "FROM NOMINEE " +
                                   "WHERE N_NAME LIKE @searchTerm OR " +
                                   "P_NAME LIKE @searchTerm OR " +
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

        private void UpdateNominee_APROV(string nIdentifier)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    string updateQuery = "UPDATE NOMINEE SET IS_APROV = NULL WHERE N_IDENTIFIER = @nIdentifier";
                    SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@nIdentifier", nIdentifier);
                    updateCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating NOMINEE: " + ex.Message);
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
                SearchNominee(searchTerm);
            }
            else
            {
                LoadNomineeData();
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
                            string nIdentifier = selectedRow.Cells["N_IDENTIFIER"].Value.ToString();

                            // Insert data into REJECTIONS table
                            Reject.InsertIntoRejections(nIdentifier, reason, adminID, ConnectionString);

                            // Update NOMINEE table
                            UpdateNominee_APROV(nIdentifier);

                            // Re-load the Nominee Data
                            LoadNomineeData();
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
                SearchNominee(searchTerm);
            }
        }
    }
}

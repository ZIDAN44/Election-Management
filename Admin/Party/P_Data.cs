using ElectionApp.Common;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ElectionApp.Admin.Party
{
    public partial class P_Data : UserControl
    {
        private SqlDataAdapter dataAdapter;
        private DataSet dataSet;

        public P_Data(string adminID, string connectionString)
        {
            AdminID = adminID;
            ConnectionString = connectionString;
            InitializeComponent();
            LoadPartyData();
        }

        private string AdminID { get; set; }

        private string ConnectionString { get; set; }

        private void LoadPartyData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT P_IDENTIFIER, 'Click to Show' AS LogoLink, LOGO, P_NAME FROM PARTY";
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
                        dataGridView1.Columns["P_IDENTIFIER"].HeaderText = "Party ID";
                        dataGridView1.Columns["P_NAME"].HeaderText = "Party Name";

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
            // Check if the clicked cell is in the LOGO column and it's a "Click to Show" text
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["LogoLink"].Index)
            {
                string pIdentifier = dataGridView1.Rows[e.RowIndex].Cells["P_IDENTIFIER"].Value.ToString();
                ShowPicture(pIdentifier);
            }
        }

        private void ShowPicture(string pIdentifier)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT LOGO FROM PARTY WHERE P_IDENTIFIER = @PIdentifier";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PIdentifier", pIdentifier);

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
                            MessageBox.Show("No logo found for this party.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void SearchParty(string searchTerm)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT P_IDENTIFIER, 'Click to Show' AS LogoLink, P_NAME " +
                                   "FROM PARTY " +
                                   "WHERE P_NAME LIKE @searchTerm OR " +
                                   "P_IDENTIFIER LIKE @searchTerm";

                    dataAdapter = new SqlDataAdapter(query, connection);
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");

                    dataSet = new DataSet();
                    dataAdapter.Fill(dataSet, "PARTY");
                    dataGridView1.DataSource = dataSet.Tables["PARTY"];
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

        private void RemoveParty(string pID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    string removePartyQuery = "DELETE FROM PARTY WHERE P_IDENTIFIER = @pID";
                    SqlCommand removePartyCommand = new SqlCommand(removePartyQuery, connection);
                    removePartyCommand.Parameters.AddWithValue("@pID", pID);
                    removePartyCommand.ExecuteNonQuery();

                    LoadPartyData();
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
                SearchParty(searchTerm);
            }
            else
            {
                LoadPartyData();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Prompt user for removal confirmation and reasons
                DialogResult result = MessageBox.Show("Are you sure you want to remove the party?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                if (result == DialogResult.OK)
                {
                    string reason = Reject.PromptReason();

                    if (!string.IsNullOrEmpty(reason))
                    {
                        try
                        {
                            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                            string pIdentifier = selectedRow.Cells["P_IDENTIFIER"].Value.ToString();

                            // Insert data into REJECTIONS table
                            Reject.InsertIntoRejections(pIdentifier, reason, AdminID, ConnectionString);

                            // Remove party from PARTY table
                            RemoveParty(pIdentifier);
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
    }
}

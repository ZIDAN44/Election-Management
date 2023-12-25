using System.Data.SqlClient;
using System.Diagnostics;

namespace ElectionApp.Nominee
{
    public partial class Election_Select : UserControl
    {
        private string givenID;
        private string connectionString;
        private List<Election_List> originalElectionItems;

        public Election_Select(string givenID, string connectionString)
        {
            GivenID = givenID;
            ConnectionString = connectionString;
            InitializeComponent();
            LoadAvailableElections();
        }

        private string GivenID
        {
            get { return givenID; }
            set { givenID = value; }
        }

        private string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        private void LoadAvailableElections()
        {
            // Clear controls outside the loop to avoid clearing on each iteration
            if (flowLayoutPanel1.Controls.Count > 0)
            {
                flowLayoutPanel1.Controls.Clear();
            }

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    string electionQuery = "SELECT E_IDENTIFIER, E_NAME, S_DATE, E_DATE, R_DOC FROM ELECTION WHERE E_DATE >= GETDATE()";
                    SqlCommand electionCommand = new SqlCommand(electionQuery, connection);
                    SqlDataReader electionReader = electionCommand.ExecuteReader();

                    while (electionReader.Read())
                    {
                        Election_List electionItem = new Election_List();
                        electionItem.Message1 = "ID: " + electionReader["E_IDENTIFIER"].ToString();
                        electionItem.Message2 = "Name: " + electionReader["E_NAME"].ToString();
                        electionItem.Message3 = "Start Date: " + ((DateTime)electionReader["S_DATE"]).ToShortDateString();
                        electionItem.Message4 = "End Date: " + ((DateTime)electionReader["E_DATE"]).ToShortDateString();

                        // Check if R_DOC contains PDF data
                        if (electionReader["R_DOC"] != DBNull.Value)
                        {
                            byte[] pdfBytes = (byte[])electionReader["R_DOC"];

                            // Attach event handler for the Election_List's OnPdfButtonClicked event
                            electionItem.OnPdfButtonClicked += (sender, e) =>
                            {
                                // Check if the byte array contains PDF data
                                if (IsPdfFile(pdfBytes))
                                {
                                    DisplayPdf(pdfBytes);
                                }
                                else
                                {
                                    MessageBox.Show("This is not a PDF file.");
                                }
                            };
                        }

                        // Handle the button click event for joining an election
                        electionItem.JoinButtonClicked += async (sender, e) =>
                        {
                            try
                            {
                                using (SqlConnection updateConnection = new SqlConnection(ConnectionString))
                                {
                                    updateConnection.Open();

                                    // Prompt user to upload a modified document
                                    DialogResult dialogResult = MessageBox.Show("Please upload your modified document.", "Upload Document", MessageBoxButtons.OKCancel);
                                    if (dialogResult == DialogResult.OK)
                                    {
                                        // Open file dialog to select the document
                                        OpenFileDialog openFileDialog = new OpenFileDialog();
                                        openFileDialog.Filter = "Document Files|*.pdf;*.docx"; // Set your acceptable document types
                                        openFileDialog.Title = "Select Document to Upload";

                                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                                        {
                                            // Read the selected file into a byte array
                                            byte[] fileBytes;
                                            using (FileStream fileStream = File.Open(openFileDialog.FileName, FileMode.Open))
                                            {
                                                fileBytes = new byte[fileStream.Length];
                                                fileStream.Read(fileBytes, 0, fileBytes.Length);
                                            }

                                            // Insert into database (assuming R_DOC field is for storing document bytes)
                                            string insertQuery = "INSERT INTO PARTICIPATES (ELECTION_ID, NOMINEE_ID, APRV, R_DOC) VALUES (@ElectionID, @NomineeID, @Approval, @Document)";
                                            SqlCommand command = new SqlCommand(insertQuery, updateConnection);
                                            command.Parameters.AddWithValue("@ElectionID", electionItem.Message1.Replace("ID: ", ""));
                                            command.Parameters.AddWithValue("@NomineeID", GivenID);
                                            command.Parameters.AddWithValue("@Approval", false);
                                            command.Parameters.AddWithValue("@Document", fileBytes);
                                            await command.ExecuteNonQueryAsync();

                                            N_Dashboard dashboard = this.ParentForm as N_Dashboard;
                                            if (dashboard != null)
                                            {
                                                dashboard.UpdateElectionStatusLabel("Selection is pending approval!");
                                            }

                                            MessageBox.Show("Document uploaded and election joined successfully!");
                                        }
                                        else
                                        {
                                            MessageBox.Show("Document upload canceled. Please upload the document to confirm selection.");
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error updating data: " + ex.Message);
                            }
                        };

                        flowLayoutPanel1.Controls.Add(electionItem);
                    }

                    originalElectionItems = flowLayoutPanel1.Controls.OfType<Election_List>().ToList();
                    electionReader.Close();
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

        private void DisplayPdf(byte[] fileData)
        {
            try
            {
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
            catch (Exception ex)
            {
                MessageBox.Show("Error opening PDF: " + ex.Message);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string searchTerm = textBox1.Text.Trim();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                List<Election_List> electionItemsToSearch = originalElectionItems ?? flowLayoutPanel1.Controls.OfType<Election_List>().ToList();

                List<Election_List> filteredEle = electionItemsToSearch.Where(p =>
                    p.Message1.ToLower().Contains(searchTerm.ToLower()) ||
                    p.Message2.ToLower().Contains(searchTerm.ToLower())
                ).ToList();

                flowLayoutPanel1.Controls.Clear();

                foreach (Election_List election in filteredEle)
                {
                    flowLayoutPanel1.Controls.Add(election);
                }
            }
            else
            {
                // If the search term is empty, reload the original election list
                if (originalElectionItems != null)
                {
                    flowLayoutPanel1.Controls.Clear();
                    foreach (Election_List election in originalElectionItems)
                    {
                        flowLayoutPanel1.Controls.Add(election);
                    }
                }
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
                e.SuppressKeyPress = true;
            }
        }

        private void Election_Select_Load(object sender, EventArgs e)
        {
            textBox1.Select();
        }
    }
}
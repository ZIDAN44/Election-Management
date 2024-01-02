using System.Data;
using System.Data.SqlClient;

namespace ElectionApp.Voter.Selects
{
    public partial class Vote_Select : UserControl
    {
        private List<Nom_list> originalNomItems;

        public Vote_Select(string givenID, string connectionString)
        {
            GivenID = givenID;
            ConnectionString = connectionString;
            InitializeComponent();
            LoadAvailableNominees();
        }

        private string GivenID { get; set; }

        private string ConnectionString { get; set; }

        private void LoadAvailableNominees()
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

                    // Fetch NOMINEE_ID from PARTICIPATES table where APRV = 1
                    string nomineeQuery = "SELECT NOMINEE_ID FROM PARTICIPATES WHERE APRV = 1";
                    SqlCommand nomineeCommand = new SqlCommand(nomineeQuery, connection);
                    SqlDataReader nomineeReader = nomineeCommand.ExecuteReader();

                    // Store NOMINEE_ID values in a list
                    List<string> nomineeIDs = new List<string>();
                    while (nomineeReader.Read())
                    {
                        nomineeIDs.Add(nomineeReader["NOMINEE_ID"].ToString());
                    }

                    nomineeReader.Close();

                    // Fetch NOMINEE details using the obtained NOMINEE_IDs
                    foreach (var nomineeID in nomineeIDs)
                    {
                        string query = "SELECT N_IDENTIFIER, N_NAME, P_NAME, LOGO FROM NOMINEE WHERE N_IDENTIFIER = @NomineeID";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@NomineeID", nomineeID);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Nom_list nomineeItem = new Nom_list();
                            nomineeItem.Message1 = "ID: " + reader["N_IDENTIFIER"].ToString();
                            nomineeItem.Message2 = "Name: " + reader["N_NAME"].ToString();
                            nomineeItem.Message3 = "Party: " + reader["P_NAME"].ToString();

                            // Load the logo image into the PictureBox
                            if (reader["LOGO"] != DBNull.Value)
                            {
                                byte[] logoBytes = (byte[])reader["LOGO"];
                                using (MemoryStream ms = new MemoryStream(logoBytes))
                                {
                                    nomineeItem.Image1 = Image.FromStream(ms);
                                }
                            }

                            // Handle the button click event for each nomineeItem
                            nomineeItem.VoteButtonClicked += (sender, e) =>
                            {
                                try
                                {
                                    using (SqlConnection updateConnection = new SqlConnection(ConnectionString))
                                    {
                                        updateConnection.Open();

                                        // Update HAS_VOTE to 1 in VOTER table
                                        string updateVoterQuery = "UPDATE VOTER SET HAS_VOTE = 1 WHERE V_IDENTIFIER = @VoterID";
                                        SqlCommand updateVoterCommand = new SqlCommand(updateVoterQuery, updateConnection);
                                        updateVoterCommand.Parameters.AddWithValue("@VoterID", GivenID);
                                        updateVoterCommand.ExecuteNonQuery();

                                        // Increment VCOUNT by +1 in NOMINEE table for the respective nominee
                                        string updateNomineeQuery = "UPDATE NOMINEE SET VCOUNT = ISNULL(VCOUNT, 0) + 1 WHERE N_IDENTIFIER = @NomineeID";
                                        SqlCommand updateNomineeCommand = new SqlCommand(updateNomineeQuery, updateConnection);
                                        updateNomineeCommand.Parameters.AddWithValue("@NomineeID", nomineeItem.Message1.Replace("ID: ", "")); // Corrected the usage of Nominee ID
                                        updateNomineeCommand.ExecuteNonQuery();

                                        // Insert into VOTES table
                                        string insertVotesQuery = "INSERT INTO VOTES (VOTER_ID, NOMINEE_ID) " +
                                                                  "VALUES (@VoterID, @NomineeID)";
                                        SqlCommand insertVotesCommand = new SqlCommand(insertVotesQuery, updateConnection);
                                        insertVotesCommand.Parameters.AddWithValue("@VoterID", GivenID);
                                        insertVotesCommand.Parameters.AddWithValue("@NomineeID", nomineeItem.Message1.Replace("ID: ", ""));
                                        insertVotesCommand.ExecuteNonQuery();

                                        V_Dashboard dashboard = this.ParentForm as V_Dashboard;
                                        if (dashboard != null)
                                        {
                                            // Update the labelApprovalStatus in the V_Dashboard panel
                                            dashboard.UpdateApprovalStatus(true);
                                            dashboard.DisableVotingPanel();
                                        }

                                        MessageBox.Show("Vote registered successfully!");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error updating data: " + ex.Message);
                                }
                            };

                            // Add each nominee item to the flowLayoutPanel
                            flowLayoutPanel1.Controls.Add(nomineeItem);
                        }

                        originalNomItems = flowLayoutPanel1.Controls.OfType<Nom_list>().ToList();
                        reader.Close();
                    }
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
                List<Nom_list> nomItemsToSearch = originalNomItems ?? flowLayoutPanel1.Controls.OfType<Nom_list>().ToList();

                List<Nom_list> filteredNominee = nomItemsToSearch.Where(p =>
                    p.Message1.ToLower().Contains(searchTerm.ToLower()) ||
                    p.Message2.ToLower().Contains(searchTerm.ToLower())
                ).ToList();

                flowLayoutPanel1.Controls.Clear();

                foreach (Nom_list nom in filteredNominee)
                {
                    flowLayoutPanel1.Controls.Add(nom);
                }
            }
            else
            {
                // If the search term is empty, reload the original nom list
                if (originalNomItems != null)
                {
                    flowLayoutPanel1.Controls.Clear();
                    foreach (Nom_list nom in originalNomItems)
                    {
                        flowLayoutPanel1.Controls.Add(nom);
                    }
                }
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
                e.SuppressKeyPress = true;
            }
        }

        private void Vote_Select_Load(object sender, EventArgs e)
        {
            textBox1.Select();
        }
    }
}

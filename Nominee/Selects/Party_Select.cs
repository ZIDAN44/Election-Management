using System.Data;
using System.Data.SqlClient;

namespace ElectionApp.Nominee
{
    public partial class Party_Select : UserControl
    {
        private string givenID;
        private string connectionString;
        private List<Party_List> originalPartyItems;

        public Party_Select(string givenID, string connectionString)
        {
            GivenID = givenID;
            ConnectionString = connectionString;
            InitializeComponent();
            LoadAvailablePartys();
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

        private void LoadAvailablePartys()
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

                    // Fetch PARTY details
                    string partyQuery = "SELECT P_IDENTIFIER, P_NAME, LOGO FROM PARTY";
                    SqlCommand partyCommand = new SqlCommand(partyQuery, connection);
                    SqlDataReader partyReader = partyCommand.ExecuteReader();

                    while (partyReader.Read())
                    {
                        Party_List partyItem = new Party_List();
                        partyItem.Message1 = "ID: " + partyReader["P_IDENTIFIER"].ToString();
                        partyItem.Message2 = "Name: " + partyReader["P_NAME"].ToString();

                        // Load the logo image into the PictureBox
                        if (partyReader["LOGO"] != DBNull.Value)
                        {
                            byte[] logoBytes = (byte[])partyReader["LOGO"];
                            using (MemoryStream ms = new MemoryStream(logoBytes))
                            {
                                partyItem.Image1 = Image.FromStream(ms);
                            }
                        }

                        // Handle the button click event for joining a party
                        partyItem.JoinButtonClicked += (sender, e) =>
                        {
                            try
                            {
                                using (SqlConnection updateConnection = new SqlConnection(ConnectionString))
                                {
                                    updateConnection.Open();

                                    // Update P_NAME in NOMINEE table
                                    string updateNomineeQuery = "UPDATE NOMINEE SET P_NAME = @PartyName WHERE N_IDENTIFIER = @NomineeID";
                                    SqlCommand updateNomineeCommand = new SqlCommand(updateNomineeQuery, updateConnection);
                                    updateNomineeCommand.Parameters.AddWithValue("@NomineeID", GivenID);
                                    updateNomineeCommand.Parameters.AddWithValue("@PartyName", partyItem.Message2.Replace("Name: ", ""));
                                    updateNomineeCommand.ExecuteNonQuery();

                                    // Insert into JOINS table
                                    string insertJoinsQuery = "INSERT INTO JOINS (PARTY_ID, NOMINEE_ID, JOIN_DATE) " +
                                                              "VALUES (@PartyID, @NomineeID, GETDATE())";
                                    SqlCommand insertJoinsCommand = new SqlCommand(insertJoinsQuery, updateConnection);
                                    insertJoinsCommand.Parameters.AddWithValue("@PartyID", partyItem.Message1.Replace("ID: ", ""));
                                    insertJoinsCommand.Parameters.AddWithValue("@NomineeID", GivenID);
                                    insertJoinsCommand.ExecuteNonQuery();

                                    N_Dashboard dashboard = this.ParentForm as N_Dashboard;
                                    if (dashboard != null)
                                    {
                                        dashboard.UpdatePartyStatusLabel(partyItem.Message2.Replace("Name: ", ""));
                                    }

                                    MessageBox.Show("You have joined the party successfully!");
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error updating data: " + ex.Message);
                            }
                        };

                        // Add each party item to the flowLayoutPanel
                        flowLayoutPanel1.Controls.Add(partyItem);
                    }
                    originalPartyItems = flowLayoutPanel1.Controls.OfType<Party_List>().ToList();
                    partyReader.Close();
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
                List<Party_List> partyItemsToSearch = originalPartyItems ?? flowLayoutPanel1.Controls.OfType<Party_List>().ToList();

                List<Party_List> filteredParties = partyItemsToSearch.Where(p =>
                    p.Message1.ToLower().Contains(searchTerm.ToLower()) ||
                    p.Message2.ToLower().Contains(searchTerm.ToLower())
                ).ToList();

                flowLayoutPanel1.Controls.Clear();

                foreach (Party_List party in filteredParties)
                {
                    flowLayoutPanel1.Controls.Add(party);
                }
            }
            else
            {
                // If the search term is empty, reload the original party list
                if (originalPartyItems != null)
                {
                    flowLayoutPanel1.Controls.Clear();
                    foreach (Party_List party in originalPartyItems)
                    {
                        flowLayoutPanel1.Controls.Add(party);
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

        private void Party_Select_Load(object sender, EventArgs e)
        {
            textBox1.Select();
        }
    }
}

using System.Data.SqlClient;

namespace ElectionApp.Common
{
    public partial class Notification_Panel : UserControl
    {
        private bool IS_APROV = false;

        public Notification_Panel(string givenID, string connectionString, string role)
        {
            GivenID = givenID;
            Role = role;
            ConnectionString = connectionString;
            InitializeComponent();

            // Check if the role is "voter"
            if (role == "voter")
            {
                ShowVoterNotifications();
            }
            else if (role == "nominee")
            {
                ShowNomineeNotifications();
            }
        }

        private string GivenID { get; set; }

        private string Role { get; set; }

        private string ConnectionString { get; set; }

        #region VotersNotification
        private void ShowVoterNotifications()
        {
            List<string> voterNotifications = new List<string>();

            string approvalMessage = GetVoterApprovalStatusMessage();
            if (!string.IsNullOrEmpty(approvalMessage))
            {
                voterNotifications.Add(approvalMessage);
            }

            if (IS_APROV)
            {
                string votingStatusMessage = GetVotingStatusMessage();
                if (!string.IsNullOrEmpty(votingStatusMessage))
                {
                    voterNotifications.Add(votingStatusMessage);
                }
            }
            AddNotificationsToFlowLayout(voterNotifications);
        }

        private string GetVoterApprovalStatusMessage()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT IS_APROV FROM VOTER WHERE V_IDENTIFIER = @GivenID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@GivenID", GivenID);
                    object isApproved = command.ExecuteScalar();

                    if (isApproved != null && isApproved != DBNull.Value)
                    {
                        bool approvalStatus = Convert.ToBoolean(isApproved);

                        if (!approvalStatus)
                        {
                            return "Your registration is pending approval";
                        }
                        else
                        {
                            IS_APROV = true;
                            return "Your registration was approved";
                        }
                    }
                    else
                    {
                        return "Your registration was rejected";
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

            return string.Empty;
        }

        private string GetVotingStatusMessage()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT HAS_VOTE FROM VOTER WHERE V_IDENTIFIER = @GivenID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@GivenID", GivenID);

                    object hasVoted = command.ExecuteScalar();

                    if (hasVoted != null && hasVoted != DBNull.Value)
                    {
                        bool hasVotedStatus = Convert.ToBoolean(hasVoted);

                        return hasVotedStatus ? "You have voted successfully" : "You still haven't voted";
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

            return string.Empty;
        }
        #endregion

        #region NomineeNotification
        private void ShowNomineeNotifications()
        {
            List<string> nomineeNotifications = new List<string>();

            string approvalMessage = GetNomineeApprovalStatusMessage();
            if (!string.IsNullOrEmpty(approvalMessage))
            {
                nomineeNotifications.Add(approvalMessage);
            }

            if (IS_APROV)
            {
                string partyStatusMessage = GetPartyStatusMessage();
                if (!string.IsNullOrEmpty(partyStatusMessage))
                {
                    nomineeNotifications.Add(partyStatusMessage);
                }

                string electionStatusMessage = GetElectionStatusMessage();
                if (!string.IsNullOrEmpty(electionStatusMessage))
                {
                    nomineeNotifications.Add(electionStatusMessage);
                }
            }
            AddNotificationsToFlowLayout(nomineeNotifications);
        }

        private string GetNomineeApprovalStatusMessage()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT IS_APROV FROM NOMINEE WHERE N_IDENTIFIER = @GivenID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@GivenID", GivenID);
                    object isApproved = command.ExecuteScalar();

                    if (isApproved != null && isApproved != DBNull.Value)
                    {
                        bool approvalStatus = Convert.ToBoolean(isApproved);

                        if (!approvalStatus)
                        {
                            return "Your registration is pending approval";
                        }
                        else
                        {
                            IS_APROV = true;
                            return "Your registration was approved";
                        }
                    }
                    else
                    {
                        return "Your registration was rejected";
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

            return string.Empty;
        }

        private string GetPartyStatusMessage()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT P_NAME FROM NOMINEE WHERE N_IDENTIFIER = @GivenID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@GivenID", GivenID);
                    object partyName = command.ExecuteScalar();

                    if (partyName != null && partyName != DBNull.Value)
                    {

                        IS_APROV = true;
                        return "You have joined a party";

                    }
                    else
                    {
                        return "You have not joined any party";
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

            return string.Empty;
        }

        private string GetElectionStatusMessage()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT APRV FROM PARTICIPATES WHERE NOMINEE_ID = @GivenID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@GivenID", GivenID);
                    object isApproved = command.ExecuteScalar();

                    if (isApproved != null && isApproved != DBNull.Value)
                    {
                        bool approvalStatus = Convert.ToBoolean(isApproved);

                        if (!approvalStatus)
                        {
                            return "Your participation in election pending approval";
                        }
                        else
                        {
                            IS_APROV = true;
                            return "Your participation in election was approved";
                        }
                    }
                    else
                    {
                        return "You haven't participated in election";
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

            return string.Empty;
        }
        #endregion

        private void AddNotificationsToFlowLayout(List<string> messages)
        {
            if (flowLayoutPanel1.Controls.Count > 0)
            {
                flowLayoutPanel1.Controls.Clear();
            }

            foreach (string message in messages)
            {
                User_Notification notificationItem = new User_Notification();

                if (message.Contains("approved") || message.Contains("voted successfully")
                    || message.Contains("a party"))
                {
                    notificationItem.Message1 = message;
                    notificationItem.Image1 = Properties.Resources.GreenTick;
                }
                else if (message.Contains("pending approval"))
                {
                    notificationItem.Message1 = message;
                    notificationItem.Image1 = Properties.Resources.Exclamation_y;
                }
                else
                {
                    notificationItem.Message1 = message;
                    notificationItem.Image1 = Properties.Resources.Exclamatory_r;
                }

                flowLayoutPanel1.Controls.Add(notificationItem);
            }
        }
    }
}
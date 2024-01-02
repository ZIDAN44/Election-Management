using System;
using System.Data;
using System.Data.SqlClient;

namespace ElectionApp.Common
{
    public partial class Notification_Panel : UserControl
    {
        private string givenID;
        private string role;
        private string connectionString;

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
        }

        private string GivenID
        {
            get { return givenID; }
            set { givenID = value; }
        }

        private string Role
        {
            get { return role; }
            set { role = value; }
        }

        private string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        #region VotersNotification
        private void ShowVoterNotifications()
        {
            List<string> voterNotifications = new List<string>();

            string approvalMessage = GetVoterApprovalStatusMessage();
            if (!string.IsNullOrEmpty(approvalMessage))
            {
                voterNotifications.Add(approvalMessage);
            }

            string votingStatusMessage = GetVotingStatusMessage();
            if (!string.IsNullOrEmpty(votingStatusMessage))
            {
                voterNotifications.Add(votingStatusMessage);
            }

            AddNotificationsToFlowLayout(voterNotifications);
        }

        private string GetVoterApprovalStatusMessage()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT IS_APROV FROM VOTER WHERE V_IDENTIFIER = @GivenID AND IS_APROV = 1";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@GivenID", givenID);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        return "Your registration was approved";
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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT HAS_VOTE FROM VOTER WHERE V_IDENTIFIER = @GivenID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@GivenID", givenID);

                    object hasVoted = command.ExecuteScalar();

                    if (hasVoted != null && hasVoted != DBNull.Value)
                    {
                        bool hasVotedStatus = Convert.ToBoolean(hasVoted);

                        if (hasVotedStatus)
                        {
                            return "You have voted successfully";
                        }
                        else
                        {
                            return "You still haven't voted";
                        }
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

                if (message.Contains("approved") || message.Contains("voted successfully"))
                {
                    notificationItem.Message1 = message;
                    notificationItem.Image1 = Properties.Resources.GreenTick;
                }
                else
                {
                    notificationItem.Message1 = message;
                    notificationItem.Image1 = Properties.Resources.Exclamatory;
                }

                flowLayoutPanel1.Controls.Add(notificationItem);
            }
        }
    }
}
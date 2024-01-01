using System.Data.SqlClient;

namespace ElectionApp.Common
{
    public class Reject
    {
        public static string PromptReason()
        {
            string reason = "";
            using (var form = new Form())
            using (var textBox = new TextBox())
            using (var button = new Button())
            {
                form.Text = "Enter Removal Reason";
                form.ClientSize = new Size(300, 120);
                form.StartPosition = FormStartPosition.CenterScreen;

                textBox.Dock = DockStyle.Top;
                textBox.Multiline = true;
                textBox.Height = 40;
                textBox.ScrollBars = ScrollBars.Vertical;

                button.Dock = DockStyle.Bottom;
                button.Text = "OK";
                button.DialogResult = DialogResult.OK;
                button.Click += (sender, e) => form.Close();

                form.Controls.AddRange(new Control[] { textBox, button });
                var dialogResult = form.ShowDialog();

                if (dialogResult == DialogResult.OK)
                {
                    reason = textBox.Text.Trim();
                }
            }
            return reason;
        }

        public static void InsertIntoRejections(string nIdentifier, string reason, string adminID, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO REJECTIONS (UID, REASONS, ADMIN_ID) VALUES (@nIdentifier, @reason, @adminID)";
                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@nIdentifier", nIdentifier);
                    insertCommand.Parameters.AddWithValue("@reason", reason);
                    insertCommand.Parameters.AddWithValue("@adminID", adminID);
                    insertCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error inserting into REJECTIONS: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}

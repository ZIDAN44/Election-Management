namespace ElectionApp.Admin.Party
{
    public partial class M_Party : UserControl
    {
        public M_Party(string adminID, string connectionString)
        {
            AdminID = adminID;
            ConnectionString = connectionString;
            InitializeComponent();

            P_Data p_Data = new P_Data(adminID, connectionString);
            adduserControl(p_Data);
        }

        private string AdminID { get; set; }

        private string ConnectionString { get; set; }

        private void adduserControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            panelContainer.Controls.Clear();
            panelContainer.Controls.Add(userControl);
            userControl.BringToFront();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            P_Data p_Data = new P_Data(AdminID, ConnectionString);
            adduserControl(p_Data);
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            // Check if an instance of P_Data exists in the panelContainer
            P_Data p_Data = panelContainer.Controls.OfType<P_Data>().FirstOrDefault();

            if (p_Data != null && p_Data.dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = p_Data.dataGridView1.SelectedRows[0];

                P_Modify p_Modify = new P_Modify(AdminID, ConnectionString);
                p_Modify.ReceiveSelectedRowData(selectedRow);
                adduserControl(p_Modify);
            }
            else
            {
                MessageBox.Show("Please select a row in Party Data to edit.");
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            P_Add p_Add = new P_Add(AdminID, ConnectionString);
            adduserControl(p_Add);
        }
    }
}

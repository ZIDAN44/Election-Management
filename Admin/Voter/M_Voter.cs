namespace ElectionApp.Admin.Voter
{
    public partial class M_Voter : UserControl
    {
        private string adminID;
        private string connectionString;

        public M_Voter(string adminID, string connectionString)
        {
            AdminID = adminID;
            ConnectionString = connectionString;
            InitializeComponent();

            V_Data v_Data = new V_Data(adminID, connectionString);
            adduserControl(v_Data);
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

        private void adduserControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            panelContainer.Controls.Clear();
            panelContainer.Controls.Add(userControl);
            userControl.BringToFront();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            V_AppVoter v_AppVoter = new V_AppVoter(adminID, connectionString);
            adduserControl(v_AppVoter);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            V_Data v_Data = new V_Data(adminID, connectionString);
            adduserControl(v_Data);
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            // Check if an instance of V_Data exists in the panelContainer
            V_Data v_Data = panelContainer.Controls.OfType<V_Data>().FirstOrDefault();

            if (v_Data != null && v_Data.dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = v_Data.dataGridView1.SelectedRows[0];

                V_Modify v_Modify = new V_Modify(adminID, connectionString);
                v_Modify.ReceiveSelectedRowData(selectedRow);
                adduserControl(v_Modify);
            }
            else
            {
                MessageBox.Show("Please select a row in Voter Data to edit.");
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            V_Add v_Add = new V_Add(adminID, connectionString);
            adduserControl(v_Add);
        }
    }
}

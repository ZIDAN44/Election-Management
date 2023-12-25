namespace ElectionApp.Admin.Nominee
{
    public partial class M_Nominee : UserControl
    {
        private string adminID;
        private string connectionString;
        public M_Nominee(string adminID, string connectionString)
        {
            AdminID = adminID;
            ConnectionString = connectionString;
            InitializeComponent();

            N_Data n_Data = new N_Data(adminID, connectionString);
            adduserControl(n_Data);
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
            N_AppNom approveRes = new N_AppNom(adminID, connectionString);
            adduserControl(approveRes);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            N_Data n_Data = new N_Data(adminID, connectionString);
            adduserControl(n_Data);
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            // Check if an instance of N_Data exists in the panelContainer
            N_Data n_Data = panelContainer.Controls.OfType<N_Data>().FirstOrDefault();

            if (n_Data != null && n_Data.dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = n_Data.dataGridView1.SelectedRows[0];

                N_Modify n_Modify = new N_Modify(adminID, connectionString);
                n_Modify.ReceiveSelectedRowData(selectedRow);
                adduserControl(n_Modify);
            }
            else
            {
                MessageBox.Show("Please select a row in Nominee Data to edit.");
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            N_Add n_Add = new N_Add(adminID, connectionString);
            adduserControl(n_Add);
        }
    }
}

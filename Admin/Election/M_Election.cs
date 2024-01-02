namespace ElectionApp.Admin.Election
{
    public partial class M_Election : UserControl
    {
        public M_Election(string adminID, string connectionString)
        {
            AdminID = adminID;
            ConnectionString = connectionString;
            InitializeComponent();

            E_Data e_Data = new E_Data(adminID, connectionString);
            adduserControl(e_Data);
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

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            E_Data e_Data = new E_Data(AdminID, ConnectionString);
            adduserControl(e_Data);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            // Check if an instance of E_Data exists in the panelContainer
            E_Data e_Data = panelContainer.Controls.OfType<E_Data>().FirstOrDefault();

            if (e_Data != null && e_Data.dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = e_Data.dataGridView1.SelectedRows[0];

                E_Modify e_Modify = new E_Modify(AdminID, ConnectionString);
                e_Modify.ReceiveSelectedRowData(selectedRow);
                adduserControl(e_Modify);
            }
            else
            {
                MessageBox.Show("Please select a row in Election List to edit.");
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            E_Add e_Add = new E_Add(AdminID, ConnectionString);
            adduserControl(e_Add);
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            E_AppCan e_AppCan = new E_AppCan(AdminID, ConnectionString);
            adduserControl(e_AppCan);
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            E_CanData e_CanData = new E_CanData(AdminID, ConnectionString);
            adduserControl(e_CanData);
        }
    }
}

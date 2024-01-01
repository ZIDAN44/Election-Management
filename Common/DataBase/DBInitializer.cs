using System.Data.SqlClient;

namespace ElectionApp.Common.DataBase
{
    public static class DBInitializer
    {
        private static string? connectionString = null;

        public static void InitializeDatabase()
        {
            GetConnectionStringFromUser();
            if (!string.IsNullOrEmpty(connectionString))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        CreateDatabaseSchema(connection);
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Invalid connection string!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void GetConnectionStringFromUser()
        {
            string dataSource = Microsoft.VisualBasic.Interaction.InputBox("Enter the Data Source:", "Enter Data Source", "");

            // Replace double backslashes with single backslashes
            dataSource = dataSource.Replace(@"\\", @"\");

            if (string.IsNullOrEmpty(dataSource))
            {
                dataSource = "LAPTOP-183GKF7M\\SQLEXPRESS01"; // Default value
            }

            // Construct the connection string using the provided values
            connectionString = $"Data Source={dataSource}; Initial Catalog=ElectionDB; Integrated Security=true;";
        }

        private static void CreateDatabaseSchema(SqlConnection connection)
        {
            string createTablesQuery = @"
                -- Create VOTER table
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'VOTER')
                BEGIN
                    CREATE TABLE VOTER (
                        NID INT IDENTITY(1,1) PRIMARY KEY,
                        V_IDENTIFIER AS 'VT-' + RIGHT('000' + CAST(NID AS VARCHAR(10)), 3) PERSISTED NOT NULL,
                        V_NAME VARCHAR(100),
                        V_EMAIL VARCHAR(100),
                        PIC VARBINARY(MAX),
                        HAS_VOTE BIT DEFAULT 0,
                        CONSTRAINT [UQ_PRODUCT_V_IDENTIFIER] UNIQUE ([V_IDENTIFIER])
                    );
                END

                -- Create VOTER_TEMP table
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'VOTER_TEMP')
                BEGIN
                    CREATE TABLE VOTER_TEMP (
                        TEMP_NID INT IDENTITY(1,1) PRIMARY KEY,
                        V_IDENTIFIER AS 'TVT-' + RIGHT('00' + CAST(TEMP_NID AS VARCHAR(10)), 2) PERSISTED NOT NULL,
                        V_NAME VARCHAR(100),
                        V_EMAIL VARCHAR(100),
                        PIC VARBINARY(MAX),
                        HAS_VOTE BIT DEFAULT 0,
                        APRV BIT DEFAULT 0,
                        APRV_NID VARCHAR(MAX),
                        CONSTRAINT [UQ_PRODUCT_VT_IDENTIFIER] UNIQUE ([V_IDENTIFIER])
                    );
                END

                -- Create NOMINEE table
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'NOMINEE')
                BEGIN
                    CREATE TABLE NOMINEE (
                        NOM_ID INT IDENTITY(1,1) PRIMARY KEY,
                        N_IDENTIFIER AS 'NT-' + RIGHT('000' + CAST(NOM_ID AS VARCHAR(10)), 3) PERSISTED NOT NULL,
                        N_NAME VARCHAR(100),
                        P_NAME VARCHAR(100),
                        N_EMAIL VARCHAR(100),
                        LOGO VARBINARY(MAX),
                        VCOUNT INT,
                        CONSTRAINT [UQ_PRODUCT_N_IDENTIFIER] UNIQUE ([N_IDENTIFIER])
                    );
                END

                -- Create NOMINEE_TEMP table
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'NOMINEE_TEMP')
                BEGIN
                    CREATE TABLE NOMINEE_TEMP (
                        TEMP_NOM_ID INT IDENTITY(1,1) PRIMARY KEY,
                        N_IDENTIFIER AS 'TNT-' + RIGHT('00' + CAST(TEMP_NOM_ID AS VARCHAR(10)), 2) PERSISTED NOT NULL,
                        N_NAME VARCHAR(100),
                        P_NAME VARCHAR(100),
                        N_EMAIL VARCHAR(100),
                        LOGO VARBINARY(MAX),
                        VCOUNT INT,
                        APRV BIT DEFAULT 0,
                        APRV_NOM_ID VARCHAR(MAX),
                        CONSTRAINT [UQ_PRODUCT_NT_IDENTIFIER] UNIQUE ([N_IDENTIFIER])
                    );
                END

                -- Create PARTY table
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PARTY')
                BEGIN
                    CREATE TABLE PARTY (
                        PA_ID INT IDENTITY(1,1) PRIMARY KEY,
                        P_IDENTIFIER AS 'PT-' + RIGHT('000' + CAST(PA_ID AS VARCHAR(10)), 3) PERSISTED NOT NULL,
                        P_NAME VARCHAR(100),
                        LOGO VARBINARY(MAX),
                        CONSTRAINT [UQ_PRODUCT_P_IDENTIFIER] UNIQUE ([P_IDENTIFIER])
                    );
                END

                -- Create JOINS table
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'JOINS')
                BEGIN
                    CREATE TABLE JOINS (
                        PARTY_ID VARCHAR(6) NOT NULL,
                        NOMINEE_ID VARCHAR(6) NOT NULL,
                        JOIN_DATE DATE NOT NULL,
                        CONSTRAINT FK_PARTY_NOMINEE FOREIGN KEY (PARTY_ID) REFERENCES PARTY (P_IDENTIFIER),
                        CONSTRAINT FK_NOMINEE_PARTY FOREIGN KEY (NOMINEE_ID) REFERENCES NOMINEE (N_IDENTIFIER)
                    );
                END

                -- Create VOTES table
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'VOTES')
                BEGIN
                    CREATE TABLE VOTES (
                        VOTER_ID VARCHAR(6) NOT NULL,
                        NOMINEE_ID VARCHAR(6) NOT NULL,
                        CONSTRAINT FK_VOTER_NOMINEE FOREIGN KEY (VOTER_ID) REFERENCES VOTER (V_IDENTIFIER),
                        CONSTRAINT FK_NOMINEE_VOTER FOREIGN KEY (NOMINEE_ID) REFERENCES NOMINEE (N_IDENTIFIER)
                    );
                END

                -- Create ADMIN table
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ADMIN')
                BEGIN
                    CREATE TABLE ADMIN (
                        A_ID INT IDENTITY(1,1) PRIMARY KEY,
                        A_IDENTIFIER AS 'AT-' + RIGHT('000' + CAST(A_ID AS VARCHAR(10)), 3) PERSISTED NOT NULL,
                        A_NAME VARCHAR(100),
                        EMAIL VARCHAR(100),
                        PHONE VARCHAR(20),
                        CONSTRAINT [UQ_PRODUCT_A_IDENTIFIER] UNIQUE ([A_IDENTIFIER])
                    );
                END

                -- Create REGISTRARS table
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'REGISTRARS')
                BEGIN
                    CREATE TABLE REGISTRARS (
                        USER_ID VARCHAR(6) NOT NULL,
                        ADMIN_ID VARCHAR(6) NOT NULL,
                        ROLE VARCHAR(50),
                        CONSTRAINT FK_ADMIN_REGISTRAR FOREIGN KEY (ADMIN_ID) REFERENCES ADMIN (A_IDENTIFIER)
                    );
                END

                -- Create ELECTION table
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ELECTION')
                BEGIN
                    CREATE TABLE ELECTION (
                        E_ID INT IDENTITY(1,1) PRIMARY KEY,
                        E_IDENTIFIER AS 'ET-' + RIGHT('000' + CAST(E_ID AS VARCHAR(10)), 3) PERSISTED NOT NULL,
                        E_NAME VARCHAR(100),
                        TYPE VARCHAR(100),
                        S_DATE DATE,
                        E_DATE DATE,
                        R_DOC VARBINARY(MAX),
                        CONSTRAINT [UQ_PRODUCT_E_IDENTIFIER] UNIQUE ([E_IDENTIFIER])
                    );
                END

                -- Create PARTICIPATES table
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PARTICIPATES')
                BEGIN
                    CREATE TABLE PARTICIPATES (
                        ELECTION_ID VARCHAR(6) NOT NULL,
                        NOMINEE_ID VARCHAR(6) NOT NULL,
                        APRV BIT DEFAULT 0,
                        R_DOC VARBINARY(MAX),
                        CONSTRAINT FK_PARTICIPATE_NOMINEE FOREIGN KEY (ELECTION_ID) REFERENCES ELECTION (E_IDENTIFIER),
                        CONSTRAINT FK_NOMINEE_PARTICIPATE FOREIGN KEY (NOMINEE_ID) REFERENCES NOMINEE (N_IDENTIFIER)
                    );
                END

                -- Create REGULATES table
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'REGULATES')
                BEGIN
                    CREATE TABLE REGULATES (
                        ELECTION_ID VARCHAR(6) NOT NULL,
                        ADMIN_ID VARCHAR(6) NOT NULL,
                        CONSTRAINT FK_REGULATE_ADMIN FOREIGN KEY (ELECTION_ID) REFERENCES ELECTION (E_IDENTIFIER),
                        CONSTRAINT FK_ADMIN_REGULATE FOREIGN KEY (ADMIN_ID) REFERENCES ADMIN (A_IDENTIFIER)
                    );
                END

                -- Create LOGIN table
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'LOGIN')
                BEGIN
                    CREATE TABLE LOGIN (
                        UID VARCHAR(6) NOT NULL,
                        PASSWORD VARCHAR(MAX),
                        ROLE VARCHAR(50)
                    );
                END

                -- Create REJECTIONS table
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'REJECTIONS')
                BEGIN
                    CREATE TABLE REJECTIONS (
                        UID VARCHAR(6) NOT NULL,
                        REASONS VARCHAR(MAX),
                        ADMIN_ID VARCHAR(6) NOT NULL,
                        CONSTRAINT FK_ADMIN_REJECTION FOREIGN KEY (ADMIN_ID) REFERENCES ADMIN (A_IDENTIFIER)
                    );
                END
            ";

            using (SqlCommand command = new SqlCommand(createTablesQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        public static string GetConnectionString()
        {
            return connectionString!;
        }
    }
}

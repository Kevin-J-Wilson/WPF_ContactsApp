using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace WPF_Contacts
{
    public class DatabaseHelper
    {
        private string _connectionString;
        private bool _isConnected = false;
        private SqlConnection _dbConnection;
        private SqlCommand _sqlCommand;

        public string ConnectionString { get {return _connectionString;} }//end prop

        public bool IsConnected { get {return GetCurrentConnectionStatus();} }//end prop


        public DatabaseHelper(string connectionString, bool connectNow = true)
        {
            _connectionString = connectionString;

            if (connectNow)
            {
                _dbConnection = new SqlConnection(_connectionString);
            }
        }//end prop

        public bool Connect()
        {
            try { 
                _dbConnection = new SqlConnection(_connectionString);
                _isConnected = true;
            } catch {
                _isConnected = false;
            }
            return _isConnected;
        }//end connect bool


        public object[][] ExecuteReader(string sqlStatement)
        {
            SqlDataReader queryReturnData = null;
            object[][] returnData = null;


            try
            {
                if (IsConnected)
                {
                    _dbConnection.Open();
                    _sqlCommand = new SqlCommand(sqlStatement, _dbConnection);
                    queryReturnData = _sqlCommand.ExecuteReader();
                    returnData = ConvertDataReaderto2DArray(queryReturnData);
                    _dbConnection.Close();
                }//end if

            }
            catch (SqlException)
            {
                throw new Exception("Invalid SQL");
            }//end try catch block
           
            
            return returnData;
        }//end reader object

        public int ExecuteNonQuery(string sqlStatement)
        {
            int recordsAffected = -1;
            try
            {
                if (IsConnected)
                {   _dbConnection.Open();
                    _sqlCommand = new SqlCommand(sqlStatement, _dbConnection);
                    recordsAffected = _sqlCommand.ExecuteNonQuery();
                    _dbConnection.Close();
                }
            }
            catch(SqlException)
            {
                throw new Exception("Invalid SQL");
            }
            return recordsAffected;
        }//end non query method

        public int GetRecordCount(string tableName)
        {
            string query = $"SELECT COUNT(*) FROM {tableName}";
            int count = 0;
            using (SqlConnection thisConnection = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmdCount = new SqlCommand(query, thisConnection))
                {
                    thisConnection.Open();
                    count = (int)cmdCount.ExecuteScalar();
                }
            }
            return count;

        }//end get record count

        public bool FlushTable(string tableName)
        {
            string query = $"DELETE FROM {tableName}";
            using (SqlConnection thisConnection = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, thisConnection))
                {
                    thisConnection.Open();
                    cmd.ExecuteNonQuery();
                    thisConnection.Close();
                }
            }
            return true;
        }//end method

        public bool DeleteTable(string tableName)
        {
            string query = $"DROP TABLE {tableName}";
            bool tableDeleted = false;
            using (SqlConnection thisConnection = new SqlConnection(_connectionString))
            {
                using (SqlCommand dropTable = new SqlCommand(query, thisConnection))
                {
                    thisConnection.Open();
                    dropTable.ExecuteNonQuery();
                    tableDeleted = true;
                    thisConnection.Close();
                }
            }

            return tableDeleted;
        }//end method

        public bool AddTable(string tableName)
        {
            string query = @"CREATE TABLE [dbo].[newTable]( [id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,[FirstName] [nchar](16) NULL,[LastName] [nchar](16) NULL,)";
                            
            bool tableAdded = false;
            using (SqlConnection thisConnection = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmdAddTable = new SqlCommand(query, thisConnection))
                {
                    thisConnection.Open();              
                    cmdAddTable.ExecuteNonQuery();
                    if (cmdAddTable.ExecuteNonQuery != null) tableAdded = true;
                    thisConnection.Close();
                }
            }
            return tableAdded;
        }//end method

        public bool Connect(string newConnectionString)
        {//overload to connect to new db
            string query = @"CREATE DATABASE newDatabase";
            newConnectionString = @"Data Source=MYZENBOOK\SQLEXPRESS;Initial Catalog = NewDatabase; Integrated Security = True; Pooling=False";

            bool dataBaseAdded = false;
            using (SqlConnection thisConnection = new SqlConnection(newConnectionString))
            {
                using (SqlCommand cmdDataBase = new SqlCommand(query, thisConnection))
                {
                    thisConnection.Open();
                    thisConnection.Close();
                }
            }
            return dataBaseAdded;
        }//end method


        private object[][] ConvertDataReaderto2DArray(SqlDataReader data)
        {  
            object[,] returnData = null;
            List<object[]> lstRows = new List<object[]>();

     
            while (data.Read())
            {
                object[] newRow = new object[data.FieldCount];

                for (int fieldIndex = 0; fieldIndex < data.FieldCount; fieldIndex++)
                {
                    newRow[fieldIndex] = data[fieldIndex];
                }
                lstRows.Add(newRow);
            }
          
                return lstRows.ToArray();
        }//end convert to 2d array

        private bool GetCurrentConnectionStatus()
        {
            bool pastConnection = _dbConnection != null;
            bool currentlyConnected = false;

            if (pastConnection == true) 
            {
                currentlyConnected = _dbConnection.State != System.Data.ConnectionState.Broken;
            }//end if

            return pastConnection && currentlyConnected;
        }


        static void PrintArray(object[] array)
        {
            foreach (object[] row in array)
            {
                foreach (object col in row)
                {
                    Console.WriteLine(col + " ");

                }
            }
        }//end print array method

    }//end class
}//end namespace

using System.Data;

namespace DBManager
{
    public enum DataProvider
    {
        Oracle, SqlServer, OleDb, Odbc
    }

    // Interface for Signing the Methods
    interface IDBManager
    {
        DataProvider ProviderType
        {
            get;
            set;
        }

        string ConnectionString
        {
            get;
            set;
        }

        
        IDbConnection Connection
        {
            get;
        }
        IDbTransaction Transaction
        {
            get;
        }

        IDataReader DataReader
        {
            get;
        }
        IDbCommand Command
        {
            get;
        }

        IDbDataParameter[] Parameters
        {
            get;
        }
        //Connection Open
        void Open();

        // Database Begin Transaction 
        void BeginTransaction();

        // Database Commit Trnsaction
        void CommitTransaction();

        // Database RollBackTransaction
        void RollBackTransaction();

        // creating parameters for the command
        void CreateParameters(int paramsCount);

        // Adding the parameters to the Command
        void AddParameters(int index, string paramName, object objValue,string Direction);

        // Initialising Datareader for fetching Data
        IDataReader ExecuteReader(CommandType commandType, string
        commandText);

        // Initialising Dataset for fetching Data
        DataSet ExecuteDataSet(CommandType commandType, string
        commandText);

        // initialising execute Scalar for fetching a single Data
        object ExecuteScalar(CommandType commandType, string commandText);

        // initialising ExecuteNonquery for performing Insert, Update, Delete Operations 
        int ExecuteNonQuery(CommandType commandType,string commandText);

        // initialising ExecuteNonquery for performing Insert, Update, Delete Operations  and give output Parameters
        object[] ExecuteNonQueryparam(CommandType commandType, string commandText);

        // Closing the Datareader
        void CloseReader();

        // Closing the Database Connection
        void Close();

        // Disposing the Database Connection
        void Dispose();
    }
}

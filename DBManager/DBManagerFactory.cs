using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;

namespace DBManager
{
    public sealed class DBManagerFactory
    {
        private DBManagerFactory(){}

        // Initialising the Database Connection based on the Provider Type
        public static IDbConnection GetConnection(DataProvider providerType)
        {
          IDbConnection iDbConnection = null;
          switch (providerType)
          {
            case DataProvider.SqlServer:
                iDbConnection = new SqlConnection();
                break;
            //case DataProvider.OleDb:
            //    iDbConnection = new OleDbConnection();
            //    break;
            case DataProvider.Odbc:
                iDbConnection = new OdbcConnection();
                break;
            //case DataProvider.Oracle:
            //    iDbConnection = new OracleConnection();
            //    break;
                default:
            return null;
        }
        return iDbConnection;
       }

       // Initialising the Command based on the Provider Type
        public static IDbCommand GetCommand(DataProvider providerType)
        {
            switch (providerType)
            {
                case DataProvider.SqlServer:
                    return new SqlCommand();
                //case DataProvider.OleDb:
                //    return new OleDbCommand();
                case DataProvider.Odbc:
                    return new OdbcCommand();
                //case DataProvider.Oracle:
                //  return new OracleCommand();
                default:
                    return null;
            }
        }
 
        // Initialising the DataAdapter based on the Provider Type
    public static IDbDataAdapter GetDataAdapter(DataProvider providerType)
    {
      switch (providerType)
      {
        case DataProvider.SqlServer:
          return new SqlDataAdapter();
        //case DataProvider.OleDb:
        //  return new OleDbDataAdapter();
        case DataProvider.Odbc:
          return new OdbcDataAdapter();
        //case DataProvider.Oracle:
        //  return new OracleDataAdapter();
        default:
          return null;
      }
    }
 
    //public static IDbTransaction GetTransaction(DataProvider providerType)
        public static IDbTransaction GetTransaction(IDbConnection Connection)
    {
        

      //IDbConnection iDbConnection =GetConnection(providerType); 
        IDbConnection iDbConnection = Connection;         
      IDbTransaction iDbTransaction =iDbConnection.BeginTransaction();
      return iDbTransaction;
    }
 
        // Initialising the Parameter based on the Database Provider Type
    public static IDataParameter GetParameter(DataProvider providerType)
    {
      IDataParameter iDataParameter = null;
      switch (providerType)
      {
        case DataProvider.SqlServer:
          iDataParameter = new SqlParameter();
          break;
        //case DataProvider.OleDb:
        //  iDataParameter = new OleDbParameter();
        //  break;
        case DataProvider.Odbc:
          iDataParameter = new OdbcParameter();
          break;
        //case DataProvider.Oracle:
        //  iDataParameter = new OracleParameter();
        //  break;
 
      }
      return iDataParameter;
    }

    // Initialising the Parameter Array based on the Database Provider Type
    public static IDbDataParameter[]GetParameters(DataProvider providerType,int paramsCount)
    {
      IDbDataParameter[]idbParams = new IDbDataParameter[paramsCount];
 
      switch (providerType)
      {
        case DataProvider.SqlServer:
          for (int i = 0; i < paramsCount;++i)
          {
            idbParams[i] = new SqlParameter();
          }
          break;
        //case DataProvider.OleDb:
        //  for (int i = 0; i < paramsCount;++i)
        //  {
        //    idbParams[i] = new OleDbParameter();
        //  }
        //  break;
        case DataProvider.Odbc:
          for (int i = 0; i < paramsCount;++i)
          {
            idbParams[i] = new OdbcParameter();
          }
          break;
        //case DataProvider.Oracle:
        //    for (int i = 0; i < paramsCount; ++i)
        //  {
        //    idbParams[i] = new OracleParameter();
        //  }
        //  break;
        default:
          idbParams = null;
          break;
      }
      return idbParams;
    }
  }
}

using System;
using System.Data;


namespace DBManager
{
    public sealed class DBManager : IDBManager, IDisposable 
    {
    private IDbConnection idbConnection;
    private IDataReader idataReader;
    private IDbCommand idbCommand;
    private DataProvider providerType;
    private IDbTransaction idbTransaction =null;
    private IDbDataParameter[]idbParameters =null;
    private string strConnection;
 
    public DBManager(){ 
    }
 
    public DBManager(DataProvider providerType)
    {
      this.providerType = providerType;
    }
 
    public DBManager(DataProvider providerType,string connectionString)
    {
      this.providerType = providerType;
      this.strConnection = connectionString;
    }
 
    public IDbConnection Connection
    {
      get
      {
        return idbConnection;
      }
    }
 
    public IDataReader DataReader
    {
      get
      {
        return idataReader;
      }
      set
      {
        idataReader = value;
      }
    }
 
    public DataProvider ProviderType
    {
      get
      {
        return providerType;
      }
      set
      {
        providerType = value;
      }
    }
 
    public string ConnectionString
    {
      get
      {
        return strConnection;
      }
      set
      {
        strConnection = value;
      }
    }
 
    public IDbCommand Command
    {
      get
      {
        return idbCommand;
      }
    }
 
    public IDbTransaction Transaction
    {
      get
      {
        return idbTransaction;
      }
    }
 
    public IDbDataParameter[]Parameters
    {
      get
      {
        return idbParameters;
      }
    }
 
    // Creating and opening a Databse connection based on the Provider Type
    public void Open()
    {
      idbConnection =
      DBManagerFactory.GetConnection(this.providerType);
      idbConnection.ConnectionString =this.ConnectionString;
      if (idbConnection.State !=ConnectionState.Open)
        idbConnection.Open();
      this.idbCommand =DBManagerFactory.GetCommand(this.ProviderType);
    }
 
    // Closing the existing open COnnection
    public void Close()
    {
      if (idbConnection.State !=ConnectionState.Closed)
        idbConnection.Close();
    }

    // Disposing the existing open COnnection
    public void Dispose()
    {
      GC.SuppressFinalize(this);
      this.Close();
      this.idbCommand.Parameters.Clear();
      this.idbParameters = null;
      this.idbCommand = null;
      this.idbTransaction = null;
      this.idbConnection = null;
    }

    // Disposing the existing open COnnection
    public void ClearParameters()
    {
        this.idbCommand.Parameters.Clear();
        this.idbParameters = null;
    }
 
   // Creating the numeber of parameters required for a Command
    public void CreateParameters(int paramsCount)
    {
       
      idbParameters = new IDbDataParameter[paramsCount];
      idbParameters =DBManagerFactory.GetParameters(this.ProviderType,paramsCount);
    }
   
    // Add the parameters to the Command
    public void AddParameters(int index, string paramName, object objValue, string Direction)
    {
      if (index < idbParameters.Length)
      {
          idbParameters[index].ParameterName = paramName;
          idbParameters[index].Value = objValue;
          switch (Direction)
          {
              case "I":
                  idbParameters[index].Direction = ParameterDirection.Input;
               //   idbParameters[index].ParameterName = paramName;
                 // idbParameters[index].Value = objValue;
                  break;
              case "IO":
                  idbParameters[index].Direction = ParameterDirection.InputOutput;
                 // idbParameters[index].ParameterName = paramName;
                 // idbParameters[index].Value = objValue;
                  break;
              case "O":
                  idbParameters[index].Direction = ParameterDirection.Output;
                  //idbParameters[index].ParameterName = paramName;                                    
                  //idbParameters[index].Value = null;
                  break;
              case "R":
                  idbParameters[index].Direction = ParameterDirection.ReturnValue;
                  //idbParameters[index].ParameterName = paramName;
                //  idbParameters[index].Value = null;
                  break;
          }         
       }
    }
 
        // Databse Begin Transaction
    public void BeginTransaction()
    {
      if (this.idbTransaction == null)
          //idbTransaction = DBManagerFactory.GetTransaction(this.ProviderType);
      idbTransaction = DBManagerFactory.GetTransaction(this.Connection);
        this.idbCommand.Transaction =idbTransaction;
    }
 
        // Database Commit Transaction
    public void CommitTransaction()
    {
      if (this.idbTransaction != null)
        this.idbTransaction.Commit();
      idbTransaction = null;
    }

        // Database RollBack Transaction
   public void RollBackTransaction()
   {
      if (this.idbTransaction != null)
        this.idbTransaction.Rollback();
      idbTransaction = null;
   }
        // Fetch Data Using Datareader
    public IDataReader ExecuteReader(CommandType commandType, string commandText)
    {
      this.idbCommand =DBManagerFactory.GetCommand(this.ProviderType);
      idbCommand.Connection = this.Connection;
      PrepareCommand(idbCommand, this.Connection, this.Transaction, commandType, commandText, this.Parameters);
      this.DataReader =idbCommand.ExecuteReader();
      idbCommand.Parameters.Clear();
      return this.DataReader;
    }

        
 
        // Close Datareader
    public void CloseReader()
    {
      if (this.DataReader != null)
        this.DataReader.Close();
    }
 
        // Attach Parameters to the Command
    private void AttachParameters(IDbCommand command,IDbDataParameter[]commandParameters)
    {
      foreach (IDbDataParameter idbParameter in commandParameters)
      {
        if ((idbParameter.Direction == ParameterDirection.InputOutput)
        &&
          (idbParameter.Value == null))
        {
          idbParameter.Value = DBNull.Value;
        }

      //  if ((idbParameter.Direction == ParameterDirection.Output)
      //&&
      //  (idbParameter.Value == null))
      //  {
      //      idbParameter.Value = DBNull.Value;
      //  }
        command.Parameters.Add(idbParameter);
      }
    }
 
        //Preparing the Database Command by passing the Provider Type, 
    private void PrepareCommand(IDbCommand command, IDbConnection connection,IDbTransaction transaction, CommandType commandType, string commandText,
      IDbDataParameter[]commandParameters)
    {
      command.Connection = connection;
      command.CommandText = commandText;
      command.CommandType = commandType;
 
      if (transaction != null)
      {
        command.Transaction = transaction;
      }
 
      if (commandParameters != null)
      {
        AttachParameters(command, commandParameters);
      }
    }

       
        // Command to execute Insert, Delete, Update Operations
    public int ExecuteNonQuery(CommandType commandType, string commandText)
    {
      this.idbCommand =DBManagerFactory.GetCommand(this.ProviderType);
      PrepareCommand(idbCommand,this.Connection, this.Transaction,
      commandType, commandText,this.Parameters);
      int returnValue =idbCommand.ExecuteNonQuery();
      idbCommand.Parameters.Clear();
      return returnValue;
    }

    // Command to execute Insert, Delete, Update Operations and return Output Parameters
    public object[] ExecuteNonQueryparam(CommandType commandType, string commandText)
    {
        this.idbCommand = DBManagerFactory.GetCommand(this.ProviderType);
        PrepareCommand(idbCommand, this.Connection, this.Transaction,
        commandType, commandText, this.Parameters);
         idbCommand.ExecuteNonQuery();
        //idbCommand.Parameters.Clear();
        return this.Parameters;
    }
  
        // Command to fetch a single value
    public object ExecuteScalar(CommandType commandType, string commandText)
    {
      this.idbCommand =DBManagerFactory.GetCommand(this.ProviderType);
      PrepareCommand(idbCommand,this.Connection, this.Transaction, commandType,commandText, this.Parameters);
      object returnValue = idbCommand.ExecuteScalar();
      idbCommand.Parameters.Clear();
      return returnValue;
    }
 
        // To Fetch data and return in a Dataset
    public DataSet ExecuteDataSet(CommandType commandType, string commandText)
    {
      this.idbCommand =DBManagerFactory.GetCommand(this.ProviderType);
      PrepareCommand(idbCommand,this.Connection, this.Transaction,
     commandType,
        commandText, this.Parameters);
      IDbDataAdapter dataAdapter =DBManagerFactory.GetDataAdapter
        (this.ProviderType);
      dataAdapter.SelectCommand = idbCommand;
      DataSet dataSet = new DataSet();

      dataAdapter.Fill(dataSet);
      idbCommand.Parameters.Clear();
      return dataSet;
    }

    }
}



namespace KI.RIS.DAL
{
    using System;
    using System.Data;
    using Microsoft.VisualBasic;
    using System.IO;
    using System.Data.SqlClient;
    using System.Data.OracleClient;
	using MySql.Data.MySqlClient;
    using System.Configuration;
    using System.Text;

    /// <summary>
    /// 
    /// </summary>
    public enum ProviderName
    {
        /// <summary>
        /// The SQL client
        /// </summary>
        SqlClient,
        /// <summary>
        /// The oracle client
        /// </summary>
        OracleClient,

		MySqlClient
    }

    /// <summary>
    /// 
    /// </summary>
    public enum DatabaseMode
    {
        /// <summary>
        /// The online
        /// </summary>
        Online,
        /// <summary>
        /// The offline
        /// </summary>
        Offline
    }

    /// <summary>
    /// 
    /// </summary>
    public class DALHelper
    {
        /// <summary>
        /// The _default conn STR name
        /// </summary>
        private static string _defaultConnStrName = "ConnString";
        /// <summary>
        /// The _secondary conn STR name
        /// </summary>
        private static string _secondaryConnStrName = "ConnString1";
        /// <summary>
        /// The _conn STR settings
        /// </summary>
        private static ConnectionStringSettings _connStrSettings;
        /// <summary>
        /// The _provider factory
        /// </summary>
        private static System.Data.Common.DbProviderFactory _providerFactory; // DbProviderFactory = Nothing
        // private static IDbConnection connection;
        #region  Properties

        /// <summary>
        /// Is SQL is checked the Provider name is "System.Data.SqlClient" if so 
        /// this will returns true else return false.
        /// </summary>
        /// <returns>bool</returns>
        /// public static bool IsSQL()
        private static bool IsSQL
        {
            get
            {
                if (_connStrSettings == null)
                {
                    //_connStrSettings = ConfigurationManager.ConnectionStrings[_connStrName];
                    InitializeVariables();
                }
                return _connStrSettings.ProviderName == "System.Data.SqlClient";
            }
        }
        /// <summary>
        /// Is Oracle is checked the Provider name is "System.Data.OracleClient" if so 
        /// this will returns true else return false.
        /// </summary>
        /// <returns>bool</returns>
        /// public static bool IsOracle()
        private static bool IsOracle
        {
            get
            {
                if (_connStrSettings == null)
                {
                    // _connStrSettings = ConfigurationManager.ConnectionStrings[_connStrName];
                    InitializeVariables();
                }
                return _connStrSettings.ProviderName == "System.Data.OracleClient";
            }
        }

		private static bool IsMySQL
		{
			get
			{
				if (_connStrSettings == null)
				{
					//_connStrSettings = ConfigurationManager.ConnectionStrings[_connStrName];
					InitializeVariables();
				}
				return _connStrSettings.ProviderName == "MySql.Data.MySqlClient";
			}
		}
        /// <summary>
        /// This Function Will Check the Connection is Sql Connection Or Not
        /// </summary>
        /// <param name="connection">IDbConnection</param>
        /// <returns>bool</returns>
        private static bool IsSqlConnection(IDbConnection connection)
        {
            return connection is System.Data.SqlClient.SqlConnection;
        }
        /// <summary>
        /// This Function Will Check the Connection is Oracle Connection Or Not
        /// </summary>
        /// <param name="connection">IDbConnection </param>
        /// <returns>bool</returns>
        private static bool IsOracleConnection(IDbConnection connection)
        {
            return connection is System.Data.OracleClient.OracleConnection;
        }

		private static bool IsMySqlConnection(IDbConnection connection)
		{
			return connection is MySql.Data.MySqlClient.MySqlConnection;
		}

        private static void InitializeVariables()
        {
            try
            {
                _connStrSettings = ConfigurationManager.ConnectionStrings[_defaultConnStrName];
                if (_connStrSettings == null)
                {
                    throw new Exception("Connection string not found in configuration file.");
                    // MessageBox.Show("Connection string not found in configuration file.", string.Empty, MessageBoxButtons.OK);
                }
                if (_connStrSettings.ProviderName.Trim() == string.Empty)
                {
                    throw new Exception("Data provider name not found.");
                    //MessageBox.Show("Data provider name not found.", string.Empty, MessageBoxButtons.OK);
                }
                if (_connStrSettings.ProviderName != "System.Data.SqlClient" & _connStrSettings.ProviderName != "System.Data.OracleClient")
					//& _connStrSettings.ProviderName != "MySql.Data.MySqlClient")
                {
                    if (_connStrSettings.ProviderName.ToUpper().Trim() == "SQL")
                    {
                        _connStrSettings = new ConnectionStringSettings(_defaultConnStrName, _connStrSettings.ConnectionString, "System.Data.SqlClient");
                    }
                    else if (_connStrSettings.ProviderName.ToUpper().Trim() == "ORACLE")
                    {
                        _connStrSettings = new ConnectionStringSettings(_defaultConnStrName, _connStrSettings.ConnectionString, "System.Data.OracleClient");
                    }
                    else if (_connStrSettings.ProviderName.ToUpper().Trim() == "SYSTEM.DATA.SQLCLIENT")
                    {
                        _connStrSettings = new ConnectionStringSettings(_defaultConnStrName, _connStrSettings.ConnectionString, "System.Data.SqlClient");
                    }
                    else if (_connStrSettings.ProviderName.ToUpper().Trim() == "SYSTEM.DATA.ORACLECLIENT")
                    {
                        _connStrSettings = new ConnectionStringSettings(_defaultConnStrName, _connStrSettings.ConnectionString, "System.Data.OracleClient");
                    }
					else if (_connStrSettings.ProviderName.ToUpper().Trim() == "MYSQL.DATA.MYSQLCLIENT")
					{
						_connStrSettings = new ConnectionStringSettings(_defaultConnStrName, _connStrSettings.ConnectionString, "MySql.Data.MySqlClient");
					}
                    else
                    {
                        throw new Exception("Data Provider <" + _connStrSettings.ProviderName.ToString() + "> is not valid.");
                        //MessageBox.Show("Data Provider <" + _connStrSettings.ProviderName + "> is not valid.", string.Empty, MessageBoxButtons.OK);
                    }
                }

                if ((!(_connStrSettings.ConnectionString.ToUpper().Contains("PASSWORD"))) & (!(_connStrSettings.ConnectionString.ToUpper().Contains("PWD"))))
                {
                    string strPassword = string.Empty;
                    strPassword = PasswordHandler.GetPassword();
                    if (strPassword == string.Empty)
                    {
                        throw new Exception("Database password not found !");
                        //MessageBox.Show("Database password not found !");
                    }
                    _connStrSettings = new ConnectionStringSettings(_connStrSettings.Name, _connStrSettings.ConnectionString + ";Password=" + strPassword, _connStrSettings.ProviderName);
                }
                CreateProviderFactory(_connStrSettings.ProviderName);
            }
            catch
            {
                throw;
            }
        }

        public static string GetPassword(string ConnectionString, string PFileName)
        {
            try
            {
                string strPassword = string.Empty;
                if ((!(ConnectionString.ToUpper().Contains("PASSWORD"))) & (!(ConnectionString.ToUpper().Contains("PWD"))))
                {
                    if (Convert.ToString(PFileName) != string.Empty && File.Exists(PFileName))
                    {
                        strPassword = PasswordHandler.GetPasswordFromFile(PFileName);
                    }
                    else
                    {
                        //_errorMessage = "File " + strFileName + " not found !"
                        throw new Exception("File " + PFileName + " not found !");
                    }
                }
                else
                {
                    if (ConnectionString.ToUpper().Contains("PASSWORD"))
                    {
                        int startIndex = ConnectionString.ToUpper().IndexOf("PASSWORD");
                        int endIndex = ConnectionString.ToUpper().Substring(startIndex + "PASSWORD=".Length).ToString().IndexOf(";");
                        if (endIndex == -1)
                        {
                            strPassword = ConnectionString.ToUpper().Substring(startIndex + "PASSWORD=".Length);
                        }
                        else
                        {
                            strPassword = ConnectionString.ToUpper().Substring(startIndex + "PASSWORD=".Length, endIndex);
                        }
                    }
                    else if (ConnectionString.ToUpper().Contains("PWD"))
                    {
                        int startIndex = ConnectionString.ToUpper().IndexOf("PWD");
                        int endIndex = ConnectionString.ToUpper().Substring(startIndex + "PWD=".Length).ToString().IndexOf(";");
                        if (endIndex == -1)
                        {
                            strPassword = ConnectionString.ToUpper().Substring(startIndex + "PWD=".Length);
                        }
                        else
                        {
                            strPassword = ConnectionString.ToUpper().Substring(startIndex + "PWD=".Length, endIndex);
                        }
                    }
                }
                return strPassword;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void CreateProviderFactory(string strProviderName)
        {
            _providerFactory = System.Data.Common.DbProviderFactories.GetFactory(strProviderName);
        }


        #region Public Properties

        /// <summary>
        /// Is SQL is checked the Provider name is "System.Data.SqlClient" if so 
        /// this will returns true else return false.
        /// </summary>
        /// <returns></returns>
        /// public static bool IsSQL()
        public static string ConnectionString
        {
            get
            {
                if (_connStrSettings == null)
                {
                    InitializeVariables();
                }
                return _connStrSettings.ConnectionString;
            }
        }

        /// <summary>
        /// This Function helps to Get the IDbConnection Object.
        /// </summary>
        /// <returns>Valid IDbConnection</returns>
        public static IDbConnection GetConnection()
        {
            IDbConnection connection = null;
            try
            {
                if (_connStrSettings == null)
                {
                    InitializeVariables();
                }
                if (_providerFactory == null && _connStrSettings != null)
                {
                    CreateProviderFactory(_connStrSettings.ProviderName);
                }
                //if (connection  == null)
                //{
                //create connection object if Connection is not present
                connection = _providerFactory.CreateConnection();
                connection.ConnectionString = _connStrSettings.ConnectionString;
                //}
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                return connection;
            }
            catch
            {
                connection = null;
                throw;
            }
        }


        /// <summary>
        /// Get a new IDbConnection Open connection.
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="ProviderName"></param>
        /// <returns></returns>
        public static IDbConnection GetConnection(string ConnectionString, ProviderName ProviderName)
        {
            IDbConnection objConnection = null;
            ConnectionStringSettings objConnStrSettings = null;
            System.Data.Common.DbProviderFactory objProviderFactory = null;
            try
            {
                if ((!(ConnectionString.ToUpper().Contains("PASSWORD"))) & (!(ConnectionString.ToUpper().Contains("PWD"))))
                {
                    string strPassword = string.Empty;
                    strPassword = PasswordHandler.GetPassword();
                    if (strPassword == string.Empty)
                    {
                        throw new Exception("Database password not found !");
                        //MessageBox.Show("Database password not found !");
                    }
                    ConnectionString += ";Password=" + strPassword;
                }
                switch (ProviderName)
                {
                    case ProviderName.SqlClient:
                        objConnStrSettings = new ConnectionStringSettings("SqlConnection", ConnectionString, "System.Data.SqlClient");
                        break;
                    case ProviderName.OracleClient:
                        objConnStrSettings = new ConnectionStringSettings("OracleConnection", ConnectionString, "System.Data.OracleClient");
                        break;
                    default:
                        throw new Exception("Invalid Provider Name.");
                }
                CreateProviderFactory(objConnStrSettings.ProviderName);
                objConnection = objProviderFactory.CreateConnection();
                objConnection.ConnectionString = objConnStrSettings.ConnectionString;
                if (objConnection.State == ConnectionState.Closed)
                {
                    objConnection.Open();
                }
            }
            catch
            {
                throw;
            }
            return objConnection;
        }
        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <param name="objDatabaseMode">The obj database mode.</param>
        /// <returns></returns>
        public static IDbConnection GetConnection(DatabaseMode objDatabaseMode)
        {
            IDbConnection con = null;
            if (objDatabaseMode == DatabaseMode.Offline)
            {
                //con=GetConnection(ConnectionString)
                con = GetConnectionBy(Convert.ToString(ConfigurationManager.ConnectionStrings["ConnString1"]), Convert.ToString(ConfigurationManager.ConnectionStrings["MedilogicsPFileLocation1"]));
            }
            else if (objDatabaseMode == DatabaseMode.Online)
            {
                con = GetConnection();
            }
            return con;
        }

        private static IDbConnection GetConnectionBy(string ConnectionString, string Pfilelocation)
        {
            IDbConnection objConnection = null;
            ConnectionStringSettings objConnStrSettings = null;
            System.Data.Common.DbProviderFactory objProviderFactory = null;
            try
            {
                if ((!(ConnectionString.ToUpper().Contains("PASSWORD"))) & (!(ConnectionString.ToUpper().Contains("PWD"))))
                {
                    string strPassword = string.Empty;
                    strPassword = PasswordHandler.GetPasswordForFirstTime(Pfilelocation);
                    if (strPassword == string.Empty)
                    {
                        throw new Exception("Database password not found !");
                        //MessageBox.Show("Database password not found !");
                    }
                    ConnectionString += ";Password=" + strPassword;
                }
                //switch (ProviderName)
                //{
                //    case ProviderName.SqlClient:
                //        objConnStrSettings = new ConnectionStringSettings("SqlConnection", ConnectionString, "System.Data.SqlClient");
                //        break;
                //    case ProviderName.OracleClient:
                //        objConnStrSettings = new ConnectionStringSettings("OracleConnection", ConnectionString, "System.Data.OracleClient");
                //        break;
                //    default:
                //        throw new Exception("Invalid Provider Name.");
                //}
                objConnStrSettings = new ConnectionStringSettings("OracleConnection", ConnectionString, "System.Data.OracleClient");


                CreateProviderFactory(objConnStrSettings.ProviderName);
                objConnection = objProviderFactory.CreateConnection();
                objConnection.ConnectionString = objConnStrSettings.ConnectionString;
                if (objConnection.State == ConnectionState.Closed)
                {
                    objConnection.Open();
                }
            }
            catch
            {
                throw;
            }
            return objConnection;
        }

        ///// <summary>
        ///// This Function helps to Start the transaction Object.
        ///// </summary>
        ///// <returns>Valid IDbConnection</returns>
        public static IDbTransaction GetTransaction()
        {
            IDbTransaction trans = null;
            IDbConnection cn = null;
            try
            {
                cn = GetConnection();
                if ((cn != null) && (cn.State == ConnectionState.Open))
                {
                    trans = cn.BeginTransaction();
                }
                return trans;
            }
            catch
            {
                CloseDB(cn);
                throw;
            }
        }
        ///// <summary>
        ///// This Function helps to Start the transaction Object.
        ///// </summary>
        ///// <returns>Valid IDbConnection</returns>
        public static IDbTransaction GetTransactionScope()
        {
            
            IDbTransaction trans = null;
            IDbConnection cn = null;
            try
            {
                cn = GetConnection();
                if ((cn != null) && (cn.State == ConnectionState.Open))
                {
                    trans = cn.BeginTransaction();
                }
                return trans;
            }
            catch
            {
                CloseDB(cn);
                throw;
            }
        }
        /// <summary>
        /// This Function helps to Get the IDbConnection Object.
        /// </summary>
        /// <returns>Valid IDbConnection</returns>
        public static IDbTransaction GetTransaction(string ConnectionString, ProviderName ProviderName)
        {
            IDbTransaction trans = null;
            IDbConnection cn = null;
            try
            {
                cn = GetConnection(ConnectionString, ProviderName);
                if ((cn != null) && (cn.State == ConnectionState.Open))
                {
                    trans = cn.BeginTransaction();
                }
                return trans;
            }
            catch
            {
                CloseDB(cn);
                throw;
            }
        }

      

        /// <summary>
        /// This will Close and Dispose the Connection Object.
        /// </summary>
        /// <param name="cn"></param>
        public static void CloseDB(IDbConnection cn)
        {
            try
            {
                if (cn != null)
                {
                    cn.Close();
                    cn.Dispose();
                    cn = null;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// This will dispose the Comit Or Rollaback the passing transaction and Dispose that Transaction Object
        /// </summary>
        /// <param name="tran">IDbTransaction Object</param>
        /// <param name="DBStatus">DatabaseOpertion Status, ie if you wants to Commit then the status should be
        /// true. if you wants to rolllback the status should be false.
        /// </param>
        public static void CloseDB(IDbTransaction tran, bool DBStatus)
        {
            try
            {
                if (tran != null)
                {
                    IDbConnection TranCon = tran.Connection;
                    if (DBStatus)
                    {
                        tran.Commit();
                    }
                    else
                    {
                        tran.Rollback();
                    }
                    DALHelper.CloseDB(TranCon);
                    tran.Dispose();
                    tran = null;
                }
            }
            catch
            {
                throw;
            }
        }
        ///// <summary>
        ///// This will dispose the Transaction Object
        ///// </summary>
        ///// <param name="tran"></param>
        ///// <param name="DBStatus"></param>
        //public static void CloseDB(IDbTransaction tran)
        //{
        //    try
        //    {
        //        if (tran != null)
        //        {
        //            tran.Dispose();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public static IDbDataParameter CreateParameter(string ParameterName, DbType ObjDBtype, ParameterDirection ParamDirection, Object value)
        {
            return CreateParameter(ParameterName, ObjDBtype, 0, ParamDirection, value);
        }
        public static IDbDataParameter CreateParameter(string ParameterName, DbType ObjDBtype, ParameterDirection ParamDirection)
        {
            return CreateParameter(ParameterName, ObjDBtype, 0, ParamDirection);
        }
        public static IDbDataParameter CreateParameter(string ParameterName, DbType ObjDBtype, int size, ParameterDirection ParamDirection)
        {
            return CreateParameter(ParameterName, ObjDBtype, size, ParamDirection, null);
        }
        public static IDbDataParameter CreateParameter(string ParameterName, DbType ObjDBtype, int size, ParameterDirection ParamDirection, Object value)
        {
            if (_connStrSettings == null)
            {
                InitializeVariables();
            }
            IDbDataParameter objParam = GetConnection().CreateCommand().CreateParameter();
            objParam.ParameterName = ParameterName;
            objParam.DbType = ObjDBtype;
            objParam.Size = size;
            if (Convert.ToString(value) != "")
            {
                objParam.Value = value;
            }
            else
            {
                objParam.Value = DBNull.Value;
            }

            objParam.Direction = ParamDirection;
            return objParam;
        }

        #endregion

        #endregion

        #region Common

        private static System.Data.SqlClient.SqlParameter[] GetSqlParameters(params IDbDataParameter[] commandParameters)
        {
            if (commandParameters == null) return null;
            System.Data.SqlClient.SqlParameter[] sqlParam = new System.Data.SqlClient.SqlParameter[commandParameters.Length];
            System.Data.SqlClient.SqlParameter param = null;
            int intCount = 0;
            foreach (IDbDataParameter ObjDbParameter in commandParameters)
            {
                param = (System.Data.SqlClient.SqlParameter)ObjDbParameter;
                sqlParam[intCount] = param;
                intCount += 1;
            }
            return sqlParam;
        }

        private static OracleParameter[] GetOracleParameters(params IDbDataParameter[] commandParameters)
        {
            if (commandParameters == null) return null;
            OracleParameter[] oracleParam = new OracleParameter[commandParameters.Length];
            OracleParameter param = null;
            int intCount = 0;
            foreach (IDbDataParameter ObjDbParameter in commandParameters)
            {
                param = (OracleParameter)ObjDbParameter;
                oracleParam[intCount] = param;
                intCount += 1;
            }

            return oracleParam;
        }
        private static MySqlParameter[] GetMySqlParameters(params IDbDataParameter[] commandParameters)
        {
            if (commandParameters == null) return null;
            MySqlParameter[] mySqlParam = new MySqlParameter[commandParameters.Length];
            MySqlParameter param = null;
            int intCount = 0;
            foreach (IDbDataParameter ObjDbParameter in commandParameters)
            {
                param = (MySqlParameter)ObjDbParameter;
                mySqlParam[intCount] = param;
                intCount += 1;
            }

            return mySqlParam;
        }
        #endregion

        #region ExecuteNonQuery
        /// <summary>
        /// Execute a SqlCommand (that returns no resultset and takes no parameters) against the database specified in 
        /// the connection string Default Read from the Application Configuration
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQueryByDirectConfig(CommandType commandType, string commandText)
        {
            int returnVal = 0;
            if (IsSQL)
            {
                //returnVal = SqlHelper.ExecuteNonQuery(ConnectionString, commandType, commandText);
                returnVal = ExecuteNonQuery(ConnectionString, ProviderName.SqlClient, commandType, commandText);
            }
            else if (IsOracle)
            {
                // returnVal = OracleHelper.ExecuteNonQuery(ConnectionString, commandType, commandText);
                returnVal = ExecuteNonQuery(ConnectionString, ProviderName.OracleClient, commandType, commandText);
            }
            else if (IsMySQL)
            {
                // returnVal = OracleHelper.ExecuteNonQuery(ConnectionString, commandType, commandText);
                returnVal = ExecuteNonQuery(ConnectionString, ProviderName.MySqlClient, commandType, commandText);
            }
            return returnVal;
        }

        /// <summary>
        /// Execute a SqlCommand (that returns no resultset and takes no parameters) against the database specified in 
        /// the connection string Default Read from the Application Configuration
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a Connection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
        {
            int returnVal = 0;
            if (IsSQL)
            {
                returnVal = ExecuteNonQuery(connectionString, ProviderName.SqlClient, commandType, commandText);
            }
            else if (IsOracle)
            {
                returnVal = ExecuteNonQuery(connectionString, ProviderName.OracleClient, commandType, commandText);
            }
            else if (IsMySQL)
            {
                returnVal = ExecuteNonQuery(connectionString, ProviderName.MySqlClient, commandType, commandText);
            }
            return returnVal;
        }

        /// <summary>
        /// Execute a SqlCommand (that returns no resultset and takes no parameters) against the database specified in 
        /// the connection string
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString,ProviderName.SqlClient, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a Connection</param>
        /// <param name="objProvider">Name of the Provider Sql, Oracle</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(string connectionString, ProviderName objProvider, CommandType commandType, string commandText)
        {
            int returnVal = 0;
            switch (objProvider)
            {
                case ProviderName.SqlClient:
                    returnVal = SqlHelper.ExecuteNonQuery(connectionString, commandType, commandText);
                    break;
                case ProviderName.OracleClient:
                    returnVal = OracleHelper.ExecuteNonQuery(connectionString, commandType, commandText);
                    break;
                case ProviderName.MySqlClient:
                    returnVal = OracleHelper.ExecuteNonQuery(connectionString, commandType, commandText);
                    break;
                default:
                    break;

            }
            return returnVal;
        }

        /// <summary>
        /// Execute a Command (that returns no resultset) against the database specified in the connection string 
        /// using the provided parameters
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(CommandType.StoredProcedure, "PublishOrders", new IDbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of IDbDataParameter used to execute the command</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQueryByDirectConfig(CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            int returnVal = 0;
            if (IsSQL)
            {
                returnVal = ExecuteNonQuery(ConnectionString, ProviderName.SqlClient, commandType, commandText, commandParameters);
            }
            else if (IsOracle)
            {
                returnVal = ExecuteNonQuery(ConnectionString, ProviderName.OracleClient, commandType, commandText, commandParameters);
            }
            else if (IsMySQL)
            {
                returnVal = ExecuteNonQuery(ConnectionString, ProviderName.MySqlClient, commandType, commandText, commandParameters);
            }
            return returnVal;
        }

        /// <summary>
        /// Execute a Command (that returns no resultset) against the database specified in the connection string 
        /// using the provided parameters
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(CommandType.StoredProcedure, "PublishOrders", new IDbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a Connection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of IDbDataParameter used to execute the command</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            int returnVal = 0;
            if (IsSQL)
            {
                returnVal = ExecuteNonQuery(connectionString, ProviderName.SqlClient, commandType, commandText, commandParameters);
            }
            else if (IsOracle)
            {
                returnVal = ExecuteNonQuery(connectionString, ProviderName.OracleClient, commandType, commandText, commandParameters);
            }
            else if (IsMySQL)
            {
                returnVal = ExecuteNonQuery(connectionString, ProviderName.MySqlClient, commandType, commandText, commandParameters);
            }
            return returnVal;
        }

        /// <summary>
        /// Execute a Command (that returns no resultset) against the database specified in the connection string 
        /// using the provided parameters
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString,ProviderName.SqlClient, CommandType.StoredProcedure, "PublishOrders", new IDbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a Connection</param>
        /// <param name="objProvider">Name of the Provider Sql, Oracle</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of IDbDataParameter used to execute the command</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(string connectionString, ProviderName objProvider, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            int returnVal = 0;
            switch (objProvider)
            {
                case ProviderName.SqlClient:
                    returnVal = SqlHelper.ExecuteNonQuery(connectionString, commandType, commandText, GetSqlParameters(commandParameters));
                    break;
                case ProviderName.OracleClient:
                    returnVal = OracleHelper.ExecuteNonQuery(connectionString, commandType, commandText, GetOracleParameters(commandParameters));
                    break;
                case ProviderName.MySqlClient:
                    returnVal = MySqlHelper.ExecuteNonQuery(connectionString, commandType, commandText, GetMySqlParameters(commandParameters));
                    break;
                default:
                    break;
            }
            return returnVal;
        }

        /// <summary>
        /// Execute a Command (that returns no resultset) against the specified IDbConnection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new IDbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">A valid IDbConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        public static int ExecuteNonQuery(IDbConnection connection, CommandType commandType, string commandText)
        {
            int returnVal = 0;
            if (IsSqlConnection(connection))
            {
                returnVal = SqlHelper.ExecuteNonQuery((SqlConnection)connection, commandType, commandText);
            }
            else if (IsOracleConnection(connection))
            {
                returnVal = OracleHelper.ExecuteNonQuery((OracleConnection)connection, commandType, commandText);
            }
            else if (IsMySqlConnection(connection))
            {
                returnVal = MySqlHelper.ExecuteNonQuery((MySqlConnection)connection, commandType, commandText);
            }
            return returnVal;
        }

        /// <summary>
        /// Execute a Command (that returns no resultset) against the specified IDbConnection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new IDbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">A valid IDbConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of IDbDataParameter used to execute the command</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(IDbConnection connection, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            int returnVal = 0;
            if (IsSqlConnection(connection))
            {
                returnVal = SqlHelper.ExecuteNonQuery((SqlConnection)connection, commandType, commandText, GetSqlParameters(commandParameters));
            }
            else if (IsOracleConnection(connection))
            {
                returnVal = OracleHelper.ExecuteNonQuery((OracleConnection)connection, commandType, commandText, GetOracleParameters(commandParameters));
            }
            else if (IsMySqlConnection(connection))
            {
                returnVal = MySqlHelper.ExecuteNonQuery((MySqlConnection)connection, commandType, commandText, GetMySqlParameters(commandParameters));
            }
            return returnVal;
        }

        /// <summary>
        /// Execute a Command (that returns no resultset) against the specified IDbTransaction
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new IDbDataParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">A valid IDbTransaction</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(IDbTransaction transaction, CommandType commandType, string commandText)
        {
            int returnVal = 0;
            if (IsSqlConnection(transaction.Connection))
            {
                returnVal = SqlHelper.ExecuteNonQuery((SqlTransaction)transaction, commandType, commandText);
            }
            else if (IsOracleConnection(transaction.Connection))
            {
                returnVal = OracleHelper.ExecuteNonQuery((OracleTransaction)transaction, commandType, commandText);
            }
            else if (IsMySqlConnection(transaction.Connection))
            {
                returnVal = MySqlHelper.ExecuteNonQuery((MySqlTransaction)transaction, commandType, commandText);
            }
            return returnVal;
        }

        /// <summary>
        /// Execute a Command (that returns no resultset) against the specified IDbTransaction
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new IDbDataParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">A valid IDbTransaction</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of IDbDataParameter used to execute the command</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(IDbTransaction transaction, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            int returnVal = 0;
            if (IsSqlConnection(transaction.Connection))
            {
                returnVal = SqlHelper.ExecuteNonQuery((SqlTransaction)transaction, commandType, commandText, GetSqlParameters(commandParameters));
            }
            else if (IsOracleConnection(transaction.Connection))
            {
                returnVal = OracleHelper.ExecuteNonQuery((OracleTransaction)transaction, commandType, commandText, GetOracleParameters(commandParameters));
            }
            else if(IsMySqlConnection(transaction.Connection))
            {
                returnVal = MySqlHelper.ExecuteNonQuery((MySqlTransaction)transaction, commandType, commandText, GetMySqlParameters(commandParameters));
            }
            return returnVal;
        }
        #endregion

        #region Fetching Methods

        /// <summary>
        /// Execute a Command (that returns a resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  FillDataset(CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
        /// <param name="tableNames">This array will be used to create table mappings allowing the DataTables to be referenced
        /// by a user defined name (probably the actual table name)
        /// </param>
        public static void FillDatasetByDirectConfig(CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            if (IsSQL)
            {
                FillDataset(ConnectionString, ProviderName.SqlClient, commandType, commandText, dataSet, tableNames);
            }
            else if (IsOracle)
            {
                FillDataset(ConnectionString, ProviderName.OracleClient, commandType, commandText, dataSet, tableNames);
            }
            else if (IsMySQL)
            {
                FillDataset(ConnectionString, ProviderName.MySqlClient, commandType, commandText, dataSet, tableNames);
            }
        }

        /// <summary>
        /// Execute a Command (that returns a resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  FillDataset(CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a Connection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
        /// <param name="tableNames">This array will be used to create table mappings allowing the DataTables to be referenced
        /// by a user defined name (probably the actual table name)
        /// </param>
        public static void FillDataset(string connectionString, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            if (IsSQL)
            {
                FillDataset(connectionString, ProviderName.SqlClient, commandType, commandText, dataSet, tableNames);
            }
            else if (IsOracle)
            {
                FillDataset(connectionString, ProviderName.OracleClient, commandType, commandText, dataSet, tableNames);
            }
            else if (IsMySQL)
            {
                FillDataset(connectionString, ProviderName.MySqlClient, commandType, commandText, dataSet, tableNames);
            }
        }


        /// <summary>
        /// Execute a Command (that returns a resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  FillDataset(connString, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a Connection</param>
        /// <param name="objProvider">The Type of Provider</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
        /// <param name="tableNames">This array will be used to create table mappings allowing the DataTables to be referenced
        /// by a user defined name (probably the actual table name)
        /// </param>
        public static void FillDataset(string connectionString, ProviderName objProvider, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            switch (objProvider)
            {
                case ProviderName.SqlClient:
                    SqlHelper.FillDataset(connectionString, commandType, commandText, dataSet, tableNames);
                    break;
                case ProviderName.OracleClient:
                    OracleHelper.FillDataset(connectionString, commandType, commandText, dataSet, tableNames);
                    break;
                case ProviderName.MySqlClient:
                    OracleHelper.FillDataset(connectionString, commandType, commandText, dataSet, tableNames);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Execute a Command (that returns a resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  FillDataset(CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
        /// <param name="tableNames">This array will be used to create table mappings allowing the DataTables to be referenced
        /// by a user defined name (probably the actual table name)
        /// </param>
        public static void FillDatasetByDirectConfig(CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params IDbDataParameter[] commandParameters)
        {
            if (IsSQL)
            {
                FillDataset(ConnectionString, ProviderName.SqlClient, commandType, commandText, dataSet, tableNames, GetSqlParameters(commandParameters));
            }
            else if (IsOracle)
            {
                FillDataset(ConnectionString, ProviderName.OracleClient, commandType, commandText, dataSet, tableNames, GetOracleParameters(commandParameters));
            }
            else if (IsMySQL)
            {
                FillDataset(ConnectionString, ProviderName.MySqlClient, commandType, commandText, dataSet, tableNames, GetMySqlParameters(commandParameters));
            }
        }

        /// <summary>
        /// Execute a Command (that returns a resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  FillDataset(connString,CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a Connection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
        /// <param name="tableNames">This array will be used to create table mappings allowing the DataTables to be referenced
        /// by a user defined name (probably the actual table name)
        /// </param>
        public static void FillDataset(string connectionString, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params IDbDataParameter[] commandParameters)
        {
            if (IsSQL)
            {
                FillDataset(connectionString, ProviderName.SqlClient, commandType, commandText, dataSet, tableNames, GetSqlParameters(commandParameters));
            }
            else if (IsOracle)
            {
                FillDataset(connectionString, ProviderName.OracleClient, commandType, commandText, dataSet, tableNames, GetOracleParameters(commandParameters));
            }
            else if (IsMySQL)
            {
                FillDataset(connectionString, ProviderName.MySqlClient, commandType, commandText, dataSet, tableNames, GetMySqlParameters(commandParameters));
            }
        }

        /// <summary>
        /// Execute a Command (that returns a resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  FillDataset(connString,ProviderName.SqlClient, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a Connection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
        /// <param name="tableNames">This array will be used to create table mappings allowing the DataTables to be referenced
        /// by a user defined name (probably the actual table name)
        /// </param>
        public static void FillDataset(string connectionString, ProviderName objProvider, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params IDbDataParameter[] commandParameters)
        {
            switch (objProvider)
            {
                case ProviderName.SqlClient:
                    SqlHelper.FillDataset(connectionString, commandType, commandText, dataSet, tableNames, GetSqlParameters(commandParameters));
                    break;
                case ProviderName.OracleClient:
                    OracleHelper.FillDataset(connectionString, commandType, commandText, dataSet, tableNames, GetOracleParameters(commandParameters));
                    break;
                case ProviderName.MySqlClient:
                    MySqlHelper.FillDataset(connectionString, commandType, commandText, dataSet, tableNames, GetMySqlParameters(commandParameters));
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// Execute a stored procedure via a Command (that returns a resultset) against the specified IDbConnection 
        /// using the provided parameter values.  This method will query the database to discover the parameters for the 
        /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// 
        /// e.g.:  
        ///  FillDataset(conn, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="connection">A valid IDbConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
        /// <param name="tableNames">This array will be used to create table mappings allowing the DataTables to be referenced
        /// by a user defined name (probably the actual table name)
        /// </param>
        public static void FillDataset(IDbConnection connection, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            if (IsSqlConnection(connection))
            {
                SqlHelper.FillDataset((SqlConnection)connection, commandType, commandText, dataSet, tableNames);
            }
            else if (IsOracleConnection(connection))
            {
                OracleHelper.FillDataset((OracleConnection)connection, commandType, commandText, dataSet, tableNames);
            }
            else if (IsMySqlConnection(connection))
            {
                MySqlHelper.FillDataset((MySqlConnection)connection, commandType, commandText, dataSet, tableNames);
            }
        }


        /// <summary>
        /// Execute a stored procedure via a Command (that returns a resultset) against the specified IDbConnection 
        /// using the provided parameter values.  This method will query the database to discover the parameters for the 
        /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// 
        /// e.g.:  
        ///  FillDataset(conn, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="connection">A valid IDbConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
        /// <param name="tableNames">This array will be used to create table mappings allowing the DataTables to be referenced
        /// by a user defined name (probably the actual table name)
        /// </param>
        /// <param name="commandParameters">An array of IDbDataParameter used to execute the command</param>
        public static void FillDataset(IDbConnection connection, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params IDbDataParameter[] commandParameters)
        {
            if (IsSqlConnection(connection))
            {
                SqlHelper.FillDataset((SqlConnection)connection, commandType, commandText, dataSet, tableNames, GetSqlParameters(commandParameters));
            }
            else if (IsOracleConnection(connection))
            {
                OracleHelper.FillDataset((OracleConnection)connection, commandType, commandText, dataSet, tableNames, GetOracleParameters(commandParameters));
            }
            else if (IsMySqlConnection(connection))
            {
                MySqlHelper.FillDataset((MySqlConnection)connection, commandType, commandText, dataSet, tableNames, GetMySqlParameters(commandParameters));
            }
        }

        /// <summary>
        /// Execute a Command (that returns a resultset) against the specified IDbTransaction
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  FillDataset(trans, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="transaction">A valid IDbTransaction</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
        /// <param name="tableNames">This array will be used to create table mappings allowing the DataTables to be referenced
        /// by a user defined name (probably the actual table name)
        /// </param>
        public static void FillDataset(IDbTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            if (IsSqlConnection(transaction.Connection))
            {
                SqlHelper.FillDataset((System.Data.SqlClient.SqlTransaction)transaction, commandType, commandText, dataSet, tableNames);
            }
            else if (IsOracleConnection(transaction.Connection))
            {
                OracleHelper.FillDataset((OracleTransaction)transaction, commandType, commandText, dataSet, tableNames);
            }
            else if (IsMySqlConnection(transaction.Connection))
            {
                MySqlHelper.FillDataset((MySqlTransaction)transaction, commandType, commandText, dataSet, tableNames);
            }
        }

        /// <summary>
        /// Execute a Command (that returns a resultset) against the specified IDbTransaction
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  FillDataset(trans, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="transaction">A valid IDbTransaction</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
        /// <param name="tableNames">This array will be used to create table mappings allowing the DataTables to be referenced
        /// by a user defined name (probably the actual table name)
        /// </param>
        /// <param name="commandParameters">An array of IDbDataParameter used to execute the command</param>
        public static void FillDataset(IDbTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params IDbDataParameter[] commandParameters)
        {
            if (IsSqlConnection(transaction.Connection))
            {
                SqlHelper.FillDataset((System.Data.SqlClient.SqlTransaction)transaction, commandType, commandText, dataSet, tableNames, GetSqlParameters(commandParameters));
            }
            else if (IsOracleConnection(transaction.Connection))
            {
                OracleHelper.FillDataset((OracleTransaction)transaction, commandType, commandText, dataSet, tableNames, GetOracleParameters(commandParameters));
            }
            else if (IsMySqlConnection(transaction.Connection))
            {
                MySqlHelper.FillDataset((MySqlTransaction)transaction, commandType, commandText, dataSet, tableNames, GetMySqlParameters(commandParameters));
            }

        }

        /// <summary>
        /// Execute a Command (that returns a resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  DataSet ds = ExecuteDataset(CommandType.StoredProcedure, "GetOrders", new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        public static DataSet ExecuteDatasetByDirectConfig(CommandType commandType, string commandText)
        {
            DataSet dsExecuted = null;
            if (IsSQL)
            {
                dsExecuted = ExecuteDataset(ConnectionString, ProviderName.SqlClient, commandType, commandText);
            }
            else if (IsOracle)
            {
                dsExecuted = ExecuteDataset(ConnectionString, ProviderName.OracleClient, commandType, commandText);
            }
            else if (IsMySQL)
            {
                dsExecuted = ExecuteDataset(ConnectionString, ProviderName.MySqlClient, commandType, commandText);
            }
            return dsExecuted;
        }

        /// <summary>
        /// Execute a Command (that returns a resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  DataSet ds = ExecuteDataset(CommandType.StoredProcedure, "GetOrders", new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
        {
            DataSet dsExecuted = null;
            if (IsSQL)
            {
                dsExecuted = ExecuteDataset(connectionString, ProviderName.SqlClient, commandType, commandText);
            }
            else if (IsOracle)
            {
                dsExecuted = ExecuteDataset(connectionString, ProviderName.OracleClient, commandType, commandText);
            }
            else if (IsMySQL)
            {
                dsExecuted = ExecuteDataset(connectionString, ProviderName.MySqlClient, commandType, commandText);
            }
            return dsExecuted;
        }

        /// <summary>
        /// Execute a Command (that returns a resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  DataSet ds = ExecuteDataset(connString, ProviderName.SqlClient,CommandType.StoredProcedure, "GetOrders", new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a Connection</param>
        /// <param name="objProvider">Name of the Provider</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        public static DataSet ExecuteDataset(string connectionString, ProviderName objProvider, CommandType commandType, string commandText)
        {
            DataSet dsExecuted = null;
            switch (objProvider)
            {
                case ProviderName.SqlClient:
                    dsExecuted = SqlHelper.ExecuteDataset(connectionString, commandType, commandText);
                    break;
                case ProviderName.OracleClient:
                    dsExecuted = OracleHelper.ExecuteDataset(connectionString, commandType, commandText);
                    break;
                case ProviderName.MySqlClient:
                    dsExecuted = MySqlHelper.ExecuteDataset(connectionString, commandType, commandText);
                    break;
                default:
                    break;
            }
            return dsExecuted;
        }

        /// <summary>
        /// Execute a Command (that returns a resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  DataSet ds = ExecuteDataset(CommandType.StoredProcedure, "GetOrders", new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of IDbDataParameter used to execute the command</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        public static DataSet ExecuteDatasetByDirectConfig(CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            if (IsSQL)
            {
                return SqlHelper.ExecuteDataset(ConnectionString, commandType, commandText, GetSqlParameters(commandParameters));
            }
            else if (IsOracle)
            {
                return OracleHelper.ExecuteDataset(ConnectionString, commandType, commandText, GetOracleParameters(commandParameters));
            }
            else if (IsMySQL)
            {
                return MySqlHelper.ExecuteDataset(ConnectionString, commandType, commandText, GetMySqlParameters(commandParameters));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Execute a Command (that returns a resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders", new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a Connection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of IDbDataParameter used to execute the command</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            if (IsSQL)
            {
                return SqlHelper.ExecuteDataset(connectionString, commandType, commandText, GetSqlParameters(commandParameters));
            }
            else if (IsOracle)
            {
                return OracleHelper.ExecuteDataset(connectionString, commandType, commandText, GetOracleParameters(commandParameters));
            }
            else if (IsMySQL)
            {
                return MySqlHelper.ExecuteDataset(connectionString, commandType, commandText, GetMySqlParameters(commandParameters));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Execute a Command (that returns a resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders", new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a Connection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of IDbDataParameter used to execute the command</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        public static DataSet ExecuteDataset(string connectionString, ProviderName objProvider, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            DataSet dsExecute = null;
            switch (objProvider)
            {
                case ProviderName.SqlClient:
                    dsExecute = SqlHelper.ExecuteDataset(connectionString, commandType, commandText, GetSqlParameters(commandParameters));
                    break;
                case ProviderName.OracleClient:
                    dsExecute = OracleHelper.ExecuteDataset(connectionString, commandType, commandText, GetOracleParameters(commandParameters));
                    break;
                case ProviderName.MySqlClient:
                    dsExecute = MySqlHelper.ExecuteDataset(connectionString, commandType, commandText, GetMySqlParameters(commandParameters));
                    break;
                default:
                    break;
            }
            return dsExecute;
        }


        /// <summary>
        /// Execute a Command (that returns a resultset) against the specified Connection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="connection">A valid Connection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        public static DataSet ExecuteDataset(IDbConnection connection, CommandType commandType, string commandText)
        {
            if (IsSqlConnection(connection))
            {
                return SqlHelper.ExecuteDataset((System.Data.SqlClient.SqlConnection)connection, commandType, commandText);
            }
            else if (IsOracleConnection(connection))
            {
                return OracleHelper.ExecuteDataset((OracleConnection)connection, commandType, commandText);
            }
            else if (IsOracleConnection(connection))
            {
                return MySqlHelper.ExecuteDataset((MySqlConnection)connection, commandType, commandText);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Execute a Command (that returns a resultset) against the specified Connection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="connection">A valid Connection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of IDbDataParameter used to execute the command</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        public static DataSet ExecuteDataset(IDbConnection connection, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            if (IsSqlConnection(connection))
            {
                return SqlHelper.ExecuteDataset((System.Data.SqlClient.SqlConnection)connection, commandType, commandText, GetSqlParameters(commandParameters));
            }
            else if (IsOracleConnection(connection))
            {
                return OracleHelper.ExecuteDataset((OracleConnection)connection, commandType, commandText, GetOracleParameters(commandParameters));
            }
            else if (IsMySqlConnection(connection))
            {
                return MySqlHelper.ExecuteDataset((MySqlConnection)connection, commandType, commandText, GetMySqlParameters(commandParameters));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Execute a Command (that returns a resultset) against the specified IDbTransaction
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders", new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="transaction">A valid IDbTransaction</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        public static DataSet ExecuteDataset(IDbTransaction transaction, CommandType commandType, string commandText)
        {
            if (IsSqlConnection(transaction.Connection))
            {
                return SqlHelper.ExecuteDataset((System.Data.SqlClient.SqlTransaction)transaction, commandType, commandText);
            }
            else if (IsOracleConnection(transaction.Connection))
            {
                return OracleHelper.ExecuteDataset((OracleTransaction)transaction, commandType, commandText);
            }
            else if (IsMySqlConnection(transaction.Connection))
            {
                return MySqlHelper.ExecuteDataset((MySqlTransaction)transaction, commandType, commandText);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Execute a Command (that returns a resultset) against the specified IDbTransaction
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders", new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="transaction">A valid IDbTransaction</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of IDbDataParameter used to execute the command</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        public static DataSet ExecuteDataset(IDbTransaction transaction, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            if (IsSqlConnection(transaction.Connection))
            {
                return SqlHelper.ExecuteDataset((System.Data.SqlClient.SqlTransaction)transaction, commandType, commandText, GetSqlParameters(commandParameters));
            }
            else if (IsOracleConnection(transaction.Connection))
            {
                return OracleHelper.ExecuteDataset((OracleTransaction)transaction, commandType, commandText, GetOracleParameters(commandParameters));
            }
            else if (IsSqlConnection(transaction.Connection))
            {
                return MySqlHelper.ExecuteDataset((MySqlTransaction)transaction, commandType, commandText, GetMySqlParameters(commandParameters));
            }
            else
            {
                return null;
            }
        }

        #region Scalar

        /// <summary>
        /// Execute a SqlCommand (that returns a 1x1 resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount", new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a Connection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText)
        {
            object objReturn = null;
            if (IsSQL)
            {
                objReturn = ExecuteScalar(connectionString, ProviderName.SqlClient, commandType, commandText);
            }
            else if (IsOracle)
            {
                objReturn = ExecuteScalar(connectionString, ProviderName.OracleClient, commandType, commandText);
            }
            else if (IsMySQL)
            {
                objReturn = ExecuteScalar(connectionString, ProviderName.MySqlClient, commandType, commandText);
            }
            return objReturn;
        }

        /// <summary>
        /// Execute a SqlCommand (that returns a 1x1 resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int orderCount = (int)ExecuteScalar(CommandType.StoredProcedure, "GetOrderCount", new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
        public static object ExecuteScalarByDirectConfig(CommandType commandType, string commandText)
        {
            object objReturn = null;
            if (IsSQL)
            {
                objReturn = ExecuteScalar(ConnectionString, ProviderName.SqlClient, commandType, commandText);
            }
            else if (IsOracle)
            {
                objReturn = ExecuteScalar(ConnectionString, ProviderName.OracleClient, commandType, commandText);
            }
            else if (IsMySQL)
            {
                objReturn = ExecuteScalar(ConnectionString, ProviderName.MySqlClient, commandType, commandText);
            }
            return objReturn;
        }

        /// <summary>
        /// Execute a SqlCommand (that returns a 1x1 resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int orderCount = (int)ExecuteScalar(connString,ProviderName.SqlClient , CommandType.StoredProcedure, "GetOrderCount", new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a Connection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
        public static object ExecuteScalar(string connectionString, ProviderName objProvider, CommandType commandType, string commandText)
        {
            object objReturn = null;
            switch (objProvider)
            {
                case ProviderName.SqlClient:
                    objReturn = SqlHelper.ExecuteScalar(connectionString, commandType, commandText);
                    break;
                case ProviderName.OracleClient:
                    objReturn = OracleHelper.ExecuteScalar(connectionString, commandType, commandText);
                    break;
                case ProviderName.MySqlClient:
                    objReturn = MySqlHelper.ExecuteScalar(connectionString, commandType, commandText);
                    break;
                default:
                    break;
            }
            return objReturn;
        }

        /// <summary>
        /// Execute a SqlCommand (that returns a 1x1 resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount", new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a Connection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of IDbDataParameter used to execute the command</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            object objReturn = null;
            if (IsSQL)
            {
                objReturn = SqlHelper.ExecuteScalar(connectionString, commandType, commandText, GetSqlParameters(commandParameters));
            }
            else if (IsOracle)
            {
                objReturn = OracleHelper.ExecuteScalar(connectionString, commandType, commandText, GetOracleParameters(commandParameters));
            }
            else if (IsMySQL)
            {
                objReturn = MySqlHelper.ExecuteScalar(connectionString, commandType, commandText, GetMySqlParameters(commandParameters));
            }
            return objReturn;
        }
        /// <summary>
        /// Execute a SqlCommand (that returns a 1x1 resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int orderCount = (int)ExecuteScalar(CommandType.StoredProcedure, "GetOrderCount", new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of IDbDataParameter used to execute the command</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
        public static object ExecuteScalarByDirectConfig(CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            object objReturn = null;
            if (IsSQL)
            {
                objReturn = ExecuteScalar(ConnectionString, ProviderName.SqlClient, commandType, commandText, GetSqlParameters(commandParameters));
            }
            else if (IsOracle)
            {
                objReturn = ExecuteScalar(ConnectionString, ProviderName.OracleClient, commandType, commandText, GetOracleParameters(commandParameters));
            }
            else if (IsMySQL)
            {
                objReturn = ExecuteScalar(ConnectionString, ProviderName.MySqlClient, commandType, commandText, GetMySqlParameters(commandParameters));
            }
            return objReturn;
        }

        /// <summary>
        /// Execute a SqlCommand (that returns a 1x1 resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount", new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a Connection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of IDbDataParameter used to execute the command</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
        public static object ExecuteScalar(string connectionString, ProviderName objProvider, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            object objReturn = null;
            switch (objProvider)
            {
                case ProviderName.SqlClient:
                    objReturn = SqlHelper.ExecuteScalar(connectionString, commandType, commandText, GetSqlParameters(commandParameters));
                    break;
                case ProviderName.OracleClient:
                    objReturn = OracleHelper.ExecuteScalar(connectionString, commandType, commandText, GetOracleParameters(commandParameters));
                    break;
                case ProviderName.MySqlClient:
                    objReturn = MySqlHelper.ExecuteScalar(connectionString, commandType, commandText, GetMySqlParameters(commandParameters));
                    break;
                default:
                    break;
            }
            return objReturn;
        }

        /// <summary>
        /// Execute a Command (that returns a 1x1 resultset) against the specified Connection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount", new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="connection">A valid IDbConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
        public static object ExecuteScalar(IDbConnection connection, CommandType commandType, string commandText)
        {
            object objReturn = null;
            if (IsSqlConnection(connection))
            {
                objReturn = SqlHelper.ExecuteScalar((System.Data.SqlClient.SqlConnection)connection, commandType, commandText);
            }
            else if (IsOracleConnection(connection))
            {
                objReturn = OracleHelper.ExecuteScalar((OracleConnection)connection, commandType, commandText);
            }
            else if (IsMySqlConnection(connection))
            {
                objReturn = MySqlHelper.ExecuteScalar((MySqlConnection)connection, commandType, commandText);
            }
            return objReturn;
        }

        /// <summary>
        /// Execute a Command (that returns a 1x1 resultset) against the specified Connection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount", new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="connection">A valid IDbConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of IDbDataParameter used to execute the command</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
        public static object ExecuteScalar(IDbConnection connection, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            object objReturn = null;
            if (IsSqlConnection(connection))
            {
                objReturn = SqlHelper.ExecuteScalar((System.Data.SqlClient.SqlConnection)connection, commandType, commandText, GetSqlParameters(commandParameters));
            }
            else if (IsOracleConnection(connection))
            {
                objReturn = OracleHelper.ExecuteScalar((OracleConnection)connection, commandType, commandText, GetOracleParameters(commandParameters));
            }
            else if (IsMySqlConnection(connection))
            {
                objReturn = MySqlHelper.ExecuteScalar((MySqlConnection)connection, commandType, commandText, GetMySqlParameters(commandParameters));
            }
            return objReturn;
        }
        /// <summary>
        /// Execute a Command (that returns a 1x1 resultset) against the specified IDbTransaction
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount", new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="transaction">A valid IDbTransaction</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
        public static object ExecuteScalar(IDbTransaction transaction, CommandType commandType, string commandText)
        {
            object objReturn = null;
            if (IsSqlConnection(transaction.Connection))
            {
                objReturn = SqlHelper.ExecuteScalar((System.Data.SqlClient.SqlTransaction)transaction, commandType, commandText);
            }
            else if (IsOracleConnection(transaction.Connection))
            {
                objReturn = OracleHelper.ExecuteScalar((OracleTransaction)transaction, commandType, commandText);
            }
            else if (IsMySqlConnection(transaction.Connection))
            {
                objReturn = MySqlHelper.ExecuteScalar((MySqlTransaction)transaction, commandType, commandText);
            }
            return objReturn;
        }
        /// <summary>
        /// Execute a Command (that returns a 1x1 resultset) against the specified IDbTransaction
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount", new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="transaction">A valid IDbTransaction</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of IDbDataParameter used to execute the command</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
        public static object ExecuteScalar(IDbTransaction transaction, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            object objReturn = null;
            if (IsSqlConnection(transaction.Connection))
            {
                objReturn = SqlHelper.ExecuteScalar((System.Data.SqlClient.SqlTransaction)transaction, commandType, commandText, GetSqlParameters(commandParameters));
            }
            else if (IsOracleConnection(transaction.Connection))
            {
                objReturn = OracleHelper.ExecuteScalar((OracleTransaction)transaction, commandType, commandText, GetOracleParameters(commandParameters));
            }
            else if (IsMySqlConnection(transaction.Connection))
            {
                objReturn = MySqlHelper.ExecuteScalar((MySqlTransaction)transaction, commandType, commandText, GetMySqlParameters(commandParameters));
            }
            return objReturn;
        }
        #endregion

        #region Reader
        /// <summary>
        /// Execute a Command (that returns a resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  IDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders", new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a Connection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>A IDataReader containing the resultset generated by the command</returns>
        public static IDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText)
        {
            IDataReader IobjReader = null;
            if (IsSQL)
            {
                IobjReader = ExecuteReader(connectionString, ProviderName.SqlClient, commandType, commandText);
            }
            else if (IsOracle)
            {
                IobjReader = ExecuteReader(connectionString, ProviderName.OracleClient, commandType, commandText);
            }
            else if (IsMySQL)
            {
                IobjReader = ExecuteReader(connectionString, ProviderName.MySqlClient, commandType, commandText);
            }
            return IobjReader;
        }
        /// <summary>
        /// Execute a Command (that returns a resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  IDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders", new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>A IDataReader containing the resultset generated by the command</returns>
        public static IDataReader ExecuteReaderByDirectConfig(CommandType commandType, string commandText)
        {
            IDataReader IobjReader = null;
            if (IsSQL)
            {
                IobjReader = ExecuteReader(ConnectionString, ProviderName.SqlClient, commandType, commandText);
            }
            else if (IsOracle)
            {
                IobjReader = ExecuteReader(ConnectionString, ProviderName.OracleClient, commandType, commandText);
            }
            else if (IsMySQL)
            {
                IobjReader = ExecuteReader(ConnectionString, ProviderName.MySqlClient, commandType, commandText);
            }
            return IobjReader;
        }

        /// <summary>
        /// Execute a Command (that returns a resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  IDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders", new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a Connection</param>
        /// <param name="objProvider">Name of the Provider</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>A IDataReader containing the resultset generated by the command</returns>
        public static IDataReader ExecuteReader(string connectionString, ProviderName objProvider, CommandType commandType, string commandText)
        {
            IDataReader IobjReader = null;
            switch (objProvider)
            {
                case ProviderName.SqlClient:
                    IobjReader = SqlHelper.ExecuteReader(connectionString, commandType, commandText);
                    break;
                case ProviderName.OracleClient:
                    IobjReader = OracleHelper.ExecuteReader(connectionString, commandType, commandText);
                    break;
                case ProviderName.MySqlClient:
                    IobjReader = OracleHelper.ExecuteReader(connectionString, commandType, commandText);
                    break;
                default:
                    break;
            }
            return IobjReader;
        }

        /// <summary>
        /// Execute a Command (that returns a resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  IDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders", new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a Connection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of IDbDataParameter used to execute the command</param>
        /// <returns>A IDataReader containing the resultset generated by the command</returns>
        public static IDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            IDataReader IobjReader = null;
            if (IsSQL)
            {
                IobjReader = ExecuteReader(connectionString, ProviderName.SqlClient, commandType, commandText, GetSqlParameters(commandParameters));
            }
            else if (IsOracle)
            {
                IobjReader = ExecuteReader(connectionString, ProviderName.OracleClient, commandType, commandText, GetOracleParameters(commandParameters));
            }
            else if (IsMySQL)
            {
                IobjReader = ExecuteReader(connectionString, ProviderName.MySqlClient, commandType, commandText, GetMySqlParameters(commandParameters));
            }
            return IobjReader;
        }

        /// <summary>
        /// Execute a Command (that returns a resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  IDataReader dr = ExecuteReader(connString,ProviderName.SqlClient, CommandType.StoredProcedure, "GetOrders", new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a Connection</param>
        /// <param name="objProvider">Name of the Provider</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of IDbDataParameter used to execute the command</param>
        /// <returns>A IDataReader containing the resultset generated by the command</returns>
        public static IDataReader ExecuteReader(string connectionString, ProviderName objProvider, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            IDataReader IobjReader = null;
            switch (objProvider)
            {
                case ProviderName.SqlClient:
                    IobjReader = SqlHelper.ExecuteReader(connectionString, commandType, commandText, GetSqlParameters(commandParameters));
                    break;
                case ProviderName.OracleClient:
                    IobjReader = OracleHelper.ExecuteReader(connectionString, commandType, commandText, GetOracleParameters(commandParameters));
                    break;
                case ProviderName.MySqlClient:
                    IobjReader = MySqlHelper.ExecuteReader(connectionString, commandType, commandText, GetMySqlParameters(commandParameters));
                    break;
                default:
                    break;
            }
            return IobjReader;
        }

        /// <summary>
        /// Execute a Command (that returns a resultset) against the specified IDbConnection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  IDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders", new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>A IDataReader containing the resultset generated by the command</returns>
        public static IDataReader ExecuteReader(IDbConnection connection, CommandType commandType, string commandText)
        {
            IDataReader IobjReader = null;
            if (IsSqlConnection(connection))
            {
                IobjReader = SqlHelper.ExecuteReader((System.Data.SqlClient.SqlConnection)connection, commandType, commandText);
            }
            else if (IsOracleConnection(connection))
            {
                IobjReader = OracleHelper.ExecuteReader((OracleConnection)connection, commandType, commandText);
            }
            else if (IsMySqlConnection(connection))
            {
                IobjReader = MySqlHelper.ExecuteReader((MySqlConnection)connection, commandType, commandText);
            }
            return IobjReader;
        }


        /// <summary>
        /// Execute a Command (that returns a resultset) against the specified IDbConnection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  IDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders", new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of IDbDataParameter used to execute the command</param>
        /// <returns>A IDataReader containing the resultset generated by the command</returns>
        public static IDataReader ExecuteReader(IDbConnection connection, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            IDataReader IobjReader = null;
            if (IsSqlConnection(connection))
            {
                IobjReader = SqlHelper.ExecuteReader((System.Data.SqlClient.SqlConnection)connection, commandType, commandText, GetSqlParameters(commandParameters));
            }
            else if (IsOracleConnection(connection))
            {
                IobjReader = OracleHelper.ExecuteReader((OracleConnection)connection, commandType, commandText, GetOracleParameters(commandParameters));
            }
            else if (IsMySqlConnection(connection))
            {
                IobjReader = MySqlHelper.ExecuteReader((MySqlConnection)connection, commandType, commandText, GetMySqlParameters(commandParameters));
            }
            return IobjReader;
        }

        /// <summary>
        /// Execute a Command (that returns a resultset) against the specified IDbTransaction
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///   SqlDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders", new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="transaction">A valid IDbTransaction</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>A IDataReader containing the resultset generated by the command</returns>
        public static IDataReader ExecuteReader(IDbTransaction transaction, CommandType commandType, string commandText)
        {
            IDataReader IobjReader = null;
            if (IsSqlConnection(transaction.Connection))
            {
                IobjReader = SqlHelper.ExecuteReader((System.Data.SqlClient.SqlTransaction)transaction, commandType, commandText);
            }
            else if (IsOracleConnection(transaction.Connection))
            {
                IobjReader = OracleHelper.ExecuteReader((OracleTransaction)transaction, commandType, commandText);
            }
            else if (IsMySqlConnection(transaction.Connection))
            {
                IobjReader = MySqlHelper.ExecuteReader((MySqlTransaction)transaction, commandType, commandText);
            }
            return IobjReader;
        }


        /// <summary>
        /// Execute a Command (that returns a resultset) against the specified IDbTransaction
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///   SqlDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders", new IDbDataParameter("prodid", 24));
        /// </remarks>
        /// <param name="transaction">A valid IDbTransaction</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of IDbDataParameter used to execute the command</param>
        /// <returns>A IDataReader containing the resultset generated by the command</returns>
        public static IDataReader ExecuteReader(IDbTransaction transaction, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            IDataReader IobjReader = null;
            IDbConnection con = transaction.Connection;
            if (IsSqlConnection(con))
            {
                IobjReader = SqlHelper.ExecuteReader((System.Data.SqlClient.SqlTransaction)transaction, commandType, commandText, GetSqlParameters(commandParameters));
            }
            else if (IsOracleConnection(con))
            {
                IobjReader = OracleHelper.ExecuteReader((OracleTransaction)transaction, commandType, commandText, GetOracleParameters(commandParameters));
            }
            else if (IsMySqlConnection(con))
            {
                IobjReader = MySqlHelper.ExecuteReader((MySqlTransaction)transaction, commandType, commandText, GetMySqlParameters(commandParameters));
            }
            return IobjReader;
        }


        #endregion

        #endregion

        #region "LOB Data"
        /// <summary>
        /// Gets the LOB data with reader and columnindex.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnIndex">Index of the column.</param>
        /// <returns></returns>
        private static string GetLOBData(IDataReader reader, int columnIndex)
        {
            string strData = string.Empty;
            if (reader is OracleDataReader)
            {
                
                using (OracleLob lobData = (reader as OracleDataReader).GetOracleLob(columnIndex))
                {
                    using (StreamReader streamreader = new StreamReader(lobData, Encoding.Unicode))
                    {
                        strData = streamreader.ReadToEnd();
                    }
                    lobData.Close();
                    lobData.Dispose();
                }
            }
            return strData;
        }
        /// <summary>
        /// LOB data with column name
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static string GetLOBData(IDataReader reader, string columnName)
        {
            return GetLOBData(reader,reader.GetOrdinal(columnName));
        }
        /// <summary>
        /// Writes the LOB data.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnIndex">Index of the column.</param>
        /// <param name="strData">The STR data.</param>
        private static void WriteLOBDataWithIndex(IDataReader reader, int columnIndex, string strData)
        {
            //if (reader is OracleDataReader)
            //{
            UnicodeEncoding tencoding = new UnicodeEncoding();
            using (OracleLob lobData = (reader as OracleDataReader).GetOracleLob(columnIndex))
            {
                byte[] buffer = new byte[11];
                using (StreamReader streamreader = new StreamReader(lobData, Encoding.Unicode))
                {
                    buffer = tencoding.GetBytes(strData);
                    lobData.Write(buffer, 0, buffer.Length);
                    lobData.Close();
                    lobData.Dispose();
                }
            }
            //}
        }

        /// <summary>
        /// Writes the LOB data.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="columnNamesWithData">The column names with data as Column Name,Data.</param>
        /// <param name="commandParameters">The command parameters.</param>
        public static void WriteLOBData(IDbTransaction transaction, CommandType commandType, string commandText, System.Collections.Generic.Dictionary<string, string> columnNamesWithData, params IDbDataParameter[] commandParameters)
        {
            IDataReader IobjReader = null;
            //if (IsSqlConnection(transaction.Connection))
            //{
            //    IobjReader = SqlHelper.ExecuteReader((System.Data.SqlClient.SqlTransaction)transaction, commandType, commandText, GetSqlParameters(commandParameters));
            //}
            //else 
            if (IsOracleConnection(transaction.Connection))
            {
                IobjReader = OracleHelper.ExecuteReader((OracleTransaction)transaction, commandType, commandText, GetOracleParameters(commandParameters));
                if (IobjReader.Read())
                {
                    foreach (var item in columnNamesWithData)
                    {
                        // 'Key' - Should be the Name of the column, 'Value' should be data of that column.
                         WriteLOBDataWithIndex(IobjReader, IobjReader.GetOrdinal(item.Key), item.Value);
                    }
                    //for (int i = 0; i < lobArray.Length; i++)
                    //{
                    //    WriteLOBDataWithIndex(IobjReader, i, lobArray[i]);
                    //}



                }
            }
            //return IobjReader;
        }

        #endregion
    }

    public class DALHelperParameterCache
    {
        private static string _connStrName = "ConnString";
        private static ConnectionStringSettings _connStrSettings;

        #region  Properties

        /// <summary>
        /// Is SQL is checked the Provider name is "System.Data.SqlClient" if so 
        /// this will returns true else return false.
        /// </summary>
        /// <returns></returns>
        /// public static bool IsSQL()
        private static bool IsSQL
        {
            get
            {
                _connStrSettings = ConfigurationManager.ConnectionStrings[_connStrName];
                return _connStrSettings.ProviderName == "System.Data.SqlClient";
            }

        }
        /// <summary>
        /// Is Oracle is checked the Provider name is "System.Data.OracleClient" if so 
        /// this will returns true else return false.
        /// </summary>
        /// <returns></returns>
        /// public static bool IsOracle()
        private static bool IsOracle
        {
            get
            {
                _connStrSettings = ConfigurationManager.ConnectionStrings[_connStrName];
                return _connStrSettings.ProviderName == "System.Data.OracleClient";
            }
        }

        #endregion

        #region Parameter Discovery Functions

        /// <summary>
        /// This Function Will Check the Connection is Sql Connection Or Not
        /// </summary>
        /// <param name="connection">IDbConnection</param>
        /// <returns>bool</returns>
        private static bool IsSqlConnection(IDbConnection connection)
        {
            return connection is System.Data.SqlClient.SqlConnection;
        }
        /// <summary>
        /// This Function Will Check the Connection is My Sql Connection Or Not
        /// </summary>
        /// <param name="connection">IDbConnection</param>
        /// <returns>bool</returns>
        private static bool IsMySqlConnection(IDbConnection connection)
        {
            return connection is MySql.Data.MySqlClient.MySqlConnection;
        }
        /// <summary>
        /// This Function Will Check the Connection is Oracle Connection Or Not
        /// </summary>
        /// <param name="connection">IDbConnection </param>
        /// <returns>bool</returns>
        private static bool IsOracleConnection(IDbConnection connection)
        {
            return connection is System.Data.OracleClient.OracleConnection;
        }


        /// <summary>
        /// Retrieves the set of OracleParameters appropriate for the stored procedure
        /// </summary>
        /// <remarks>
        /// This method will query the database for this information, and then store it in a cache for future requests.
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a OracleConnection</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <returns>An array of OracleParameters</returns>
        //public static OracleParameter[] GetSpParameterSet(string connectionString, string spName)
        //{
        //    return GetSpParameterSet(connectionString, spName, false);
        //}

        /// <summary>
        /// Retrieves the set of OracleParameters appropriate for the stored procedure
        /// </summary>
        /// <remarks>
        /// This method will query the database for this information, and then store it in a cache for future requests.
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a Connection. </param>
        /// <param name="provider">The name of the Provider using</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="includeReturnValueParameter">A bool value indicating whether the return value parameter should be included in the results</param>
        /// <returns>An array of OracleParameters</returns>
        public static IDbDataParameter[] GetSpParameterSet(string connectionString,ProviderName provider, string spName)
        {
            IDbDataParameter[] Parameters = null;
            switch (provider)
            {
                case ProviderName.SqlClient :
                    Parameters = GetDbDataParameter(SqlHelperParameterCache.GetSpParameterSet(connectionString, spName, false));
                    break;

                case ProviderName.OracleClient :
                    Parameters = GetDbDataParameter(OracleHelperParameterCache.GetSpParameterSet(connectionString, spName, true));
                    break;
                default :
                    break;
            }
            return Parameters;
        }

        /// <summary>
        /// Retrieves the set of OracleParameters appropriate for the stored procedure
        /// </summary>
        /// <remarks>
        /// This method will query the database for this information, and then store it in a cache for future requests.
        /// </remarks>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="includeReturnValueParameter">A bool value indicating whether the return value parameter should be included in the results</param>
        /// <returns>An array of OracleParameters</returns>
        //public static IDbDataParameter[] GetSpParameterSet(string spName)
        //{
        //    IDbDataParameter[] Parameters = null;
        //    if (IsSQL == true)
        //    {
        //        Parameters= GetDbDataParameter(SqlHelperParameterCache.GetSpParameterSet(_connStrSettings.ConnectionString.ToString(), spName, false));
        //    }
        //    else if (IsOracle == true)
        //    {
        //        Parameters = GetDbDataParameter(OracleHelperParameterCache.GetSpParameterSet(_connStrSettings.ConnectionString.ToString(), spName, true));
        //    }
        //    return Parameters;
        //}

        /// <summary>
        /// Retrieves the set of OracleParameters appropriate for the stored procedure
        /// </summary>
        /// <remarks>
        /// This method will query the database for this information, and then store it in a cache for future requests.
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a OracleConnection</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="includeReturnValueParameter">A bool value indicating whether the return value parameter should be included in the results</param>
        /// <returns>An array of OracleParameters</returns>
        public static IDbDataParameter[] GetSpParameterSet(IDbConnection connection, string spName)
        {
            IDbDataParameter[] Parameters = null;

            if (IsSqlConnection(connection))  
            {
                Parameters = GetDbDataParameter(SqlHelperParameterCache.GetSpParameterSet((SqlConnection)connection, spName, false));
            }
            else if (IsOracleConnection(connection))  
            {
                Parameters = GetDbDataParameter(OracleHelperParameterCache.GetSpParameterSet((OracleConnection)connection, spName, true));
            }
            else if(IsMySqlConnection(connection))
            {
                Parameters = GetDbDataParameter(MySqlHelperParameterCache.GetSpParameterSet((MySqlConnection)connection, spName, true));
            }
            return Parameters;
        }

        /// <summary>
        /// Retrieves the set of OracleParameters appropriate for the stored procedure
        /// </summary>
        /// <param name="objTrans">The obj trans.</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <returns>An array of OracleParameters</returns>
        /// <remarks>
        /// This method will query the database for this information, and then store it in a cache for future requests.
        /// </remarks>
        public static IDbDataParameter[] GetSpParameterSet(IDbTransaction objTrans, string spName)
        {
            return GetSpParameterSet(objTrans.Connection, spName);   
        }
        private static IDbDataParameter[] GetDbDataParameter(params SqlParameter[] commandParameters)
        {
            IDbDataParameter[] IDbParam = new IDbDataParameter[commandParameters.Length]; 
            Int32 intCount=0;
            foreach (SqlParameter sqlParam in commandParameters)
            {
                sqlParam.ParameterName = sqlParam.ParameterName.Substring(1); 
                IDbParam[intCount] = (IDbDataParameter)((ICloneable)sqlParam);
                intCount = intCount + 1;
            }
            return IDbParam;

        }

        private static IDbDataParameter[] GetDbDataParameter(params MySqlParameter[] commandParameters)
        {
            IDbDataParameter[] IDbParam = new IDbDataParameter[commandParameters.Length];
            Int32 intCount = 0;
            foreach (MySqlParameter sqlParam in commandParameters)
            {
                sqlParam.ParameterName = sqlParam.ParameterName.Substring(1);
                IDbParam[intCount] = (IDbDataParameter)((ICloneable)sqlParam);
                intCount = intCount + 1;
            }
            return IDbParam;

        }
        private static IDbDataParameter[] GetDbDataParameter(params OracleParameter[] commandParameters)
        {
            IDbDataParameter[] IDbParam = new IDbDataParameter[commandParameters.Length];
            Int32 intCount = 0;
            foreach (OracleParameter OracleParam in commandParameters)
            {
                IDbParam[intCount] = (IDbDataParameter)((ICloneable)OracleParam);
                intCount = intCount + 1;
            }
            return IDbParam;

        }

        #endregion Parameter Discovery Functions

    }

    class PasswordHandler
    {

        private static string _errorMessage = string.Empty;
        private static string _passwordResult = string.Empty;

        public static string GetPassword()
        {
            try
            {
                if (_passwordResult == string.Empty)
                {
                    // add a key "MedilogicsPFileLocation" in App.Config file and give Password.pwd path as value
                    _passwordResult = GetPasswordForFirstTime(Convert.ToString(ConfigurationManager.AppSettings["MedilogicsPFileLocation"]));
                }
            }
            catch 
            {
                throw;
            }

            return _passwordResult;
        }

        public static string GetPasswordForFirstTime(string strPFileLocation)
        {
            string strPWDFileName = null;
            string strDecryptedPassword = string.Empty;

            try
            {
                // to find file Password.pwd
                // add a key "MedilogicsPFileLocation" in App.Config file and give Password.pwd path as value
                strPWDFileName = GetPasswordFileName(strPFileLocation);
                strDecryptedPassword = GetPasswordFromFile(strPWDFileName);
            }
            catch 
            {
                strDecryptedPassword = string.Empty;
                throw;
            }

            return strDecryptedPassword;
        }

        public static string GetPasswordFromFile(string strPWDFileName)
        {
            string strEncryptedString = string.Empty;
            string strEncryptedPassword = string.Empty;
            string strDecryptedPassword = string.Empty;  
            string strDateTimeCode = null;

            int intKey = 0;
            int intStart = 0;
            int intEnd = 0;
            int intLength = 0;

            StreamReader strReader = null;
            strReader = File.OpenText(strPWDFileName);
            strEncryptedString = strReader.ReadLine();
            strReader.Close();

            // fetch the date and time combination code for decryption
            strDateTimeCode = strEncryptedString.Substring(32, 12);

            if (!(Microsoft.VisualBasic.Information.IsNumeric(strDateTimeCode)) && strEncryptedString.Length > 66)
            {
                //if SQL Server this will work.
                strDateTimeCode = strEncryptedString.Substring(60, 5);
                if (!(Microsoft.VisualBasic.Information.IsNumeric(strDateTimeCode)))
                {
                    throw new Exception("Invalid Conversion of Password...");
                }
                //Fetch the begining postion of the password in the string
                intStart = Convert.ToInt16(strEncryptedString.Substring(65, 2));
                strEncryptedPassword = strEncryptedString.Substring(intStart - 1, 60 - intStart + 1);
            }
            else if (Microsoft.VisualBasic.Information.IsNumeric(strDateTimeCode))
            {
                //if Oracle this will work.
                intStart = strEncryptedString.IndexOf("7");
                intStart += 1;
                intEnd = strEncryptedString.IndexOf("9");
                intLength = (intEnd - intStart);
                strEncryptedPassword = strEncryptedString.Substring(intStart, intLength);
            }
            else
            {
                throw new Exception("Invalid Conversion of Password...");
            }
            intKey = GenerateKey(strDateTimeCode);
            strDecryptedPassword = Decrypt(strEncryptedPassword, intKey);
            return strDecryptedPassword;
        }

        private static string GetPasswordFileName(string strFileName)
        {
            string strGetFileName = string.Empty;
            //string strFileName = null;

            try
            {
                //strFileName = ConfigurationManager.AppSettings["MedilogicsPFileLocation"];
                //strFileName = Configuration.ConfigurationSettings.AppSettings.Item("MedilogicsPFileLocation")
                if (strFileName != null)
                {
                    if (!(strFileName.EndsWith("\\")))
                    {
                        strFileName += "\\";
                    }
                    strFileName += "Password.pwd";
                    try
                    {
                        if (File.Exists(strFileName))
                        {
                            strGetFileName = strFileName;
                        }
                        else
                        {
                            //_errorMessage = "File " + strFileName + " not found !"
                            throw new Exception("File " + strFileName + " not found !");
                        }
                    }
                    catch (Exception)
                    {
                        //MessageBox.Show("First file check error")
                        //_errorMessage = ex.Message
                        throw;
                    }
                }
                else
                {
                    throw new FileNotFoundException("Password file not found");
                }

                if (strGetFileName == string.Empty)
                {
                    strFileName = Environment.CurrentDirectory + "\\Password.pwd";
                    try
                    {
                        if (File.Exists(strFileName))
                        {
                            strGetFileName = strFileName;
                        }
                        else
                        {
                            //_errorMessage = _errorMessage & Environment.NewLine & "File " + strFileName + " not found !"
                            throw new Exception("File " + strFileName + " not found !");
                        }
                    }
                    catch (Exception)
                    {
                        //MessageBox.Show("Second file check error")
                        //_errorMessage = ex.Message
                        throw;
                    }
                }

            }
            catch (Exception)
            {
                strGetFileName = "";
                throw;
            }

            return strGetFileName;
        }

        private static int GenerateKey(string dateTimeCode)
        {
            int retValue = 0;

            try
            {
                //encryption key will be generated on the basis of the number derived from the time and date
                //all the numbers in the code will be added and / by  3 if the value is <2  then the code is 2
                //or if the values if >26 value is set as 24

                for (int i = 1; i <= dateTimeCode.Length; i++)
                {
                    retValue = retValue + System.Convert.ToInt32(dateTimeCode.Substring(i - 1, 1));
                }

                retValue = Convert.ToInt32(retValue / 3.0);
                if (retValue < 2)
                {
                    retValue = 2;
                }
                else if (retValue > 26)
                {
                    retValue = 24;
                }

            }
            catch (Exception)
            {
                throw;
            }

            return retValue;
        }

        private static string Decrypt(string encryptedPassword, int decryptionKey)
        {
            int i = 0;
            int currASC = 0;
            int newASC = 0;
            string decryptedPassword = string.Empty;

            try
            {
                // Decrypt based on the key
                for (i = 1; i <= encryptedPassword.Length; i++)
                {
                    currASC = System.Convert.ToInt32(encryptedPassword[i - 1]);
                    if (currASC <= 90)
                    {
                        newASC = currASC - decryptionKey;
                    }
                    else
                    {
                        newASC = 90 - (decryptionKey - (currASC - 97 + 1));
                    }
                    decryptedPassword += (char)(newASC);
                }

            }
            catch (Exception)
            {
                throw;
            }

            return decryptedPassword;
        }

    }

}
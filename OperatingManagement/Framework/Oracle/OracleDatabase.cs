using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace OperatingManagement.Framework
{
    /// <summary>
    /// Provides implementation of Oracle database.
    /// </summary>
    public class OracleDatabase
    {
        private string _connectionString;
        private bool _unSafe = false;

        private OracleDatabase()
        { }
        /// <summary>
        /// Initializes a new instance of <see cref="OperatingManagement.Framework.OracleDatabase"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string of Oracle database.</param>
        /// <returns></returns>
        public static OracleDatabase FromConnectionString(string connectionString)
        {
            OracleDatabase database = new OracleDatabase();
            database._connectionString = connectionString;
            return database;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="OperatingManagement.Framework.OracleDatabase"/> class.
        /// </summary>
        /// <param name="nodeName">The node name of 'connectionStrings' in Web.Config file.</param>
        /// <returns></returns>
        public static OracleDatabase FromConfiguringNode(string nodeName)
        {
            OracleDatabase database = new OracleDatabase();
            database._connectionString = ConfigurationManager.ConnectionStrings[nodeName].ConnectionString;
            if (string.IsNullOrEmpty(database._connectionString))
            {
                throw new ConfigurationErrorsException("cant found configured node. node name: " + nodeName);
            }

            return database;
        }
        /// <summary>
        /// Create and prepare an OracleCommand, and call ExecuteReader with the appropriate procedure.
        /// </summary>
        /// <remarks>
        /// If we created and opened the connection, we want the connection to be closed when the DataReader is closed.
        /// 
        /// If the caller provided the connection, we want to leave it to them to manage.
        /// </remarks>
        /// <param name="spName">the stored procedure name.</param>
        /// <param name="parameters">an array of OracleParameter to be associated with the command or 'null' if no parameters are required</param>
        /// <returns></returns>
        public OracleDataReader SpExecuteReader(string spName, params OracleParameter[] parameters)
        {
            OracleConnection connection = new OracleConnection(_connectionString);

            try
            {
                OracleCommand command = SpPrepareCommand(connection, spName, parameters);

                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch(Exception ex)
            {
                connection.Close();
                throw ex;
            }
        }
        /// <summary>
        /// Execute an OracleCommand (that returns a resultset and takes no parameters) against the database specified in 
        /// the connection string. 
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  DataTable dt = SpExecuteTable(spName, "GetOrders");
        /// </remarks>
        /// <param name="spName">the stored procedure name.</param>
        /// <param name="parameters">an array of OracleParameter to be associated with the command or 'null' if no parameters are required</param>
        /// <returns></returns>
        public DataTable SpExecuteTable(string spName, params OracleParameter[] parameters)
        {
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                OracleCommand command = SpPrepareCommand(connection, spName, parameters);

                OracleDataAdapter adapter = new OracleDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);

                return table;
            }
        }

        /// <summary>
        /// Execute an OracleCommand (that returns a resultset and takes no parameters) against the database specified in 
        /// the connection string. 
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  DataSet ds = SpExecuteDataSet(spName, "GetOrders");
        /// </remarks>
        /// <param name="spName">the stored procedure name.</param>
        /// <param name="parameters">an array of OracleParameter to be associated with the command or 'null' if no parameters are required</param>
        /// <returns></returns>
        public DataSet SpExecuteDataSet(string spName, params OracleParameter[] parameters)
        {
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                OracleCommand command = SpPrepareCommand(connection, spName, parameters);

                OracleDataAdapter adapter = new OracleDataAdapter(command);
                DataSet set = new DataSet();
                adapter.Fill(set);

                return set;
            }
        }
        /// <summary>
        /// Execute an OracleCommand (that returns no resultset and takes no parameters) against the database specified in 
        /// the connection string. 
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = SpExecuteNonQuery(spName, "PublishOrders");
        /// </remarks>
        /// <param name="spName">the stored procedure name.</param>
        /// <param name="parameters">an array of OracleParameter to be associated with the command or 'null' if no parameters are required</param>
        /// <returns></returns>
        public int SpExecuteNonQuery(string spName, params OracleParameter[] parameters)
        {
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                OracleCommand command = SpPrepareCommand(connection, spName, parameters);

                return command.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Execute an OracleCommand (that returns a 1x1 resultset and takes no parameters) against the database specified in 
        /// the connection string. 
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int orderCount = SpExecuteScalar<![CDATA[<int>]]>(connString, CommandType.StoredProcedure, "GetOrderCount");
        /// </remarks>
        /// <typeparam name="T">The type of return value.</typeparam>
        /// <param name="spName">the stored procedure name.</param>
        /// <param name="parameters">an array of OracleParameter to be associated with the command or 'null' if no parameters are required</param>
        /// <returns></returns>
        public T SpExecuteScalar<T>(string spName, params OracleParameter[] parameters)
        {
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                OracleCommand command = SpPrepareCommand(connection, spName, parameters);

                return (T)command.ExecuteScalar();
            }
            
        }

        private OracleCommand SpPrepareCommand(OracleConnection connection, string spName, OracleParameter[] parameters)
        {
            if(connection.State!= ConnectionState.Open)
                connection.Open();
            OracleCommand command = new OracleCommand(spName, connection);
            command.CommandType = CommandType.StoredProcedure;
            if (parameters != null)
            {
                foreach (OracleParameter p in parameters)
                    command.Parameters.Add(p);
            }

            return command;
        }


        #region --Method Added by Tangjia----

        #region private methods

        private void OpenConnection(OracleConnection conn)
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                throw ex;
            }
        }

        private static bool QueryProcedureNeedsCursorParameter(OracleCommand command)
        {
            foreach (OracleParameter parameter in command.Parameters)
            {
                if (parameter.OracleDbType == OracleDbType.RefCursor)
                {
                    return false;
                }
            }
            return true;
        }

        private void PrepareCWRefCursor(OracleCommand command)
        {
            if (command == null) throw new ArgumentNullException("command");

            if (CommandType.StoredProcedure == command.CommandType)
            {
                if (QueryProcedureNeedsCursorParameter(command))
                {
                    AddOutParameter(command, RefCursorName, OracleDbType.RefCursor, 0);
                }
            }
        }

        #endregion

        public bool UnSafe
        {
            get { return _unSafe; }

            set { _unSafe = value; }
        }

        /// <summary>
        /// 从sql语句构建OracleCommand对象
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public OracleCommand GetSqlStringCommand(string sql)
        {
            OracleConnection conn = new OracleConnection(_connectionString);
            OracleCommand command = new OracleCommand(sql, conn);

            return command;
        }

        /// <summary>
        /// 从存储过程名构建OracleCommand对象
        /// </summary>
        /// <param name="procName"></param>
        /// <returns></returns>
        public OracleCommand GetStoreProcCommand(string procName)
        {
            OracleConnection conn = new OracleConnection(_connectionString);
            OracleCommand command = new OracleCommand(procName, conn);
            command.CommandType = CommandType.StoredProcedure;

            return command;
        }

        /// <summary>
        /// 向已有的OracleCommand对象中添加输入参数
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="oraType"></param>
        public void AddInParameter(OracleCommand command, string name, OracleDbType oraType)
        {
            AddInParameter(command, name, oraType, DBNull.Value);
        }

        /// <summary>
        ///  向已有的OracleCommand对象中添加输入参数
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="oraType"></param>
        /// <param name="value"></param>
        public void AddInParameter(OracleCommand command, string name, OracleDbType oraType, object value)
        {
            if (null != value && oraType == OracleDbType.NVarchar2 && Encoding.Default.GetBytes(value.ToString()).Length > 4000)
            {
                oraType = OracleDbType.Clob;
            }
            OracleParameter param = new OracleParameter(name, oraType);
            param.Value = (null == value) ? DBNull.Value : value;

            command.Parameters.Add(param);
        }

        /// <summary>
        ///  向已有的OracleCommand对象中添加输出参数
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="oraType"></param>
        /// <param name="size"></param>
        public void AddOutParameter(OracleCommand command, string name, OracleDbType oraType, int size)
        {
            OracleParameter param = new OracleParameter(name, oraType, size);
            param.Direction = ParameterDirection.Output;

            command.Parameters.Add(param);
        }

        /// <summary>
        /// 夺取OracleCommand中指定参数的值
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetParameterValue(OracleCommand command, string name)
        {
            OracleParameter param = command.Parameters[name];

            return param.Value;
        }

        /// <summary>
        /// 设置OracleCommand中指定参数的值
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetParameterValue(OracleCommand command, string name, object value)
        {
            if (null != value && command.Parameters[name].OracleDbType == OracleDbType.NVarchar2 && Encoding.Default.GetBytes(value.ToString()).Length > 4000)
            {
                command.Parameters[name].OracleDbType = OracleDbType.Clob;
            }
            OracleParameter param = command.Parameters[name];
            param.Value = (null == value) ? DBNull.Value : value;
        }

        /// <summary>
        /// 执行OracleCommand，返回命令影响的行数
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(OracleCommand command)
        {
            int result = -1;

            if (!UnSafe)
            {
                for (int i = 0; i < command.Parameters.Count; i++)
                {
                    OracleParameter parameter = command.Parameters[i];
                    if (parameter.OracleDbType == OracleDbType.RefCursor)
                    {
                        FilterCommand(command);
                        break;
                    }
                }
            }
            OracleConnection conn = command.Connection;
            OpenConnection(conn);

            try
            {
                result = command.ExecuteNonQuery();
            }
            finally
            {
                if (null == command.Transaction)
                {
                    conn.Close();
                }
            }

            return result;
        }

        /// <summary>
        /// 执行OracleCommand，返回结果。
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public object ExecuteScalar(OracleCommand command)
        {
            if (!UnSafe)
            {
                for (int i = 0; i < command.Parameters.Count; i++)
                {
                    OracleParameter parameter = command.Parameters[i];
                    if (parameter.OracleDbType == OracleDbType.RefCursor)
                    {
                        FilterCommand(command);
                        break;
                    }
                }
            }
            object result = null;

            OracleConnection conn = command.Connection;
            OpenConnection(conn);

            try
            {
                result = command.ExecuteScalar();
            }
            finally
            {
                if (null == command.Transaction)
                {
                    conn.Close();
                }
            }

            return result;
        }

        /// <summary>
        /// 执行OracleCommand返回Oracle数据阅读器
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public OracleDataReader ExecuteReader(OracleCommand command)
        {
            PrepareCWRefCursor(command);
            if (!UnSafe)
            {
                FilterCommand(command);
            }
            OracleDataReader reader = null;

            OracleConnection conn = command.Connection;
            OpenConnection(conn);

            if (null == command.Transaction)
            {
                try
                {
                    reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                }
                catch (Exception ex)
                {
                    conn.Close();
                    throw ex;
                }
            }
            else
            {
                reader = command.ExecuteReader();
            }

            return reader;
        }

        /// <summary>
        /// 执行OracleCommand，返回结果集
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(OracleCommand command)
        {
            PrepareCWRefCursor(command);
            if (!UnSafe)
            {
                FilterCommand(command);
            }
            DataSet ds = new DataSet();
            OracleDataAdapter adapter = new OracleDataAdapter(command);
            adapter.Fill(ds);

            return ds;
        }

        public void FilterCommand(OracleCommand command)
        {
            for (int i = 0; i < command.Parameters.Count; i++)
            {
                OracleParameter parameter = command.Parameters[i];
                if (parameter.OracleDbType == OracleDbType.NVarchar2 && null != parameter.Value && Convert.DBNull != parameter.Value)
                {
                    string oldValue = parameter.Value.ToString();
                    string newValue = oldValue.Replace("'", "''");
                    parameter.Value = newValue;
                }
            }
        }

        #region static fields
        private const string RefCursorName = "o_cursor";

        #endregion
        #endregion
    }
}

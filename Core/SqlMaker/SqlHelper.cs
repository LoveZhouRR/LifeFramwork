using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Core.SqlMaker
{
    public abstract class SqlHelper
    {
        public static readonly string ConnectionStringLocal = ConfigurationManager.AppSettings["conn"];

        #region add by raja 2013.1.15

        public static DataSet ExecuteDataSet(string strConnectionString, string strCmdText, CommandType commandType, params SqlParameter[] parasCommandParameters)
        {
            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                SqlCommand cmd = new SqlCommand(strCmdText, conn);
                cmd.CommandType = commandType;
                if (parasCommandParameters != null && parasCommandParameters.Length > 0)
                    cmd.Parameters.AddRange(parasCommandParameters);

                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = cmd;
                DataSet dataSet = new DataSet();
                sqlDA.Fill(dataSet, "Anonymous");

                return dataSet;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strConnectionString">连接:对哪操作</param>
        /// <param name="strCmdText">命令：做什么操作(sql语句或者存储过程名)</param>
        /// <param name="commandType"></param>
        /// <param name="parasCommandParameters">命令的参数</param>
        /// <returns></returns>
        public static object ExecuteScalar(string strConnectionString, string strCmdText, CommandType commandType, params SqlParameter[] parasCommandParameters)
        {
            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                SqlCommand cmd = new SqlCommand(strCmdText, conn);
                cmd.CommandType = commandType;
                if (parasCommandParameters != null && parasCommandParameters.Length > 0)
                    cmd.Parameters.AddRange(parasCommandParameters);

                return cmd.ExecuteScalar();
            }
        }

        public static int ExecuteNonQuery(string strConnectionString, string strCmdText, CommandType commandType, params SqlParameter[] parasCommandParameters)
        {
            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                SqlCommand cmd = new SqlCommand(strCmdText, conn);
                cmd.CommandType = commandType;
                if (parasCommandParameters != null && parasCommandParameters.Length > 0)
                    cmd.Parameters.AddRange(parasCommandParameters);

                int rowsAffected = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return rowsAffected;
            }
        }


        #endregion



        public static object ExecuteScalar(string connectionString, string cmdText)
        {
            //Log.Dac("Start ExecuteScalar 2:cmdText=" + cmdText);
            DateTime time = DateTime.Now;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = cmdText;
            object o = ExecuteScalar(connectionString, CommandType.Text, cmd);
            return o;
        }

        public static object ExecuteScalar(SqlCommand cmd)
        {
            return ExecuteScalar(SqlHelper.ConnectionStringLocal, CommandType.Text, cmd);
        }

        public static object ExecuteScalar(string connectionString, CommandType cmdType, SqlCommand cmd)
        {
            //Log.Dac("Start ExecuteScalar 3:cmd.CommandText=" + cmd.CommandText);
            DateTime time = DateTime.Now;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = cmdType;
                object o = cmd.ExecuteScalar();
                return o;
            }
        }

        public static int ExecuteNonQuery(string cmdText)
        {
            //Log.Dac("Start ExecuteNonQuery 1:cmdText=" + cmdText);
            DateTime time = DateTime.Now;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = cmdText;
            int n = ExecuteNonQuery(SqlHelper.ConnectionStringLocal, CommandType.Text, cmd);
            return n;
        }

        public static int ExecuteNonQuery(SqlCommand cmd)
        {
            return ExecuteNonQuery(SqlHelper.ConnectionStringLocal, CommandType.Text, cmd);
        }

        public static int ExecuteNonQuery(string connectionString, SqlCommand cmd)
        {
            return ExecuteNonQuery(connectionString, CommandType.Text, cmd);
        }

        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, SqlCommand cmd)
        {
            //Log.Dac("Start ExecuteNonQuery 2:cmd.CommandText=" + cmd.CommandText);
            DateTime time = DateTime.Now;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Connection = conn;
                cmd.CommandType = cmdType;

                int rowsAffected = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return rowsAffected;
            }
        }

        /// <summary>
        ///  执行存储过程,获取output参数 add by zona  2011-4-2 
        /// </summary> 
        /// <param name="connectionString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static int Proc_ExecuteNonQuery(string connectionString, CommandType cmdType, SqlCommand cmd)
        {
            //Log.Dac("Start Proc_ExecuteNonQuery:cmd.CommandText=" + cmd.CommandText);
            DateTime time = DateTime.Now;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = cmdType;
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected;
            }
        }

        /// <summary>
        /// 执行存储过程返回DataSet | added by nicocao 2011-09-01
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static DataSet Proc_ExecuteDataSet(string connectionString, CommandType cmdType, SqlCommand cmd)
        {
            //Log.Dac("Start Proc_ExecuteDataSet:cmd.CommandText=" + cmd.CommandText);
            DateTime time = DateTime.Now;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Connection = conn;
                cmd.CommandType = cmdType;

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                return ds;
            }
        }

        public static int ExecuteNonQuery(SqlCommand cmd, out int sysno)
        {
            return ExecuteNonQuery(SqlHelper.ConnectionStringLocal, CommandType.Text, cmd, out sysno);
        }

        public static int ExecuteNonQuery(string connectionString, SqlCommand cmd, out int sysno)
        {
            return ExecuteNonQuery(connectionString, CommandType.Text, cmd, out sysno);
        }


        public static int ExecuteNonQuery(string connectionString, string cmdText)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = cmdText;
            return ExecuteNonQuery(connectionString, cmd);
        }

        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, SqlCommand cmd, out int sysno)
        {
            //Log.Dac("Start ExecuteNonQuery 4:cmd.CommandText=" + cmd.CommandText);
            DateTime time = DateTime.Now;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = cmdType;
                int rowsAffected = cmd.ExecuteNonQuery();
                //必须符合下面的条件
                if (cmd.Parameters.Contains("@SysNo") && cmd.Parameters["@SysNo"].Direction == ParameterDirection.Output)
                    sysno = (int)cmd.Parameters["@SysNo"].Value;
                else
                {
                    throw new Exception("SqlHelper: Does not contain SysNo or ParameterDirection is Not Output");
                }
                cmd.Parameters.Clear();
                return rowsAffected;
            }
        }

        /// <summary> 
        ///  在事务中执行,返回受影响行数   add by zona 2011-3-31
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdType">CommandType</param>
        /// <param name="cmdText">执行的sql语句</param>
        /// <returns>受影响行数 -1,出错。</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText)
        {
            //Log.Dac("Start ExecuteNonQuery 5:cmdText=" + cmdText);
            DateTime time = DateTime.Now;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                int rowsAffected = 0;
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                SqlTransaction trans = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                try
                {
                    SqlCommand cmd = new SqlCommand(cmdText, conn, trans);
                    cmd.CommandTimeout = 120;
                    cmd.CommandType = cmdType;
                    rowsAffected = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                    rowsAffected = -1;
                }
               
                return rowsAffected;

            }
        }

        //ExecuteNonQuery TimeOut  30Min 
        /// <summary> 
        /// Cmd TimeOut 30 Minute   add by Zachary 2011-5-20
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdType">CommandType</param>
        /// <param name="cmdText">执行的sql语句</param>
        /// <returns>受影响行数 -1,出错。</returns>
        public static int ExecuteNonQuery30Min(string connectionString, CommandType cmdType, string cmdText)
        {
            //Log.Dac("Start ExecuteNonQuery 6:cmdText=" + cmdText);
            DateTime time = DateTime.Now;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                int rowsAffected = 0;
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                SqlCommand cmd = new SqlCommand(cmdText, conn);
                cmd.CommandTimeout = 1800;
                cmd.CommandType = cmdType;
                rowsAffected = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
               
                return rowsAffected;
            }
        }

        public static DataSet ExecuteDataSet(string cmdText)
        {
            return ExecuteDataSet(new SqlCommand(){CommandText = cmdText});
        }
        public static DataSet ExecuteDataSet(string connectionString, string cmdText)
        {
            return ExecuteDataSet(connectionString, cmdText, null);
        }

        public static DataSet ExecuteDataSet(string cmdText, SqlParameter[] paras)
        {
            return ExecuteDataSet(SqlHelper.ConnectionStringLocal, cmdText, paras);
        }

        public static DataSet ExecuteDataSet(SqlConnection conn, SqlCommand cmd)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            SqlDataAdapter sqlDA = new SqlDataAdapter();
            sqlDA.SelectCommand = cmd;
            DataSet dataSet = new DataSet();
            sqlDA.Fill(dataSet, "Anonymous");

            return dataSet;
        }

        public static DataSet ExecuteDataSet(string connectionString, string cmdText, SqlParameter[] paras)
        {
            //Log.Dac("Start ExecuteDataSet :cmdText=" + cmdText);
            DateTime time = DateTime.Now;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = cmdText;
                cmd.CommandType = CommandType.Text;
                if (paras != null && paras.Length > 0)
                    cmd.Parameters.AddRange(paras);

                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = cmd;
                DataSet dataSet = new DataSet();
                sqlDA.Fill(dataSet, "Anonymous");
               
                return dataSet;
            }
        }

        public static DataSet ExecuteDataSet(SqlCommand cmd)
        {
           //Log.Dac("Start ExecuteDataSet 2 : cmd.CommandText="  + cmd.CommandText);
            DateTime time = DateTime.Now;          
            using (SqlConnection conn = new SqlConnection(SqlHelper.ConnectionStringLocal))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Connection = conn;

                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = cmd;
                DataSet dataSet = new DataSet();
                sqlDA.Fill(dataSet, "Anonymous");
                
                return dataSet;
            }
        }

        public static DataSet ExecuteDataSetPWDB(string connectionString, string cmdText, SqlParameter[] paras)
        {
            //Log.Dac("Start ExecuteDataSetPWDB:cmdText=" + cmdText);
            DateTime time = DateTime.Now;        
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = cmdText;
                cmd.CommandType = CommandType.Text;
                if (paras != null && paras.Length > 0)
                    cmd.Parameters.AddRange(paras);

                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = cmd;
                DataSet dataSet = new DataSet();
                sqlDA.Fill(dataSet, "Anonymous");
               
                return dataSet;
            }
        }

        public static DataSet ExecuteSPDataSet(string sql, SqlParameter[] paras)
        {
            //Log.Dac("Start ExecuteSPDataSet:sql=" + sql);
            DateTime time = DateTime.Now;       
            SqlConnection conn = new SqlConnection(SqlHelper.ConnectionStringLocal);
            SqlCommand sqlcom = new SqlCommand(sql, conn);
            sqlcom.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter param in paras)
            {
                sqlcom.Parameters.Add(param);
            }
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = sqlcom;
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
           
            return ds;
        }

        public static DataSet ExecuteSPDataSet(string spString)
        {
            //Log.Dac("Start ExecuteSPDataSet 2:spString=" + spString);
            DateTime time = DateTime.Now;     
            SqlConnection conn = new SqlConnection(SqlHelper.ConnectionStringLocal);
            SqlCommand sqlcom = new SqlCommand(spString, conn);
            sqlcom.CommandType = CommandType.StoredProcedure; 
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = sqlcom;
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            
            return ds;
        }

        /// <summary>
        /// 执行有返回结果集的查询方法
        /// </summary>
        /// <param name="connectionString">参数1：数据库连接字符串(string)</param>
        /// <param name="cmdType">参数2：CommandType</param>
        /// <param name="cmdText">参数3：查询语句(command命令)</param>
        /// <param name="commandParameters">参数4：参数数组</param>
        /// <returns>返回值：结果集</returns>
        public static SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            //Log.Dac("Start ExecuteReader:cmdText=" + cmdText);
            DateTime time = DateTime.Now;      
            SqlCommand cmd = new SqlCommand();
            //SqlConnection conn = new SqlConnection(connectionString);
            SqlConnection conn = new SqlConnection(SqlHelper.ConnectionStringLocal); 
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                ParametersClear(cmd);
               
                return rdr;
            }
            catch
            {
                ConnectionClose(conn);
                throw;
            }
        }

        /// <summary>
        /// 在事务中执行有返回结果集的查询方法
        /// </summary>
        /// <param name="trans">参数1：SqlTransaction</param>
        /// <param name="cmdType">参数2：CommandType</param>
        /// <param name="cmdText">参数3：查询语句(command命令)</param>
        /// <param name="commandParameters">参数4：参数数组</param>
        /// <returns>返回值：结果集</returns>
        public static SqlDataReader ExecuteReader(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            //Log.Dac("Start ExecuteReader 2:cmdText=" + cmdText);
            DateTime time = DateTime.Now;    
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            SqlDataReader rdr = cmd.ExecuteReader();
            ParametersClear(cmd);
           
            return rdr;
        }

        /// <summary>
        /// 执行有返回结果集的查询方法
        /// </summary>
        /// <param name="connection">参数1：SqlConnection</param>
        /// <param name="cmdType">参数2：CommandType</param>
        /// <param name="cmdText">参数3：查询语句(command命令)</param>
        /// <param name="commandParameters">参数4：参数数组</param>
        /// <returns>返回值：结果集</returns>
        public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            //Log.Dac("Start ExecuteReader 3:cmdText=" + cmdText);
            DateTime time = DateTime.Now;
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            ParametersClear(cmd);
           
            return rdr;
        }

        public static object ExecuteScalar(string cmdText, params SqlParameter[] commandParameters)
        {
            //Log.Dac("Start ExecuteScalar A1:cmdText=" + cmdText);
            DateTime time = DateTime.Now;
            using (SqlConnection connection = new SqlConnection(SqlHelper.ConnectionStringLocal))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, CommandType.Text, cmdText, commandParameters);
                object o= cmd.ExecuteScalar();
                
                return o;
            }
        }

        #region 新添加方法

        public static int ExecuteNonQuery(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] paras)
        {
            //Log.Dac("Start ExecuteNonQuery New:cmdText=" + cmdText);
            DateTime time = DateTime.Now;           
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = cmdType;
            cmd.CommandText = cmdText;
            cmd.Connection = connection;
            if (connection.State != ConnectionState.Open)
                connection.Open();
            cmd.Parameters.AddRange(paras);
            int n= cmd.ExecuteNonQuery();
            return n;
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmd"></param>
        /// <param name="sysno"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(SqlConnection conn, CommandType cmdType, SqlCommand cmd, out int sysno)
        {
           //Log.Dac("Start ExecuteNonQuery New2:cmd.CommandText=" + cmd.CommandText);
            DateTime time = DateTime.Now;
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandType = cmdType;

            int rowsAffected = cmd.ExecuteNonQuery();

            //必须符合下面的条件
            if (cmd.Parameters.Contains("@SysNo") && cmd.Parameters["@SysNo"].Direction == ParameterDirection.Output)
                sysno = (int)cmd.Parameters["@SysNo"].Value;
            else
            {
                throw new Exception("SqlHelper: Does not contain SysNo or ParameterDirection is Not Output");
            }
            cmd.Parameters.Clear();
           
            return rowsAffected;
        }

        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] paras)
        {
            //Log.Dac("Start ExecuteNonQuery New3:cmdText=" + cmdText);
            DateTime time = DateTime.Now;           
            using (SqlConnection connection = new SqlConnection(SqlHelper.ConnectionStringLocal))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = cmdType;
                cmd.CommandText = cmdText;
                cmd.Connection = connection;
                connection.Open();
                cmd.Parameters.AddRange(paras);
                int n= cmd.ExecuteNonQuery();
                return n;
            }
        }


        public static int ExecuteNonQuery(SqlConnection connection, SqlCommand cmd)
        {
            //Log.Dac("Start ExecuteNonQuery New4:cmd.CommandText=" + cmd.CommandText);
            DateTime time = DateTime.Now;
            cmd.Connection = connection;
            if (connection.State != ConnectionState.Open)
                connection.Open();
            int n = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return n;
        }


        /// <summary>
        /// 执行有返回结果集的查询方法
        /// </summary>
        /// <param name="connection">参数1：SqlConnection</param>
        /// <param name="cmdType">参数2：CommandType</param>
        /// <param name="cmdText">参数3：查询语句(command命令)</param>
        /// <param name="commandParameters">参数4：参数数组</param>
        /// <returns>返回值：结果集</returns>
        public static SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, params SqlParameter[] paras)
        {
            //Log.Dac("Start ExecuteReader New1:cmdText=" + cmdText);
            DateTime time = DateTime.Now;
            SqlConnection connection = new SqlConnection(SqlHelper.ConnectionStringLocal);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = cmdType;
            cmd.CommandText = cmdText;
            cmd.Parameters.AddRange(paras);
            cmd.Connection = connection;
            connection.Open();
            SqlDataReader dr= cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return dr;

        }



   
        /// <summary>
        /// 请保证数据库字段名和参数名一致.(大小写不计)
        /// 根据属性名和参数名对应的添加参数方法,如果mapping中有和实体类属性相关的参数,自行在外面赋值和设置direction
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="mapping"></param>
        /// <param name="cmd"></param>
        public static void BuildObject(object obj, IDataReader dr)
        {
            if (dr.Read())
            {
                Type t = obj.GetType();
                PropertyInfo[] ps = t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
                string name = "";
                DataTable dt = dr.GetSchemaTable();
                int idx;
                object val;
                foreach (PropertyInfo p in ps)// 
                {
                    name = p.Name;
                    if (dt.Select("ColumnName='" + name + "'").Length > 0)
                    {
                        val = dr[name];
                        if (DBNull.Value != val)
                            p.SetValue(obj, val, null);
                    }
                }
            }
        }

        public static DataTable ExecuteDataTable(string tableName, SqlCommand cmd)
        {
            DateTime time = DateTime.Now;
            using (SqlConnection conn = new SqlConnection(SqlHelper.ConnectionStringLocal))
            {
                conn.Open();
                cmd.Connection = conn;
                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = cmd;
                DataSet dataSet = new DataSet();
                sqlDA.Fill(dataSet, tableName);
                return dataSet.Tables[0];
            }
        }

  

        #endregion

        /// <summary>
        /// 为Command命令的执行做准备的方法
        /// </summary>
        /// <param name="cmd">参数1：SqlCommand</param>
        /// <param name="conn">参数2：SqlConnection</param>
        /// <param name="trans">参数3：SqlTransaction事务</param>
        /// <param name="cmdType">参数4：CommandType</param>
        /// <param name="cmdText">参数5：查询语句(command命令)</param>
        /// <param name="cmdParms">参数6：参数数组</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }


        /// <summary>
        /// 清空SqlCommand参数
        /// </summary>
        /// <param name="cmd">参数1：SqlCommand</param>
        public static void ParametersClear(SqlCommand cmd)
        {
            if (cmd != null)
            {
                cmd.Parameters.Clear();
            }
        }

        /// <summary>
        /// 关闭DataReade
        /// </summary>
        /// <param name="reader">参数1：SqlDataReader</param>
        public static void DataReaderClose(SqlDataReader reader)
        {
            if (reader != null)
            {
                reader.Dispose();
                reader.Close();
            }
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        /// <param name="conn">参数1：数据库连接</param>
        public static void ConnectionClose(SqlConnection conn)
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

    }
}
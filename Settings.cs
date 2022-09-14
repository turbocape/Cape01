using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace CapeInterface.Properties {
    // 通过此类可以处理设置类的特定事件: 
    //  在更改某个设置的值之前将引发 SettingChanging 事件。
    //  在更改某个设置的值之后将引发 PropertyChanged 事件。
    //  在加载设置值之后将引发 SettingsLoaded 事件。
    //  在保存设置值之前将引发 SettingsSaving 事件。
    public class ConfigSetting
    {
        /// <summary>
        /// 获取HIS数据库连接字符串
        /// </summary>
        public string OracleConnectionString
        {
            get
            {
                var config = ConfigurationManager.ConnectionStrings["ConnectionString"];
                return config.ConnectionString;
            }
        }
    }

    public class CommDAL
    {
        public DataSet getHisProcedure(string as_method, string as_inparm, ref string isSuccess, ref string errorMsg)
        {
            List<OracleParameter> paramList = new List<OracleParameter>();
            paramList.Add(DBHelper.CreateParameter("as_type", 50, ParameterDirection.Input, as_method));
            paramList.Add(DBHelper.CreateParameter("as_inParm", 250, ParameterDirection.Input, as_inparm));
            paramList.Add(DBHelper.CreateParameter("ar_outparm", ParameterDirection.Output, OracleDbType.RefCursor));
            OracleParameter[] pm = paramList.ToArray();
            DataSet ds = new DataSet();
            try
            {
                ds = DBHelper.RunProc_His_DataSet("comm.pkg_cape.sp_interface", ref pm);
            }
            catch (Exception e)
            {
                isSuccess = "false";
                errorMsg = e.Message;
                TJYD.Helper.Logger.Write(e.ToString());
            }
            return ds;
        }
    }
    internal sealed partial class Settings {
        public Settings() {
            // // 若要为保存和更改设置添加事件处理程序，请取消注释下列行: 
            //
            // this.SettingChanging += this.SettingChangingEventHandler;
            //
            // this.SettingsSaving += this.SettingsSavingEventHandler;
            //
        }
    }
    public class DBHelper
    {
        public static ConfigSetting connStr = new ConfigSetting();

        /// <summary>
        /// 调用HIS互联网医院存储过程
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="pm"></param>
        /// <returns></returns>
        public static DataSet RunProc_His_DataSet(string procName, ref OracleParameter[] pm)
        {
            DataSet ds = new DataSet();
            using (OracleConnection conn = new OracleConnection(connStr.OracleConnectionString))
            {
                conn.Open();
                OracleTransaction ot = ot = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                try
                {
                    OracleCommand cmd = new OracleCommand(procName, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(pm);
                    cmd.Transaction = ot;
                    OracleDataAdapter sda = new OracleDataAdapter(cmd);
                    sda.Fill(ds);
                    ot.Commit();
                }
                catch (Exception e)
                {
                    ot.Rollback();
                    throw e;
                }
            }
            return ds;
        }

        /// <summary>
        /// Oracle参数生成函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="size"></param>
        /// <param name="p_direction"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static OracleParameter CreateParameter(string name, int size, ParameterDirection p_direction, object value, OracleDbType type = OracleDbType.Varchar2)
        {
            OracleParameter pm = new OracleParameter(name, type, size);
            pm.Direction = p_direction;
            if (value == null)
                pm.Value = "";
            else
                pm.Value = value;
            return pm;
        }

        public static OracleParameter CreateParameter(string name, ParameterDirection p_direction, OracleDbType type)
        {
            OracleParameter pm = new OracleParameter(name, type);
            pm.Direction = p_direction;
            return pm;
        }

    }
}

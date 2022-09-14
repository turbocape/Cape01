using System;
using Newtonsoft.Json;
using CapeInterface.Properties;

namespace CapeInterface.Business
{
    public class Service
    {
        /// <summary>
        /// 调用HIS
        /// </summary>
        /// <param name="as_inparm"></param>
        /// <returns></returns>
        public static string Execution(string as_method, string as_inparm)
        {
            TJYD.Helper.Logger.Write("调用接口:" + as_method + "；" + "入参:" + as_inparm);
            string result = "";
            errorResponse error = new errorResponse();
            try
            {
                CommDAL dal = new CommDAL();
                string errorMsg = "";
                string isSuccess = "true";
                var ds = dal.getHisProcedure(as_method, as_inparm, ref isSuccess, ref errorMsg);
                if (ds.Tables.Count > 0)
                {
                    result = ds.Tables[0].Rows[0][0].ToString();
                }
                if (isSuccess == "false")
                {
                    error.isSuccess = "false";
                    error.errorCode = "-1";
                    error.errorMsg = errorMsg;
                }
            }
            catch (Exception ex)
            {
                error.isSuccess = "false";
                error.errorCode = "-1";
                error.errorMsg = ex.Message;
                TJYD.Helper.Logger.Write(ex.ToString());
            }
            if (error.isSuccess == null)
            {
                TJYD.Helper.Logger.Write("出参:" + result);
                return result;
            }
            else
            {
                TJYD.Helper.Logger.Write("出参:" + JsonConvert.SerializeObject(error));
                return JsonConvert.SerializeObject(error);
            }
        }

        public class errorResponse
        {
            public string isSuccess { get; set; }
            public string errorCode { get; set; }
            public string errorMsg { get; set; }
        }
    }
}

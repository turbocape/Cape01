using System.Web.Services;

namespace CapeInterface
{
    /// <summary>
    /// WebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://turbocape.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {
        [WebMethod(Description = "公共接口", EnableSession = true)]
        public string publicInterface(string as_method, string as_inparm)
        {
            return Business.Service.Execution(as_method, as_inparm);
        }
    }
}
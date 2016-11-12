using Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class LogInformation : System.Web.UI.Page
    {
        public string UserIP
        {
            get
            {
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (!string.IsNullOrEmpty(ipAddress))
                {
                    string[] addresses = ipAddress.Split(',');
                    if (addresses.Length != 0)
                    {
                        return addresses[0];
                    }
                }

                return context.Request.ServerVariables["REMOTE_ADDR"];
            }
        }
        public Guid simulationGameID
        {
            get
            {
                return new Guid(Request.Params["simulationGameID"]);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UserLog log = new UserLog();
            log.GameID = simulationGameID;
            log.Country = RegionInfo.CurrentRegion.DisplayName; // this can also be pulled based on the user IP address
            log.HeaderData = Request.Headers.ToString();
            log.Time = DateTime.Now;
            log.UserAgent = Request.Browser.Browser;
            log.UserIP = this.UserIP;

            DAL.SelectSelectionByPage(log);
        }
    }
}
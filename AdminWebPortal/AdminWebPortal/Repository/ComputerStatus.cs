using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace AdminWebPortal.Repository
{
    public class ComputerStatus
    {
        const String ComputerSeesionID = "ComputerSeesionID";
        private HttpSessionState session;
        public int ComputerIDFromSession
        {
            get
            {
                session = this.GetSession();
                return Convert.ToInt32((session[ComputerSeesionID] ?? 0).ToString());
            }
            set
            {
                session = this.GetSession();
                session[ComputerSeesionID] = value;
            }
        }

        private HttpSessionState GetSession()
        {
            HttpSessionState session = HttpContext.Current.Session;
            return session;
        }
    }
}
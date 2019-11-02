using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using iom;
using inc;

namespace IncAppHandler
{
    public class AppIncHandler : IHttpModule
    {
        public AppIncHandler() { }

        public void Init(HttpApplication app)
        {
            app.PreRequestHandlerExecute += new EventHandler(this.Application_PreRequestHandlerExecute);
            app.Error += new EventHandler(this.Application_OnError);
        }

        private void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            HttpContext cntxt = app.Context;
            String ext = String.Empty;
            Boolean ok = false;

            if (app.Session["incuser"] == null)
            {
                String ckynm = FormsAuthentication.FormsCookieName;

                if (app.Request.Cookies[ckynm] != null)
                    using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                    {
                        String ckyvl = app.Request.Cookies[ckynm].Value.ToString();
                        IncCooky rw = db.IncCookies.FirstOrDefault(obj => obj.cookie_name == ckynm && obj.cookie_value == ckyvl);
                        if (rw != null)
                        {
                            IncAuthentication auth = new IncAuthentication();
                            auth.AuthenticateFromCookie(rw.usr_name, rw.cookie_name, rw.cookie_value, ref ok);
                            if (ok) app.Session["incuser"] = auth;
                        }
                    }
            }
        }

        private void Application_OnError(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            HttpContext cntxt = app.Context;

            String eprm = IncUtilxs.GetParameterName("error_message");
            String errmsg = "XYZ";

            if (app.Server.GetLastError() != null)
            {
                Exception xcp = app.Server.GetLastError();
                errmsg = String.Format("Error: {0}", xcp.Message);
            }

            String url = String.Format("error.aspx?{0}={1}", eprm, HttpUtility.UrlEncode(errmsg));
            cntxt.Response.Redirect(url);
        }

        public void Dispose()
        {
        }
    }
}
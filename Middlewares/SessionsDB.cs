using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Team1_Website.Data;
using Team1_Website.Models;

namespace Team1_Website.Middlewares
{
    public class SessionsDB
    {
        private readonly RequestDelegate next;

        public SessionsDB(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, [FromServices] DBContext dbContext)
        {
            Session mySession = null;

            if (context.Request.Cookies["SessionId"] != null)
            {
                Guid sessionId = Guid.Parse(context.Request.Cookies["SessionId"]);
                mySession = dbContext.Sessions.FirstOrDefault(x =>
                    x.Id == sessionId
                );
            }

            if (mySession == null)
            {
                mySession = new Session();
                dbContext.Sessions.Add(mySession);
                dbContext.SaveChanges();

                context.Response.Cookies.Append("SessionId", mySession.Id.ToString());
                context.Response.Cookies.Delete("FullName");
                context.Response.Cookies.Delete("BringTo");
            }

            context.Items["mySession"] = mySession;

            await this.next(context);
        }
    }
}

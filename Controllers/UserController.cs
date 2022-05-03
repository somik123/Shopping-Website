using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Team1_Website.Data;
using Team1_Website.Models;

namespace Team1_Website.Controllers
{
    public class UserController : Controller
    {
        private DBContext DbConn;
        public UserController(DBContext DbConn)
        {
            this.DbConn = DbConn;
        }

        public IActionResult Index()
        {
            Session mySession = (Session)HttpContext.Items["mySession"];

            if (mySession.User == null)
            {
                Response.Cookies.Append("BringTo", "/User");
                return RedirectToAction("Login");
            }

            List<Comment> comments = DbConn.Comments.Where(x => x.User.Equals(mySession.User)).ToList();
            List<Rate> rates = DbConn.Rates.Where(x => x.User.Equals(mySession.User)).ToList();
            ViewBag.comments = comments;
            ViewBag.rates = rates;
            ViewBag.purchaseHistory = mySession.User.PurchaseHistory;
            return View();
        }

        public IActionResult Login(string username, string password)
        {
            return View();
        }

        public IActionResult ChangePassword(string old_password_hash, string new_password_hash)
        {
            string myMessage = "";
            bool error = false;

            //Session mySession = ValidateSession();
            Session mySession = (Session)HttpContext.Items["mySession"];

            if (mySession.User == null)
            {
                Response.Cookies.Append("BringTo", "/User/ChangePassword");
                return RedirectToAction("Login");
            }

            if (old_password_hash != null && new_password_hash != null)
            {
                if (mySession.User.Password == old_password_hash)
                {
                    mySession.User.Password = new_password_hash;
                    DbConn.SaveChanges();

                    myMessage = "Password changed successfully";
                    error = false;
                }
                else
                {
                    myMessage = "Current password do not match";
                    error = true;
                }
            }

            ViewBag.Message = myMessage;
            ViewBag.Error = error;

            return View();
        }

        public IActionResult Authenticate(string username, string password_hash)
        {
            //Session mySession = ValidateSession();
            Session mySession = (Session)HttpContext.Items["mySession"];

            string error_txt = "";
            // If user enters username and password
            if (username != null && password_hash != null)
            {
                // Match against database
                User myUser = DbConn.Users.FirstOrDefault(x =>
                    x.UserName == username
                );

                if (myUser != null && myUser.Password == password_hash)
                {
                    mySession.User = myUser;
                    DbConn.SaveChanges();

                    Response.Cookies.Append("FullName", myUser.FirstName + " " + myUser.LastName);

                    if (Request.Cookies["BringTo"] != null)
                    {
                        Response.Cookies.Delete("BringTo");
                        return Redirect(Request.Cookies["BringTo"]);
                    }

                    return RedirectToAction("Index", "Shop");
                }
                else
                {
                    error_txt = "Username or password does not match.";
                }
            }

            ViewBag.error_txt = error_txt;
            return View("Login");
        }

        public IActionResult Logout()
        {
            // remove session from our database
            if (Request.Cookies["SessionId"] != null)
            {
                Guid sessionId = Guid.Parse(Request.Cookies["sessionId"]);
                Session session = DbConn.Sessions.FirstOrDefault(x => x.Id == sessionId);

                if (session != null)
                {
                    DbConn.Remove(session);
                    DbConn.SaveChanges();
                }
            }

            // Clear user session to log him out
            HttpContext.Session.Clear();
            Response.Cookies.Delete("SessionId");
            Response.Cookies.Delete("FullName");
            Response.Cookies.Delete("BringTo");
            return RedirectToAction("Login");
        }
    }
}

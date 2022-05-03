using Microsoft.AspNetCore.Mvc;
using Team1_Website.Models;
using Team1_Website.Data;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text.Json;
using Newtonsoft.Json;

namespace Team1_Website.Controllers
{
    public class APIController : Controller
    {
        private DBContext DbConn;

        public APIController(DBContext DbConn)
        {
            this.DbConn = DbConn;
        }

        public IActionResult Index(string action)
        {
            string myOutput = "";
            ViewBag.myOutput = myOutput;
            return PartialView();
        }

        public IActionResult ManageCart(string productId, string action)
        {
            //Session mySession = ValidateSession();
            Session mySession = (Session)HttpContext.Items["mySession"];

            DB myDB = new DB(DbConn);

            Dictionary<string, string> myDictionary = null;

            if (action == "add")
                myDictionary = myDB.AddProductToCart(mySession, productId);
            else if (action == "rem")
                myDictionary = myDB.RemoveProductToCart(mySession, productId);
            else if (action == "del")
                myDictionary = myDB.DeleteProductToCart(mySession, productId);

            ViewBag.myOutput = JsonConvert.SerializeObject(myDictionary);
            return PartialView("Index");
        }

        public IActionResult ConfirmPayment([FromBody] JsonElement body, string TransactionCode)
        {
            //Session mySession = ValidateSession();
            Session mySession = (Session)HttpContext.Items["mySession"];

            Dictionary<string, string> myDictionary = new Dictionary<string, string>();

            string ClientResponse = System.Text.Json.JsonSerializer.Serialize(body);

            DB myDB = new DB(DbConn);
            bool status = myDB.ProcessPayment(mySession, ClientResponse, TransactionCode);

            if (status)
                myDictionary["status"] = "OK";

            ViewBag.myOutput = JsonConvert.SerializeObject(myDictionary);
            return PartialView("Index");
        }

        public String Install()
        {
            DbConn.Database.EnsureDeleted();
            DbConn.Database.EnsureCreated();

            DB myDb = new DB(DbConn);
            myDb.Seed();

            return "Database recreated and reseeded";
        }

        public IActionResult LeaveComment(string comment, Guid productId)
        {
            //Session mySession = ValidateSession();
            Session mySession = (Session)HttpContext.Items["mySession"];

            DB myDB = new DB(DbConn);
            Product product = DbConn.Products.Where(x => x.Id.Equals(productId)).FirstOrDefault();
            myDB.LeaveComment(mySession, comment, product);
            return View("Index", "Shop");
        }

        public IActionResult DeleteComment(Guid commentId)
        {
            //Session mySession = ValidateSession();
            Session mySession = (Session)HttpContext.Items["mySession"];

            DB myDB = new DB(DbConn);
            myDB.DeleteComment(commentId);
            return View("Index", "Shop");
        }

        public IActionResult Rating(double rate, Guid productId)
        {
            Session mySession = (Session)HttpContext.Items["mySession"];
            DB myDB = new DB(DbConn);
            Product product = DbConn.Products.Where(x => x.Id.Equals(productId)).FirstOrDefault();
            myDB.Rating(mySession, rate, product);
            return View("Index", "Shop");
        }

        public IActionResult UpdateRating(double rate, Guid productId)
        {
            Session mySession = (Session)HttpContext.Items["mySession"];
            DB myDB = new DB(DbConn);
            Product product = DbConn.Products.Where(x => x.Id.Equals(productId)).FirstOrDefault();
            myDB.UpdateRating(mySession, rate, product);
            return View("Index", "Shop");
        }
    }
}

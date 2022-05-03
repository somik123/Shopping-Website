using Microsoft.AspNetCore.Mvc;
using Team1_Website.Models;
using Team1_Website.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System;
using Newtonsoft.Json;
using System.IO;
using MimeKit;

namespace Team1_Website.Controllers
{
    public class ShopController : Controller
    {
        private DBContext DbConn;
        public ShopController(DBContext DbConn)
        {
            this.DbConn = DbConn;
        }

        public IActionResult Index(string search_txt, string min_price, string max_price)
        {
            Session mySession = (Session)HttpContext.Items["mySession"];

            if (search_txt == null)
                search_txt = "";

            // C# search does not return non-selected products, so JS search breaks...
            /*
            List<Product> myProducts = DbConn.Products.Where(x =>
                    (x.Name.Contains(search_txt) || x.Desc.Contains(search_txt)) &&
                    x.Price > StringToInt(min_price,0) && x.Price < StringToInt(max_price,500)
            ).ToList();
            */

            List<Product> myProducts = DbConn.Products.ToList();
            List<Comment> comments = DbConn.Comments.ToList();

            DB myDB = new DB(DbConn);
            (_, int totalItems, _) = myDB.ProductToCartHelper(mySession, null);

            ViewBag.myProducts = myProducts;
            ViewBag.search_txt = search_txt;
            ViewBag.min_price = StringToInt(min_price, 0);
            ViewBag.max_price = StringToInt(max_price, 500);

            ViewData["ShopButton"] = "<span id=\"cart_price_btn\" onclick=\"location.href='/Shop/Cart';\" class=\"btn btn-outline-primary font-weight-bold bg-white text-dark\" style=\"border-radius:20px; cursor: pointer;\"><img src=\"/img/cart.png\" height=\"24\" /> " + totalItems + "</span>";
            List<Product> recommendProducts = myDB.recommendProducts();
            ViewBag.recommendProducts = recommendProducts;
            return View();
        }

        public IActionResult Cart()
        {
            Session mySession = (Session)HttpContext.Items["mySession"];

            DB myDB = new DB(DbConn);
            (_, _, float totalPrice) = myDB.ProductToCartHelper(mySession, null);

            ViewData["ShopButton"] = "<span id=\"cart_price_btn\" onclick=\"location.href='/Shop/Cart';\" class=\"btn btn-outline-primary font-weight-bold bg-white text-dark\" style=\"border-radius:20px; cursor: pointer;\"> SGD " + String.Format("{0:c}", totalPrice) + " </span>";

            ViewBag.Cart = mySession.ShoppingCart;
            ViewBag.totalPrice = totalPrice;

            return View();
        }

        public IActionResult Payment()
        {
            Session mySession = (Session)HttpContext.Items["mySession"];

            // Ensure user is logged in before proceeding
            if (mySession.User == null)
            {
                Response.Cookies.Append("BringTo", "/Shop/Payment");
                return RedirectToAction("Login", "User");
            }

            DB myDB = new DB(DbConn);
            (_, _, float totalPrice) = myDB.ProductToCartHelper(mySession, null);

            List<PaypalJS> myPaypal = new List<PaypalJS>();

            bool cartEmpty = false;
            string paypal_json = "";
            if (mySession.ShoppingCart == null || mySession.ShoppingCart.CartItems == null || mySession.ShoppingCart.CartItems.Count() == 0)
            {
                cartEmpty = true;
            }
            else
            {
                foreach (CartItem myItem in mySession.ShoppingCart.CartItems)
                {
                    myPaypal.Add(new PaypalJS
                    {
                        name = myItem.Product.Name,
                        description = myItem.Product.Desc,
                        unit_amount = new UnitAmount
                        {
                            currency_code = "SGD",
                            value = myItem.Product.Price.ToString()
                        },
                        quantity = myItem.Quantity.ToString()
                    });
                }
                paypal_json = JsonConvert.SerializeObject(myPaypal);
            }


            ViewData["ShopButton"] = "<span id=\"cart_price_btn\" onclick=\"location.href='/Shop/Cart';\" class=\"btn btn-outline-primary font-weight-bold bg-white text-dark\" style=\"border-radius:20px; cursor: pointer;\"> SGD " + String.Format("{0:c}", totalPrice) + " </span>";

            ViewBag.cartEmpty = cartEmpty;
            ViewBag.totalPrice = totalPrice;
            ViewBag.paypal_json = paypal_json;

            return View();
        }


        private int StringToInt(string data, int defaultVal)
        {
            int myData;
            bool status = int.TryParse(data, out myData);
            if (status)
                return myData;
            else
                return defaultVal;
        }

        public IActionResult DownloadFile(string filepath)
        {
            Session mySession = (Session)HttpContext.Items["mySession"];

            // Ensure user is logged in before proceeding
            if (mySession.User == null)
            {
                return RedirectToAction("Login", "User");
            }

            return GetDownloadFile(filepath);
        }

        private IActionResult GetDownloadFile(string filePath)
        {
            var path = Path.Combine(
               Directory.GetCurrentDirectory(),
               "wwwroot/img", filePath);

            return PhysicalFile(path, MimeTypes.GetMimeType(path), Path.GetFileName(path));
        }


        public IActionResult ViewDetail(string Id)
        {
            DB myDB = new DB(DbConn);
            Session mySession = (Session)HttpContext.Items["mySession"];
            (Product myProduct, int totalItems, _) = myDB.ProductToCartHelper(mySession, Id);
            
            /*
            Guid productId = new Guid(Id);
            Product currentProduct = myDB.DetailProduct(productId);
            List<Comment> comments = myDB.getcommentsperProduct(myProduct);
            */

            List<Comment> comments = DbConn.Comments.Where(x => 
                x.Product.Id.ToString() == Id
            ).ToList();

            int apromixRate = 0;
            List<Rate> rates = DbConn.Rates.Where(x => x.Product.Id.ToString() == Id).ToList();
            if (rates.Count != 0)
            {
                double rate = Math.Round(rates.Select(x => x.Stars).Average(), 0);
                apromixRate = Convert.ToInt32(rate);
            }
            

            ViewData["ShopButton"] = "<span id=\"cart_price_btn\" onclick=\"location.href='/Shop/Cart';\" class=\"btn btn-outline-primary font-weight-bold bg-white text-dark\" style=\"border-radius:20px; cursor: pointer;\"><img src=\"/img/cart.png\" height=\"24\" /> " + totalItems + "</span>";

            ViewBag.myProducts = myProduct;
            ViewBag.comments = comments;
            ViewBag.rate = apromixRate;
            return View("ViewDetail");
        }
    }
}

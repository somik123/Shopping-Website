using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Team1_Website.Models;

namespace Team1_Website.Data
{
    public class DB
    {
        private DBContext dbContext;

        public DB(DBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Seed()
        {
            SeedUsers();
            SeedProducts();
        }

        private void SeedUsers()
        {
            dbContext.Add(new User
            {
                FirstName = "Kim",
                LastName = "Wong",
                UserName = "kimwg",
                Password = HashString_SHA256("ahAsdw23")
            });

            dbContext.Add(new User
            {
                FirstName = "Susan",
                LastName = "Tan",
                UserName = "susant",
                Password = HashString_SHA256("kbtJG112")
            });


            dbContext.Add(new User
            {
                FirstName = "Jean",
                LastName = "Lim",
                UserName = "jeanl",
                Password = HashString_SHA256("oplkFDas99")
            });


            dbContext.Add(new User
            {
                FirstName = "James",
                LastName = "Sim",
                UserName = "jamess",
                Password = HashString_SHA256("YkwerTHs34")
            });


            dbContext.Add(new User
            {
                FirstName = "John",
                LastName = "Ho",
                UserName = "johnh",
                Password = HashString_SHA256("Bcfasd6890")
            });

            dbContext.Add(new User
            {
                FirstName = "Somik",
                LastName = "Khan",
                UserName = "somik",
                Password = HashString_SHA256("mypassword")
            });

            dbContext.SaveChanges();
        }

        private void SeedProducts()
        {
            dbContext.Add(new Product
            {
                Name = ".NET Charts",
                Desc = "Brings powerful charting capabilities to your .NET applications",
                Price = 99,
                ImagePath = "charts.png"   //here what is the location of imagepath
            });



            dbContext.Add(new Product
            {
                Name = ".NET PayPal",
                Desc = "Integrate your .NET apps with PayPal the easy way!",
                Price = 69,
                ImagePath = "paypal.jpg"
            });


            dbContext.Add(new Product
            {
                Name = ".NET ML",
                Desc = "Supercharged .NET machine learning libraries",
                Price = 299,
                ImagePath = "ml.jpg"
            });



            dbContext.Add(new Product
            {
                Name = ".NET Analytics",
                Desc = "Performs data mining and analytics easily in .NET",
                Price = 299,
                ImagePath = "analytics.jpg"
            });




            dbContext.Add(new Product
            {
                Name = ".NET Logger",
                Desc = "Logs and aggregrates events easily in your .NET apps",
                Price = 49,
                ImagePath = "logger.jpg"
            });



            dbContext.Add(new Product
            {
                Name = ".NET Numerics",
                Desc = "Powerful numerical methods for your .NET simulations",
                Price = 199,
                ImagePath = "numeric.jpg"
            });

            dbContext.SaveChanges();
        }




        public Dictionary<string, string> AddProductToCart(Session mySession, string productId)
        {
            Dictionary<string, string> myDictionary = new Dictionary<string, string>();

            (Product myProduct, int totalItems, float totalPrice) = ProductToCartHelper(mySession, productId);

            // Invalid productID
            if (myProduct == null)
            {
                myDictionary["status"] = "fail";
                myDictionary["error"] = "Invalid product ID";
                return myDictionary;
            };

            // Check if product exists in cart or not
            bool itemFound = false;
            foreach (CartItem myItem in mySession.ShoppingCart.CartItems)
            {
                if (myItem.Product.Id.ToString() == productId)
                {
                    myItem.Quantity++;
                    itemFound = true;
                }
            }

            if (itemFound == false)
            {
                mySession.ShoppingCart.CartItems.Add(new CartItem
                {
                    Product = myProduct,
                    Quantity = 1
                });
            }

            totalItems++;
            totalPrice += myProduct.Price;

            dbContext.SaveChanges();

            myDictionary["status"] = "OK";
            myDictionary["Total_Items"] = totalItems.ToString();
            myDictionary["Total_Price"] = String.Format("{0:c}", totalPrice);
            myDictionary["Product_Name"] = myProduct.Name;
            return myDictionary;
        }






        public Dictionary<string, string> RemoveProductToCart(Session mySession, string productId)
        {
            Dictionary<string, string> myDictionary = new Dictionary<string, string>();

            (Product myProduct, int totalItems, float totalPrice) = ProductToCartHelper(mySession, productId);

            // Invalid productID
            if (myProduct == null)
            {
                myDictionary["status"] = "fail";
                myDictionary["error"] = "Invalid product ID";
                return myDictionary;
            };

            // Check if product exists in cart or not
            foreach (CartItem myItem in mySession.ShoppingCart.CartItems)
            {
                if (myItem.Product.Id.ToString() == productId)
                {
                    if (myItem.Quantity > 1)
                        myItem.Quantity--;
                }
            }

            totalItems--;
            totalPrice -= myProduct.Price;

            dbContext.SaveChanges();

            myDictionary["status"] = "OK";
            myDictionary["Total_Items"] = totalItems.ToString();
            myDictionary["Total_Price"] = String.Format("{0:c}", totalPrice);
            return myDictionary;
        }





        public Dictionary<string, string> DeleteProductToCart(Session mySession, string productId)
        {
            Dictionary<string, string> myDictionary = new Dictionary<string, string>();

            (Product myProduct, int totalItems, float totalPrice) = ProductToCartHelper(mySession, productId);

            // Invalid productID
            if (myProduct == null)
            {
                myDictionary["status"] = "fail";
                myDictionary["error"] = "Invalid product ID";
                return myDictionary;
            };

            foreach (CartItem myItem in mySession.ShoppingCart.CartItems)
            {
                if (myItem.Product.Id.ToString() == productId)
                {
                    mySession.ShoppingCart.CartItems.Remove(myItem);
                    break;
                }
            }
            dbContext.SaveChanges();

            myDictionary["status"] = "OK";
            myDictionary["Reload"] = "YES";
            return myDictionary;
        }





        public (Product, int, float) ProductToCartHelper(Session mySession, string productId)
        {
            // Ensure shopping cart exists or create it
            if (mySession.ShoppingCart == null)
                mySession.ShoppingCart = new ShoppingCart();

            // Ensure cart items list exists or create it
            if (mySession.ShoppingCart.CartItems == null)
                mySession.ShoppingCart.CartItems = new List<CartItem>();

            // Match product with db
            Product myProduct = dbContext.Products.FirstOrDefault(
                    x => x.Id.ToString() == productId
            );

            int totalItems = 0;
            float totalPrice = 0;
            foreach (CartItem myItem in mySession.ShoppingCart.CartItems)
            {
                totalItems += myItem.Quantity;
                totalPrice += myItem.Quantity * myItem.Product.Price;
            }

            return (myProduct, totalItems, totalPrice);
        }




        public bool ProcessPayment(Session mySession, string ClientResponse, string TransactionCode)
        {
            // Ensure shopping cart exists or create it
            if (mySession.ShoppingCart == null)
                mySession.ShoppingCart = new ShoppingCart();

            mySession.ShoppingCart.Payment = new Payment
            {
                ClientResponse = ClientResponse,
                TransactionCode = TransactionCode
            };

            //Move items to user's inventory
            Dictionary<string, int> productList = new Dictionary<string, int>();

            foreach (CartItem myItem in mySession.ShoppingCart.CartItems)
            {
                productList[myItem.Product.Id.ToString()] = myItem.Quantity;
            }

            PurchaseProduct(mySession.User.Id.ToString(), mySession.ShoppingCart.Payment, productList);

            mySession.ShoppingCart = new ShoppingCart();

            dbContext.SaveChanges();

            return true;
        }


        public void LeaveComment(Session session, string commentString, Product product)
        {
            Comment comment = new Comment();
            comment.CommentString = commentString;
            comment.User = session.User;
            comment.Product = product;
            dbContext.Add(comment);
            dbContext.SaveChanges();
        }

        public void DeleteComment(Guid commentId)
        {
            Comment comment = dbContext.Comments.Where(x => x.Id.Equals(commentId)).FirstOrDefault();
            dbContext.Remove(comment);
            dbContext.SaveChanges();
        }

        public void Rating(Session session, double rateStars, Product product)
        {
            Rate rate = new Rate();
            rate.Product = product;
            rate.Stars = rateStars;
            rate.User = session.User;
            dbContext.Add(rate);
            dbContext.SaveChanges();
        }

        public void UpdateRating(Session session, double rateStars, Product product)
        {
            Rate rate = dbContext.Rates.Where(x => x.User.Equals(session.User)).Where(x => x.Product.Equals(product)).FirstOrDefault();
            rate.Stars = rateStars;
            dbContext.SaveChanges();
        }


        public void PurchaseProduct(string userId, Payment myPayment, Dictionary<string, int> productList)
        {
            User myUser = dbContext.Users.FirstOrDefault(x => x.Id.ToString() == userId);

            List<PurchaseList> myPurchaseList = new List<PurchaseList>();
            foreach (KeyValuePair<string, int> myProductId in productList)
            {
                List<ActivationCode> myActivationCodes = new List<ActivationCode>();

                Product myProduct = dbContext.Products.FirstOrDefault(
                    x => x.Id.ToString() == myProductId.Key
                );
                for (int i = 0; i < myProductId.Value; i++)
                {
                    myActivationCodes.Add(new ActivationCode());
                }

                myPurchaseList.Add(new PurchaseList
                {
                    Product = myProduct,
                    ActivationCodes = myActivationCodes,
                    Quantity = myActivationCodes.Count,
                    DownloadLink = myProduct.ImagePath
                });
            }

            myUser.PurchaseHistory.Add(new PurchaseHistory
            {
                PurchaseDate = System.DateTime.Now,
                PurchaseList = myPurchaseList,
                Payment = myPayment
            });

            dbContext.SaveChanges();
        }


        public string HashString_SHA256(string myString)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(myString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }


        public List<Product> recommendProducts()
        {
            List<PurchaseHistory> purchaseHistory = new List<PurchaseHistory>();
            List<PurchaseList> purchaseList = new List<PurchaseList>();
            Product product = new Product();
            List<Product> recommendProducts = new List<Product>();

            recommendProducts = dbContext.Products.Where(x => x.Price <= 50).Take(5).ToList();

            return recommendProducts;
        }

        /*
        public Product DetailProduct(Guid productId)
        {
            Product currentProduct = dbContext.Products.FirstOrDefault(x => x.Id == productId);
            return currentProduct;
        }
        

        public List<Comment> getcommentsperProduct(Product currentProduct)
        {
            List<Comment> comment = dbContext.Comments.Where(x => x.Product.Id == currentProduct.Id).ToList();
            return comment;
        }
        */
    }
}

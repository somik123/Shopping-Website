# Shopping-Cart

### .NET CORE CA Project Feature List

**Overall**
1. [x] Merge Nyan's code
1. [x] Merge Amber's code
1. [x] Merge Ziyu's code

**User Login Page**
1. [x] Add SHA256 encryption to passwords in DB
1. [x] Add user "somik" with password "mypassword" and seed PurchaseHistory in DB
1. [x] Encrypt passwords with SHA256 on client side using JS before sending
1. [x] Add validations to user login page
1. [x] Redirect user based on where he was before logon

**User Controller**
1. [x] Generate session and validate it
1. [x] Authenticate user login
1. [x] Process logout
1. [x] Remove cookie if user not logged in
1. [x] Save full name to cookie
1. [x] Add MyPurchases page
1. [x] Add functionality for "Download" button
1. [x] Redirect user based on where he was before logon
1. [x] Save user and session to db

**Layout Page**
1. [x] Add full name to Layout page
1. [x] Fix layout navbar nav menu
1. [x] Change nav color to dark
1. [x] Add cart icon to Shop pages
1. [x] Add price display icon on payment page

**Shop Controller**
1. [x] Add shop gallery
1. [x] Keep original search option when pressing enter or clicking button
1. [x] Add Ajax request for "Add to cart" button
1. [x] Add carts page
1. [x] Allow change quantity on carts page
1. [x] Dynamically update total items in cart based on input
1. [x] Validate payment is accepted by paypal
1. [x] Save payment details to database under shopping cart
1. [x] Add features to reduce or remove items from cart

**DB Data file**
1. [x] Change seeded purchases to go by userID and purchaseID
1. [x] Add method to bring items in cart to user inventory upon successful purchase

**Models**
1. [x] Add additional model to handle cart items
1. [x] Add additional model to handle payments
1. [x] Add both models to ShoppingCart

**OPTIONAL - Additional Features??**
1. [x] Integrate payment gateway
1. [x] Add a product details page
1. [x] Rate products
1. [x] Review products
1. [x] Manage account or change password
1. [x] Recommend products (most popular/similar)
1. [ ] Discounts/Coupons/Flash-sales/Giveway
1. [ ] Spin the wheel to get a free product
1. [x] Add "responsive" search using only JS
1. [x] Search by price
1. [x] Confirmation message for adding item to cart
1. [ ] Add option to show number of users online
1. [ ] Remove expired sessions

// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function AddRemoveFromCart(productId, add_remove) {
    let params = "productId=" + productId + "&action=" + add_remove;

    let urlParts = location.href.split("/");
    let pagePath = urlParts[urlParts.length - 1];

    // Update quantity live
    if (pagePath == "Cart") {
        let quantity_elem = document.getElementById("quantity_" + productId);

        if (add_remove == "add") {
            quantity_elem.innerHTML = parseInt(quantity_elem.innerHTML) + 1;
        }
        else if (add_remove == "rem") {
            if (parseInt(quantity_elem.innerHTML) > 1)
                quantity_elem.innerHTML = quantity_elem.innerHTML - 1;
            else
                return false;
        }
    }
    AddRemoveFromCart_API(params, pagePath);
    //alert(params);
    return false;
}


function AddRemoveFromCart_API(params, pagePath) {
    var http = new XMLHttpRequest();
    http.open("POST", "/API/ManageCart", true);
    http.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    http.send(params);

    http.onload = function () {
        if (this.status == 200) {
            let api_reply = JSON.parse(http.responseText);
            //alert(http.responseText);
            if (api_reply['status'] == "OK") {

                if (api_reply["Reload"] == "YES") {
                    //alert("Reloading...");
                    location.href = location.href;
                    return;
                }
                let cart_price_btn = document.getElementById("cart_price_btn");
                if (pagePath == "Cart") {
                    cart_price_btn.innerHTML = "SGD " + api_reply["Total_Price"];
                }
                else {
                    ItemAddedNotification(api_reply["Product_Name"], api_reply["Total_Items"]);
                }
                return true;
            }
        }
    }
    return false;
}

function LeaveComment(productId,reload=true) {
    let commentString = document.getElementById('comment_'+productId).value;
    let params = "productId=" + productId + "&comment=" + commentString;
    var http = new XMLHttpRequest();
    http.open("POST", "/API/LeaveComment/", true);
    http.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    http.send(params);
    http.onload = function () {
        if (this.status == 200) {
            if (reload)
                location.reload();
        } else {
            return false
        }
    }
}

function DeleteComment(commentId) {
    
    let params = "commentId=" + commentId;
    var http = new XMLHttpRequest();
    http.open("POST", "/API/DeleteComment/", true);
    http.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    http.send(params);
    http.onload = function () {
        if (this.status == 200) {
            location.reload();
        } else {
            return false
        }
    }
}

function Rating(obj, productId) {
    //Submit comment first
    let commentString = document.getElementById('comment_' + productId).value;
    if (commentString.length>1)
        LeaveComment(productId, reload = false);

    //var obj = document.getElementById("purchase_rating");
    var index = obj.selectedIndex;
    var val = obj.options[index].value;
    let params = "productId=" + productId + "&rate=" + val;
    var http = new XMLHttpRequest();
    http.open("POST", "/API/Rating/", true);
    http.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    http.send(params);
    http.onload = function () {
        if (this.status == 200) {
            location.reload();
        } else {
            return false
        }
    }
}

function UpdateRating(obj, productId) {
    //Submit comment first
    let commentString = document.getElementById('comment_' + productId).value;
    if (commentString.length > 1)
        LeaveComment(productId, reload = false);

    //var obj = document.getElementById("purchase_rating");
    var index = obj.selectedIndex;
    var val = obj.options[index].value;
    let params = "productId=" + productId + "&rate=" + val;
    var http = new XMLHttpRequest();
    http.open("POST", "/API/UpdateRating/", true);
    http.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    http.send(params);
    http.onload = function () {
        if (this.status == 200) {
            location.reload();
        } else {
            return false
        }
    }
}


function ProcessPayment_API(json_data, TransactionCode) {
    let params = "TransactionCode=" + TransactionCode;
    var http = new XMLHttpRequest();
    http.open("POST", "/API/ConfirmPayment?" + params, true);
    http.setRequestHeader("Content-type", "application/json");
    http.send(json_data);

    http.onload = function () {
        if (this.status == 200) {
            let api_reply = JSON.parse(http.responseText);
            //alert(http.responseText);
            if (api_reply['status'] == "OK") {

                document.getElementById("paypal-button-container").innerHTML = '<h3>Payment Verified. Redirecting...</h3>';
                // Add delay to prevent redirect "before" the purchase is added to user's page
                setTimeout(function () {
                    location.href = "/User";
                }, 6000);
                return true;
            }
        }
    }
    return false;
}






function SubmitValidate() {
    let user_validate = FormValidate('username', 5);
    let pass_validate = FormValidate('password', 8);

    if (user_validate && pass_validate) {
        return true;
    }
    else {
        return false;
    }
}



function SubmitValidate2() {
    let user_validate = FormValidate('current-password', 8);
    let pass_validate1 = FormValidate('new-password', 8);
    let pass_validate2 = validateNewPasswords();

    if (user_validate && pass_validate1 && pass_validate2) {
        return true;
    }
    else {
        return false;
    }
}


function FormValidate(id_name, min_length) {
    let data_obj = document.getElementById(id_name);
    let validation_obj = document.getElementById("invalid-" + id_name);

    if (data_obj.value.length < min_length) {
        validation_obj.innerHTML = "Invalid " + id_name.replaceAll("-"," ") + ". Must be atleast " + min_length + " characters.";
        validation_obj.style.display = "block";
        return false;
    }
    else {
        validation_obj.style.display = "none";
        return true;
    }
}


function EncryptPassword(id_name) {
    let data_obj = document.getElementById(id_name);
    FormValidate(id_name, 8);
    let password = data_obj.value;
    let hashed_password = SHA256(password);
    document.getElementById(id_name + "-hash").value = hashed_password;
}


function validateNewPasswords() {
    let password_1 = document.getElementById("new-password");
    let password_2 = document.getElementById("confirm-password");
    let validation_obj = document.getElementById("invalid-confirm-password");

    if (password_2.value == "") {
        validation_obj.innerHTML = "Confirm password cannot be blank";
        validation_obj.style.display = "block";
    }
    else if (password_1.value != password_2.value) {
        validation_obj.innerHTML = "New passwords do not match.";
        validation_obj.style.display = "block";
        return false;
    }
    else {
        validation_obj.style.display = "none";
        EncryptPassword("confirm-password");
        return true;
    }
}
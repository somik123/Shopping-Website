
function SearchProductJS() {
    SearchStr = document.getElementById("search_txt").value.toLowerCase();

    let min_price = parseInt(document.getElementById("min_price").value);
    let max_price = parseInt(document.getElementById("max_price").value);

    let titleArr = document.getElementsByClassName("product-name");
    let descArr = document.getElementsByClassName("product-desc");
    let PriceArr = document.getElementsByClassName("product-price");


    //alert(titleArr.length);

    for (let i = 0; i < titleArr.length; i++) {
        let title = titleArr[i];
        let desc = descArr[i];
        let price = parseInt(PriceArr[i].value);

        //alert(price + "\n" + min_price + "\n" + max_price);

        let str_found = false;
        let price_match = false;

        if (price > min_price && price < max_price) {
            price_match = true;
        }

        if (SearchStr.length === 0) {
            str_found = true;
        }
        else if (title.innerHTML.toLowerCase().includes(SearchStr)) {
            str_found = true;
        }
        else if (desc.innerHTML.toLowerCase().includes(SearchStr)) {
            str_found = true;
        }

        if (str_found && price_match) {
            title.parentElement.parentElement.style.display = "";
        }
        else {
            desc.parentElement.parentElement.style.display = "none";
        }
    }
}





function ItemAddedNotification(item_name,quantity) {
    let cart_price_btn = document.getElementById("cart_price_btn");

    let message = item_name + " added to cart";

    cart_price_btn.innerHTML = message;

    setTimeout(function () {
        cart_price_btn.innerHTML = "<img src=\"/img/cart.png\" height=\"24\" /> " + quantity;
    }, 2000);
}



function setCookie(cname, cvalue, hours) {
    const d = new Date();
    d.setTime(d.getTime() + (hours * 60 * 60 * 1000));
    let expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}








function activateCheats() {
    setCookie("Konami_Code", "Active", 1); // Activate for 1 hour

    alert("Cheats activated!");
    location.href = "/Home/Code";
}




window.onload = function () {

    SearchProductJS();

    // a key map of allowed keys
    var allowedKeys = {
        37: 'left',
        38: 'up',
        39: 'right',
        40: 'down',
        65: 'a',
        66: 'b'
    };

    // the 'official' Konami Code sequence
    var konamiCode = ['up', 'up', 'down', 'down', 'left', 'right', 'left', 'right', 'b', 'a'];

    // a variable to remember the 'position' the user has reached so far.
    var konamiCodePosition = 0;

    // add keydown event listener
    document.addEventListener('keydown', function (e) {
        // get the value of the key code from the key map
        var key = allowedKeys[e.keyCode];
        // get the value of the required key from the konami code
        var requiredKey = konamiCode[konamiCodePosition];

        if (key == requiredKey) {
            konamiCodePosition++;

            // if the last key is reached, activate cheats
            if (konamiCodePosition == konamiCode.length) {
                activateCheats();
                konamiCodePosition = 0;
            }
        } else {
            konamiCodePosition = 0;
        }
    });

}
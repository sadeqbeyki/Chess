"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
//۰۰۰
document.getElementById("messageInput").addEventListener("keypress", function (event) {
    if (event.key === "Enter") {
        event.preventDefault(); // جلوگیری از ارسال فرم پیش‌فرض
        sendMessage(); // فراخوانی تابع ارسال پیام
    }
});

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.innerHTML = `<strong>${user}</strong> : ${message}`;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").getAttribute("data-username");
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

//۰۰۰
function sendMessage() {
    var message = document.getElementById("messageInput").value;
    var user = document.getElementById("userInput").getAttribute("data-username");

    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });

    // پاک کردن محتوای ورودی پیام
    document.getElementById("messageInput").value = "";
}


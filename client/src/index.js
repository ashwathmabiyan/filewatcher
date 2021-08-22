"use strict";
exports.__esModule = true;
require("./css/index.css");
require("./css/main.css");
var signalR = require("@microsoft/signalr");
var divMessages = document.querySelector("#upperblock");
var textInput = document.querySelector("#btnText");
var btnAppend = document.querySelector("#btnAppend");
var btnCreate = document.querySelector("#btnCreate");
var btnRename = document.querySelector("#btnRename");
var btnDelete = document.querySelector("#btnDelete");
var connection = new signalR.HubConnectionBuilder()
    .withUrl("http://127.0.0.1:5500/fileHub").withAutomaticReconnect()
    .build();
connection.on("messageReceived", function (message) {
    var m = document.createElement("div");
    m.innerHTML =
        "<div class=\"message-author\"><pre>FileName:" + message.FileName + " | DateTime:" + message.FileChangeDateTime + " |EventName: " + message.EventType + "</pre></div>";
    divMessages.appendChild(m);
    divMessages.scrollTop = divMessages.scrollHeight;
});
connection.on("Error", function (Error) {
    var m = document.createElement("div");
    m.innerHTML =
        "<div class=\"message-author\"><pre style=\"color: red;\">Error:" + Error + " </pre></div>";
    divMessages.appendChild(m);
    divMessages.scrollTop = divMessages.scrollHeight;
});
connection.start()["catch"](function (err) { return document.write(err); });
btnAppend.addEventListener("click", append);
btnCreate.addEventListener("click", create);
btnRename.addEventListener("click", rename);
btnDelete.addEventListener("click", deletet);
function append() {
    if (textInput.value == "") {
        alert("Please provide text to append.");
        return;
    }
    connection.send("ChangeFile", textInput.value).then(function () { return textInput.value = ""; });
}
function create() {
    if (textInput.value == "") {
        alert("Please provide text to create.");
        return;
    }
    connection.send("CreateFile", textInput.value).then(function () { return textInput.value = ""; });
}
function rename() {
    if (textInput.value == "") {
        alert("Please provide text to rename.");
        return;
    }
    connection.send("RenameFile", textInput.value).then(function () { return textInput.value = ""; });
}
function deletet() {
    connection.send("DeleteFile");
}

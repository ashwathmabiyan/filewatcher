import "./css/index.css";
import "./css/main.css";
import * as signalR from "@microsoft/signalr";

const divMessages: HTMLDivElement = document.querySelector("#upperblock");

const textInput: HTMLInputElement = document.querySelector("#btnText");
const btnAppend: HTMLButtonElement = document.querySelector("#btnAppend");
const btnCreate: HTMLButtonElement = document.querySelector("#btnCreate");
const btnRename: HTMLButtonElement = document.querySelector("#btnRename");
const btnDelete: HTMLButtonElement = document.querySelector("#btnDelete");

const connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5500/fileHub").withAutomaticReconnect()
    .build();

connection.on("messageReceived", (message) => {
    let m = document.createElement("div");

    m.innerHTML =
        `<div class="message-author"><pre>FileName:${message.FileName} | DateTime:${message.FileChangeDateTime} |EventName: ${message.EventType}</pre></div>`;

    divMessages.appendChild(m);
    divMessages.scrollTop = divMessages.scrollHeight;
});

connection.on("Error", (Error:string) => {

    let m = document.createElement("div");

    m.innerHTML =
        `<div class="message-author"><pre style="color: red;">Error:${Error} </pre></div>`;

    divMessages.appendChild(m);
    divMessages.scrollTop = divMessages.scrollHeight;
});

connection.start().catch(err => document.write(err));


btnAppend.addEventListener("click", append);
btnCreate.addEventListener("click", create);
btnRename.addEventListener("click", rename);
btnDelete.addEventListener("click", deletet);


function append() {
    if (textInput.value == "") {
        alert("Please provide text to append.")
        return;
    }
    connection.send("ChangeFile", textInput.value).then(() => textInput.value = "");
}

function create() {
    if (textInput.value == "") {
        alert("Please provide text to create.")
        return;
    }
    connection.send("CreateFile", textInput.value).then(() => textInput.value = "");
}

function rename() {
    if (textInput.value == "") {
        alert("Please provide text to rename.")
        return;
    }
    connection.send("RenameFile", textInput.value).then(() => textInput.value = "");
}

function deletet(){  
    connection.send("DeleteFile");
}

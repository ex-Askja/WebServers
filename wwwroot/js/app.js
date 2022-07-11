const APP_ROOT = "#app";
const API_ROOT = "/Api";
const API_METHOD = "POST";

function create(selector) {
    return document.createElement(selector);
}

function get(selector) {
    let object = (typeof selector === "string" ? document.querySelector(selector) : selector);

    return (object !== null ? object : create(selector));
}

function getAll(selector) {
    let object = (typeof selector === "string" ? document.querySelectorAll(selector) : selector);

    return (object !== null ? object : create(selector));
}

function request(
    uri,
    method,
    data,
    callback = (res) => console.log(res),
    error = (err) => console.log(err)
) {
    let xhr = new XMLHttpRequest(true, true);

    xhr.open(method, uri, true);

    xhr.onreadystatechange = function (state) {
        if (this.status === 200) {
            callback(this.responseText);
        } else {
            error(this.responseText);
        }
    };

    xhr.send(data);
}

function apiRequest(method, data, callback) {
    try {
        var formData = new FormData();

        for (var key in data) {
            formData.append(key, data[key]);
        }

        request(API_ROOT + "/" + method, API_METHOD, formData, function (data) {
            callback(data);
        }, function (data) {
            console.error("%cAPI Error", "padding: 2px;background-color:blue;color:white;", data);
        });
    } catch (ex) {
        console.error("%Parse Error", "padding: 2px;background-color:blue;color:white;", "Cannot parse a response text");
    }
}

document.addEventListener("DOMContentLoaded", function () {
    getAll(".add").onclick = function () {
        this.setAttribute("disabled", true);
        let element = this;

        apiRequest("Add", null, function (response) {
            element.removeAttribute("disabled");

            location.reload();
        });
    };

    getAll(".mark").onclick = function () {
        this.setAttribute("disabled", true);
        let element = this;

        apiRequest("MarkForRemove", {
            Id: this.parentElement.dataset["vid"]
        }, function () {
            element.removeAttribute("disabled");

            location.reload();
        });
    };

    getAll(".remove").onclick = function () {
        this.setAttribute("disabled", true);
        let element = this;

        apiRequest("Remove", {
            Id: this.parentElement.dataset["vid"]
        }, function () {
            element.removeAttribute("disabled");

            location.reload();
        });
    };
});
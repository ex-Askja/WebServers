const API_ROOT = "/Api";
const API_METHOD = "POST";

function create(selector) {
    return document.createElement(selector);
}

function get(selector) {
    return (typeof selector === "string" ? document.querySelector(selector) : selector);
}

function getAll(selector) {
    let object = (typeof selector === "string" ? document.querySelectorAll(selector) : selector);

    return (object !== null ? object : create(selector));
}

function setText(selector, text) {
    let element = get(selector);

    if (element !== null) {
        element.innerText = text;
    }
}

function setHtml(selector, html) {
    let element = get(selector);

    if (element !== null) {
        element.innerHTML = html;
    }
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

    xhr.onload = function () {
        if (xhr.status === 200) {
            callback(xhr.responseText);
        } else {
            error(xhr.responseText);
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

function bindActions() {
    getAll(".mark").forEach((btn) => {
        btn.onclick = function () {
            this.setAttribute("disabled", true);
            let element = this;

            apiRequest("MarkForRemove", {
                Id: this.parentElement.dataset["vid"]
            }, function () {
                element.removeAttribute("disabled");

                loadServers();
            });
        };
    });

    getAll(".remove").forEach((btn) => {
        btn.onclick = function () {
            this.setAttribute("disabled", true);
            let element = this;

            apiRequest("Remove", {
                Id: this.parentElement.dataset["vid"]
            }, function () {
                element.removeAttribute("disabled");

                loadServers();
            });
        };
    });
}

function loadServers() {
    let table = get(".servers");

    if (table !== null) {
        setHtml(table, "");
        setHtml(".updated", "Servers loading...");

        apiRequest("Load", null, function (response) {
            if (response === "") {
                return false;
            }

            if (typeof response === "string") {
                response = JSON.parse(response);
            }

            let data = "";

            response.servers.forEach(function (server) {
                let buttons = `<div class="d-flex" data-vid="${server.virtualServerId}">`,
                    mark = "", remove = "", status = "", removed = "";

                if (!server.selectedForRemove && !server.removed) {
                    mark = '<a style="color: #ac2bac;" href="#!" role="button" class="mark"><i class="fas fa-xmark"></i></a>';
                }

                if (server.selectedForRemove && !server.removed) {
                    remove = '<a style="color: #dd4b39;" href="#!" role="button" class="remove"><i class="fas fa-trash-can"></i></a>';
                }

                buttons += mark + remove + "</div>";

                if (server.selectedForRemove) {
                    status = '<i class="fas fa-hourglass-clock text-primary"></i>';
                }

                if (server.removed) {
                    removed = server.removeDateTime;
                }

                data += `<tr><td>${server.virtualServerId}</td><td class="text-center">${server.createDateTime}</td><td class="text-center">${removed}</td><td class="text-center">${status}</td><td class="text-center">${buttons}</td></tr>`;
            });

            let now = new Date();
            let current =
                now.getDate() + "/" + now.getMonth() + "/" + now.getFullYear()
                + " " + now.getHours() + ":" + now.getMinutes() + ":" + now.getSeconds();

            setHtml(".currentTime", response.currentDateTime);
            setHtml(".usageTime", response.totalUsageTime);
            setHtml(".updated", "Updated: " + current);

            setHtml(table, data);

            bindActions();
        });
    }
}

document.addEventListener("DOMContentLoaded", function () {
    get(".add").onclick = function () {
        this.setAttribute("disabled", true);
        let element = this;

        apiRequest("Add", null, function (response) {
            element.removeAttribute("disabled");

            loadServers();
        });
    };

    loadServers();
});
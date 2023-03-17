/// <reference path="oidc-client.js" />

function log() {
    document.getElementById('results').innerText = '';

    Array.prototype.forEach.call(arguments, function (msg) {
        if (msg instanceof Error) {
            msg = "Error: " + msg.message;
        }
        else if (typeof msg !== 'string') {
            msg = JSON.stringify(msg, null, 2);
        }
        document.getElementById('results').innerText += msg + '\r\n';
    });
}

document.getElementById("login").addEventListener("click", login, false);
document.getElementById("api").addEventListener("click", api, false);
document.getElementById("logout").addEventListener("click", logout, false);

var config = {
    authority: "https://localhost:5001",
    client_id: "js",
    redirect_uri: "https://localhost:5013/callback.html",
    response_type: "code",
    scope:"openid profile api1",
    post_logout_redirect_uri: "https://localhost:5013/index.html",
    //automaticSilentRenew: true,
    silent_redirect_uri: 'https://localhost:5013/identityserver-silent.html',
    loadUserInfo: false,
    //prompt: 'login',
    //max_age: 60
};
var mgr = new Oidc.UserManager(config);
Oidc.Log.logger = console;
Oidc.Log.level = Oidc.Log.DEBUG;

mgr.events.addUserSignedOut(logout);

mgr.getUser().then(function (user) {
    if (user) {
        log("User logged in", user.profile);
    }
    else {
        log("User not logged in");
    }
});

function login() {
    mgr.signinRedirect();
}

function api() {
    mgr.getUser().then(function (user) {
        var url = "https://localhost:6001/identity";

        var xhr = new XMLHttpRequest();
        xhr.open("GET", url);
        xhr.onload = function () {
            log(xhr.status, JSON.parse(xhr.responseText));
        }
        xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
        xhr.send();
    });
}

function logout() {
    mgr.clearStaleState();
    mgr.removeUser()
    mgr.signoutRedirect();
}

mgr.events.addSilentRenewError(e => {
    console.log("silent renew error", e.message);
});

//mgr.events.addAccessTokenExpired(() => {
//    console.log("token expired");
//    this.signinSilent();
//});

signinSilent = () => {
    mgr.signinSilent()
        .then(user => {
            console.log("signed in", user);
        })
        .catch(err => {
            console.log(err);
        });
};
//signinSilentCallback = () => {
//    console.log("Token renew method");
//    mgr.signinSilentCallback();
//};
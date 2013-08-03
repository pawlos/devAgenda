var providers_large = {
    google: {
        name: "Google",
        url: "https://www.google.com/accounts/o8/id",
        x: -1, 
        y: -1,
        type: 1
    },
    yahoo: {
        name: "Yahoo",
        url: "http://yahoo.com/",
        x: -1,
        y: -63,
        type: 1
    },
    myopenid: {
        name: "MyOpenID",
        url: "http://myopenid.com/",
        x: -1,
        y: -125,
        type: 1
    },
    facebook: {
        name: "Facebook",
        /*oauth_version: "2.0",
        oauth_server: "https://graph.facebook.com/oauth/authorize",*/
        x: -1,
        y: -187,
        type: 2
    },
    twitter: {
        name: "Twitter",
        x: -1,
        y: -249,
        type: 3
    }
};

var providers =
    $.extend({}, providers_large);

var openid = {
    img_path: null,
    input_id: null,

    init: function (n, g) {
        openid.img_path = g;
        openid.input_id = n;

        $("body").css("cursor", "default");

        var openIdButtons = $("#openid_btns");

        for (id in providers_large) {
            openIdButtons.append(this.getBoxHTML(providers_large[id], "large"));
        }

        openIdButtons.append("<br>");

        $("#show-more-options").appendTo(openIdButtons);
    },

    signin: function (providerName) {
        var provider = providers[providerName];

        if (!provider) {
            return;
        }

        if (provider.type == 1) {
            this.setOpenIdUrl(provider.url);
        }

        $("#authType").val(provider.type);

        /*if (providerName == "facebook") {
        this.facebookLogin(provider, providerName);
        return;
        }

        $("." + providerName).css("cursor", "wait");
        this.setOpenIdUrl(provider.url);*/
        
        $("#openid_form").submit();
    },

//    facebookLogin: function (provider, providerName) {
//        $("." + providerName).css("cursor", "wait");

//        this.setOAuthInfo(provider.oauth_version, provider.oauth_server);

//        $("#openid_form").submit();
//    },

    getBoxHTML: function (provider, buttonSize) {
        var buttonStyle = "background: #fff url(" + this.img_path + "); background-position: " + provider.x + "px " + provider.y + "px";
        var providerName = provider.name.toLowerCase();
        var buttonClass = providerName + " openid_" + buttonSize + "_btn";

        return '<a title="log in with ' + provider.name.replace("_", " ") + '" href="javascript:openid.signin(\'' + providerName + '\');" style="' + buttonStyle + '" class="' + buttonClass + '"></a>';
    },

    setOAuthInfo: function (oAuthVersion, oAuthServer) {
        $("#oauth_version").val(oAuthVersion);
        $("#oauth_server").val(oAuthServer);
        $("#" + this.input_id).val("");
    },

    setOpenIdUrl: function (openIdUrl) {
        $("#" + this.input_id).val(openIdUrl);
        /*$("#oauth_version").val("");
        $("#oauth_server").val("");*/
    }
};
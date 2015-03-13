var fbAppId = 161236587286870;
var facebookPermissionScopes = [];

facebookPermissionScopes = facebookPermissionScopes.join(",");
console.log("facebookPermissionScopes: %s", facebookPermissionScopes);

$(document).ready(function () {

    $(document).on('gotFbToken', function (e, fb) {
        var fbId = fb.uid;
        var fbToken = fb.token;

        var jsonData = JSON.stringify({ fbToken: fbToken });
        var successCallback = function (result) {
            console.log(result);
            $.Dialog({
                shadow: true,
                overlay: false,
                icon: '<span class="icon-checkmark"></span>',
                title: 'Get Token',
                width: 500,
                padding: 10,
                content: 'Token Valid !!'
            });
            $('#token .card').addClass('flipped');
            setTimeout(function () {
                $('#token .card').removeClass('flipped');
            }, 3500);
        };

        var errorCallback = function (result) {
            console.log(result);
            alert("error FbToken");
        };

        $.ajax({
            url: "/fb/token",
            data: jsonData,
            type: 'POST',
            success: successCallback,
            error: errorCallback,
            async: false
        });
    });

    $('#publish').click(function () {
        var successCallback = function (result) {
            $.Dialog({
                shadow: true,
                overlay: false,
                icon: '<span class="icon-broadcast"></span>',
                title: 'Publish',
                width: 500,
                padding: 10,
                content: 'Published Already !!'
            });
            $('#publish .card').addClass('flipped');
            setTimeout(function () {
                $('#publish .card').removeClass('flipped');
            }, 3500);
        };

        var errorCallback = function (result) {
            console.log(result);
            alert("error publish");
        };

        $.ajax({
            url: "/fb/publishmessage",
            type: 'GET',
            success: successCallback,
            error: errorCallback,
            async: false
        });
    });

    $('#summary').click(function () {
        var successCallback = function (result) {
            console.log('result success : %o', result);

            var viewModel = new ViewModel(result);
            console.log('viewModel : %o', viewModel);

            ko.applyBindings(viewModel);

            $('#summary').unbind('click');

            var nums = [];
            for (var i = 0; i < result.TotalWords; i++) {
                nums.push(i);
            }

            setInterval(function () {
                var num = nums[Math.floor(Math.random() * nums.length)];
                $('.word:eq(' + num + ') .card').addClass('flipped');
                setTimeout(function () {
                    $('.word:eq(' + num + ') .card').removeClass('flipped');
                }, 2000);
            }, 100);
        };

        var errorCallback = function (result) {
            console.log(result);
            alert("error summary");
        };

        $.ajax({
            url: "/fb/summary",
            type: 'GET',
            dataType: 'json',
            contentType: "application/json; charset=UTF-8",
            success: successCallback,
            error: errorCallback,
            async: false
        });
    });

    $(document).on('fbLoaded', function () {
        console.log('fbLoaded called');

        FB.getLoginStatus(function (response) {
            if (response.status === 'connected') {

                console.log('User connected and authorize');
                var uid = response.authResponse.userID;
                var accessToken = response.authResponse.accessToken;

                $(document).trigger('gotFbToken', [{ uid: uid, token: accessToken }]);

            } else if (response.status === 'not_authorized') {
                // the user is logged in to Facebook, but has not authenticated your app
                console.log('User connected but not authorize');
                try {
                    FB.logout(function (response) {
                        // user is now logged out
                        console.log('User log out');
                        FB.login(logInFacebookCallback, {
                            scope: facebookPermissionScopes,
                            return_scopes: true
                        });
                    });
                } catch (err) {
                    console.log(err);
                }


            } else {
                // the user isn't logged in to Facebook.
                console.log('User log in first time');
                try {
                    FB.login(logInFacebookCallback, {
                        scope: facebookPermissionScopes,
                        return_scopes: true
                    });
                } catch (err) {
                    console.log(err);
                }
            }
        });

        function logInFacebookCallback(response) {
            if (!response.authResponse) {
                console.log(response.authResponse);
                return;
            }
            var accessToken = response.authResponse.accessToken;
            var uid = response.authResponse.userID;
            $(document).trigger('gotFbToken', [{ uid: uid, token: accessToken }]);
        }

    });
});

function ViewModel(data) {
    var self = this;
    self.words = ko.observableArray(data.Words);
}

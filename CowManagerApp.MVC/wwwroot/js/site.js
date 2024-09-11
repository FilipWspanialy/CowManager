window.addEventListener('beforeunload', function (e) {
    var userIsLoggedIn = document.body.getAttribute('data-user-logged-in') === 'true';

    if (userIsLoggedIn) {
        navigator.sendBeacon('/account/logout');
    }
});
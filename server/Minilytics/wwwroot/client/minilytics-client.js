(function () {
    function generateRandomUserId() {
        return Math.random() * Number.MAX_VALUE; 
    }
    function getUserId() {
        if (localStorage) {
            var userId = localStorage.getItem("userid");
            if (!userId) {
                userId = generateRandomUserId();
                localStorage.setItem("userid", userId);
            }
            return userId;
        }
        return generateRandomUserId();
    }

    setTimeout(function() {
        var message = {
            sentDateTime: new Date(),
            eventName: "pageview",
            fullUrl: document.location.href,
            path: document.location.pathname,
            referrer: document.referrer,
            userId: getUserId()
        };

        var request = new XMLHttpRequest();
        request.open('POST', '/ana/api/events', true);
        request.write(JSON.stringify(message));
        request.send();
    }, 1000);
})();
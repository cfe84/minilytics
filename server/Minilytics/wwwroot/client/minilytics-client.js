(function () {
    var NOW = new Date();
    var USER_LOCALSTORAGE_KEY = "user";
    var VIEW_LOCALSTORAGE_KEY = "views-";
    var TEST_LOCALSTORAGE_KEY = "test";
    var EVENTS_ENDPOINT = '/ana/api/events';
    var EXCEPTION_ENDPOINT = '/ana/api/exceptions';
    var PAGEVIEW_EVENTTYPE = "pageview";
    var USERTYPES = {
        repeat: "repeat",
        new: "new",
        anonymous: "anonymous"
    }

    function generateRandomUserId() {
        return Math.random() * Number.MAX_SAFE_INTEGER; 
    }

    function createUser(userType) {
        return {
            userId: generateRandomUserId(),
            totalPages: 1,
            firstVisitDate: NOW,
            userType: userType
        };
    }

    function getUser() {
        var user;
        if (localStorage) {
            var serializedUser = localStorage.getItem(USER_LOCALSTORAGE_KEY);
            if (!serializedUser) {
                user = createUser(USERTYPES.new);
            } else {
                user = JSON.parse(serializedUser);
                user.userType = USERTYPES.repeat;
                user.totalPages++;
            }
            localStorage.setItem(USER_LOCALSTORAGE_KEY, JSON.stringify(user));
        } else {
            user = createUser(USERTYPES.anonymous);
        }
        return user;
    }

    function newViewStats() {
        return {
            firstViewDate: NOW,
            viewCount: 1
        };
    }

    function getPageViewStats() {
        if (localStorage) {
            var key = VIEW_LOCALSTORAGE_KEY + document.location.pathname.replace(/[^a-zA-Z0-9]/g, "");
            var serializedView = localStorage.getItem(key);
            if (!serializedView) {
                view = newViewStats();
            } else {
                view = JSON.parse(serializedView);
                view.viewCount++;
            }
            localStorage.setItem(key, JSON.stringify(view));
            return view;
        } else {
            return newViewStats();
        }
    }

    function isStorageEnabled() {
        try {
            if (!localStorage) return "localStorage doesn't exist";
            localStorage.setItem(TEST_LOCALSTORAGE_KEY, "1234");
            var res = localStorage.getItem(TEST_LOCALSTORAGE_KEY) === "1234";
            localStorage.removeItem(TEST_LOCALSTORAGE_KEY);
            return res ? "localStorage works" : "localStorage exposed but doesn't work";
        }
        catch (exception) {
            return "Exception: " + exception.message;
        }
    }

    function getBrowserInfo() {
        return navigator ?
            navigator.appName + "-"
            + navigator.appCodeName + "-"
            + navigator.appVersion + "-"
            + navigator.buildID + "-"
            + navigator.platform
            : "No navigator info";
    }

    setTimeout(function () {
        try {
            var user = getUser();
            var view = getPageViewStats();
            var NO_NAVIGATOR_MSG = "No navigator variable"
            var message = {
                sentDateTime: NOW,
                eventName: PAGEVIEW_EVENTTYPE,
                fullUrl: document.location.href,
                path: document.location.pathname,
                referrer: document.referrer,
                userId: user.userId,
                userType: user.userType,
                userFirstVisitDate: user.firstVisitDate,
                userTotalVisits: user.totalPages,
                pageFirstViewDate: view.firstViewDate,
                pageViewCount: view.viewCount,
                browserAppCodeName: navigator ? navigator.appCodeName : NO_NAVIGATOR_MSG,
                browserPlatform: navigator ? navigator.platform : NO_NAVIGATOR_MSG,
                browserVersion: navigator ? navigator.appVersion : NO_NAVIGATOR_MSG,
                browserBuild: navigator ? navigator.buildID : NO_NAVIGATOR_MSG,
                browserLanguage: navigator ? navigator.language : NO_NAVIGATOR_MSG
            };

            var request = new XMLHttpRequest();
            request.open('POST', EVENTS_ENDPOINT, true);
            request.setRequestHeader("content-type", "application/json");
            request.send(JSON.stringify(message));
        } catch (exception) {
            var request = new XMLHttpRequest();
            request.open('POST', EXCEPTION_ENDPOINT, true);
            request.setRequestHeader("content-type", "application/json");
            var message = {
                error: exception.message,
                browser: getBrowserInfo(),
                localStorageStatus: isStorageEnabled()
            };
            request.send(JSON.stringify(message));
        }
    }, 1000);
})();
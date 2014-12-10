///<reference path="~/Scripts/jasmine/jasmine.js"/>
///<reference path="~/Scripts/angular.js"/>
///<reference path="~/Scripts/angular-route.js"/>
///<reference path="~/Scripts/angular-mocks.js"/>
///<reference path="~/Scripts/angular-local-storage.js"/>
///<reference path="~/Scripts/loading-bar.js"/>
///<reference path="~/Scripts/common.js"/>
///<reference path="~/Scripts/App/Services/authInterceptorService.js"/>
///<reference path="~/Scripts/App/Services/authService.js"/>
///<reference path="~/Scripts/App/app.js"/>
///<reference path="~/Scripts/App/aboutController.js"/>

describe("AboutControllerSpec", function () {

    beforeEach(module("timeTrackerApp"));

    it('should create an about view with title', inject(function ($controller) {
        var scope = {},
            ctrl = $controller('AboutController', { $scope: scope });
        expect(scope.title).toBe("About Time Tracker");
    }));

});
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
///<reference path="~/Scripts/App/homeController.js"/>

describe("homeControllerSpec", function () {

    beforeEach(module("timeTrackerApp"));

    it('should create a home view', inject(function ($controller) {
        var scope = {},
            ctrl = $controller('homeController', { $scope: scope });
        expect(scope.authentication).toBeDefined();
        expect(scope.authentication.isAuth).toBeFalsy();
    }));

});
///<reference path="~/Scripts/jasmine/jasmine.js"/>
///<reference path="~/Scripts/angular.js"/>
///<reference path="~/Scripts/angular-route.js"/>
///<reference path="~/Scripts/angular-mocks.js"/>
///<reference path="~/Scripts/angular-local-storage.js"/>
///<reference path="~/Scripts/loading-bar.js"/>
///<reference path="~/Scripts/common.js"/>
///<reference path="~/Scripts/App/Services/authInterceptorService.js"/>
///<reference path="~/Scripts/App/Services/authService.js"/>
///<reference path="~/Scripts/App/dataFactory.js"/>
///<reference path="~/Scripts/App/app.js"/>
///<reference path="~/Scripts/App/indexController.js"/>

'use strict';
describe("indexController Spec", function () {

    beforeEach(module("timeTrackerApp"));

    var scope;
    var location;

    beforeEach(inject(function ($controller, $rootScope, $location) {
        scope = $rootScope.$new();
        location = $location;
        $controller('indexController', { $scope: scope, $location : location });
    }));

    
    it('should create a index view with authentication', inject(function ($controller) {
        expect(scope.authentication);
    }));

    it('logout should set location to home', inject(function ($controller) {
        spyOn(location, 'path');
        scope.logOut();
        expect(location.path).toHaveBeenCalledWith('/');
    }));

});
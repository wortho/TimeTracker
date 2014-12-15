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
///<reference path="~/Scripts/App/homeController.js"/>

'use strict';
describe("Route Config Spec", function () {

    beforeEach(module("timeTrackerApp"));
    
    it('home route matches homeController',
    inject(function ($route) {

        expect($route.routes['/home'].controller).toBe('homeController');
        expect($route.routes['/home'].templateUrl).toEqual('/views/Home.html');

    }));

    it('login route matches loginController',
    inject(function ($route) {

        expect($route.routes['/login'].controller).toBe('loginController');
        expect($route.routes['/login'].templateUrl).toEqual('/views/Login.html');

    }));

    it('signup route matches loginController',
    inject(function ($route) {

        expect($route.routes['/signup'].controller).toBe('signupController');
        expect($route.routes['/signup'].templateUrl).toEqual('/views/Signup.html');

    }));

    it('about route matches aboutController',
    inject(function ($route) {

        expect($route.routes['/about'].controller).toBe('aboutController');
        expect($route.routes['/about'].templateUrl).toEqual('/views/About.html');

    }));

    it('customers route matches loginController',
    inject(function ($route) {

        expect($route.routes['/customers'].controller).toBe('customersController');
        expect($route.routes['/customers'].templateUrl).toEqual('/views/Customers/CustomerList.html');

    }));



});
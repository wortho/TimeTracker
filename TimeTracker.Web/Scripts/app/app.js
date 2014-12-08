var timeTrackerApp = angular.module('timeTrackerApp', ['ngRoute', 'LocalStorageModule', 'angular-loading-bar']);
timeTrackerApp.config([
    '$routeProvider', function ($routeProvider) {
        var viewBase = '/views/';
        $routeProvider
            .when('/home', {
                controller: 'homeController',
                templateUrl: viewBase + 'Home.html'
            })
            .when('/login', {
                controller: 'loginController',
                templateUrl: viewBase + 'Login.html'
            })
            .when('/signup', {
                controller: 'signupController',
                templateUrl: viewBase + 'Signup.html'
            })
            .when('/customers', {
                controller: 'customersController',
                templateUrl: viewBase + 'Customers/CustomerList.html'
            })
            .when('/about', {
                controller: 'aboutController',
                templateUrl: viewBase + 'About.html'
            })
            .when('/customer/:customerId?', {
                controller: 'customerController',
                templateUrl: viewBase + 'Customers/CustomerEdit.html',
            })
            .when('/projects/customer/:customerId?', {
                controller: 'projectsController',
                templateUrl: viewBase + 'Projects/ProjectList.html',
            })
            .when('/project/:projectId?', {
                controller: 'projectController',
                templateUrl: viewBase + 'Projects/ProjectEdit.html',
            })
            .when('/timeEntries/project/:projectId?', {
                controller: 'timeEntriesController',
                templateUrl: viewBase + 'TimeEntries/TimeEntryList.html',
            })
            .when('/timeEntry/:timeEntryId', {
                controller: 'timeEntryController',
                templateUrl: viewBase + 'TimeEntries/TimeEntryEdit.html',
            })
            .when('/timeEntry/new/:projectId', {
                controller: 'timeEntryController',
                templateUrl: viewBase + 'TimeEntries/TimeEntryEdit.html',
            })
            .otherwise({ redirectTo: '/home' });
    }
]);

timeTrackerApp.constant('ngAuthSettings', { clientId: 'ngAuthApp' });

timeTrackerApp.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

timeTrackerApp.config(function (localStorageServiceProvider) {
    localStorageServiceProvider.setPrefix('yourAppName');
});

timeTrackerApp.run(['authService', function (authService) {
    authService.fillAuthData();
}]);



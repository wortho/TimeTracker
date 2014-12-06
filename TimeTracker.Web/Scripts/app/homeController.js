'use strict';
timeTrackerApp.controller('homeController', ['$scope', '$location', 'authService', function ($scope, $location, authService) {

    $scope.authentication = authService.authentication;

}]);
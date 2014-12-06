'use strict';
timeTrackerApp.controller('customersController', ['$scope', 'dataFactory',
    function ($scope, dataFactory) {

        $scope.customers = [];
        $scope.orderby = 'Id';
        $scope.reverse = false;
        $scope.status = 'starting';
        $scope.title = 'Customers';
        getCustomers();

        function getCustomers() {
            $scope.status = 'loading';
            dataFactory.getCustomers()
                .success(function (custs) {
                    $scope.customers = custs;
                    $scope.status = '';
                })
                .error(function (error) {
                    $scope.status = 'Unable to load customer data: ' + error.message;
                });
        }
    }
]);

timeTrackerApp.controller('customerController', [
    '$scope', '$routeParams', 'dataFactory',
    function ($scope, $routeParams, dataFactory) {
        var customerId = ($routeParams.customerId) ? parseInt($routeParams.customerId) : 0;

        $scope.customer = {};
        $scope.states = [];
        $scope.title = (customerId > 0) ? 'Edit' : 'Add';
        $scope.buttonText = (customerId > 0) ? 'Update' : 'Add';
        $scope.updateStatus = false;
        $scope.errorMessage = '';

        $scope.isStateSelected = function (customerStateId, stateId) {
            return customerStateId === stateId;
        };

        getCustomer(customerId);

        function getCustomer(id) {
            dataFactory.getCustomer(id)
                .success(function (cust) {
                    $scope.customer = cust;
                })
                .error(function (error) {
                    $scope.status = 'Unable to load data: ' + error.message;
                });
        }

        $scope.updateCustomer = function () {
            var cust = $scope.customer;
            dataFactory.updateCustomer(cust).then(processSuccess(), processError);
        };

        function processSuccess() {
            $scope.editForm.$dirty = false;
            $scope.updateStatus = true;
            $scope.title = 'Edit';
            $scope.buttonText = 'Update';
            startTimer();
        }

        function processError(error) {
            $scope.errorMessage = error.message;
            startTimer();
        }

        function startTimer() {
            var timer = $timeout(function () {
                $timeout.cancel(timer);
                $scope.errorMessage = '';
                $scope.updateStatus = false;
            }, 3000);
        }
    }
]);

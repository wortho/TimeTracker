'use strict';
timeTrackerApp.controller('timeEntriesController', [
    '$scope', '$routeParams', 'dataFactory',
    function ($scope, $routeParams, dataFactory) {
        var projectId = ($routeParams.projectId) ? parseInt($routeParams.projectId) : 0;

        $scope.timeEntries = [];
        $scope.orderby = 'StartDate';
        $scope.reverse = false;
        $scope.status = 'starting';
        $scope.title = 'Time Entries';
        if (projectId > 0) {
            $scope.projectId = projectId;
            $scope.title = 'Project ' + projectId + ' Time Entries';
        }

        getTimeEntries(projectId);

        function getTimeEntries(id) {
            $scope.status = 'loading';
            dataFactory.getTimeEntries(id)
                .success(function (timeEntries) {
                    $scope.timeEntries = timeEntries;
                    $scope.status = '';
                })
                .error(function (error) {
                    $scope.status = 'Unable to load Time Entries data: ' + error.message;
                });
        }
    }
]);

timeTrackerApp.controller('timeEntryController', ['$scope', '$location', '$routeParams', '$timeout', 'dataFactory',
    function ($scope, $location, $routeParams, $timeout, dataFactory) {
        var timeEntryId = ($routeParams.timeEntryId) ? parseInt($routeParams.timeEntryId) : 0;

        $scope.timeEntry = {};
        var projectId = ($routeParams.projectId) ? parseInt($routeParams.projectId) : 0;
        if (projectId > 0) {
            $scope.timeEntry.ProjectId = projectId;
        }
        $scope.states = [];
        $scope.title = (timeEntryId > 0) ? 'Edit' : 'Add';
        $scope.buttonText = (timeEntryId > 0) ? 'Update' : 'Add';
        $scope.updateStatus = false;
        $scope.errorMessage = '';

        $scope.isStateSelected = function (timeEntryStateId, stateId) {
            return timeEntryStateId === stateId;
        };

        if (timeEntryId > 0) {
            getTimeEntry(timeEntryId);
        }

        function getTimeEntry(id) {
            dataFactory.getTimeEntry(id)
                .success(function (entry) {
                    $scope.timeEntry = entry;
                })
                .error(function (error) {
                    $scope.status = 'Unable to project load data: ' + error.message;
                });
        }

        $scope.updateTimeEntry = function () {
            if ($scope.editForm.$valid) {
                if (!$scope.timeEntry.Id) {
                    dataFactory.insertTimeEntry($scope.timeEntry).then(processSuccess, processError);
                }
                else {
                    dataFactory.updateTimeEntry($scope.timeEntry).then(processSuccess, processError);
                }
            }
        };

        function processSuccess(data) {
            $scope.editForm.$dirty = false;
            $scope.updateStatus = true;
            if (!$scope.timeEntry.Id) {
                $location.path('/timeEntry/' + data.Id);
            }
            else {
                $scope.title = 'Edit';
                $scope.buttonText = 'Update';
                startTimer();
            }
        }

        function processError(error) {
            $scope.errorMessage = 'An error occurred: ' + error.message;
            startTimer();
        }

        function startTimer() {
            var timer = $timeout(function () {
                $timeout.cancel(timer);
                $scope.errorMessage = '';
                $scope.updateStatus = false;
            }, 3000);
        }

    }]);

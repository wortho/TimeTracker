'use strict';
timeTrackerApp.controller('projectsController',
    ['$scope', '$route', '$routeParams', 'dataFactory',
    function ($scope, $route, $routeParams, dataFactory) {
            var customerId = ($routeParams.customerId) ? parseInt($routeParams.customerId) : 0;

        $scope.projects = [];
        $scope.orderby = 'CustomerId';
        $scope.reverse = false;
        $scope.status = 'starting';
        $scope.title = 'Projects';
        if (customerId > 0) {
            $scope.title = 'Customer ' + customerId + ' Projects';
        }

        getProjects();

        function getProjects() {
            $scope.status = 'loading';
            dataFactory.getProjects(customerId)
                .success(function(projects) {
                    $scope.projects = projects;
                    $scope.status = '';
                })
                .error(function(error) {
                    $scope.status = 'Unable to load projects data: ' + error.message;
                });
        }
    }
]);

timeTrackerApp.controller('projectController',
    ['$scope', '$routeParams', '$timeout', 'dataFactory',
    function ($scope, $routeParams, $timeout, dataFactory){
        var projectId = ($routeParams.projectId) ? parseInt($routeParams.projectId) : 0;

        $scope.project = {};
        $scope.states = [];
        $scope.title = (projectId > 0) ? 'Edit' : 'Add';
        $scope.buttonText = (projectId > 0) ? 'Update' : 'Add';
        $scope.updateStatus = false;
        $scope.errorMessage = '';

        $scope.isStateSelected = function(projectStateId, stateId) {
            return projectStateId === stateId;
        };

        getProject(projectId);

        function getProject(id) {
            dataFactory.getProject(id)
                .success(function(proj) {
                    $scope.project = proj;
                })
                .error(function(error) {
                    $scope.status = 'Unable to project load data: ' + error.message;
                });
        }

        $scope.updateProject = function() {
            var project = $scope.project;
            dataFactory.updateProject(project).then(processSuccess, processError);
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
            var timer = $timeout(function() {
                $timeout.cancel(timer);
                $scope.errorMessage = '';
                $scope.updateStatus = false;
            }, 3000);
        }

    }
]);
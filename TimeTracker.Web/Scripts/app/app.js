angular.module('timeTrackerApp', ['ngRoute'])
    .config(['$routeProvider', function ($routeProvider) {

        $routeProvider
            .when('/', {
                controller: 'customersController',
                templateUrl: '/Views/Customers/CustomerList.html'
            })
            .when('/customer/:customerId', {
                controller: 'customerController',
                templateUrl: '/Views/Customers/CustomerEdit.html',
            })
            .when('/projects/:customerId?', {
                controller: 'projectsController',
                templateUrl: '/Views/Projects/ProjectList.html',
            })
            .when('/project/:projectId', {
                controller: 'projectController',
                templateUrl: '/Views/Projects/ProjectEdit.html',
            })
            .when('/timeEntries/:projectId?', {
                controller: 'timeEntriesController',
                templateUrl: '/Views/TimeEntries/TimeEntryList.html',
            })
            .when('/timeEntry/:timeEntryId', {
                controller: 'timeEntryController',
                templateUrl: '/Views/TimeEntries/TimeEntryEdit.html',
            })
            .otherwise({ redirectTo: '/' });
    }])

     .factory('dataFactory', ['$http', function ($http) {

         var urlBase = '/api/';
         var customers = 'customers';
         var projects = 'projects';
         var timeEntries = 'timeEntries';

         var dataFactory = {};

         dataFactory.getCustomers = function () {
             return $http.get(urlBase + customers);
         };

         dataFactory.getCustomer = function (id) {
             return $http.get(urlBase + customers + '/' + id);
         };

         dataFactory.updateCustomer = function (cust) {
             return $http.put(urlBase + customers + '/' + cust.Id, cust);
         };

         dataFactory.getProjects = function (customerId) {
             var urlProjects = urlBase + projects;
             if (customerId > 0) {
                 urlProjects = urlBase + customers + '/' + customerId + '/' + projects;
             }
             return $http.get(urlProjects);
         };

         dataFactory.getProject = function (id) {
             return $http.get(urlBase + projects + '/' + id);
         };

         dataFactory.updateProject = function (project) {
             return $http.put(urlBase + projects + '/' + project.Id, project);
         };


         dataFactory.getTimeEntries = function (projectId) {
             var urlProjects = urlBase + timeEntries;
             if (projectId > 0) {
                 urlProjects = urlBase + projects + '/' + projectId + '/' + timeEntries;
             }
             return $http.get(urlProjects);
         };

         dataFactory.getTimeEntry = function (id) {
             return $http.get(urlBase + timeEntries + '/' + id);
         };

         dataFactory.updateTimeEntry = function (entry) {
             return $http.put(urlBase + timeEntries + '/' + entry.Id, entry);
         };

         return dataFactory;

     }])

    .controller('customersController', ['$scope', 'dataFactory',
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
        }])

    .controller('customerController', ['$scope', '$routeParams', 'dataFactory',
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
        }])

.controller('projectsController', ['$scope', '$routeParams', '$filter', 'dataFactory',
        function ($scope, $routeParams, $filter, dataFactory) {
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
                    .success(function (projects) {
                        $scope.projects = projects;
                        $scope.status = '';
                    })
                    .error(function (error) {
                        $scope.status = 'Unable to load projects data: ' + error.message;
                    });
            }
        }])

    .controller('projectController', ['$scope', 'dataFactory', '$routeParams',
        function ($scope, dataFactory, $routeParams) {
            var projectId = ($routeParams.projectId) ? parseInt($routeParams.projectId) : 0;

            $scope.project = {};
            $scope.states = [];
            $scope.title = (projectId > 0) ? 'Edit' : 'Add';
            $scope.buttonText = (projectId > 0) ? 'Update' : 'Add';
            $scope.updateStatus = false;
            $scope.errorMessage = '';

            $scope.isStateSelected = function (projectStateId, stateId) {
                return projectStateId === stateId;
            };

            getProject(projectId);

            function getProject(id) {
                dataFactory.getProject(id)
                    .success(function (proj) {
                        $scope.project = proj;
                    })
                    .error(function (error) {
                        $scope.status = 'Unable to project load data: ' + error.message;
                    });
            }

            $scope.updateProject = function () {
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
                var timer = $timeout(function () {
                    $timeout.cancel(timer);
                    $scope.errorMessage = '';
                    $scope.updateStatus = false;
                }, 3000);
            }

        }])

    .controller('timeEntriesController', ['$scope', '$routeParams', '$filter', 'dataFactory',
        function ($scope, $routeParams, $filter, dataFactory) {
            var projectId = ($routeParams.projectId) ? parseInt($routeParams.projectId) : 0;

            $scope.timeEntries = [];
            $scope.orderby = 'StartDate';
            $scope.reverse = false;
            $scope.status = 'starting';
            $scope.title = "Time Entries";
            if (projectId > 0) {
                $scope.title = "Project " + projectId + " Time Entries";
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
        }])

    .controller('timeEntryController', ['$scope', 'dataFactory', '$routeParams',
        function ($scope, dataFactory, $routeParams) {
            var timeEntryId = ($routeParams.timeEntryId) ? parseInt($routeParams.timeEntryId) : 0;

            $scope.timeEntry = {};
            $scope.states = [];
            $scope.title = (timeEntryId > 0) ? 'Edit' : 'Add';
            $scope.buttonText = (timeEntryId > 0) ? 'Update' : 'Add';
            $scope.updateStatus = false;
            $scope.errorMessage = '';

            $scope.isStateSelected = function (timeEntryStateId, stateId) {
                return timeEntryStateId === stateId;
            };

            getTimeEntry(timeEntryId);

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
                var entry = $scope.timeEntry;
                dataFactory.updateTimeEntry(entry).then(processSuccess, processError);
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

        }]);


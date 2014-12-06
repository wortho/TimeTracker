'use strict';
timeTrackerApp.factory('dataFactory', ['$http', function ($http) {

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
    }]);

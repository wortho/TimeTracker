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
///<reference path="~/Scripts/App/dataFactory.js"/>
///<reference path="~/Scripts/App/timeEntriesController.js"/>

describe("Time Entries Controller Spec", function () {
    var $httpBackend, scope, ctrl, authRequestHandler;

    beforeEach(module("timeTrackerApp"));

    beforeEach(inject(function (_$httpBackend_, $controller, $rootScope) {
        $httpBackend = _$httpBackend_;
        $httpBackend.expectGET('/api/timeEntries')
            .respond([{ "Id": 1, "ProjectId": 1, "Project": null, "UserId": "09d47e09-2c5e-4440-8582-a40b9da081c2", "User": null, "Description": "Entry1", "StartTime": "2014-12-08T12:00:00", "EndTime": "2014-12-08T17:00:00" }, { "Id": 2, "ProjectId": 3, "Project": null, "UserId": "6e37c971-09cb-4d39-904d-3fac94805a9a", "User": null, "Description": "Pølsevogn", "StartTime": "2014-12-08T08:00:00", "EndTime": "2014-12-08T11:00:00" }]);
        scope = $rootScope.$new();
        ctrl = $controller('timeEntriesController', { $scope: scope });
    }));

    afterEach(function () {
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });

    it('should have title Time Entries ', inject(function () {
        expect(scope.title).toBe('Time Entries');
        $httpBackend.flush();
        expect(scope.status).toBe('');

    }));

    it('should get a time entries list', inject(function () {
        expect(scope.timeEntries).toEqual([]);
        $httpBackend.flush();
        expect(scope.timeEntries.length).toBe(2);
        expect(scope.status).toBe('');

    }));

});
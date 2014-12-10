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
///<reference path="~/Scripts/App/customersController.js"/>

describe("Customer Controller Spec", function () {
    var $httpBackend, scope, ctrl, authRequestHandler;

    beforeEach(module("timeTrackerApp"));

    beforeEach(inject(function (_$httpBackend_, $controller, $rootScope) {
        $httpBackend = _$httpBackend_;
        $httpBackend.expectGET('/api/customers').respond([{Id:1,CompanyName:"SmithJohn",ContactFirstName:"John",ContactLastName:"Smith",ContactEmail:"John.Smith@gmail.com",Address:"1 Main St.",City:"London",Postcode:1000,Projects:null}]);
        scope = $rootScope.$new();
        ctrl = $controller('customersController', { $scope: scope });
    }));

    afterEach(function () {
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });

    it('should have title customers', inject(function () {
        expect(scope.title).toBe('Customers');
        $httpBackend.flush();
        expect(scope.status).toBe('');

    }));

    it('should get a customers list', inject(function () {
        expect(scope.customers).toEqual([]);
        $httpBackend.flush();
        expect(scope.customers.length).toBe(1);
        expect(scope.status).toBe('');

    }));

    it('should return one customer called John Smith', inject(function () {
        expect(scope.customers).toEqual([]);
        $httpBackend.flush();
        expect(scope.customers.length).toBe(1);
        expect(scope.customers[0].ContactFirstName).toBe("John");
        expect(scope.customers[0].ContactLastName).toBe("Smith");
        expect(scope.status).toBe('');
    }));


});
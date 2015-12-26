var app = angular.module('AngularAuthApp', ['ngRoute', 'LocalStorageModule', 'angular-loading-bar']);

var serviceBase = 'http://localhost:24860/';
//http://localhost:24860/
app.config(function ($routeProvider) {

    $routeProvider.when("/home", {
        controller: "homeController",
        templateUrl: "/ClientJS/views/home.html"
    });

    $routeProvider.when("/login", {
        controller: "loginController",
        templateUrl: "/ClientJS/views/login.html"
    });

    $routeProvider.when("/signup", {
        controller: "signupController",
        templateUrl: "/ClientJS/views/signup.html"
    });
    $routeProvider.when("/associate", {
        controller: "associateController",
        templateUrl: "/ClientJS/views/associate.html"
    });

    $routeProvider.when("/tasteIdentification", {
        controller: "tasteIdentificationController",
        templateUrl: "/ClientJS/views/tasteIdentification.html"
    });

 
    $routeProvider.otherwise({ redirectTo: "/home" });
});

app.constant('ngAuthSettings', {
    apiServiceBaseUri: serviceBase,
    clientId: 'wisher'
});

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);

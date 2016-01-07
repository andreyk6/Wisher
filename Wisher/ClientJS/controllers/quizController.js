'use strict';
app.controller('quizController', ['$scope', '$http', 'authService', function ($scope, $http, authService) {
    $scope.authentication = authService.authentication;

    $scope.title = "loading question...";
    $scope.options = [{ title: "Хочу", value: true }, { title: "Не хочу", value: false }];

    $scope.progress = 0;
    $scope.categoryId = -1;

    $scope.answered = false;
    $scope.working = false;

    $scope.nextQuestion = function() {
        $scope.submitAnswer($scope.options[1]);
    };

    $scope.submitAnswerTrue = function () {
        $scope.submitAnswer($scope.options[0]);
    };
    $scope.submitAnswerFalse = function () {
        $scope.submitAnswer($scope.options[1]);
    };

    $scope.submitAnswer = function(option) {
        $scope.working = true;
        $scope.answered = true;
        $scope.title = "loading question...";

        var data = JSON.stringify({
            userId: $scope.authentication.userName,
            categoryId: $scope.categoryId,
            isLiked: option.value
        });
        $http.post("api/wish/dowish/", data).success(function(data, status) {
            $scope.categoryId = data.categoryId;
            $scope.progress = data.progress;
            $scope.title = data.name;
            $scope.answered = false;
            $scope.working = false;
        }).error(function(data, status, headers, config) {
            $scope.title = "Oops... something went wrong";
            $scope.working = false;
        });
    };
}]);
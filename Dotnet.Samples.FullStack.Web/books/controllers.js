angular.module('booksApp.controllers', []).controller('RetrieveAllController', function($scope, $state, popupService, $window, Book) {
    $scope.books = Book.query();
});
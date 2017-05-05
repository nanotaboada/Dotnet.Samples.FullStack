angular.module('booksApp', ['ui.router', 'ngResource', 'booksApp.controllers', 'booksApp.services']);

angular.module('booksApp').config(function($stateProvider) {
    $stateProvider.state('books', {
        url: '/books',
        templateUrl: 'books/books.html',
        controller: 'RetrieveAllController'
    });
}).run(function($state) {
    $state.go('books');
});
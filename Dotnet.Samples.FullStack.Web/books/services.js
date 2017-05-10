angular.module('booksApp.services', []).factory('Book', function($resource) {
    return $resource('http://localhost:56033/api/v1/books/:id', { id: '@isbn' }, { update: { method: 'PUT' } });
});

angular.module('booksApp.services').service('popupService', ['$window', function($window) {
    this.showPopup = function(message) {
        return $window.confirm(message);
    }
}]);
(function (angular) {
    'use strict';
    /* jshint latedef:nofunc */

    angular.module('dashboardModule').factory('dashboardFactory', dashboardFactory);
    dashboardFactory.$inject = ['$http', 'baseUrl'];

    function dashboardFactory($http, baseUrl) {
        return {
            getDashboard,
            getToDoList,
            getAnnouncement
        };

        function getDashboard() {
            return $http.get(baseUrl + 'Dashboard/GetDashboard');
        }

        function getToDoList() {
            return $http.get(baseUrl + 'Dashboard/GetToDoList');
        }

        function getAnnouncement() {
            return $http.get(baseUrl + 'Dashboard/GetAnnouncement');
        }
    }
})(angular);
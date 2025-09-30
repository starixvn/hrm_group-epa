(function (angular) {
    'use strict';

    angular.module('jobLevelModule').factory('jobLevelFactory', jobLevelFactory);
    jobLevelFactory.$inject = ['$http', 'baseUrl'];

    function jobLevelFactory($http, baseUrl) {
        return {
            getUserPermission: getUserPermission,
            createJobLevel: createJobLevel,
            editJobLevel: editJobLevel,
            deleteJobLevel: deleteJobLevel
        };

        function getUserPermission() {
            return $http.get(baseUrl + 'JobLevel/GetUserPermission');
        }

        function createJobLevel(jobLevel) {
            return $http.post(baseUrl + 'JobLevel/Create', jobLevel);
        }

        function editJobLevel(jobLevel) {
            return $http.post(baseUrl + 'JobLevel/Edit', jobLevel);
        }

        function deleteJobLevel(jobLevelId) {
            return $http['delete'](baseUrl + 'JobLevel/Delete', { params: { jobLevelId: jobLevelId } });
        }
    }
})(angular);
(function (angular) {
    'use strict';
    /* jshint latedef:nofunc */

    angular.module('jobLevelStandardModule').factory('JobLevelStandardKPIFactory', kpiFactory);
    kpiFactory.$inject = ['$http', 'baseUrl'];

    function kpiFactory($http, baseUrl) {
        return {
            createKPI: createKPI,
            deleteKPI: deleteKPI,
            getJobLevelStandKPI: getJobLevelStandKPI,
            getUserPermissionOnKPI: getUserPermissionOnKPI,
            updateKPI: updateKPI,
            updateStatusKPI: updateStatusKPI
        };

        function createKPI(kpi) {
            return $http.post(baseUrl + 'JobLevelStandardKPI/Create', kpi);
        }

        function deleteKPI(kpiId) {
            return $http['delete'](baseUrl + 'JobLevelStandardKPI/Delete', { params: { kpiId: kpiId } });
        }

        function getJobLevelStandKPI(isActive) {
            return $http.get(baseUrl + 'JobLevelStandardKPI/GetKPI', {
                params: {
                    isActive: isActive
                }
            });
        }

        function getUserPermissionOnKPI() {
            return $http.get(baseUrl + 'JobLevelStandardKPI/GetUserPermission');
        }

        function updateKPI(kpi) {
            return $http.put(baseUrl + 'JobLevelStandardKPI/Update', kpi);
        }

        function updateStatusKPI(kpi) {
            return $http.put(baseUrl + 'JobLevelStandardKPI/UpdateStatus', kpi);
        }
    }
})(angular);
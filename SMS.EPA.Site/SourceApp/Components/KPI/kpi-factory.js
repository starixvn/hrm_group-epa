(function (angular) {
    'use strict';
    /* jshint latedef:nofunc */

    angular.module('kpiModule').factory('kpiFactory', kpiFactory);
    kpiFactory.$inject = ['$http', 'baseUrl'];

    function kpiFactory($http, baseUrl) {
        return {
            createKPI: createKPI,
            deleteKPI: deleteKPI,
            getKPI: getKPI,
            getUserPermissionOnKPI: getUserPermissionOnKPI,
            updateKPI: updateKPI,
            updateRequiredEvaluateKPI: updateRequiredEvaluateKPI,
            updateStatusKPI: updateStatusKPI
        };

        function createKPI(kpi) {
            return $http.post(baseUrl + 'KPI/CreateKPI', kpi);
        }

        function deleteKPI(kpiId) {
            return $http['delete'](baseUrl + 'KPI/DeleteKPI', { params: { kpiId: kpiId } });
        }

        function getKPI(kpiGroupId, isActive, isRequiredEvaluate) {
            return $http.get(baseUrl + 'KPI/GetKPI', {
                params: {
                    kpiGroupId: kpiGroupId,
                    isActive: isActive,
                    isRequiredEvaluate: isRequiredEvaluate
                }
            });
        }

        function getUserPermissionOnKPI() {
            return $http.get(baseUrl + 'KPI/GetUserPermissionOnKPI');
        }

        function updateKPI(kpi) {
            return $http.put(baseUrl + 'KPI/UpdateKPI', kpi);
        }

        function updateRequiredEvaluateKPI(kpi) {
            return $http.put(baseUrl + 'KPI/UpdateRequiredEvaluateKPI', kpi);
        }

        function updateStatusKPI(kpi) {
            return $http.put(baseUrl + 'KPI/UpdateStatusKPI', kpi);
        }
    }
})(angular);
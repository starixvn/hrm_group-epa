(function (angular) {
    'use strict';
    /* jshint latedef:nofunc */

    angular.module('kpiGroupModule').factory('kpiGroupFactory', kpiGroupFactory);
    kpiGroupFactory.$inject = ['$http', 'baseUrl'];

    function kpiGroupFactory($http, baseUrl) {
        return {
            createKPIGroup: createKPIGroup,
            deleteKPIGroup: deleteKPIGroup,
            getKPIGroup: getKPIGroup,
            getKPIGroupById: getKPIGroupById,
            getUserPermissionOnKPIGroup: getUserPermissionOnKPIGroup,
            updateKPIGroup: updateKPIGroup,
            updateStatusKPIGroup: updateStatusKPIGroup
        };

        function createKPIGroup(kpiGroup) {
            return $http.post(baseUrl + 'KPIGroup/CreateKPIGroup', kpiGroup);
        }

        function deleteKPIGroup(kpiGroupId) {
            return $http['delete'](baseUrl + 'KPIGroup/DeleteKPIGroup', { params: { kpiGroupId: kpiGroupId } });
        }

        function getKPIGroup(isActive) {
            return $http.get(baseUrl + 'KPIGroup/GetKPIGroup', { params: { isActive: isActive } });
        }

        function getKPIGroupById(kpiGroupId) {
            return $http.get(baseUrl + 'KPIGroup/GetKPIGroupById', { params: { kpiGroupId: kpiGroupId } });
        }

        function getUserPermissionOnKPIGroup() {
            return $http.get(baseUrl + 'KPIGroup/GetUserPermissionOnKPIGroup');
        }

        function updateKPIGroup(kpiGroup) {
            return $http.put(baseUrl + 'KPIGroup/UpdateKPIGroup', kpiGroup);
        }

        function updateStatusKPIGroup(kpiGroup) {
            return $http.put(baseUrl + 'KPIGroup/UpdateStatusKPIGroup', kpiGroup);
        }
    }
})(angular);
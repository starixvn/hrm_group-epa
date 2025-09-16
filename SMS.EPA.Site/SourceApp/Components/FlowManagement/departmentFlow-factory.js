(function (angular) {
    'use strict';
    /* jshint latedef:nofunc */

    angular.module('flowMngModule').factory('deptFlowFactory', deptFlowFactory);
    deptFlowFactory.$inject = ['$http', 'baseUrl'];

    function deptFlowFactory($http, baseUrl) {
        return {
            createDepartmentFlow: function (model) {
                return $http.post(baseUrl + 'GroupFlow/CreateDepartmentFlow', model);
            },
            deleteDepartmentFlow: function (departmentId, flowTemplateId) {
                return $http['delete'](baseUrl + 'GroupFlow/DeleteDepartmentFlow', {
                    params: {
                        departmentId: departmentId,
                        flowTemplateId: flowTemplateId
                    }
                });
            },
            getDepartmentFlow: function (flowTemplateId) {
                return $http.get(baseUrl + 'GroupFlow/GetByFlowTemplateId', {
                    params: {
                        flowTemplateId: flowTemplateId
                    }
                });
            },
            getDepartmentFlowById: function (groupId, flowTemplateId) {
                return $http.get(baseUrl + 'GroupFlow/GetById', {
                    params: {
                        groupId: groupId,
                        flowTemplateId: flowTemplateId
                    }
                });
            },
            getDepartmentHasSetting: function (flowTemplateId) {
                return $http.get(baseUrl + 'GroupFlow/GetDepartmentHasSetting', { params: { flowTemplateId: flowTemplateId } });
            }
        };
    }
})(angular);
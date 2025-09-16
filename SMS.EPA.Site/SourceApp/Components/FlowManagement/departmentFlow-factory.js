(function (angular) {
    'use strict';
    /* jshint latedef:nofunc */

    angular.module('flowMngModule').factory('deptFlowFactory', deptFlowFactory);
    deptFlowFactory.$inject = ['$http', 'baseUrl'];

    function deptFlowFactory($http, baseUrl) {
        return {
            createDepartmentFlow: function (model) {
                return $http.post(baseUrl + 'DepartmentFlow/CreateDepartmentFlow', model);
            },
            deleteDepartmentFlow: function (departmentId, flowTemplateId) {
                return $http['delete'](baseUrl + 'DepartmentFlow/DeleteDepartmentFlow', {
                    params: {
                        departmentId: departmentId,
                        flowTemplateId: flowTemplateId
                    }
                });
            },
            getDepartmentFlow: function (flowTemplateId) {
                return $http.get(baseUrl + 'DepartmentFlow/GetByFlowTemplateId', {
                    params: {
                        flowTemplateId: flowTemplateId
                    }
                });
            },
            getDepartmentFlowById: function (groupId, flowTemplateId) {
                return $http.get(baseUrl + 'DepartmentFlow/GetById', {
                    params: {
                        groupId: groupId,
                        flowTemplateId: flowTemplateId
                    }
                });
            },
            getDepartmentHasSetting: function (flowTemplateId) {
                return $http.get(baseUrl + 'DepartmentFlow/GetDepartmentHasSetting', { params: { flowTemplateId: flowTemplateId } });
            }
        };
    }
})(angular);
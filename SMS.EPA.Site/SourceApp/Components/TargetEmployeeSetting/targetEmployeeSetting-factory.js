(function (angular) {
    'use strict';
    /* jshint latedef:nofunc */

    angular.module('targetEmployeeSettingModule').factory('targetEmployeeSettingFactory', targetEmployeeSettingFactory);
    targetEmployeeSettingFactory.$inject = ['$http', 'baseUrl'];

    function targetEmployeeSettingFactory($http, baseUrl) {
        return {
            assignTargetTemplateToDepartment,
            assignTargetTemplateToEmployee,
            deleteTargetDepartmentSetting,
            deleteTargetEmployeeSetting,
            getTargetDepartmentSetting,
            getTargetEmployeeSetting,
            getUserPermissionOnTargetEmployeeSetting
        };

        function assignTargetTemplateToDepartment(params) {
            return $http.put(baseUrl + 'TargetEmployeeSetting/AssignTargetTemplateToDepartment', params);
        }

        function assignTargetTemplateToEmployee(params) {
            return $http.put(baseUrl + 'TargetEmployeeSetting/AssignTargetTemplateToEmployee', params);
        }

        function getTargetDepartmentSetting(departmentIds, targetTypeId, showAll) {
            return $http.get(baseUrl + 'TargetEmployeeSetting/GetTargetDepartmentSetting', {
                params: { departmentIds, targetTypeId, showAll }
            });
        }

        function getTargetEmployeeSetting(teamIds, employeeId, targetTypeId, showAll) {
            return $http.get(baseUrl + 'TargetEmployeeSetting/GetTargetEmployeeSetting', {
                params: { teamIds, employeeId, targetTypeId, showAll }
            });
        }

        function deleteTargetDepartmentSetting(targetTemplateId, departmentId, isGroup) {
            return $http['delete'](baseUrl + 'TargetEmployeeSetting/DeleteTargetDepartmentSetting', {
                params: { targetTemplateId, departmentId, isGroup }
            });
        }

        function deleteTargetEmployeeSetting(targetTemplateId, employeeId) {
            return $http['delete'](baseUrl + 'TargetEmployeeSetting/DeleteTargetEmployeeSetting', {
                params: { employeeId, targetTemplateId }
            });
        }

        function getUserPermissionOnTargetEmployeeSetting() {
            return $http.get(baseUrl + 'TargetEmployeeSetting/GetUserPermissionOnTargetEmployeeSetting');
        }
    }
})(angular);
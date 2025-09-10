(function (angular) {
    'use strict';
    /* jshint latedef:nofunc */

    angular.module('commonModule').factory('commonFactory', commonFactory);
    commonFactory.$inject = ['$http', 'baseUrl'];

    function commonFactory($http, baseUrl) {
        return {
            searchEmployee: function (query, teamIds, isSpecialUser, isDisabledUser, getByApprover, includeVerifier) {
                return $http.get(baseUrl + 'Common/SearchEmployee', {
                    params: { query, teamIds, isSpecialUser, isDisabledUser, getByApprover, includeVerifier }
                });
            },
            getTargetType: function (isActive, excludeTargetTypeIds) {
                return $http.get(baseUrl + 'Common/GetTargetType', {
                    params: {
                        isActive: isActive,
                        excludeTargetTypeIds: excludeTargetTypeIds
                    }
                });
            },
            getSystemRoles: function (isActive) {
                return $http.get(baseUrl + 'Common/GetSystemRoles', { params: { isActive } });
            },
            getKPI: function (kpiGroupId, isActive, isRequiredEvaluate) {
                return $http.get(baseUrl + 'Common/GetKPI', {
                    params: {
                        kpiGroupId: kpiGroupId,
                        isActive: isActive,
                        isRequiredEvaluate: isRequiredEvaluate
                    }
                });
            },
            getKPIGroup: function (isActive) {
                return $http.get(baseUrl + 'Common/GetKPIGroup', { params: { isActive: isActive } });
            },
            getEmployeeById: function (employeeId) {
                return $http.get(baseUrl + 'Common/GetEmployeeById', { params: { employeeId: employeeId } });
            },
            getAllGroup: function () {
                return $http.get(baseUrl + 'Common/GetAllGroup');
            },
            getAllStatus: function () {
                return $http.get(baseUrl + 'Common/GetAllStatus');
            },
            getPeriod: function (targetTypeId, isActive) {
                return $http.get(baseUrl + 'Common/GetPeriod', {
                    params: {
                        targetTypeId: targetTypeId,
                        isActive: isActive
                    }
                });
            },
            getStandardGrade: function () {
                return $http.get(baseUrl + 'Common/GetStandardGrade');
            },
            getAllPeriodIncludeImport: function () {
                return $http.get(baseUrl + 'Common/GetAllPeriodIncludeImport');
            },
            getTargetTemplate: function (targetTypeId, isActive) {
                return $http.get(baseUrl + 'Common/GetAllTargetTemplate', {
                    params: {
                        targetTypeId: targetTypeId,
                        isActive: isActive
                    }
                });
            },
            getAllMonthlyPeriod: function () {
                return $http.get(baseUrl + 'MonthlyPeriod/GetAllMonthlyPeriod');
            },
            getMenusByEmployeeId: function () {
                return $http.get(baseUrl + 'Common/GetMenusByEmployeeId');
            },
            getAllJobTitle: function () {
                return $http.get(baseUrl + 'Common/GetAllJobTitle');
            },
            getUserPermissionOnFlowManagement: function () {
                return $http.get(baseUrl + 'Common/GetUserPermissionOnFlowManagement');
            },
            getJobLevel: function (isDeleted) {
                return $http.get(baseUrl + 'Common/GetJobLevel', { params: { isDeleted: isDeleted } });
            },
            getOrgChart: function () {
                return $http.get(baseUrl + 'Common/GetOrgChart');
            },
            getOrgChartJLSTemplateSetting: function () {
                return $http.get(baseUrl + 'Common/GetOrgChartJLSTemplateSetting');
            }
        };
    }
})(angular);
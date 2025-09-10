(function (angular) {
    'use strict';
    /* jshint latedef:nofunc */

    angular.module('monthlyPeriodModule').factory('monthlyPeriodFactory', monthlyPeriodFactory);
    monthlyPeriodFactory.$inject = ['$http', 'baseUrl'];

    function monthlyPeriodFactory($http, baseUrl) {
        return {
            getMonthlyPeriod: function (fromDate, toDate) {
                return $http.get(baseUrl + 'MonthlyPeriod/GetMonthlyPeriod', {
                    params: {
                        fromDate: fromDate,
                        toDate: toDate
                    }
                });
            },
            getMonthlyPeriodById: function (periodId) {
                return $http.get(baseUrl + 'MonthlyPeriod/GetMonthlyPeriodById', { params: { periodId: periodId } });
            },
            createMonthlyPeriod: function (period) {
                return $http.post(baseUrl + 'MonthlyPeriod/CreateMonthlyPeriod', period);
            },
            updateBasicInfoMonthlyPeriod: function (period) {
                return $http.put(baseUrl + 'MonthlyPeriod/UpdateBasicInfoMonthlyPeriod', period);
            },
            getProcessSettingByPeriodId: function (periodId) {
                return $http.get(baseUrl + 'MonthlyPeriod/GetProcessSettingByPeriodId', { params: { periodId: periodId } });
            },
            updateProcessSettingMonthlyPeriod: function (processSetting) {
                return $http.put(baseUrl + 'MonthlyPeriod/UpdateProcessSettingMonthlyPeriod', processSetting);
            },
            deleteMonthlyPeriod: function (periodId) {
                return $http['delete'](baseUrl + 'MonthlyPeriod/DeleteMonthlyPeriod', { params: { periodId: periodId } });
            },
            getEmployeeSettingById: function (periodId, teamIds, isLocal, isExcludedEmployee, isGeneratedEPA, isEvaluated, epaStatus) {
                return $http.get(baseUrl + 'MonthlyPeriod/GetEmployeeSettingMonthlyPeriodById', {
                    params: { periodId, teamIds, isLocal, isExcludedEmployee, isGeneratedEPA, isEvaluated, epaStatus }
                });
            },
            addEmployeeToMonthlyPeriod: function (employeeInfo) {
                return $http.post(baseUrl + 'MonthlyPeriod/AddEmployeeToMonthlyPeriod', employeeInfo);
            },
            updateEmployeeSettingOfPeriod: function (employeeSetting) {
                return $http.put(baseUrl + 'MonthlyPeriod/UpdateEmployeeSettingOfPeriod', employeeSetting);
            },
            generateEvaluation: function (period) {
                return $http.post(baseUrl + 'MonthlyPeriod/GenerateEvaluation', period);
            },
            updateIncludeStatusEmployeePeriod: function (employeeStatusSetting) {
                return $http.put(baseUrl + 'MonthlyPeriod/UpdateIncludeStatusEmployeePeriod', employeeStatusSetting);
            },
            checkSelfProcessStarted: function (periodId, employeeIds) {
                return $http.get(baseUrl + 'MonthlyPeriod/CheckSelfProcessStarted', {
                    params: {
                        periodId: periodId,
                        employeeIds: employeeIds
                    }
                });
            },
            getPermissionOnMonthlyPeriod: function () {
                return $http.get(baseUrl + 'MonthlyPeriod/GetUserPermissionOnMonthlyPeriod');
            }
        };
    }
})(angular);
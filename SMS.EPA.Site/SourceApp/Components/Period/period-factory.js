(function (angular) {
    'use strict';
    /* jshint latedef:nofunc */

    angular.module('periodModule').factory('periodFactory', periodFactory);
    periodFactory.$inject = ['$http', 'baseUrl'];

    function periodFactory($http, baseUrl) {
        return {
            checkSelfProcessStarted,
            createPeriod,
            deletePeriod,
            generateEvaluation,
            getPeriod,
            getPeriodById,
            getPeriodEmployeeSettingById,
            getPermissionOnPeriod,
            getProcessSettingByPeriodId,
            updateBasicInformationPeriod,
            addEmployeeSetting,
            updateEmployeeSettingOfPeriod,
            updateIncludeStatusEmployeePeriod,
            updatePeriodProcessSetting,
            updateStatusPeriod
        };

        function addEmployeeSetting(params) {
            return $http.post(baseUrl + 'Period/AddEmployeToPeriod', params);
        }

        function checkSelfProcessStarted(periodId, employeeIds) {
            return $http.get(baseUrl + 'Period/CheckSelfProcessStarted', {
                params: { periodId, employeeIds }
            });
        }

        function createPeriod(period) {
            return $http.post(baseUrl + 'Period/CreatePeriod', period);
        }

        function deletePeriod(periodId) {
            return $http['delete'](baseUrl + 'Period/DeletePeriod', { params: { periodId: periodId } });
        }

        function generateEvaluation(period) {
            return $http.post(baseUrl + 'Period/GenerateEvaluation', period);
        }

        function getPeriod(targetTypeId, isActive) {
            return $http.get(baseUrl + 'Period/GetPeriod', { params: { targetTypeId: targetTypeId, isActive: isActive } });
        }

        function getPeriodById(periodId) {
            return $http.get(baseUrl + 'Period/GetPeriodById', { params: { periodId: periodId } });
        }

        function getPeriodEmployeeSettingById(periodId, teamIds, isLocal, isExcludedEmployee, isGeneratedEPA, isEvaluated, hasMonthlyEPA, epaStatus) {
            return $http.get(baseUrl + 'Period/GetPeriodEmployeeSettingById', {
                params: { periodId, teamIds, isLocal, isExcludedEmployee, isGeneratedEPA, isEvaluated, hasMonthlyEPA, epaStatus }
            });
        }

        function getPermissionOnPeriod() {
            return $http.get(baseUrl + 'Period/GetUserPermissionOnPeriod');
        }

        function getProcessSettingByPeriodId(periodId) {
            return $http.get(baseUrl + 'Period/GetProcessSettingByPeriodId', { params: { periodId: periodId } });
        }

        function updateBasicInformationPeriod(period) {
            return $http.put(baseUrl + 'Period/UpdateBasicInformationPeriod', period);
        }

        function updateEmployeeSettingOfPeriod(employeeSetting) {
            return $http.put(baseUrl + 'Period/UpdateEmployeeSettingOfPeriod', employeeSetting);
        }

        function updateIncludeStatusEmployeePeriod(employeeStatusSetting) {
            return $http.put(baseUrl + 'Period/UpdateIncludeStatusEmployeePeriod', employeeStatusSetting);
        }

        function updatePeriodProcessSetting(periodProcessSetting) {
            return $http.put(baseUrl + 'Period/UpdatePeriodProcessSetting', periodProcessSetting);
        }

        function updateStatusPeriod(period) {
            return $http.put(baseUrl + 'Period/UpdateStatusPeriod', period);
        }
    }
})(angular);
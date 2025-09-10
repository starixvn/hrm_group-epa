(function (angular) {
    'use strict';
    /* jshint latedef:nofunc */

    angular.module('jobLevelStandardModule').factory('jobLevelStandardPeriodFactory', periodFactory);
    periodFactory.$inject = ['$http', 'baseUrl'];

    function periodFactory($http, baseUrl) {
        return {
            getPermission,
            getPeriods,
            getPeriodById,
            createPeriod,
            editPeriod,
            editStatusPeriod,
            deletePeriod,
            getEmployeeSettings,
            addEmployeeSetting,
            updateJobLevel,
            editEmployeeSetting,
            editStatusEmployeeSetting,
            editAssessmentStatus,
            getJobLevelSettings,
            getJobLevelSettingBySectionId,
            editJobLevelSetting,
            copyDefaultJobLevelSetting
        };

        function getPermission() {
            return $http.get(baseUrl + 'JobLevelStandardPeriod/GetPermission');
        }

        function getPeriods(isActive) {
            return $http.post(baseUrl + 'JobLevelStandardPeriod/GetPeriods', { isActive });
        }

        function getPeriodById(periodId) {
            return $http.get(baseUrl + 'JobLevelStandardPeriod/GetById', { params: { periodId } });
        }

        function createPeriod(period) {
            return $http.post(baseUrl + 'JobLevelStandardPeriod/Create', period);
        }

        function editPeriod(period) {
            return $http.put(baseUrl + 'JobLevelStandardPeriod/Edit', period);
        }

        function editStatusPeriod(period) {
            return $http.put(baseUrl + 'JobLevelStandardPeriod/EditStatus', period);
        }

        function deletePeriod(periodId) {
            return $http['delete'](baseUrl + 'JobLevelStandardPeriod/Delete', { params: { periodId } });
        }

        function getEmployeeSettings(params) {
            return $http.post(baseUrl + 'JobLevelStandardPeriod/GetEmployeeSettings', params);
        }

        function addEmployeeSetting(params) {
            return $http.put(baseUrl + 'JobLevelStandardPeriod/AddEmployeeSetting', params);
        }

        function updateJobLevel(params) {
            return $http.put(baseUrl + 'JobLevelStandardPeriod/UpdateJobLevel', params);
        }

        function editEmployeeSetting(params) {
            return $http.put(baseUrl + 'JobLevelStandardPeriod/EditEmployeeSetting', params);
        }

        function editStatusEmployeeSetting(params) {
            return $http.put(baseUrl + 'JobLevelStandardPeriod/EditStatusEmployeeSetting', params);
        }

        function editAssessmentStatus(params) {
            return $http.put(baseUrl + 'JobLevelAssessment/EditStatus', params);
        }

        function getJobLevelSettings(params) {
            return $http.post(baseUrl + 'JobLevelStandardPeriod/GetJobLevelSettings', params);
        }

        function getJobLevelSettingBySectionId(params) {
            return $http.post(baseUrl + 'JobLevelStandardPeriod/GetJobLevelSettingBySectionId', params);
        }

        function editJobLevelSetting(params) {
            return $http.put(baseUrl + 'JobLevelStandardPeriod/EditJobLevelSetting', params);
        }

        function copyDefaultJobLevelSetting(params) {
            return $http.post(baseUrl + 'JobLevelStandardPeriod/CopyDefaultJobLevelSetting', params);
        }
    }
})(angular);
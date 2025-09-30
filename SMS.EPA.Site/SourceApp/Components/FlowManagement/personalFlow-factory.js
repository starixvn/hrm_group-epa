(function (angular) {
    'use strict';
    /* jshint latedef:nofunc */

    angular.module('flowMngModule').factory('personalFlowFactory', personalFlowFactory);
    personalFlowFactory.$inject = ['$http', 'baseUrl'];

    function personalFlowFactory($http, baseUrl) {
        return {
            getPersonalFlow: function (flowTemplateId) {
                return $http.get(baseUrl + 'PersonalFlow/GetByFlowTemplateId', { params: { flowTemplateId } });
            },
            getPersonalFlowById: function (employeeId, flowTemplateId) {
                return $http.get(baseUrl + 'PersonalFlow/GetById', { params: { employeeId, flowTemplateId } });
            },
            createPersonalFlow: function (model) {
                return $http.post(baseUrl + 'PersonalFlow/CreatePersonalFlow', model);
            },
            deletePersonalFlow: function (employeeId, flowTemplateId) {
                return $http['delete'](baseUrl + 'PersonalFlow/DeletePersonalFlow', { params: { employeeId, flowTemplateId } });
            },
            getEmployeeFlowSetting: function (flowTemplateId, teamIds, isMonthly) {
                return $http.get(baseUrl + 'PersonalFlow/GetEmployeeFlowSetting', { params: { flowTemplateId, teamIds, isMonthly } });
            }
        };
    }
})(angular);
(function (angular) {
    'use strict';
    /* jshint latedef:nofunc */

    angular.module('epaModule').factory('selfEvaluationFactory', selfEvaluationFactory);
    selfEvaluationFactory.$inject = ['$http', 'baseUrl'];

    function selfEvaluationFactory($http, baseUrl) {
        return {
            getEPAByEmployee: getEPAByEmployee,
            getEPAInfoByEmployee: getEPAInfoByEmployee,
            getUserPermissionOnEmployee: getUserPermissionOnEmployee,
            updateEPAScoreByEmployee: updateEPAScoreByEmployee
        };

        function getEPAByEmployee() {
            return $http.get(baseUrl + 'SelfEvaluation/GetEPAByEmployee');
        }

        function getEPAInfoByEmployee(epaId) {
            return $http.get(baseUrl + 'SelfEvaluation/GetEPAInfoByEmployee', { params: { epaId: epaId } });
        }

        function getUserPermissionOnEmployee() {
            return $http.get(baseUrl + 'SelfEvaluation/GetUserPermissionOnEmployee');
        }

        function updateEPAScoreByEmployee(criteria) {
            return $http.put(baseUrl + 'SelfEvaluation/UpdateEPAScoreByEmployee', criteria);
        }
    }
})(angular);
(function (angular) {
    'use strict';
    /* jshint latedef:nofunc */

    angular.module('epaModule').factory('updateFinalScoreFactory', updateFinalScoreFactory);
    updateFinalScoreFactory.$inject = ['$http', 'baseUrl'];

    function updateFinalScoreFactory($http, baseUrl) {
        return {
            getEmployeeToUpdateFinalScore,
            getEPAToUpdateFinalScore,
            getPeriodToUpdateFinalScore,
            updateFinalScore
        };

        function getEmployeeToUpdateFinalScore(periodId, teamIds, isLocal, query) {
            return $http.get(baseUrl + 'UpdateFinalScore/GetEmployeeToUpdateFinalScore', {
                params: { periodId, teamIds, isLocal, query }
            });
        }

        function getEPAToUpdateFinalScore(periodId, teamIds, isLocal, employeeId) {
            return $http.get(baseUrl + 'UpdateFinalScore/GetEPAToUpdateFinalScore', {
                params: { periodId, teamIds, isLocal, employeeId }
            });
        }

        function getPeriodToUpdateFinalScore() {
            return $http.get(baseUrl + 'UpdateFinalScore/GetPeriodToUpdateFinalScore');
        }

        function updateFinalScore(params) {
            return $http.put(baseUrl + 'UpdateFinalScore/UpdateFinalScore', params);
        }
    }
})(angular);
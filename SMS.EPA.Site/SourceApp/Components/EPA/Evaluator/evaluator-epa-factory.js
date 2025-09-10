(function (angular) {
    'use strict';
    /* jshint latedef:nofunc */

    angular.module('epaModule').factory('evaluatorEPAFactory', evaluatorEPAFactory);
    evaluatorEPAFactory.$inject = ['$http', 'baseUrl'];

    function evaluatorEPAFactory($http, baseUrl) {
        return {
            getEPAByEvaluator: getEPAByEvaluator,
            getEPAInfoByEvaluator: getEPAInfoByEvaluator,
            getPeriodByEvaluator: getPeriodByEvaluator,
            getGroupByEvaluator: getGroupByEvaluator,
            getMemberByEvaluator: getMemberByEvaluator,
            getUserPermissionOnEvaluator: getUserPermissionOnEvaluator,
            updateEPAScoreByEvaluator: updateEPAScoreByEvaluator
        };

        function getEPAByEvaluator(periodId, groupId, isLocal, isMonthly, employeeIds, statusProcess) {
            return $http.get(baseUrl + 'EvaluatorEPA/GetEPAByEvaluator', {
                params: {
                    periodId: periodId,
                    groupId: groupId,
                    isLocal: isLocal,
                    isMonthly: isMonthly,
                    employeeIds: employeeIds,
                    statusProcess: statusProcess
                }
            });
        }

        function getEPAInfoByEvaluator(epaId, evaluatorId) {
            return $http.get(baseUrl + 'EvaluatorEPA/GetEPAInfoByEvaluator', {
                params: {
                    epaId: epaId,
                    evaluatorId: evaluatorId
                }
            });
        }

        function getPeriodByEvaluator() {
            return $http.get(baseUrl + 'EvaluatorEPA/GetPeriodByEvaluator');
        }

        function getGroupByEvaluator(periodId) {
            return $http.get(baseUrl + 'EvaluatorEPA/GetGroupByEvaluator', { params: { periodId: periodId } });
        }

        function getMemberByEvaluator(periodId, groupId, isLocal, isMonthly) {
            return $http.get(baseUrl + 'EvaluatorEPA/GetMemberByEvaluator', {
                params: {
                    periodId: periodId,
                    groupId: groupId,
                    isLocal: isLocal,
                    isMonthly: isMonthly
                }
            });
        }

        function getUserPermissionOnEvaluator() {
            return $http.get(baseUrl + 'EvaluatorEPA/GetUserPermissionOnEvaluator');
        }

        function updateEPAScoreByEvaluator(criteria) {
            return $http.put(baseUrl + 'EvaluatorEPA/UpdateEPAScoreByEvaluator', criteria);
        }
    }
})(angular);
(function (angular) {
    'use strict';
    /* jshint latedef:nofunc */

    angular.module('targetTemplateModule').factory('targetTemplateFactory', targetTemplateFactory);
    targetTemplateFactory.$inject = ['$http', 'baseUrl'];

    function targetTemplateFactory($http, baseUrl) {
        return {
            addKPIGroupWeight: addKPIGroupWeight,
            createTargetTemplate: createTargetTemplate,
            deleteKPIGroupOfTargetTemplate: deleteKPIGroupOfTargetTemplate,
            deleteTargetTemplate: deleteTargetTemplate,
            getKPIGroupOfTargetTemplate: getKPIGroupOfTargetTemplate,
            getKPIOfTargetTemplate: getKPIOfTargetTemplate,
            getPermissonOnTargetTemplate: getPermissonOnTargetTemplate,
            getRatingType: getRatingType,
            getScoreRatingType: getScoreRatingType,
            getTargetTemplate: getTargetTemplate,
            getTargetTemplateById: getTargetTemplateById,
            updateTargetTemplate: updateTargetTemplate,
            updateTargetTemplateBasicInfo: updateTargetTemplateBasicInfo,
            updateStatusTargetTemplate: updateStatusTargetTemplate
        };

        function addKPIGroupWeight(kpiGroupWeightModel) {
            return $http.post(baseUrl + 'TargetTemplate/AddKPIGroupWeight', kpiGroupWeightModel);
        }

        function createTargetTemplate(targetTemplate) {
            return $http.post(baseUrl + 'TargetTemplate/CreateTargetTemplate', targetTemplate);
        }

        function deleteTargetTemplate(targetTemplateId) {
            return $http['delete'](baseUrl + 'TargetTemplate/DeleteTargetTemplate', { params: { targetTemplateId } });
        }

        function deleteKPIGroupOfTargetTemplate(targetTemplateId, kpiGroupId) {
            return $http['delete'](baseUrl + 'TargetTemplate/DeleteKPIGroupOfTargetTemplate',
                { params: { targetTemplateId: targetTemplateId, kpiGroupId: kpiGroupId } });
        }

        function getKPIGroupOfTargetTemplate(targetTemplateId) {
            return $http.get(baseUrl + 'TargetTemplate/GetKPIGroupOfTargetTemplate', { params: { targetTemplateId } });
        }

        function getKPIOfTargetTemplate(targetTemplateId, onlySelected) {
            return $http.get(baseUrl + 'TargetTemplate/GetKPIOfTargetTemplate',
                { params: { targetTemplateId: targetTemplateId, onlySelected: onlySelected } });
        }

        function getPermissonOnTargetTemplate() {
            return $http.get(baseUrl + 'TargetTemplate/GetUserPermissionOnTargetTemplate');
        }

        function getRatingType() {
            return $http.get(baseUrl + 'TargetTemplate/GetRatingType');
        }

        function getScoreRatingType(scoreRatingTypeId, isActive) {
            return $http.get(baseUrl + 'TargetTemplate/GetScoreRatingType',
                {
                    params: {
                        scoreRatingTypeId,
                        isActive
                    }
                });
        }

        function getTargetTemplate(targetTypeId, isActive, teamIds, scoreRatingTypeId) {
            return $http.get(baseUrl + 'TargetTemplate/GetAllTargetTemplate', {
                params: {
                    targetTypeId,
                    isActive,
                    teamIds,
                    scoreRatingTypeId
                }
            });
        }

        function getTargetTemplateById(targetTemplateId) {
            return $http.get(baseUrl + 'TargetTemplate/GetTargetTemplateById', { params: { targetTemplateId } });
        }

        function updateTargetTemplate(targetTemplate) {
            return $http.put(baseUrl + 'TargetTemplate/UpdateTargetTemplate', targetTemplate);
        }

        function updateTargetTemplateBasicInfo(targetTemplate) {
            return $http.put(baseUrl + 'TargetTemplate/UpdateTargetTemplateBasicInfo', targetTemplate);
        }

        function updateStatusTargetTemplate(targetTemplate) {
            return $http.put(baseUrl + 'TargetTemplate/UpdateStatusTargetTemplate', targetTemplate);
        }
    }
})(angular);
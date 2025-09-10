(function (angular) {
    'use strict';
    /* jshint latedef:nofunc */

    angular.module('jobLevelStandardModule').factory('jobLevelStandardTemplateFactory', templateFactory);
    templateFactory.$inject = ['$http', 'baseUrl'];

    function templateFactory($http, baseUrl) {
        return {
            getTemplate: getTemplate,
            createTemplate: createTemplate,
            deleteTemplate: deleteTemplate,
            getPermissionOnTemplate: getPermissionOnTemplate,
            getTemplateById: getTemplateById,
            updateTemplateBasicInfo: updateTemplateBasicInfo,
            updateStatusTemplate: updateStatusTemplate,
            updateTemplateKPI: updateTemplateKPI,
            getKPI: getKPI
        };

        function getTemplate(isActive) {
            return $http.get(baseUrl + 'JobLevelStandardTemplate/GetTemplate', {
                params: { isActive: isActive }
            });
        }

        function createTemplate(template) {
            return $http.post(baseUrl + 'JobLevelStandardTemplate/Create', template);
        }

        function deleteTemplate(templateId) {
            return $http['delete'](baseUrl + 'JobLevelStandardTemplate/Delete', { params: { templateId: templateId } });
        }

        function getTemplateById(id) {
            return $http.get(baseUrl + 'JobLevelStandardTemplate/GetById', { params: { templateId: id } });
        }

        function updateTemplateBasicInfo(template) {
            return $http.put(baseUrl + 'JobLevelStandardTemplate/Update', template);
        }

        function updateStatusTemplate(template) {
            return $http.put(baseUrl + 'JobLevelStandardTemplate/UpdateStatus', template);
        }

        function updateTemplateKPI(template) {
            return $http.put(baseUrl + 'JobLevelStandardTemplate/UpdateTemplateKPI', template);
        }

        function getKPI(id, isSelected) {
            return $http.get(baseUrl + 'JobLevelStandardTemplate/GetKPI',
                { params: { templateId: id, isSelected: isSelected } });
        }

        function getPermissionOnTemplate() {
            return $http.get(baseUrl + 'JobLevelStandardTemplate/GetUserPermission');
        }
    }
})(angular);
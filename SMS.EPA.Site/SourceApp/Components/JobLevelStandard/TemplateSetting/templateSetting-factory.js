(function (angular) {
    'use strict';
    /* jshint latedef:nofunc */

    angular.module('jobLevelStandardModule').factory('JLSTemplateSettingFactory', factory);
    factory.$inject = ['$http', 'baseUrl'];

    function factory($http, baseUrl) {
        return {
            getTemplateSetting,
            getUserPermission,
            deleteTemplateSetting,
            getTemplate,
            assignTemplate
        };

        function assignTemplate(params) {
            return $http.put(baseUrl + 'JobLevelStandardTemplateSetting/Assign', params);
        }

        function getTemplate() {
            return $http.get(baseUrl + 'Common/GetJobLevelStandardTemplate', {
                params: { isActive: true }
            });
        }

        function deleteTemplateSetting(objectId, objectTypeId) {
            return $http['delete'](baseUrl + 'JobLevelStandardTemplateSetting/Delete', {
                params: { objectId, objectTypeId }
            });
        }

        function getTemplateSetting(objectIds, objectTypeId, showAll) {
            return $http.get(baseUrl + 'JobLevelStandardTemplateSetting/GetTemplateSetting', {
                params: { objectIds, objectTypeId, showAll }
            });
        }

        function getUserPermission() {
            return $http.get(baseUrl + 'JobLevelStandardTemplateSetting/GetUserPermission');
        }
    }
})(angular);
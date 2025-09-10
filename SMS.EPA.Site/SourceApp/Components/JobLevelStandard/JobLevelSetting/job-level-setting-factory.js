(function (angular) {
    'use strict';
    /* jshint latedef:nofunc */

    angular.module('jobLevelStandardModule').factory('jobLevelSettingFactory', jobLevelSettingFactory);
    jobLevelSettingFactory.$inject = ['$http', 'baseUrl'];

    function jobLevelSettingFactory($http, baseUrl) {
        return {
            getPermission,
            getSettings,
            getBySectionId,
            edit
        };

        function getPermission() {
            return $http.get(baseUrl + 'JobLevelSetting/GetPermission');
        }

        function getSettings(params) {
            return $http.post(baseUrl + 'JobLevelSetting/GetSettings', params);
        }

        function getBySectionId(sectionId) {
            return $http.get(baseUrl + 'JobLevelSetting/GetBySectionId', { params: { sectionId } });
        }

        function edit(command) {
            return $http.put(baseUrl + 'JobLevelSetting/Edit', command);
        }
    }
})(angular);
(function (angular) {
  'use strict';
  /* jshint latedef:nofunc */

  angular.module('jobLevelStandardModule').factory('jobLevelSettingFactory', jobLevelSettingFactory);
  jobLevelSettingFactory.$inject = ['$http', 'baseUrl'];

  function jobLevelSettingFactory($http, baseUrl) {
    return {
      getPermission,
      getSettings,
      getByGroupId,
      edit
    };

    function getPermission() {
      return $http.get(baseUrl + 'JobLevelSetting/GetPermission');
    }

    function getSettings(params) {
      return $http.post(baseUrl + 'JobLevelSetting/GetSettings', params);
    }

    function getByGroupId(groupId) {
      return $http.get(baseUrl + 'JobLevelSetting/GetByGroupId', { params: { groupId } });
    }

    function edit(command) {
      return $http.put(baseUrl + 'JobLevelSetting/Edit', command);
    }
  }
})(angular);

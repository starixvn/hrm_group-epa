(function (angular) {
  'use strict';
  /* jshint latedef:nofunc */
  /* jshint validthis:true */

  angular.module('jobLevelStandardModule').controller('EditJobLevelSettingController', editJobLevelSettingController);
  editJobLevelSettingController.$inject = ['$uibModalInstance', 'JobLevelSettingModel', 'utilitiesFactory', 'lockScreenService', 'permission', 'group'];

  function editJobLevelSettingController($uibModalInstance, JobLevelSettingModel, utilitiesFactory, lockScreenService, permission, group) {
    let vm = this;

    vm.group = group;
    vm.settings = [];
    vm.permission = permission;
    vm.policy = utilitiesFactory.policy;

    function getSettings() {
      lockScreenService.start();

      JobLevelSettingModel.getByGroupId(vm.group.GroupId).then(function () {
        vm.settings = JobLevelSettingModel.settings;
        lockScreenService.stop();
      });
    }

    vm.cancel = function () {
      $uibModalInstance.dismiss('cancel');
    };

    vm.save = function () {
      const command = {
        groupIds: vm.group.GroupId,
        settings: vm.settings
      };
      JobLevelSettingModel.editJobLevelSetting(command, $uibModalInstance);
    };

    getSettings();
  }
})(angular);

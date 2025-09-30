(function (angular) {
  'use strict';
  /* jshint latedef:nofunc */
  /* jshint validthis:true */

  angular.module('jobLevelStandardModule').controller('JobLevelSettingController', jobLevelSettingController);
  jobLevelSettingController.$inject = ['JobLevelSettingModel', 'CommonModel', 'utilitiesFactory', 'lockScreenService'];

  function jobLevelSettingController(JobLevelSettingModel, CommonModel, utilitiesFactory, lockScreenService) {
    let vm = this;

    vm.groups = [];
    vm.modalInstance = '';
    vm.filter = { hasSetting: '' };
    vm.currentPage = 1;
    vm.pageSize = 20;
    vm.permission = JobLevelSettingModel.permission;
    vm.policy = utilitiesFactory.policy;
    utilitiesFactory.numberEntryList(vm);

    function getSettings(filter) {
      lockScreenService.start();

      JobLevelSettingModel.getSettings(filter).then(function () {
        vm.groups = JobLevelSettingModel.groups;
        utilitiesFactory.searchOnPage(vm, vm.groups);
        lockScreenService.stop();
      });
    }

    vm.query = function (filter) {
      vm.searchKey = '';
      vm.currentPage = 1;
      getSettings(filter);
    };

    vm.getIndex = function (index, currentPage, pageSize) {
      return CommonModel.getIndex(index, currentPage, pageSize);
    };

    vm.edit = function (group) {
      JobLevelSettingModel.openPopupEditJobLevelSetting(vm, group)
        .then(function (isSuccessful) {
          if (isSuccessful) {
            getSettings(vm.filter);
          }
        })
        ['finally'](function () {
          vm.modalInstance = '';
        });
    };

    vm.copy = function (group) {
      const groups = vm.groups.filter((x) => x.GroupId !== group.GroupId);
      JobLevelSettingModel.openPopupCopyJobLevelSetting(vm, group, groups)
        .then(function (isSuccessful) {
          if (isSuccessful) {
            getSettings(vm.filter);
          }
        })
        ['finally'](function () {
          vm.modalInstance = '';
        });
    };

    function initialize() {
      JobLevelSettingModel.getPermission().then(function () {
        vm.permission = JobLevelSettingModel.permission;

        getSettings(vm.filter);
      });
    }

    initialize();
  }
})(angular);

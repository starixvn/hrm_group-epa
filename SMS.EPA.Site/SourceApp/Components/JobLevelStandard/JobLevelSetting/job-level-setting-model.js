(function (angular) {
  'use strict';
  /* jshint latedef:nofunc */

  angular.module('jobLevelStandardModule').factory('JobLevelSettingModel', jobLevelSettingModel);
  jobLevelSettingModel.$inject = ['$uibModal', 'jobLevelSettingFactory', 'errorFactory', 'notificationFactory'];

  function jobLevelSettingModel($uibModal, jobLevelSettingFactory, errorFactory, notificationFactory) {
    let model = {};

    model.permission = 0;
    model.groups = [];
    model.settings = [];

    model.getPermission = function () {
      return jobLevelSettingFactory
        .getPermission()
        .success(function (permisson) {
          model.permission = permisson;
        })
        .error(function (response, status) {
          errorFactory.errorHandle(status);
        });
    };

    model.getSettings = function (params) {
      return jobLevelSettingFactory
        .getSettings(params)
        .success(function (groups) {
          model.groups = groups;
        })
        .error(function (response, status) {
          errorFactory.errorHandle(status);
        });
    };

    model.getByGroupId = function (groupId) {
      return jobLevelSettingFactory
        .getByGroupId(groupId)
        .success(function (settings) {
          model.settings = settings;
        })
        .error(function (response, status) {
          errorFactory.errorHandle(status);
        });
    };

    model.openPopupEditJobLevelSetting = function (vm, group) {
      vm.modalInstance = $uibModal.open({
        templateUrl: 'SourceApp/Components/JobLevelStandard/JobLevelSetting/Templates/edit.html',
        controller: 'EditJobLevelSettingController as editCtrl',
        backdrop: 'static',
        resolve: {
          permission: function () {
            return vm.permission;
          },
          group: function () {
            return group;
          }
        },
        windowClass: 'modal-confirm-center modal-edit-job-level-setting'
      });

      return vm.modalInstance.result;
    };

    model.editJobLevelSetting = function (command, modalInstance) {
      return jobLevelSettingFactory
        .edit(command)
        .success(function (response) {
          if (response.IsSuccessful) {
            notificationFactory.success('UpdatedSuccessfully');
            modalInstance.close(true);
          } else {
            notificationFactory.error('', response.Message);
          }
        })
        .error(function (response, status) {
          errorFactory.errorHandle(status);
        });
    };

    model.openPopupCopyJobLevelSetting = function (vm, selectedGroup, groups) {
      vm.modalInstance = $uibModal.open({
        templateUrl: 'SourceApp/Components/JobLevelStandard/JobLevelSetting/Templates/copy.html',
        controller: 'CopyJobLevelSettingController as copyCtrl',
        backdrop: 'static',
        resolve: {
          selectedGroup: function () {
            return selectedGroup;
          },
          groups: function () {
            return groups;
          }
        },
        windowClass: 'modal-confirm-center modal-copy-job-level-setting'
      });

      return vm.modalInstance.result;
    };

    return model;
  }
})(angular);

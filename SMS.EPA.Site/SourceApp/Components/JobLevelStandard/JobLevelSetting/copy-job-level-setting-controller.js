(function (angular) {
  'use strict';

  angular.module('jobLevelStandardModule').controller('CopyJobLevelSettingController', copyJobLevelSettingController);
  copyJobLevelSettingController.$inject = ['$uibModalInstance', 'notificationFactory', 'JobLevelSettingModel', 'selectedGroup', 'groups'];

  function copyJobLevelSettingController($uibModalInstance, notificationFactory, JobLevelSettingModel, selectedGroup, groups) {
    let vm = this;

    vm.selectedGroup = selectedGroup;
    vm.groups = [];
    vm.selectGroupIds = '';

    function initialize() {
      angular.forEach(groups, function (group) {
        // Find Business Unit index by name since UnitId might not exist
        let unitIndex = vm.groups.findIndex((x) => x.Text === group.Unit);

        // Find Department index within the Business Unit
        let departmentIndex = -1;
        if (unitIndex !== -1) {
          departmentIndex = vm.groups[unitIndex].Items.findIndex((x) => x.Id === group.DepartmentId);
        }

        // Create Team item
        const teamItem = {
          Id: group.GroupId,
          Text: group.Name,
          Enabled: true,
          Checked: false,
          IsLeaf: true,
          ParentId: group.DepartmentId,
          Items: []
        };

        // Create Department item if not exists
        const departmentItem = {
          Id: group.DepartmentId,
          Text: group.Department,
          Enabled: true,
          Checked: false,
          IsLeaf: false,
          ParentId: group.Unit || group.UnitId || group.DepartmentId, // Use Unit name as fallback
          Items: [teamItem]
        };

        // Create Business Unit item if not exists
        const unitItem = {
          Id: group.UnitId || group.Unit, // Use Unit name as ID if UnitId doesn't exist
          Text: group.Unit,
          Enabled: true,
          Checked: false,
          IsLeaf: false,
          ParentId: null,
          Items: [departmentItem]
        };

        if (unitIndex === -1) {
          // Business Unit doesn't exist, create it
          vm.groups.push(unitItem);
        } else if (departmentIndex === -1) {
          // Department doesn't exist within this Business Unit, add it
          vm.groups[unitIndex].Items.push(departmentItem);
        } else {
          // Department exists, just add the team
          vm.groups[unitIndex].Items[departmentIndex].Items.push(teamItem);
          // Update Department's ParentId to point to the correct Business Unit
          vm.groups[unitIndex].Items[departmentIndex].ParentId = group.UnitId || group.Unit;
        }
      });
    }

    function validate() {
      let isValid = true;
      if (vm.selectGroupIds === '') {
        isValid = false;
        notificationFactory.error('', 'Please select group you want to copy setting.');
      }

      return isValid;
    }

    vm.onSelectGroup = function (groupIds) {
      vm.selectGroupIds = groupIds;
    };

    vm.cancel = function () {
      $uibModalInstance.dismiss('cancel');
    };

    vm.save = function () {
      const isValid = validate();
      if (isValid) {
        JobLevelSettingModel.getByGroupId(vm.selectedGroup.GroupId).then(function () {
          const settings = JobLevelSettingModel.settings;

          const command = {
            groupIds: vm.selectGroupIds,
            settings: settings
          };
          JobLevelSettingModel.editJobLevelSetting(command, $uibModalInstance);
        });
      }
    };

    initialize();
  }
})(angular);

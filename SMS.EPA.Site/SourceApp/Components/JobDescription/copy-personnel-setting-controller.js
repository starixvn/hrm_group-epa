(function (angular) {
    'use strict';

    angular.module('jobDescriptionModule').controller('CopyEmployeeSettingController', copyEmployeeSettingController);
    copyEmployeeSettingController.$inject = ['$uibModalInstance', 'notificationFactory', 'JobDescriptionModel', 'employee'];

    function copyEmployeeSettingController($uibModalInstance, notificationFactory, JobDescriptionModel, employee) {
        var vm = this;

        vm.fromEmployeeId = '';
        vm.employee = angular.extend({}, employee, {
            displayName: employee.FullName + (employee.Nickname ? ' (' + employee.Nickname + ')' : '')
        });
        vm.employeeHasSettings = [];
        vm.cancel = cancel;
        vm.save = save;

        initialize();

        function initialize() {
            JobDescriptionModel.getEmployeeHasSetting().then(
                function () {
                    vm.employeeHasSettings = JobDescriptionModel.employeeHasSettings
                        .filter(function(item) { return item.EmployeeId !== vm.employee.EmployeeId; })
                        .map(function(item) { 
                            return angular.extend({}, item, { 
                                displayName: item.FullName + (item.Nickname ? ' (' + item.Nickname + ')' : '') 
                            }); 
                        });
                });
        }

        function validate() {
            var isValid = true;
            if (vm.fromEmployeeId === '') {
                isValid = false;
                notificationFactory.error('', 'Please select employee you want to copy setting.');
            }

            return isValid;
        }

        function cancel() {
            $uibModalInstance.dismiss('cancel');
        }

        function save() {
            if (validate()) {
                var setting = { fromEmployeeId: vm.fromEmployeeId, toEmployeeId: vm.employee.EmployeeId };
                JobDescriptionModel.copyEmployeeSetting(setting, $uibModalInstance);
            }
        }
    }
})(angular);
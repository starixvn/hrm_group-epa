(function (angular) {
    'use strict';

    angular.module('jobDescriptionModule').factory('jobDescriptionFactory', jobDescriptionFactory);
    jobDescriptionFactory.$inject = ['$http', 'baseUrl'];

    function jobDescriptionFactory($http, baseUrl) {
        return {
            getUserPermission,
            getGroupSettingInfo,
            getGroupHasSetting,
            getGroupSettingById,
            editGroupSetting,
            copyGroupSetting,
            getEmployeeSettingInfo,
            getEmployeeHasSetting,
            getEmployeeSettingById,
            editEmployeeSetting,
            copyEmployeeSetting,
            createJobDescriptionGroupItem,
            editJobDescriptionGroupItem,
            deleteJobDescriptionGroupItem,
            createJobDescriptionItem,
            editJobDescriptionItem,
            deleteJobDescriptionItem
        };

        function getUserPermission() {
            return $http.get(baseUrl + 'JobDescription/GetUserPermission');
        }

        function getGroupSettingInfo() {
            return $http.post(baseUrl + 'JobDescription/GetGroupSettingInfo', {});
        }

        function getGroupHasSetting() {
            return $http.get(baseUrl + 'JobDescription/GetGroupHasSetting');
        }

        function getGroupSettingById(groupId, jobLevelId) {
            return $http.get(baseUrl + 'JobDescription/GetGroupSettingById', { params: { groupId: groupId, jobLevelId: jobLevelId } });
        }

        function editGroupSetting(setting) {
            return $http.post(baseUrl + 'JobDescription/EditGroupSetting', setting);
        }

        function copyGroupSetting(setting) {
            return $http.post(baseUrl + 'JobDescription/CopyGroupSetting', setting);
        }

        function getEmployeeSettingInfo(teamIds, employeeId, showAll) {
            return $http.post(baseUrl + 'JobDescription/GetEmployeeSettingInfo', { teamIds, employeeId, showAll });
        }

        function getEmployeeHasSetting() {
            return $http.get(baseUrl + 'JobDescription/GetEmployeeHasSetting',);
        }

        function getEmployeeSettingById(employeeId) {
            return $http.get(baseUrl + 'JobDescription/GetEmployeeSettingById', { params: { employeeId: employeeId } });
        }

        function editEmployeeSetting(setting) {
            return $http.post(baseUrl + 'JobDescription/EditEmployeeSetting', setting);
        }

        function copyEmployeeSetting(setting) {
            return $http.post(baseUrl + 'JobDescription/CopyEmployeeSetting', setting);
        }

        function createJobDescriptionGroupItem(groupItem) {
            return $http.post(baseUrl + 'JobDescription/CreateJobDescriptionGroupItem', groupItem);
        }

        function createJobDescriptionItem(item) {
            return $http.post(baseUrl + 'JobDescription/CreateJobDescriptionItem', item);
        }

        function editJobDescriptionGroupItem(groupItem) {
            return $http.post(baseUrl + 'JobDescription/EditJobDescriptionGroupItem', groupItem);
        }

        function editJobDescriptionItem(item) {
            return $http.post(baseUrl + 'JobDescription/EditJobDescriptionItem', item);
        }

        function deleteJobDescriptionGroupItem(groupItemId) {
            return $http['delete'](baseUrl + 'JobDescription/DeleteJobDescriptionGroupItem', { params: { groupItemId: groupItemId } });
        }

        function deleteJobDescriptionItem(itemId) {
            return $http['delete'](baseUrl + 'JobDescription/DeleteJobDescriptionItem', { params: { itemId: itemId } });
        }
    }
})(angular);
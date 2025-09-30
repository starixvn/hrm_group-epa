(function (angular) {
    'use strict';
    /* jshint latedef:nofunc */

    angular.module('roleModule').factory('roleFactory', roleFactory);
    roleFactory.$inject = ['$http', 'baseUrl'];

    function roleFactory($http, baseUrl) {
        return {
            assignFunctionToRole: assignFunctionToRole,
            getRoleFunctionByRoleId: getRoleFunctionByRoleId
        };

        function assignFunctionToRole(roleFunction) {
            return $http.post(baseUrl + 'Role/AssignFunctionToRole', roleFunction);
        }

        function getRoleFunctionByRoleId(roleId) {
            return $http.get(baseUrl + 'Role/GetRoleFunctionByRoleId', { params: { roleId: roleId } });
        }
    }
})(angular);
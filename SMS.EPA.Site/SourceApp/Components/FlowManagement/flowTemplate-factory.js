(function (angular) {
    'use strict';
    /* jshint latedef:nofunc */

    angular.module('flowMngModule').factory('flowTemplateFactory', flowTemplateFactory);
    flowTemplateFactory.$inject = ['$http', 'baseUrl'];

    function flowTemplateFactory($http, baseUrl) {
        return {
            getFlowTemplate: function (targetTypeId, isActive) {
                return $http.get(baseUrl + 'FlowTemplate/GetFlowTemplate', {
                    params: {
                        targetTypeId: targetTypeId,
                        isActive: isActive
                    }
                });
            },
            getFlowTemplateById: function (flowTemplateId) {
                return $http.get(baseUrl + 'FlowTemplate/GetFlowTemplateById', { params: { flowTemplateId: flowTemplateId } });
            },
            createFlowTemplate: function (flowTemplate) {
                return $http.post(baseUrl + 'FlowTemplate/CreateFlowTemplate', flowTemplate);
            },
            updateFlowTemplate: function (flowTemplate) {
                return $http.put(baseUrl + 'FlowTemplate/UpdateFlowTemplate', flowTemplate);
            },
            updateStatusFlowTemplate: function (flowTemplate) {
                return $http.put(baseUrl + 'FlowTemplate/UpdateStatusFlowTemplate', flowTemplate);
            },
            deleteFlowTemplate: function (flowTemplateId) {
                return $http['delete'](baseUrl + 'FlowTemplate/DeleteFlowTemplate', { params: { flowTemplateId: flowTemplateId } });
            },
            getUserPermissionOnFlowManagement: function () {
                return $http.get(baseUrl + 'FlowTemplate/GetUserPermissionOnFlowManagement');
            }
        };
    }
})(angular);
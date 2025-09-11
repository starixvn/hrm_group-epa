using SMS.EPA.Models.JobDescription;

namespace SMS.EPA.Services.Interfaces;

public interface IJobDescriptionService
{
    JobDescriptionGroupSetting GetGroupSettingById(int groupId, int jobLevelId);

    JobDescriptionEmployeeSetting GetEmployeeSettingById(int employeeId);
}
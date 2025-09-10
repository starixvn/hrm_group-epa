# Script to add using GO.UserControl.Client.Core.Session; to all API controllers

$controllers = @(
    "Controllers/API/JobLevelStandard/JobLevelSettingController.cs",
    "Controllers/API/JobLevelStandard/JobLevelAssessmentReportController.cs",
    "Controllers/API/JobDescriptionController.cs",
    "Controllers/API/KPIGroupController.cs",
    "Controllers/API/JobLevelStandard/JobLevelAssessmentController.cs",
    "Controllers/API/ManagementEPAController.cs",
    "Controllers/API/KPIController.cs",
    "Controllers/API/JobLevelController.cs",
    "Controllers/API/PersonalFlowController.cs",
    "Controllers/API/PeriodController.cs",
    "Controllers/API/MonthlyPeriodController.cs",
    "Controllers/API/EvaluatorEPAController.cs",
    "Controllers/API/JobLevelStandard/JobLevelStandardPeriodController.cs",
    "Controllers/API/SelfEvaluationController.cs",
    "Controllers/API/ReviewerEPAController.cs",
    "Controllers/API/TargetEmployeeSettingController.cs",
    "Controllers/API/JobLevelStandard/JobLevelStandardKPIController.cs",
    "Controllers/API/JobLevelStandard/JobLevelStandardTemplateSettingController.cs",
    "Controllers/API/JobLevelStandard/JobLevelStandardTemplateController.cs",
    "Controllers/API/TargetTemplateController.cs"
)

foreach ($controller in $controllers) {
    if (Test-Path $controller) {
        Write-Host "Processing $controller..."
        
        # Read the file content
        $content = Get-Content $controller -Raw
        
        # Add using statement for IUserControlSession if not present
        if ($content -notmatch "using GO\.UserControl\.Client\.Core\.Session;") {
            # Find the first using statement and add after it
            $content = $content -replace "(using [^;]+;)", "`$1`nusing GO.UserControl.Client.Core.Session;"
        }
        
        # Write the modified content back
        Set-Content $controller -Value $content -NoNewline
        Write-Host "Added using statement to $controller"
    } else {
        Write-Host "File not found: $controller"
    }
}

Write-Host "All using statements added!"

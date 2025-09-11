using System.Globalization;
using SMS.EPA.Core.Services;
using SMS.EPA.Core.Services.Interfaces;

namespace SMS.EPA.Models.EmployeeEPA;

public class EPASummaryImportModel
{
    public int Id { get; set; }

    public float OriginFinalScore { get; set; }

    public string FinalScore
    {
        get
        {
            ICryptographyService cryptographyService = new CryptographyService();
            return cryptographyService.Encrypt(OriginFinalScore.ToString(CultureInfo.InvariantCulture));
        }
    }
}
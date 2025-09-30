using SMS.EPA.Models.Email;

namespace SMS.EPA.Services.Interfaces;

public interface IBaseMailService
{
    void SendEmail(MailModel email);

    string ReadTextFile(string filePath);
}
namespace SMS.EPA.Core.Model;

using Resources;

public class CommandResult
{
    public int ObjectId { get; set; }

    public int Code { get; set; } = 1;

    public string Message { get; set; } = Lang.InvalidInputData;

    public bool IsSuccessful => Code == 0;
}
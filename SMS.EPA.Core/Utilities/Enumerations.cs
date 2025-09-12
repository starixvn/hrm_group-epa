namespace SMS.EPA.Core.Utilities;

using System;
using System.ComponentModel;

public static class Enumerations
{
    public static string GetDescription(Enum enumType)
    {
        var desciption = string.Empty;

        if (enumType != null)
        {
            var fi = enumType.GetType().GetField(enumType.ToString());
            var attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            desciption = attributes.Length > 0 ? attributes[0].Description : enumType.ToString();
        }

        return desciption;
    }
}
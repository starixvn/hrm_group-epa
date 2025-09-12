namespace SMS.EPA.Core.Security;

using System.Security.Cryptography;

public static class Random
{
    #region Numeric

    public static string Numberic(int size)
    {
        const string numeric = "0123456789";
        return RandomString(numeric, size);
    }

    #endregion Numeric

    #region Alphabet

    public static string Alphabet(int size)
    {
        const string alphabet = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";
        return RandomString(alphabet, size);
    }

    public static string AnyAlphabet(int size)
    {
        const string alphabet = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890~!@#$%&()<>?-+=\|/";
        return RandomString(alphabet, size);
    }

    #endregion Alphabet

    #region RNGCrypRandom

    public static byte[] RngCrypto(int size)
    {
        var valueArray = new byte[size];
        using var crytoService = new RNGCryptoServiceProvider();
        crytoService.GetBytes(valueArray);

        return valueArray;
    }

    #endregion RNGCrypRandom

    private static string RandomString(string allowChars, int size)
    {
        var length = allowChars.Length;
        var valueArray = new char[size];

        var rd = new System.Random();
        for (var i = 0; i < size; i++)
        {
            valueArray[i] = allowChars[rd.Next(length)];
        }

        return new string(valueArray);
    }
}
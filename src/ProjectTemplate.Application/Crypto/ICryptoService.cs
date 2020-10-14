using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectTemplate.Application.Crypto
{
    public interface ICryptoService
    {
        string CreateUniqueKey(int length = 32);

        string Encrypt(string plainText, string key);

        string Decrypt(string cipherText, string key);
    }
}

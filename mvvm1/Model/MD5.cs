using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace mvvm1.Model
{
    class MD5
    {
        string strAlgName = HashAlgorithmNames.Md5;
        HashAlgorithmProvider objAlgProv = null;
        CryptographicHash objHash = null;
        public string MMD5(string value)
        {
            objAlgProv = HashAlgorithmProvider.OpenAlgorithm(strAlgName);
            objHash = objAlgProv.CreateHash();
            IBuffer buffMsg = CryptographicBuffer.ConvertStringToBinary(value, BinaryStringEncoding.Utf16BE);
            objHash.Append(buffMsg);
            IBuffer buffHash = objHash.GetValueAndReset();
            string strHash = CryptographicBuffer.EncodeToBase64String(buffHash);
            return strHash;
        }
    }
}

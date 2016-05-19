using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace Leaf.Model
{
    public class Md5
    {
        private readonly string _strAlgName = HashAlgorithmNames.Md5;
        private HashAlgorithmProvider _objAlgProv = null;
        private CryptographicHash _objHash = null;
        public string ToMd5(string value)
        {
            _objAlgProv = HashAlgorithmProvider.OpenAlgorithm(_strAlgName);
            _objHash = _objAlgProv.CreateHash();
            var buffMsg = CryptographicBuffer.ConvertStringToBinary(value, BinaryStringEncoding.Utf16BE);
            _objHash.Append(buffMsg);
            var buffHash = _objHash.GetValueAndReset();
            var strHash = CryptographicBuffer.EncodeToBase64String(buffHash);
            return strHash;
        }
    }
}

using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;

namespace Leaf.Model
{
    /// <summary>
    /// md5加密类，用于md5加密
    /// </summary>
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
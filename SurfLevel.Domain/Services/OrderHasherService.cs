using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using SurfLevel.Contracts.Interfaces.Services;
using SurfLevel.Domain.Options;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace SurfLevel.Domain.Services
{
    public class OrderHasherService : IOrderHasherService
    {
        private readonly string _secretKey;

        private class SerializableHashContainer
        {
            [JsonProperty("h")]
            public string HashKey;
        }

        public OrderHasherService(IOptions<HasherOptions> secretKey)
        {
            _secretKey = secretKey?.Value.SecretKey;
        }

        public string CreateOrderHash(string hashKey)
        {
            if (string.IsNullOrWhiteSpace(hashKey))
                throw new ArgumentNullException($"The given {nameof(hashKey)} is empty.");

            var obj = new SerializableHashContainer()
            {
                HashKey = hashKey
            };

            var convertedJson = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj, Formatting.None));

            using (var msi = new MemoryStream())
            {
                using (var mso = new MemoryStream(convertedJson))
                {
                    using (var gs = new GZipStream(mso, CompressionMode.Compress))
                    {
                        msi.CopyTo(gs);
                    }

                    return HttpUtility.UrlEncode(Convert.ToBase64String(mso.ToArray()));
                }
            }
        }

        public string ReadOrderHash(string hashedOrder, bool supportInherited = true)
        {
            try
            {
                var plainHash = Convert.FromBase64String(HttpUtility.UrlDecode(hashedOrder));

                try
                {
                    using (var msi = new MemoryStream(plainHash))
                    {
                        using (var mso = new MemoryStream())
                        {
                            using (var gs = new GZipStream(mso, CompressionMode.Decompress))
                            {
                                gs.CopyTo(msi);
                            }

                            var obj = JsonConvert.DeserializeObject<SerializableHashContainer>(Encoding.UTF8.GetString(msi.ToArray()));

                            return obj.HashKey;
                        }
                    }
                }
                catch
                {
                    if (supportInherited && !string.IsNullOrEmpty(_secretKey))
                        return DecryptInheritedFormatHash(plainHash);

                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        private string DecryptInheritedFormatHash(byte[] oldHash)
        {
            try
            {
                var criptedHash = new byte[oldHash.Length - 16];
                Buffer.BlockCopy(oldHash, 16, criptedHash, 0, criptedHash.Length);

                var nonce = new byte[16];
                Buffer.BlockCopy(oldHash, 0, nonce, 0, nonce.Length);

                var hashpass = string.Join(string.Empty, SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(_secretKey)).Select(p => p.ToString("x2")));
                var secret = Enumerable.Range(0, hashpass.Length / 2).Select(p => Convert.ToByte(hashpass.Substring(p * 2, 2), 16)).ToArray();

                var cipher = CipherUtilities.GetCipher("AES/CTR/NoPadding");
                cipher.Init(false, new ParametersWithIV(ParameterUtilities.CreateKeyParameter("AES", secret), nonce));

                var decrypted = Encoding.UTF8.GetString(cipher.DoFinal(criptedHash));

                if (decrypted.All(char.IsLetterOrDigit))
                    return decrypted;

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}

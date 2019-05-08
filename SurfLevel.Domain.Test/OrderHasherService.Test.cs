using NUnit.Framework;
using SurfLevel.Contracts.Interfaces.Services;
using SurfLevel.Domain.Options;
using SurfLevel.Domain.Services;
using System;
using System.Text;
using MSOpt = Microsoft.Extensions.Options.Options;

namespace SurfLevel.Domain.Test
{
    public class OrderHasherServiceTest
    {
        private IHasherService<string> _hasher;

        [SetUp]
        public void SetUp()
        {
            _hasher = new OrderHasherService(MSOpt.Create(new HasherOptions() { SecretKey = "<secretkey>" }));
        }

        static readonly object[] Crypted =
        {
            new Tuple<string, string> ("<hash>", "<encoded>")
        };

        //[TestCaseSource("Crypted")]
        public void Can_Decrypt_Old_Format_Hash(Tuple<string, string> hashPair)
        {
            var result = _hasher.Read(hashPair.Item2);

            Assert.False(string.IsNullOrEmpty(result));

            Assert.AreEqual(hashPair.Item1, result);
        }

        [Test]
        public void Can_Write_And_Reed_Hash()
        {
            var hash = Guid.NewGuid().ToString();

            var hashed = _hasher.Create(hash);

            var back = _hasher.Read(hashed);

            Assert.NotNull(back);

            Assert.AreEqual(hash, back);
        }

        [Test]
        public void Wrong_Hash_Reading_Resulted_As_Null()
        {
            var hash = Convert.ToBase64String(Encoding.ASCII.GetBytes("531sssfa9128242dasdasdasdas41241284718414"));

            var readed = _hasher.Read(hash);

            Assert.IsNull(readed);
        }
    }
}

using Newtonsoft.Json;
using NUnit.Framework;
using SurfLevel.Contracts.Interfaces.Services;
using SurfLevel.Contracts.Models.ViewModels.Search.DTO;
using SurfLevel.Domain.Services;
using System;

namespace SurfLevel.Domain.Test
{
    public class SeachHasherServiceTest
    {
        private ISearchHasherService _hasher;

        [SetUp]
        public void Setup()
        {
            _hasher = new SearchHasherService();
        }

        static readonly object[] Requests =
        {
            new SearchRequest()
            {
                From = DateTime.Now.Date,
                Till = DateTime.Now.AddDays(7).Date,
                Pax = 2,
                WithAccommodation = true
            },
            new SearchRequest()
            {
                From = DateTime.Now.Date,
                Till = null,
                Pax = 2,
                WithAccommodation = false
            }
        };

        [TestCaseSource("Requests")]
        public void Assure_convertation_passed_well(SearchRequest request)
        {
            // serialize
            string hash = string.Empty;
            Assert.DoesNotThrow(() => hash = _hasher.Create(request));
            Assert.That(!string.IsNullOrEmpty(hash));

            //deserialize
            SearchRequest serialized = null;
            Assert.DoesNotThrow(() => serialized = _hasher.Read<SearchRequest>(hash));
            Assert.NotNull(serialized);

            // проверяем что значения полей объектов идентичны
            // немного подхачим
            var settings = new JsonSerializerSettings() { DateTimeZoneHandling = DateTimeZoneHandling.Unspecified };
            Assert.AreEqual(JsonConvert.SerializeObject(request, settings), JsonConvert.SerializeObject(serialized, settings));
        }
    }
}

using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SurfLevel.Contracts.Interfaces.Services;
using SurfLevel.Domain.ViewModels.Search;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SurfLevel.Domain.Services
{
    public class SearchHasherService : IHasherService<SearchRequest>
    {
        private readonly JsonSerializerSettings _settings;
        
        private class SerializableRequest
        {
            [JsonProperty("f")]
            public DateTime DateFrom;
            [JsonProperty("t")]
            public DateTime? DateTill;
            [JsonProperty("p")]
            public int Pax;
        }

        public SearchHasherService()
        {
            _settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DateTimeZoneHandling = DateTimeZoneHandling.Unspecified,
                DateFormatString = "ddMMyyyy"
            };
        }

        public string Create(SearchRequest request)
        {
            if (request == null)
                throw new ArgumentException("Unable to create search hash: expected params, null was given", nameof(request));

            var req = new SerializableRequest
            {
                DateFrom = request.From,
                DateTill = request.WithAccommodation ? request.Till : null,
                Pax = request.Pax
            };

            var json = JsonConvert.SerializeObject(req, Formatting.None, _settings);
            // избавимся от служебных символов, они не несут полезной информации
            var rawData = Regex.Replace(json.Substring(1, json.Length - 2), "[:\"]", string.Empty);
            var bytes = Encoding.UTF8.GetBytes(rawData);

            return Base64UrlEncoder.Encode(bytes);
        }

        public SearchRequest Read(string hash)
        {
            if (string.IsNullOrWhiteSpace(hash))
                throw new ArgumentException("Unable to read search hash. The argument was not defined.", nameof(hash));

            SerializableRequest req;
            try
            {
                var rawData = Base64UrlEncoder.Decode(hash);
                // восстановим служебные символы
                var withSeparation = string.Join(",", rawData.Split(',', options: StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => $"\"{p.Substring(0, 1)}\":\"{p.Substring(1, p.Length - 1)}\"")
                );
                req = JsonConvert.DeserializeObject<SerializableRequest>($"{{{withSeparation}}}", _settings);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Unable to read search hash. Hash format is unknown.", nameof(hash), ex);
            }

            return new SearchRequest()
            {
                From = req.DateFrom,
                Till = req.DateTill,
                Pax = req.Pax,
                WithAccommodation = req.DateTill.HasValue
            };
        }
    }
}

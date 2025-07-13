using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HealthCare.Project.Core.Entities
{
    public class MedicineAlternative
    {
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("composition")]

        public string? Composition { get; set; }
        [JsonPropertyName("class")]
        public string? Class { get; set; }
        [JsonPropertyName("company")]

        public string? Company { get; set; }
        [JsonPropertyName("price")]

        public string? Price { get; set; }
    }
}

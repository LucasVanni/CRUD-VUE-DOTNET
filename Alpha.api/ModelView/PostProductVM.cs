using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Alpha.api.Models
{
    public class PostProductVM
    {                               
        [JsonProperty("title")]
        [MaxLength(100)]
        public string? Name { get; set; }        
        
        [JsonProperty("price")]
        public decimal? Price { get; set; }

        [JsonProperty("barCode")]        
        [MaxLength(100)]
        public string? BarCode { get; set; }        

        [JsonProperty("imageBase64")]                
        public string? ImageBase64 { get; set; }
    }
}

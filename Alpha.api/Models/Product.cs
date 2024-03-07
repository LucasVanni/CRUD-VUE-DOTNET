using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Alpha.api.Models
{
    public class Product
    {

        internal string? category;
        internal string? title;
        internal string? price;

        [Column("Id")]
        [Display(Name = "Code")]
        [JsonProperty("id")]
        public int Id { get; set; }

        [Column("Name")]
        [Display(Name = "Name")]
        [JsonProperty("title")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Column("Price")]
        [Display(Name = "Price")]
        [JsonProperty("price")]
        public decimal Price { get; set; }

        [Column("BarCode")]
        [Display(Name = "BarCode")]
        [MaxLength(100)]
        public string BarCode { get; set; }

        [JsonProperty("image")]
        [Column("ImageUrl")]
        [Display(Name = "ImageUrl")]
        public string Image { get; set; }
    }
}

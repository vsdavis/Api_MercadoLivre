using System.Text.Json.Serialization;

namespace ApiMercado.Models
{
    public class Produto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("category_id")]
        public string Category_id { get; set; }

        [JsonPropertyName("price")]
        public int Price { get; set; }

        [JsonPropertyName("base_price")]
        public int Base_price { get; set; }

        [JsonPropertyName("original_price")]
        public double Original_price { get; set; }

        [JsonPropertyName("currency_id")]
        public string Currency { get; set; }

        [JsonPropertyName("official_store_id")]
        public int Official_store_id { get; set; }



    }
}

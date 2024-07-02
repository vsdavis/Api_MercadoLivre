using static System.Net.Mime.MediaTypeNames;
using System.Text.Json.Serialization;

using System.Text.Json.Serialization;

namespace ApiMercado.Models;

public class ProdutoMercadoLivre
{
    [JsonPropertyName("results")]
    public Produto? Produto { get; set; }
}
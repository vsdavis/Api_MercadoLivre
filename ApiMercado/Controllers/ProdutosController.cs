using System.Net;
using System.Net.Http.Headers; // Adicione esta diretiva para AuthenticationHeaderValue
using System.Text.Json;

namespace ApiMercado.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly HttpClient _httpClient;

    public ProdutosController(AppDbContext context, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _httpClient = httpClientFactory.CreateClient("MLB");
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Produto>> GetProduto(string id)
    {
        try
        {
            var produto = await _context.Produto.FindAsync(id);

            if (produto == null)
            {
                return NotFound();
            }

            return produto;
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao buscar o produto: {ex.Message}");
        }
    }


    [HttpPost]
    public async Task<ActionResult<Produto>> PostProduto(Produto produto)
    {
        _context.Produto.Add(produto);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduto(string id, Produto produto)
    {
        if (id != produto.Id)
        {
            return BadRequest();
        }

        _context.Entry(produto).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProdutoExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    private bool ProdutoExists(string id)
    {
        return _context.Produto.Any(e => e.Id == id);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduto(string id)
    {
        var produto = await _context.Produto.FindAsync(id);
        if (produto == null)
        {
            return NotFound();
        }

        _context.Produto.Remove(produto);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("MercadoLivre/{id}")]
    public async Task<IActionResult> GetProdutoDoMercadoLivre(string id)
    {
        try
        {
            var url = $"{id}"; // append the id to the base address set in the HttpClient

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "APP_USR-6500094978999069-070307-c0edc088225a328a3aae3522dd720a9d-1855456057");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return StatusCode(401, "Não autorizado para acessar o produto do Mercado Livre");
                }
                return StatusCode((int)response.StatusCode, "Erro ao acessar o produto do Mercado Livre");
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var produto = JsonSerializer.Deserialize<ProdutoMercadoLivre>(jsonString);

            return Ok(produto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao processar a solicitação: {ex.Message}");
        }
    }
}

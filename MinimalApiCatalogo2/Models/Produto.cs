namespace MinimalApiCatalogo2.Models;

public class Produto
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? descricao { get; set; }
    public decimal Preco { get; set; }
    public string? ImagemUrl { get; set; }
    public DateTime DataCompra { get; set; }
    public int Estoque { get; set; }

    //Afirmação explicita de relacionamento um para muitos
    public int CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }
}

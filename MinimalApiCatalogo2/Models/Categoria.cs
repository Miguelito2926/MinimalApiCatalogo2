﻿namespace MinimalApiCatalogo2.Models;


public class Categoria
{
    public int CategoriaId { get; set; }
    public string? Nome { get; set; }
    public string? Descricao { get; set;}

    // Relacionamento um para muitos
    public ICollection<Produto>? Produtos { get; set; }
}

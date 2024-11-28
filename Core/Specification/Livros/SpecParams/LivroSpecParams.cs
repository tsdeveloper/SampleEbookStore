namespace Core.Specification.Livros.SpecParams;

public class LivroSpecParams : BaseSpecParams
{
  public int? CodL { get; set; }
  public int? Edicao { get; set; }
  public string? AnoPublicacao { get; set; }
  public string? Editora { get;  set; }
    public bool IncluirAutores { get;  set; }
    public bool IncluirAssuntos { get; set; }
}

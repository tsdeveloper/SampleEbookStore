namespace Core.Entities;

public class Assunto
{
  public int CodAs { get; set; }
  public string Descricao { get; set; }
  public ICollection<Livro_Assunto> Livro_AssuntoList { get; set; } = new List<Livro_Assunto>();
}

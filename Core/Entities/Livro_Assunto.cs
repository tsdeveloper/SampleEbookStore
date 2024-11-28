namespace Core.Entities;

public class Livro_Assunto
{
  public int Livro_CodL { get; set; }
  public int Assunto_CodAs { get; set; }
  public Livro Livro { get; set; }
  public Assunto Assunto { get; set; }
}

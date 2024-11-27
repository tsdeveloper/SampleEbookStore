namespace Core.Entities;

public class Autor
{

    public int CodAu { get; set; }
  public string Nome { get; set; }
  public ICollection<Livro_Autor> Livro_AutorList { get; set; } = new List<Livro_Autor>();

}

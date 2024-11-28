namespace Core.Entities;

public class Livro
{

  public Livro()
  {

  }
  public Livro(int livroIds)
  {
    CodL = livroIds;
  }

  public int CodL { get; set; }
  public string Titulo { get; set; }
  public string Editora { get; set; }
  public int Edicao { get; set; }
  public string AnoPublicacao { get; set; }
  public ICollection<Livro_Autor> Livro_AutorList { get; set; } = new List<Livro_Autor>();
  public ICollection<Livro_Assunto> Livro_AssuntoList { get; set; } = new List<Livro_Assunto>();


}

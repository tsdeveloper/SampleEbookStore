using Dapper.Contrib.Extensions;

namespace Core.Entities;

public class Livro_Autor
{
   
    public int Livro_CodL { get; set; }
   
    public int Autor_CodAu { get; set; }

  
  public Livro Livro { get; set; } 
    public Autor Autor { get; set; }

}

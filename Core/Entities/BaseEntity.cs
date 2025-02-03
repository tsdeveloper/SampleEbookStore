namespace Core.Entities;

public class BaseEntity
{
  public DateTime CriadoEm { get; set; }
  public DateTime AtualizadoEm { get; set; }
  public bool Deletado { get; set; }
}

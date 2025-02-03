using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Config;

public class Livro_AutorConfig : IEntityTypeConfiguration<Livro_Autor>
{
    public void Configure(EntityTypeBuilder<Livro_Autor> b)
    {
        b.HasKey(x => new { x.Autor_CodAu, x.Livro_CodL });

        b.HasOne(x => x.Livro)
                 .WithMany(a => a.Livro_AutorList)
                 .HasForeignKey(b => b.Livro_CodL);

        b.HasOne(x => x.Autor)
                 .WithMany(a => a.Livro_AutorList)
                 .HasForeignKey(b => b.Autor_CodAu);                 
    }
}

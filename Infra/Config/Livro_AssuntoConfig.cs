using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Config;

public class Livro_AssuntoConfig : IEntityTypeConfiguration<Livro_Assunto>
{
    public void Configure(EntityTypeBuilder<Livro_Assunto> b)
    {
        b.HasKey(x => new { x.Livro_CodL, x.Assunto_CodAs });

        b.HasOne(x => x.Livro)
                 .WithMany(a => a.Livro_AssuntoList)
                 .HasForeignKey(b => b.Livro_CodL);

        b.HasOne(x => x.Assunto)
                 .WithMany(a => a.Livro_AssuntoList)
                 .HasForeignKey(b => b.Assunto_CodAs);
    }
}

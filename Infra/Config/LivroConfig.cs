using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Config;

public class LivroConfig : IEntityTypeConfiguration<Livro>
{
    public void Configure(EntityTypeBuilder<Livro> b)
    {
        b.HasKey(x => x.CodL);

        b.Property(x => x.Titulo)
            .HasMaxLength(40)
            .IsRequired();

        b.Property(x => x.Editora)
            .HasMaxLength(40)
            .IsRequired();

        b.Property(x => x.Edicao)
            .IsRequired();                    

        b.Property(x => x.AnoPublicacao)
            .HasMaxLength(4)
            .IsRequired();        
    }
}

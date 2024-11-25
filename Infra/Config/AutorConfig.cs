using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Config;

public class AutorConfig : IEntityTypeConfiguration<Autor>
{
    public void Configure(EntityTypeBuilder<Autor> b)
    {
        b.HasKey(x => x.CodAu);

        b.Property(x => x.Nome)
            .HasMaxLength(40)
            .IsRequired();
    }
}

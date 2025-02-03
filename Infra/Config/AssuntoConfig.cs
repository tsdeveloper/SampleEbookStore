using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Config;

public class AssuntoConfig : IEntityTypeConfiguration<Assunto>
{
    public void Configure(EntityTypeBuilder<Assunto> b)
    {
        b.HasKey(x => x.CodAs);

        b.Property(x => x.Descricao)
            .HasMaxLength(20)
            .IsRequired();
    }
}

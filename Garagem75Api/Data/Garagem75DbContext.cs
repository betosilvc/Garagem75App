using Garagem75.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Garagem75.Api.Data;

public class Garagem75DBContext : DbContext
{
    public Garagem75DBContext(DbContextOptions<Garagem75DBContext> options) : base(options)
    {
    }
    public DbSet<Veiculo> Veiculos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Peca> Pecas { get; set; }
    public DbSet<TipoUsuario> TipoUsuarios { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }
    public DbSet<OrdemServico> OrdemServicos { get; set; }
    public DbSet<OrdemServicoPeca> OrdemServicoPecas { get; set; }
    public DbSet<BlogPost> BlogPosts { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Configurações adicionais podem ser feitas aqui
        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<OrdemServico>()
        .Property(p => p.MaoDeObra)
        .HasPrecision(18, 2);

        modelBuilder.Entity<OrdemServico>()
            .Property(p => p.ValorDesconto)
            .HasPrecision(18, 2);

        modelBuilder.Entity<OrdemServico>()
            .Property(p => p.ValorTotal)
            .HasPrecision(18, 2);
        // Configuração da Chave Composta para OrdemServicoPeca
        modelBuilder.Entity<OrdemServicoPeca>()
            .HasKey(osp => new { osp.OrdemServicoId, osp.PecaId });

        // Configuração do relacionamento OrdemServico -> OrdemServicoPeca
        modelBuilder.Entity<OrdemServicoPeca>()
            .HasOne(osp => osp.OrdemServico)
            .WithMany(os => os.PecasAssociadas)
            .HasForeignKey(osp => osp.OrdemServicoId);

        // Configuração do relacionamento Peca -> OrdemServicoPeca
        modelBuilder.Entity<OrdemServicoPeca>()
            .HasOne(osp => osp.Peca)
            .WithMany() // A Peca pode não ter uma propriedade de navegação de volta para OrdemServicoPeca se não for necessária
            .HasForeignKey(osp => osp.PecaId);

        modelBuilder.Entity<TipoUsuario>().HasData(
        new TipoUsuario
        {
            IdTipoUsuario = 1,
            DescricaoTipoUsuario = "Administrador"
        },
        new TipoUsuario
        {
            IdTipoUsuario = 2,
            DescricaoTipoUsuario = "Mecânico"
        }
        );
    }

}



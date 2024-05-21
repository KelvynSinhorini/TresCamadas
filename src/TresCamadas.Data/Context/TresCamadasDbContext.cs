using Microsoft.EntityFrameworkCore;
using TresCamadas.Business.Models;

namespace TresCamadas.Data.Context;
public class TresCamadasDbContext : DbContext
{
    public TresCamadasDbContext(DbContextOptions<TresCamadasDbContext> options) : base(options)
    {
    }

    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }
    public DbSet<Fornecedor> Fornecedores { get; set; }
}

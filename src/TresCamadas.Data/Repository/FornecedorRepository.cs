using Microsoft.EntityFrameworkCore;
using TresCamadas.Business.Interfaces;
using TresCamadas.Business.Models;
using TresCamadas.Data.Context;

namespace TresCamadas.Data.Repository;

public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepository
{
    public FornecedorRepository(TresCamadasDbContext context) : base(context) { }

    public async Task<Fornecedor> ObterFornecedorEndereco(Guid id)
    {
        return await Db.Fornecedores.AsNoTracking()
            .Include(c => c.Endereco)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Fornecedor> ObterFornecedorProdutosEndereco(Guid id)
    {
        return await Db.Fornecedores.AsNoTracking()
            .Include(c => c.Produtos)
            .Include(c => c.Endereco)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Endereco> ObterEnderecoPorFornecedor(Guid fornecedorId)
    {
        return await Db.Enderecos.AsNoTracking()
            .FirstOrDefaultAsync(f => f.FornecedorId == fornecedorId);
    }

    public async Task RemoverEnderecoFornecedor(Endereco endereco)
    {
        Db.Enderecos.Remove(endereco);
        await SaveChanges();
    }
}

using TresCamadas.Business.Interfaces;
using TresCamadas.Business.Models;
using TresCamadas.Business.Models.Validations;

namespace TresCamadas.Business.Services;

public class FornecedorService : BaseService, IFornecedorService
{
    private readonly IFornecedorRepository _fornecedorRepository;

    public FornecedorService(IFornecedorRepository fornecedorRepository)
    {
        _fornecedorRepository = fornecedorRepository;
    }

    public async Task Adicionar(Fornecedor fornecedor)
    {
        if (!ExecutarValidacao(new FornecedorValidation(), fornecedor ) ||
            !ExecutarValidacao(new EnderecoValidation(), fornecedor.Endereco)) 
            return;

        // Validar se ja nao existe outro fornecedor com o mesmo doc.

        await _fornecedorRepository.Adicionar(fornecedor);
    }

    public async Task Atualizar(Fornecedor fornecedor)
    {
        if (!ExecutarValidacao(new FornecedorValidation(), fornecedor))
            return;

        await _fornecedorRepository.Atualizar(fornecedor);
    }

    public async Task Remover(Guid id)
    {
        await _fornecedorRepository.Remover(id);
    }

    public void Dispose()
    {
        _fornecedorRepository?.Dispose();
    }
}

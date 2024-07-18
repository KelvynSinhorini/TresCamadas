using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TresCamadas.Api.ViewModels;
using TresCamadas.Business.Interfaces;
using TresCamadas.Business.Models;

namespace TresCamadas.Api.Controllers;

[Route("api/fornecedores")]
public class FornecedoresController : MainController
{
    private readonly IMapper _mapper;
    private readonly IFornecedorRepository _fornecedorRepository;
    private readonly IFornecedorService _fornecedorService;

    public FornecedoresController(IMapper mapper,
                                  IFornecedorRepository fornecedorRepository,
                                  IFornecedorService fornecedorService,
                                  INotificador notificador) : base(notificador)
    {
        _mapper = mapper;
        _fornecedorRepository = fornecedorRepository;
        _fornecedorService = fornecedorService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<FornecedorViewModel>>> ObterTodos()
    {
        try
        {
            var fornecedores = await _fornecedorRepository.ObterTodos();
            return Ok(_mapper.Map<IEnumerable<FornecedorViewModel>>(fornecedores));
        }
        catch (Exception ex)
        {
            // Logar o erro
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao obter fornecedores.");
        }
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<FornecedorViewModel>> ObterPorId(Guid id)
    {
        try
        {
            var fornecedor = await ObterFornecedorProdutosEndereco(id);

            if (fornecedor == null) return NotFound();

            return Ok(fornecedor);
        }
        catch (Exception ex)
        {
            // Logar o erro
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao obter fornecedor.");
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(FornecedorViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<FornecedorViewModel>> Adicionar(FornecedorViewModel fornecedorViewModel)
    {
        if (!ModelState.IsValid)
            return CustomResponse(ModelState);

        try
        {
            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
            await _fornecedorService.Adicionar(fornecedor);

            return CustomResponse(HttpStatusCode.Created, fornecedorViewModel);
        }
        catch (Exception ex)
        {
            // Logar o erro
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao adicionar fornecedor.");
        }
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<FornecedorViewModel>> Atualizar(Guid id, FornecedorViewModel fornecedorViewModel)
    {
        if (id != fornecedorViewModel.Id)
        {
            NotificarErro("O id informado não é o mesmo que foi passado na request!");
            return CustomResponse();
        }

        if (!ModelState.IsValid) return CustomResponse(ModelState);

        try
        {
            var fornecedor = await ObterFornecedorEndereco(id);
            if (fornecedor == null)
                return NotFound("Fornecedor não encontrado.");

            await _fornecedorService.Atualizar(_mapper.Map<Fornecedor>(fornecedorViewModel));
            return CustomResponse(HttpStatusCode.NoContent);
        }
        catch (Exception ex)
        {
            // Logar o erro
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao atualizar fornecedor.");
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<FornecedorViewModel>> Excluir(Guid id)
    {
        try
        {
            var fornecedor = await ObterFornecedorEndereco(id);
            if (fornecedor == null)
                return NotFound("Fornecedor não encontrado.");

            await _fornecedorService.Remover(id);

            return CustomResponse(HttpStatusCode.NoContent);
        }
        catch (Exception ex)
        {
            // Logar o erro
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao excluir fornecedor.");
        }
    }

    private async Task<FornecedorViewModel> ObterFornecedorProdutosEndereco(Guid id)
    {
        return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));
    }

    private async Task<FornecedorViewModel> ObterFornecedorEndereco(Guid id)
    {
        return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorEndereco(id));
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TresCamadas.Api.ViewModels;
using TresCamadas.Business.Interfaces;
using TresCamadas.Business.Models;

namespace TresCamadas.Api.Controllers;

[Route("api/produtos")]
public class ProdutosController : MainController
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IProdutoService _produtoService;
    private readonly IMapper _mapper;

    public ProdutosController(IProdutoRepository produtoRepository,
                              IProdutoService produtoService,
                              IMapper mapper,
                              INotificador notificador) : base(notificador)
    {
        _produtoRepository = produtoRepository;
        _produtoService = produtoService;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<ProdutoViewModel>>> ObterTodos()
    {
        try
        {
            var produtos = await _produtoRepository.ObterProdutosFornecedores();
            return Ok(_mapper.Map<IEnumerable<ProdutoViewModel>>(produtos));
        }
        catch (Exception ex)
        {
            // Logar o erro
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao obter produtos.");
        }
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProdutoViewModel>> ObterPorId(Guid id)
    {
        try
        {
            var produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null) return NotFound();

            return produtoViewModel;
        }
        catch (Exception ex)
        {
            // Logar o erro
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao obter produto.");
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProdutoViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProdutoViewModel>> Adicionar(ProdutoViewModel produtoViewModel)
    {
        try
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _produtoService.Adicionar(_mapper.Map<Produto>(produtoViewModel));

            return CustomResponse(HttpStatusCode.Created, produtoViewModel);
        }
        catch (Exception ex)
        {
            // Logar o erro
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao adicionar produto.");
        }
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Atualizar(Guid id, ProdutoViewModel produtoViewModel)
    {
        try
        {
            if (id != produtoViewModel.Id)
            {
                NotificarErro("O id informado não é o mesmo que foi passado na request!");
                return CustomResponse();
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var produtoAtualizacao = await ObterProduto(id);

            if (produtoAtualizacao == null) return NotFound();

            produtoAtualizacao.FornecedorId = produtoViewModel.FornecedorId;
            produtoAtualizacao.Nome = produtoViewModel.Nome;
            produtoAtualizacao.Descricao = produtoViewModel.Descricao;
            produtoAtualizacao.Valor = produtoViewModel.Valor;
            produtoAtualizacao.Ativo = produtoViewModel.Ativo;

            await _produtoService.Atualizar(_mapper.Map<Produto>(produtoAtualizacao));

            return CustomResponse(HttpStatusCode.NoContent);
        }
        catch (Exception ex)
        {
            // Logar o erro
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao atualizar produto.");
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProdutoViewModel>> Excluir(Guid id)
    {
        try
        {
            var produto = await ObterProduto(id);

            if (produto == null) return NotFound();

            await _produtoService.Remover(id);

            return CustomResponse(HttpStatusCode.NoContent);
        }
        catch (Exception ex)
        {
            // Logar o erro
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao excluir produto.");
        }
    }

    private async Task<ProdutoViewModel> ObterProduto(Guid id)
    {
        return _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterProdutoFornecedor(id));
    }
}

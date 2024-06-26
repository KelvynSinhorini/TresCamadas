﻿using TresCamadas.Business.Interfaces;
using TresCamadas.Business.Models;
using TresCamadas.Business.Models.Validations;

namespace TresCamadas.Business.Services;

public class ProdutoService : BaseService, IProdutoService
{
    private readonly IProdutoRepository _produtoRepository;

    public ProdutoService(IProdutoRepository produtoRepository,
                          INotificador notificador) : base(notificador)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task Adicionar(Produto produto)
    {
        if (!ExecutarValidacao(new ProdutoValidation(), produto))
            return;

        if(_produtoRepository.ObterPorId(produto.Id) != null)
        {
            Notificar("Já existe um produto com o ID infomado.");
            return;
        }

        await _produtoRepository.Adicionar(produto);
    }

    public async Task Atualizar(Produto produto)
    {
        if (!ExecutarValidacao(new ProdutoValidation(), produto))
            return;

        await _produtoRepository.Atualizar(produto);
    }

    public async Task Remover(Guid id)
    {
        await _produtoRepository.Remover(id);
    }

    public void Dispose()
    {
        _produtoRepository?.Dispose();
    }
}

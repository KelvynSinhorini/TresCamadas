﻿using TresCamadas.Business.Models;

namespace TresCamadas.Business.Interfaces;

public interface IProdutoService : IDisposable
{
    Task Adicionar(Produto produto);
    Task Atualizar(Produto produto);
    Task Remover(Guid id);
}

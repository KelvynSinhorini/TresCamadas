﻿using TresCamadas.Business.Models;

namespace TresCamadas.Business.Interfaces;

public interface IFornecedorService : IDisposable
{
    Task Adicionar(Fornecedor fornecedor);
    Task Atualizar(Fornecedor fornecedor);
    Task Remover(Guid id);
}

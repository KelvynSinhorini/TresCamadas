﻿using FluentValidation;
using TresCamadas.Business.Models.Validations.Documentos;

namespace TresCamadas.Business.Models.Validations;
public class FornecedorValidation : AbstractValidator<Fornecedor>
{
    public FornecedorValidation()
    {
        RuleFor(c => c.Nome)
            .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido.")
            .Length(2, 100).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres.");

        RuleFor(c => c.Documento)
            .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido.");

        When(f => f.TipoFornecedor == TipoFornecedor.PessoaFisica, () =>
        {
            RuleFor(f => f.Documento.Length).Equal(CpfValidacao.TamanhoCpf)
                .WithMessage("O campo Documento precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}.");

            RuleFor(f => CpfValidacao.Validar(f.Documento)).Equal(true)
                .WithMessage("O documento fornecido é inválido.");
        });

        When(f => f.TipoFornecedor == TipoFornecedor.PessoaJuridica, () =>
        {
            RuleFor(f => f.Documento.Length).Equal(CnpjValidacao.TamanhoCnpj)
                .WithMessage("O campo Documento precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}.");

            RuleFor(f => CnpjValidacao.Validar(f.Documento)).Equal(true)
                .WithMessage("O documento fornecido é inválido.");
        });
    }
}

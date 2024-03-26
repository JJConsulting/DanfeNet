﻿using DanfeNet.Models;

namespace DanfeNet.Tests;

public static class FabricaFake
{
    public const double Value = 1234.56;

    public static LocalEntregaRetiradaInfo LocalEntregaRetiradaFake()
    {
        return new LocalEntregaRetiradaInfo()
        {
            NomeRazaoSocial = "Umbrella Corp",
            Endereco = "Rua João Augusto Morais, 267",
            InscricaoEstadual = "361499373647",
            Bairro = "Vila Doutor Eiras",
            CnpjCpf = "22257735000138",
            Cep = "08010150",
            Municipio = "São Paulo",
            Uf = "SP",
            Telefone = "1012345678"
        };
    }

    public static CalculoImposto CalculoImpostoViewModel()
    {
        return new CalculoImposto
        {
            BaseCalculoIcms = Value,
            BaseCalculoIcmsSt = Value,
            Desconto = Value,
            OutrasDespesas = Value,
            ValorAproximadoTributos = Value,
            ValorCofins = Value,
            ValorFrete = Value,
            ValorIcms = Value,
            ValorIcmsSt = Value,
            ValorII = Value,
            ValorIpi = Value,
            ValorPis = Value,
            ValorSeguro = Value,
            ValorTotalNota = Value,
            ValorTotalProdutos = Value,
            vFCPUFDest = Value,
            vICMSUFDest = Value,
            vICMSUFRemet = Value
        };
    }

    public static CalculoIssqn CalculoIssqnViewModel()
    {
        return new CalculoIssqn
        {
            BaseIssqn = Value,
            InscricaoMunicipal = "123456789",
            Mostrar = true,
            ValorIssqn = Value,
            ValorTotalServicos = Value
        };
    }

    public static Danfe DanfeViewModel_1()
    {
        var m = new Danfe()
        {
            NfNumero = 888888888,
            NfSerie = 888,
            ChaveAcesso = new string('0', 44),
            Emitente = new EmpresaInfo()
            {
                CnpjCpf = new string('0', 14),
                RazaoSocial = "Abstergo do Brasil Indústria de Tecnologia Ltda.",
                NomeFantasia = "Abstergo",
                Email = "fake@mail.123",
                EnderecoBairro = "Bairro",
                EnderecoCep = "12345678",
                EnderecoComplemento = "Compl",
                EnderecoLogadrouro = "Avenida Brasil",
                EnderecoNumero = "S/N",
                EnderecoUf = "SP",
                Municipio = "São Paulo",
                Ie = "87787",
                IeSt = "87878",
                IM = "45454",
                Telefone = "0000000000",
                CRT = "3"
            },
            Destinatario = new EmpresaInfo()
            {
                CnpjCpf = new string('1', 14),
                RazaoSocial = "Umbrella Corp Ltda",
                Email = "fake@mail.123",
                EnderecoBairro = "Bairro",
                EnderecoCep = "12345678",
                EnderecoComplemento = "Compl",
                EnderecoLogadrouro = "Avenida Brasil",
                EnderecoNumero = "S/N",
                EnderecoUf = "SP",
                Municipio = "São Paulo",
                Ie = "87787",
                IeSt = "87878",
                IM = "45454",
                Telefone = "0000000000"
            },
            Transportadora = new Transportadora()
            {
                RazaoSocial = "Correios",
                CnpjCpf = new string('8', 14),
                VeiculoUf = "RS",
                QuantidadeVolumes = 123.1234,
                CodigoAntt = new string('8', 20),
                EnderecoBairro = "Bairo",
                EnderecoCep = "00000",
                EnderecoComplemento = "Complemento",
                Especie = "Especie",
                Placa = "MMMWWWW",
                EnderecoLogadrouro = "Logadrouro",
                Ie = "12334",
                EnderecoUf = "RS",
                PesoLiquido = 456.7794,
                Marca = "DanfeNet",
                EnderecoNumero = "101",
                ModalidadeFrete = 4,
                PesoBruto = 101.1234
            },
            InformacoesComplementares = "Aqui vai as informações complementares."
        };
        m.CalculoImposto = CalculoImpostoViewModel();
        m.CalculoIssqn = CalculoIssqnViewModel();

        m.Duplicatas = new List<DuplicataInfo>();

        for (int i = 1; i <= 10; i++)
        {
            var d = new DuplicataInfo()
            {
                Numero = i.ToString(),
                Valor = i * Math.PI,
                Vecimento = new DateTime(9999, 12, 30)
            };

            m.Duplicatas.Add(d);
        }

        m.Produtos = new List<ProdutoInfo>();

        for (int i = 1; i <= 100; i++)
        {
            var p = new ProdutoInfo()
            {
                Descricao = $"Produto da linha {i}",
                Codigo = i.ToString(),
                Quantidade = i * Math.PI * 10,
                AliquotaIcms = 99.88,
                Unidade = "PEC",
                Ncm = new string('8', 8)
            };

            if(i % 10 == 0)
            {
                p.Descricao = string.Concat(Enumerable.Repeat(p.Descricao + " ", 15));
            }

            m.Produtos.Add(p);
        }


        return m;
    }



}
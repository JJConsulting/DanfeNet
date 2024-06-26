﻿using DanfeNet.Elementos;
using DanfeNet.Models;

namespace DanfeNet.Blocos;

internal class BlocoCalculoIssqn : BlocoBase
{
    public BlocoCalculoIssqn(Danfe viewModel, Estilo estilo) : base(viewModel, estilo)
    {
        var m = viewModel.CalculoIssqn;

        AdicionarLinhaCampos()
            .ComCampo("INSCRIÇÃO MUNICIPAL", m.InscricaoMunicipal, AlinhamentoHorizontal.Centro)
            .ComCampoNumerico("VALOR TOTAL DOS SERVIÇOS", m.ValorTotalServicos)
            .ComCampoNumerico("BASE DE CÁLCULO DO ISSQN", m.BaseIssqn)
            .ComCampoNumerico("VALOR TOTAL DO ISSQN", m.ValorIssqn)
            .ComLargurasIguais();
    }

    public override PosicaoBloco Posicao => PosicaoBloco.Base;
    public override string Cabecalho => "CÁLCULO DO ISSQN";
}
﻿using System;
using DanfeNet.Atributos;
using DanfeNet.Graphics;

namespace DanfeNet.Elementos;

[AlturaFixa]
internal class Duplicata : ElementoBase
{
    public Fonte FonteA { get; private set; }
    public Fonte FonteB { get; private set; }
    public Models.DuplicataInfo ViewModel { get; private set; }

    private static readonly string[] Chaves = { "Número", "Vencimento:", "Valor:" };

    public Duplicata(Estilo estilo, Models.DuplicataInfo viewModel) : base(estilo)
    {
        ViewModel = viewModel;
        FonteA = estilo.CriarFonteRegular(7.5F);
        FonteB = estilo.CriarFonteNegrito(7.5F);
    }

    public override void Draw(Gfx gfx)
    {
        base.Draw(gfx);

        var r = BoundingBox.InflatedRetangle(Estilo.PaddingSuperior, Estilo.PaddingInferior, Estilo.PaddingHorizontal);

        string[] valores = { ViewModel.Numero, ViewModel.Vecimento.Formatar(), ViewModel.Valor.FormatarMoeda() };

        for (int i = 0; i < Chaves.Length; i++)
        {
            gfx.DrawString(Chaves[i], r, FonteA, AlinhamentoHorizontal.Esquerda);
            gfx.DrawString(valores[i], r, FonteB, AlinhamentoHorizontal.Direita);
            r = r.CutTop(FonteB.AlturaLinha);
        }    

    }

    public override float Height 
    {
        get => 3*FonteB.AlturaLinha + Estilo.PaddingSuperior + Estilo.PaddingInferior;
        set => throw new NotSupportedException();
    }
}
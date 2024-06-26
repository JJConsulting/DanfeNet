﻿using System.Drawing;
using DanfeNet.Elementos;
using DanfeNet.Models;
using org.pdfclown.documents.contents.composition;
using org.pdfclown.documents.contents.xObjects;

namespace DanfeNet.Blocos.NFC;

internal class BlocoIdentificacaoEmitenteNFC : BlocoBase
{
    public const float LarguraCampoChaveNFe = 93F;
    public const float AlturaLinha1 = 30;

    NumeroNfSerie2 ifdNfe;
    IdentificacaoEmitente idEmitente;

    public BlocoIdentificacaoEmitenteNFC(Danfe viewModel, Estilo estilo, PrimitiveComposer primitiveComposer) : base(viewModel, estilo)
    {
        primitiveComposer.BeginLocalState();
        primitiveComposer.SetFont(estilo.FonteCampoTituloNegrito.FonteInterna, estilo.FonteCampoTituloNegrito.Tamanho);
        int y = 0;

        var emitente = viewModel.Emitente;

        if (emitente.RazaoSocial.Length > 39)
        {
            primitiveComposer.ShowText(emitente.RazaoSocial.Substring(0, 39), new PointF(140, 10), XAlignmentEnum.Center, YAlignmentEnum.Top, 0);
            primitiveComposer.ShowText(emitente.RazaoSocial.Substring(39), new PointF(140, 20), XAlignmentEnum.Center, YAlignmentEnum.Top, 0);

            primitiveComposer.ShowText($"CNPJ - {Formatador.FormatarCnpj(emitente.CnpjCpf)}", new PointF(140, 30), XAlignmentEnum.Center, YAlignmentEnum.Top, 0);

            y = 30;
        }
        else
        {
            primitiveComposer.ShowText(emitente.RazaoSocial, new PointF(140, 10), XAlignmentEnum.Center, YAlignmentEnum.Top, 0);
            primitiveComposer.ShowText($"CNPJ - {Formatador.FormatarCnpj(emitente.CnpjCpf)}", new PointF(140, 20), XAlignmentEnum.Center, YAlignmentEnum.Top, 0);

            y = 20;
        }

        primitiveComposer.SetFont(estilo.FonteCampoConteudoNegrito.FonteInterna, estilo.FonteCampoConteudoNegrito.Tamanho);

        if (!string.IsNullOrWhiteSpace(emitente.EnderecoLogadrouro) &&
            !string.IsNullOrWhiteSpace(emitente.EnderecoNumero) &&
            !string.IsNullOrWhiteSpace(emitente.EnderecoBairro) &&
            !string.IsNullOrWhiteSpace(emitente.Municipio))
        {
            if (emitente.EnderecoLogadrouro.Length >= 25)
                primitiveComposer.ShowText($"{emitente.EnderecoLogadrouro.Substring(0, 25)}, {emitente.EnderecoNumero} - {emitente.EnderecoBairro} - {emitente.Municipio} - {emitente.EnderecoUf}",
                    new PointF(140, y + 10), XAlignmentEnum.Center, YAlignmentEnum.Top, 0);
            else
                primitiveComposer.ShowText($"{emitente.EnderecoLogadrouro}, {emitente.EnderecoNumero} - {emitente.EnderecoBairro} - {emitente.Municipio} - {emitente.EnderecoUf}",
                    new PointF(140, y + 10), XAlignmentEnum.Center, YAlignmentEnum.Top, 0);
        }
        primitiveComposer.DrawLine(new PointF(15, y + 20), new PointF(265, y + 20));
        primitiveComposer.SetLineDash(new org.pdfclown.documents.contents.LineDash(new double[] { 3, 2 }));
        primitiveComposer.Stroke();
        primitiveComposer.End();

        Y_NFC = y + 20;
    }

    public XObject Logo
    {
        get => idEmitente.Logo;
        set => idEmitente.Logo = value;
    }

    public override PosicaoBloco Posicao => PosicaoBloco.Topo;
    public override bool VisivelSomentePrimeiraPagina => false;
}
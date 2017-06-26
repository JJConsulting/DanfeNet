﻿using DanfeSharp.Esquemas.NFe;
using DanfeSharp.Modelo;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DanfeSharp
{
    public static class DanfeViewModelCreator
    {
        private static EmpresaViewModel CreateEmpresaFrom(Empresa empresa)
        {
            EmpresaViewModel model = new EmpresaViewModel();

            model.Nome = empresa.xNome;
            model.CnpjCpf = !String.IsNullOrWhiteSpace(empresa.CNPJ) ? empresa.CNPJ : empresa.CPF;
            model.Ie = empresa.IE;
            model.IeSt = empresa.IEST;

            var end = empresa.Endereco;

            if (end != null)
            {
                model.EnderecoLogadrouro = end.xLgr;
                model.EnderecoNumero = end.nro;
                model.EnderecoBairro = end.xBairro;
                model.Municipio = end.xMun;
                model.EnderecoUf = end.UF;
                model.EnderecoCep = end.CEP;
                model.Telefone = end.fone;
                model.Email = empresa.email;
            }

            if (empresa is Emitente)
            {
                var emit = empresa as Emitente;
                model.IM = emit.IM;
            }

            return model;
        }

        internal static DanfeViewModel CreateFromXmlString(String xml)
        {
            ProcNFe nfe = null;
            XmlSerializer serializer = new XmlSerializer(typeof(ProcNFe));

            try
            {
                using (TextReader reader = new StringReader(xml))
                {
                    nfe = (ProcNFe)serializer.Deserialize(reader);
                }

                return CreateFromXml(nfe);
            }
            catch (System.InvalidOperationException e)
            {
                throw new Exception("Não foi possível interpretar o texto Xml.", e);
            }
        }

        internal static DanfeViewModel CreateFromXmlFile(String path)
        {
            if (String.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("O arquivo Xml precisa ser especificado.");
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("O arquivo Xml não foi encontrado.", path);
            }

            ProcNFe nfe = null;
            XmlSerializer serializer = new XmlSerializer(typeof(ProcNFe));

            try
            {
                using (StreamReader reader = new StreamReader(path, true))
                {
                    nfe = (ProcNFe)serializer.Deserialize(reader);
                }

                return CreateFromXml(nfe);
            }
            catch (System.InvalidOperationException e)
            {
                if (e.InnerException is XmlException)
                {
                    XmlException ex = (XmlException)e.InnerException;
                    throw new Exception(String.Format("Não foi possível interpretar o Xml. Linha {0} Posição {1}.", ex.LineNumber, ex.LinePosition));
                }

                throw new XmlException("O Xml não parece ser uma NF-e processada.", e);
            }
        }

        public static void ExtrairDatas(DanfeViewModel model, InfNFe infNfe)
        {
            var ide = infNfe.ide;

            if (infNfe.Versao.Maior >= 3)
            {
                model.DataHoraEmissao = ide.dhEmi;
                model.DataSaidaEntrada = ide.dhSaiEnt;

                if (model.DataSaidaEntrada.HasValue)
                {
                    model.HoraSaidaEntrada = model.DataSaidaEntrada.Value.TimeOfDay;
                }
            }
            else
            {
                model.DataHoraEmissao = ide.dEmi;
                model.DataSaidaEntrada = ide.dSaiEnt;

                if (!String.IsNullOrWhiteSpace(ide.hSaiEnt))
                {
                    model.HoraSaidaEntrada = TimeSpan.Parse(ide.hSaiEnt);
                }

            }
        }

        public static CalculoImpostoViewModel CriarCalculoImpostoViewModel(ICMSTotal i)
        {
            return new CalculoImpostoViewModel()
            {
                ValorAproximadoTributos = i.vTotTrib,
                BaseCalculoIcms = i.vBC,
                ValorIcms = i.vICMS,
                BaseCalculoIcmsSt = i.vBCST,
                ValorIcmsSt = i.vST,
                ValorTotalProdutos = i.vProd,
                ValorFrete = i.vFrete,
                ValorSeguro = i.vSeg,
                Desconto = i.vDesc,
                ValorII = i.vII,
                ValorIpi = i.vIPI,
                ValorPis = i.vPIS,
                ValorCofins = i.vCOFINS,
                OutrasDespesas = i.vOutro,
                ValorTotalNota = i.vNF,
                vFCPUFDest = i.vFCPUFDest,
                vICMSUFDest = i.vICMSUFDest,
                vICMSUFRemet = i.vICMSUFRemet
            };
        }

        public static DanfeViewModel CreateFromXml(ProcNFe procNfe)
        {
            DanfeViewModel model = new DanfeViewModel();

            var nfe = procNfe.NFe;
            var infNfe = nfe.infNFe;
            var ide = infNfe.ide;     

            if (ide.mod != 55)
            {
                throw new Exception("Somente o mod==55 está implementado.");
            }

            if (ide.tpEmis != FormaEmissao.Normal)
            {
                throw new Exception("Somente o tpEmis==1 está implementado.");
            }

            var infProt = procNfe.protNFe.infProt;
            model.CodigoStatusReposta = infProt.cStat;
            model.DescricaoStatusReposta = infProt.xMotivo;

            model.TipoAmbiente = (int)ide.tpAmb;
            model.NfNumero = ide.nNF;
            model.NfSerie = ide.serie;
            model.NaturezaOperacao = ide.natOp;
            model.ChaveAcesso = procNfe.NFe.infNFe.Id.Substring(3);
            model.TipoNF = (int)ide.tpNF;

            model.Emitente = CreateEmpresaFrom(infNfe.emit);
            model.Destinatario = CreateEmpresaFrom(infNfe.dest);

            // Informações adicionais de compra
            if(infNfe.compra != null)
            {
                StringBuilder sb = new StringBuilder();

                if (!String.IsNullOrWhiteSpace(infNfe.compra.xNEmp))
                    sb.Append("Nota de Empenho: ").Append(infNfe.compra.xNEmp);

                if (!String.IsNullOrWhiteSpace(infNfe.compra.xPed))
                    sb.Append("Pedido: ").Append(infNfe.compra.xPed);

                if (!String.IsNullOrWhiteSpace(infNfe.compra.xCont))
                    sb.Append("Contrato: ").Append(infNfe.compra.xCont);

                if (sb.Length > 0) model.InformacaoCompra = sb.ToString();
            }

            foreach (var det in infNfe.det)
            {
                ProdutoViewModel produto = new ProdutoViewModel();
                produto.Codigo = det.prod.cProd;
                produto.Descricao = det.prod.xProd;
                produto.Ncm = det.prod.NCM;
                produto.Cfop = det.prod.CFOP;
                produto.Unidade = det.prod.uCom;
                produto.Quantidade = det.prod.qCom;
                produto.ValorUnitario = det.prod.vUnCom;
                produto.ValorTotal = det.prod.vProd;
                produto.InformacoesAdicionais = det.infAdProd;

                var imposto = det.imposto;

                if (imposto != null)
                {
                    if (imposto.ICMS != null)
                    {
                        var icms = imposto.ICMS.ICMS;

                        if (icms != null)
                        {
                            produto.ValorIcms = icms.vICMS;
                            produto.BaseIcms = icms.vBC;
                            produto.AliquotaIcms = icms.pICMS;
                            produto.OCst = icms.orig + icms.CST + icms.CSOSN;
                        }
                    }

                    if (imposto.IPI != null)
                    {
                        var ipi = imposto.IPI.IPITrib;

                        if (ipi != null)
                        {
                            produto.ValorIpi = ipi.vIPI;
                            produto.AliquotaIpi = ipi.pIPI;
                        }
                    }
                }

                model.Produtos.Add(produto);
            }

            if (infNfe.cobr != null)
            {
                foreach (var item in infNfe.cobr.dup)
                {
                    DuplicataViewModel duplicata = new DuplicataViewModel();
                    duplicata.Numero = item.nDup;
                    duplicata.Valor = item.vDup;
                    duplicata.Vecimento = item.dVenc;

                    model.Duplicatas.Add(duplicata);
                }
            }   
            
            model.CalculoImposto = CriarCalculoImpostoViewModel(infNfe.total.ICMSTot);           
            
            var issqnTotal = infNfe.total.ISSQNtot;

            if (issqnTotal != null)
            {
                var c = model.CalculoIssqn;
                c.InscricaoMunicipal = infNfe.emit.IM;
                c.BaseIssqn = issqnTotal.vBC;
                c.ValorTotalServicos = issqnTotal.vServ;
                c.ValorIssqn = issqnTotal.vISS;
                c.Mostrar = true;
            }

            var transp = infNfe.transp;
            var transportadora = transp.transporta;
            var transportadoraModel = model.Transportadora;

            transportadoraModel.ModalidadeFrete = (int)transp.modFrete;

            if (transp.veicTransp != null)
            {
                transportadoraModel.VeiculoUf = transp.veicTransp.UF;
                transportadoraModel.CodigoAntt = transp.veicTransp.RNTC;
                transportadoraModel.Placa = transp.veicTransp.placa;
            }

            if (transportadora != null)
            {
                transportadoraModel.Nome = transportadora.xNome;
                transportadoraModel.EnderecoUf = transportadora.UF;
                transportadoraModel.CnpjCpf = !String.IsNullOrWhiteSpace(transportadora.CNPJ) ? transportadora.CNPJ : transportadora.CPF;
                transportadoraModel.EnderecoLogadrouro = transportadora.xEnder;
                transportadoraModel.Municipio = transportadora.xMun;
                transportadoraModel.Ie = transportadora.IE;
            }


            var vol = transp.vol.FirstOrDefault();

            if (vol != null)
            {
                transportadoraModel.QuantidadeVolumes = vol.qVol;
                transportadoraModel.Especie = vol.esp;
                transportadoraModel.Marca = vol.marca;
                transportadoraModel.Numeracao = vol.nVol;
                transportadoraModel.PesoBruto = vol.pesoB;
                transportadoraModel.PesoLiquido = vol.pesoL;
            }


            var infAdic = infNfe.infAdic;
            if (infAdic != null)
            {
                model.InformacoesComplementares = procNfe.NFe.infNFe.infAdic.infCpl;
                model.InformacoesAdicionaisFisco = procNfe.NFe.infNFe.infAdic.infAdFisco;
            }

            var infoProto = procNfe.protNFe.infProt;

            model.ProtocoloAutorizacao = String.Format("{0} - {1}", infoProto.nProt, infoProto.dhRecbto);

            ExtrairDatas(model, infNfe);

            return model;
        }

    }
}
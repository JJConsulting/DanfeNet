﻿using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using DanfeNet.Esquemas;
using DanfeNet.Mappers;
using DanfeNet.Models;

namespace DanfeNet;

public static class DanfeFactory
{
    /// <summary>
    /// Cria o modelo a partir de uma string xml.
    /// </summary>
    public static Danfe FromXmlString(string str)
    {
        if (str == null) 
            throw new ArgumentNullException(nameof(str));

        using (var sr = new StringReader(str))
        {
            return FromStream(sr);
        }
    }

    public static Danfe FromStream(TextReader reader)
    {
        ProcNFe nfe = null;

        try
        {
            var procNFeSerializer = new XmlSerializer(typeof(ProcNFe));
            nfe = (ProcNFe)procNFeSerializer.Deserialize(reader);
            return FromNFe(nfe);
        }
        catch (InvalidOperationException e)
        {
            if (e.InnerException is XmlException ex)
            {
                throw new XmlException(string.Format("Não foi possível interpretar o Xml. Linha {0} Posição {1}.", ex.LineNumber, ex.LinePosition), e.InnerException, ex.LineNumber, ex.LinePosition);
            }

            throw new XmlException("O Xml não parece ser uma NF-e processada.", e);
        }
    }
    
    /// <summary>
    /// Cria o modelo a partir de um arquivo xml.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Danfe FromXmlFilePath(string path)
    {
        using (var sr = new StreamReader(path, true))
        {
            return FromStream(sr);
        }
    }
    
    public static Danfe FromNFe(ProcNFe procNfe)
    {
        return DanfeMapper.CreateNFe(procNfe);
    }
    
    


    
}
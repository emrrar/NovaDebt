﻿using AutoMapper;
using NovaDebt.Models;
using NovaDebt.Models.DTOs;
using NovaDebt.Models.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NovaDebt
{
    public static class XmlProcess
    {
        private const string PathCannotBeNullErrorMessage = "Path cannot be null.";
        private const string FileDoesntExistErrorMessage = "File doesn't exist.";

        public static IEnumerable<Transactor> DeserializeXml(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(PathCannotBeNullErrorMessage);
            }
            if (!File.Exists(path))
            {
                throw new InvalidOperationException(FileDoesntExistErrorMessage);
            }

            string xmlText = File.ReadAllText(path);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(TransactorDTO[]),
                                          new XmlRootAttribute("Transactors"));

            TransactorDTO[] transactorDTOs = (TransactorDTO[])xmlSerializer.Deserialize(new StringReader(xmlText));

            Transactor[] transactors = Mapper.Map<Transactor[]>(transactorDTOs).ToArray();

            // Setting the Id manually since both debtors and transactors are stored in 1 file
            // Which maens that the Id will be different
            // I dont want Ids (№) like 2, 5, 6, 9 on the debtors/creditors list when they show up.
            // That's why the Id setting becomes necessary.
            // Altought I'm planning to implement new propety just for the enumeration and leave the Id alone.
            for (int i = 0; i < transactors.Length; i++)
            {
                transactors[i].No = i + 1;
            }

            return transactors;
        }
        
        public static IEnumerable<Transactor> DeserializeXmlWithTransactorType(string path, TransactorType transactorType)
        {
            if (path == null)
            {
                throw new ArgumentNullException(PathCannotBeNullErrorMessage);
            }
            if (!File.Exists(path))
            {
                throw new InvalidOperationException(FileDoesntExistErrorMessage);
            }

            string xmlText = File.ReadAllText(path);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(TransactorDTO[]),
                                          new XmlRootAttribute("Transactors"));

            TransactorDTO[] transactorDTOs = (TransactorDTO[])xmlSerializer.Deserialize(new StringReader(xmlText));

            Transactor[] transactors = Mapper.Map<Transactor[]>(transactorDTOs)
                                       .Where(t => t.TransactorType.ToLower() == transactorType.ToString().ToLower())
                                       .ToArray();

            // Setting the Id manually since both debtors and transactors are stored in 1 file
            // Which maens that the Id will be different
            // I dont want Ids ("№" as in the table) like 2, 5, 6, 9 on the debtors/creditors list when they show up.
            // That's why the Id setting becomes necessary.
            // Altought I'm planning to implement new propety just for the enumeration and leave the Id alone.
            for (int i = 0; i < transactors.Length; i++)
            {
                transactors[i].No = i + 1;
            }

            return transactors;
        }

        public static void SerializeXmlWithTransactors(string path, IEnumerable<Transactor> transactors)
        {
            StringBuilder result = new StringBuilder();

            if (path == null)
            {
                throw new ArgumentNullException(PathCannotBeNullErrorMessage);
            }
            if (!File.Exists(path))
            {
                File.Create(path);
            }
            else
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(TransactorDTO[]),
                                              new XmlRootAttribute("Transactors"));

                // Removing only the unnecessary namespace headers.
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);

                TransactorDTO[] transactorDTOs = Mapper.Map<TransactorDTO[]>(transactors).ToArray();

                using (StringWriter writer = new StringWriter(result))
                {
                    // TODO: It will be a good idea to serialize without spaces, line breaks.
                    xmlSerializer.Serialize(writer, transactorDTOs, namespaces);
                }

                File.WriteAllText(path, result.ToString());
            }
        }
    }
}

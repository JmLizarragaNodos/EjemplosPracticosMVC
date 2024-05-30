using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Linq;
using System.Text;
using OfficeOpenXml;
using System;
using System.Data;
using ExcelDataReader;
using System.Xml;

//==================================================>>>>
// Excel a XML:
// Instalar  ExcelDataReader            3.6.0
// Instalar  ExcelDataReader.DataSet    3.6.0
//==================================================>>>>

namespace Web.Helpers
{
    public static class ExcelHelper
    {
        public static List<string> ObtenerListaCabecera(HttpPostedFile archivo)
        {
            List<string> listaCabecera = new List<string>();

            if (archivo.ContentType == "text/csv" || archivo.ContentType == "application/vnd.ms-excel")  // Si es CSV
            {
                using (var memoryStream = new MemoryStream())
                {
                    archivo.InputStream.CopyTo(memoryStream);
                    memoryStream.Position = 0; // Reiniciar el índice de lectura

                    using (var reader = new StreamReader(memoryStream, Encoding.GetEncoding("Windows-1252")))
                    {
                        // Leer la primera línea del archivo CSV
                        var primeraFila = reader.ReadLine();

                        if (!string.IsNullOrEmpty(primeraFila))
                        {
                            // Dividir la línea en campos utilizando el delimitador adecuado
                            string[] campos = primeraFila.Split(';');

                            foreach (var campo in campos)
                            {
                                listaCabecera.Add(campo);
                            }
                        }
                    }
                }
            }
            else if (archivo.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") // Si es Excel
            {
                using (var memoryStream = new MemoryStream())
                {
                    archivo.InputStream.CopyTo(memoryStream);
                    memoryStream.Position = 0; // Reiniciar el índice de lectura

                    using (var package = new ExcelPackage(memoryStream))
                    {
                        var worksheet = package.Workbook.Worksheets.First(); // Obtener la primera hoja de cálculo

                        if (worksheet.Dimension != null && worksheet.Dimension.End.Row >= 1)
                        {
                            var firstRowCells = worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column]; // Obtener todas las celdas de la primera fila

                            foreach (var cell in firstRowCells)
                            {
                                var value = cell.Value?.ToString(); // Obtener el valor de la celda

                                listaCabecera.Add(value);
                            }
                        }
                    }
                }
            }
            //else if (archivo.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            //{
            //    using (var stream = archivo.InputStream)
            //    using (var package = new OfficeOpenXml.ExcelPackage(stream))
            //    {
            //        var worksheet = package.Workbook.Worksheets.First(); // Obtener la primera hoja de cálculo

            //        if (worksheet.Dimension != null && worksheet.Dimension.End.Row >= 1)
            //        {
            //            var firstRowCells = worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column]; // Obtener todas las celdas de la primera fila

            //            foreach (var cell in firstRowCells)
            //            {
            //                var value = cell.Value?.ToString(); // Obtener el valor de la celda

            //                listaCabecera.Add(value);
            //            }
            //        }
            //    }
            //}

            return listaCabecera;
        }

        public static string ExcelAsXml(Stream stream, string contentType, ref string path, ref string columns)
        {
            string strXmlOut = string.Empty;
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(ExcelAsXml(stream, contentType));
                strXmlOut = xmlDocument.InnerXml;
                foreach (XmlNode xmlNode in xmlDocument.ChildNodes)
                {
                    path = "/" + xmlNode.Name + "/";
                    foreach (XmlNode node in xmlNode.ChildNodes)
                    {
                        path += node.Name;
                        foreach (XmlNode col in node.ChildNodes)
                        {
                            columns += col.Name + ",";
                        }
                        break;
                    }
                    break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return strXmlOut;
        }

        public static string ExcelAsXml(Stream stream, string contentType)
        {
            DataSet dataSet = null;
            string strXmlOut = string.Empty;
            try
            {
                dataSet = ExcelAsDataSet(stream, contentType);
                using (MemoryStream ms = new MemoryStream())
                {
                    dataSet.Tables[0].WriteXml(ms);
                    XmlDocument xmlDocument = new XmlDocument();
                    ms.Position = 0;
                    xmlDocument.Load(ms);
                    strXmlOut = xmlDocument.InnerXml;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return strXmlOut;
        }

        public static DataSet ExcelAsDataSet(Stream stream, string contentType)
        {
            DataSet dataSet = null;

            try
            {
                // if (contentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")   // Es xlsx
                // if (contentType == "text/csv")                   // Es csv en Edge
                // if (contentType == "application/vnd.ms-excel")   // Es csv en Firefox  

                using (
                    IExcelDataReader excelDataReader = (contentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") ?
                        ExcelReaderFactory.CreateOpenXmlReader(stream) :        // Si es excel 
                            (contentType == "text/csv") ?
                                ExcelReaderFactory.CreateCsvReader(stream)      // Si es csv
                                : ExcelReaderFactory.CreateCsvReader(stream)    // Default
                )
                {
                    try
                    {
                        ExcelDataSetConfiguration excelDataSetConfiguration = new ExcelDataSetConfiguration()
                        {
                            ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true
                            }
                        };

                        dataSet = excelDataReader.AsDataSet(excelDataSetConfiguration);


                        foreach (DataTable table in dataSet.Tables)             // Iterar a través de cada tabla en el DataSet
                        {
                            foreach (DataRow row in table.Rows)                 // Iterar a través de cada fila en la tabla
                            {
                                for (int i = 0; i < table.Columns.Count; i++)   // Iterar a través de cada columna en la fila
                                {
                                    if (row[i] is double doubleValue)           // Verificar si el valor de la celda es un double
                                    {
                                        decimal roundedValue = decimal.Round((decimal)doubleValue, 2);  // Convertir el valor double a decimal y redondearlo
                                        row[i] = roundedValue;                                          // Asignar el valor redondeado a la celda
                                    }
                                }
                            }
                        }
                    }
                    catch (ExcelDataReader.Exceptions.ExcelReaderException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        excelDataReader.Close();
                        excelDataReader.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            //var stringDataSet = ConvertToDataSetOfStrings(dataSet);
            //return stringDataSet;

            return dataSet;
        }
    }
}
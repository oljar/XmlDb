using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ListViewTest
{
    public class MyData
    {
        /// <summary>
        /// Saves items to MyData.xml file in bin folder.
        /// </summary>
        /// <param name="items"></param>
        public void Save(System.Windows.Data.CollectionView items)  // JO  items to zmienna , Sys...to klasa - tylko def metody.
        // Save odpowiada za zapis
        { 
            XDocument xdoc = new XDocument();

            XElement xeRoot = new XElement("Data");  // JO definicja nowej instancji Węzeła Głównego Data
            XElement xeSubRoot = new XElement("Rows"); // JO definicja nowej instancji podwęzeła .

            foreach (var item in items)
            {
                ListViewData lvc = (ListViewData)item;

                XElement xRow = new XElement("Row"); // JO definicja nowej instancji elementu xRow
                xRow.Add(new XElement("col1", lvc.Col1));// JO Dodanie kolumny 01 do elementu xROW))
                xRow.Add(new XElement("col2", lvc.Col2)); // JO dodanie kolumny 02 do elementu xROW)

                xeSubRoot.Add(xRow); // JO Dodanie  elementu do podwęzła 
            }
            xeRoot.Add(xeSubRoot); //  JO Dodanie podwęzła do węzła 
            xdoc.Add(xeRoot); // JO Dodanie węzłą do dokumentu

            xdoc.Save("MyData.xml"); // Zapis dokumentu.
        }

        /// <summary>
        /// Gets data from MyData.xml as rows.  
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetRows()
        {
            // odowiada za odczyt
            List<ListViewData> rows = new List<ListViewData>(); // JO nowa instancja kolekcji 

            if (File.Exists("MyData.xml"))
            {
                // Create the query --- JO zapytanie
                var rowsFromFile = from c in XDocument.Load(
                            "MyData.xml").Elements(
                            "Data").Elements("Rows").Elements("Row") // wskazuje na najmniejszy element ROW
                                   select c;

                // Execute the query ---- JO wykonanie zaytania
                foreach (var row in rowsFromFile)
                {
                    rows.Add(new ListViewData(row.Element("col1").Value,
                            row.Element("col2").Value));
                }
            }
            return rows;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Xml;

namespace TablePet.Win.CustomCon
{
    public class DocumentConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            { 
                string data = value.ToString();
                if (!string.IsNullOrEmpty(data))
                {
                    Stream s = new MemoryStream(Encoding.UTF8.GetBytes(data));
                    FlowDocument doc = XamlReader.Load(s) as FlowDocument;
                    s.Close();
                    return doc;
                }
            } catch { }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            FlowDocument document = value as FlowDocument;

            if (document != null)
            {
                Stream s = new MemoryStream();
                XamlWriter.Save(document, s);
                StreamReader sr = new StreamReader(s);
                s.Position = 0;
                string data = sr.ReadToEnd();
                sr.Close();
                return data;
            }
            return null;
        }

        #endregion
    }
}

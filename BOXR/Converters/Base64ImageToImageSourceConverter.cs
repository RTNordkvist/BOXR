using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BOXR.UI.Converters
{
    [ValueConversion(typeof(string), typeof(ImageSource))]
    public class Base64ImageToImageSourceConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string base64 = value?.ToString();

            if (base64 == null)
            {
                byte[] imageArray = System.IO.File.ReadAllBytes(Directory.GetCurrentDirectory() + "/Resources/DefaultProfileImage.png");
                base64 = System.Convert.ToBase64String(imageArray);
            }

            byte[] binaryData = System.Convert.FromBase64String(base64);

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = new MemoryStream(binaryData);
            bi.EndInit();

            return bi;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}

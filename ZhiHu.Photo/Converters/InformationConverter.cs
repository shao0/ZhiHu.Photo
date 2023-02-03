using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ZhiHu.Photo.Models;

namespace ZhiHu.Photo.Converters
{
    public class InformationConverter : DataTemplateSelector
    {
        public DataTemplate DataTemplateText { get; set; }

        public DataTemplate DataTemplateImage { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var result = base.SelectTemplate(item, container);
            if (item is Information info)
            {
                switch (info.InfoType)
                {
                    case InfoType.Text:
                        result = DataTemplateText;
                        break;
                    case InfoType.Gif:
                    case InfoType.Image:
                    case InfoType.Video:
                        result = DataTemplateImage;
                        break;
                }

            }


            return result;
        }
    }
}

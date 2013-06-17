using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2NET_Gui.Extensions
{
    public static class ExtensionsMethods
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumeration)
        {
            return new ObservableCollection<T>(enumeration);
        }
    }
}

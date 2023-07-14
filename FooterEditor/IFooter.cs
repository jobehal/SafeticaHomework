using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FooterEditor
{
    public interface IFooter
    {
        void AddProperty(string name, string value);
        void RemoveProperty(string name);
        void EditPropety(string name, string value);
        string ToString();
    }
}

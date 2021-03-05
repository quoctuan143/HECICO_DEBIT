using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APP_GACH_NO.Interface
{
    public interface IProcessLoader
    {
        void  Hide();
        void  Show(string title = "Loading");
    }
}

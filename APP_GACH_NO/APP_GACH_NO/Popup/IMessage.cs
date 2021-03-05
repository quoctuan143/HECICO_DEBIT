using System;
using System.Collections.Generic;
using System.Text;

namespace APP_GACH_NO.Popup
{
   public interface IMessage
    {
        void LongAlert(string message);
        void ShortAlert(string message);
    }
}

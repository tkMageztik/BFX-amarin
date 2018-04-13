using System;
using System.Collections.Generic;
using System.Text;

namespace NS.MBX_amarin.Six
{
    public interface ISixService
    {
        string SendMessage(string Trx, short Length, string Message);
        string TransferenciaCtaPropia(string Trx, short Length, string Message);
    }
}

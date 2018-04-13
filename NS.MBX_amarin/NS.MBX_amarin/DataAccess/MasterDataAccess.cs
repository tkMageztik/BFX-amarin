using System;
using System.Collections.Generic;
using System.Text;

namespace NS.MBX_amarin.DataAccess
{
    public class MasterDataAccess
    {
        public string dbHomeBanking;
        public string dbHomeBankingCE;

        public MasterDataAccess()
        {
            dbHomeBanking = "";// System.Configuration.ConfigurationManager.ConnectionStrings["HomeBanking"].ConnectionString;
            dbHomeBankingCE = "";//System.Configuration.ConfigurationManager.ConnectionStrings["HomeBankingCE"].ConnectionString;
        }
    }
}

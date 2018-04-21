using System;
using System.Collections.Generic;
using System.Text;

namespace NS.MBX_amarin.Common
{
    public class MasterDataAccessMBX
    {
        public string dbHomeBanking;
        public string dbHomeBankingCE;

        public MasterDataAccessMBX()
        {
            dbHomeBanking = "";// System.Configuration.ConfigurationManager.ConnectionStrings["HomeBanking"].ConnectionString;
            dbHomeBankingCE = "";// System.Configuration.ConfigurationManager.ConnectionStrings["HomeBankingCE"].ConnectionString;
        }
    }
}

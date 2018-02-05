using NS.MBX.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NS.MBX.Web.Data
{
    public static class DbInitializer
    {
        public static void Initialize(NSMBXWebContext context)
        {
            context.Database.EnsureCreated();

            if (context.TipoDocumentoViewModel.Any())
            {
                return;
            }

            var tipDocs = new TipoDocumentoViewModel[]
            {
                new TipoDocumentoViewModel{ TipDoc = "DNI", DesDoc="DNI"},
                new TipoDocumentoViewModel{ TipDoc = "Pasaporte", DesDoc="Pasaporte"},
                new TipoDocumentoViewModel{ TipDoc="CE", DesDoc="Carné de extranjería"}
            };
            foreach (TipoDocumentoViewModel s in tipDocs)
            {
                context.TipoDocumentoViewModel.Add(s);
            }
            context.SaveChanges();
        }

    }
}

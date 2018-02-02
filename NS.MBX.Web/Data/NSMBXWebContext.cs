using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NS.MBX.Web.Models
{
    public class NSMBXWebContext : DbContext
    {
        public NSMBXWebContext (DbContextOptions<NSMBXWebContext> options)
            : base(options)
        {
        }

        public DbSet<NS.MBX.Web.Models.TipoDocumentoViewModel> TipoDocumentoViewModel { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NS.MBX.Web.Models;

namespace NS.MBX.Web.Data
{
    public class NSMBXWebContext : DbContext
    {
        public NSMBXWebContext (DbContextOptions<NSMBXWebContext> options)
            : base(options)
        {
        }
        public DbSet<TipoDocumentoViewModel> TipoDocumentoViewModel { get; set; }
    }
}

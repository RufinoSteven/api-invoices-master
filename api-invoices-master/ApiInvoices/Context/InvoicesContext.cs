using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ApiInvoices.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiInvoices.Context
{
    public class InvoicesContext :DbContext
    {
        public InvoicesContext(DbContextOptions<InvoicesContext> optrions) : base(optrions)
        {


        }
        public DbSet<Invoices> Invoices { get; set; }
        public DbSet<Detalle> Detalles { get; set; }
    }
}

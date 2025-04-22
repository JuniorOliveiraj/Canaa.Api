using Canaa.DataContracts.Auth.Context;
using Canaa.Infra.ExternalServices.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canaa.Infra.ExternalServices.Context
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IUserContext _userContext;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IUserContext userContext)
            : base(options)
        {
            _userContext = userContext;
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Usuario> Ususario { get; set; } 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
         
        }
    }

 
}

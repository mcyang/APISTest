﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace APISTest.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class JohnTestEntities : DbContext
    {
        public JohnTestEntities()
            : base("name=JohnTestEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<CustomerTeam> CustomerTeams { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<ProductGroup> ProductGroups { get; set; }
        public virtual DbSet<RDR> RDRs { get; set; }
        public virtual DbSet<CarMaker> CarMakers { get; set; }
    }
}

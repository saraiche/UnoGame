﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UnoEntitys
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class unoDbModelContainer : DbContext
    {
        public unoDbModelContainer()
            : base("name=unoDbModelContainer")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Player> PlayerSet1 { get; set; }
        public virtual DbSet<Credentials> CredentialsSet1 { get; set; }
        public virtual DbSet<Images> ImagesSet1 { get; set; }
    }
}

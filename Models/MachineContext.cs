using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Machinetest.Models
{
    public class MachineContext :DbContext
    {
        public MachineContext() : base("DefaultConnection") 
        {
        }
        public DbSet<Category> Categories { get; set; }     

        public DbSet<Product> Products { get; set; }    
    }
}
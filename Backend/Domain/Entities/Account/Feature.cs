using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.Product;

namespace Domain.Entities.Account
{
    public class Feature
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public int TypePrivilegeId { get; set; }
        public TypePrivilege TypePrivilege { get; set; }
    }
}

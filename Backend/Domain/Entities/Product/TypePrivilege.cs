using System.Collections.Generic;
using Domain.Entities.Account;
using Domain.Purchase;

namespace Domain.Entities.Product
{
    public class TypePrivilege : Product
    {
        public int Id { get; set; }

        public string GroupName { get; set; }
        
        public int Lvl { get; set; }
        
        public string ImageSource { get; set; }
        
        public List<Feature> Features { get; set; }
        
        public bool Sale { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Product;
using Domain.Entities.Account;

namespace Domain.Entities.Purchase
{
    public class Payment
    {
        public int Id { get; set; }
        public string BillId { get; set; }

        public int TypePrivilegeId { get; set; }
        public TypePrivilege TypePrivilege { get; set; }

        public int PrivilegeId { get; set; }
        public Privilege.Privilege Privilege { get; set; }

        public decimal Value { get; set; }
        public string Phone { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}

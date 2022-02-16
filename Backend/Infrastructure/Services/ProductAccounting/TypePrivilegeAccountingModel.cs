using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.Privilege;
using Domain.Entities.Product;
using Infrastructure.Exceptions;

namespace Infrastructure.Services
{
    public class TypePrivilegeAccountingModel : AccountingModel
    {
        public int SteamId32 {get; set;}
        public Privilege Privilege { get; set; }
        public TypePrivilege CurrentTypePrivilege { get; set; }

        public TypePrivilegeAccountingModel(int steamId32, Privilege privilege, TypePrivilege currentTypePrivilege)
        {
            SteamId32 = steamId32;
            Privilege = privilege;
            CurrentTypePrivilege = currentTypePrivilege;
        }

        public override decimal CalculateFinallyPrice(Product product)
        {
            if (CurrentTypePrivilege.Discount == null && product.Discount != null) // когда учитываем привилегию и скидку
            {
                return GetDiscount(product) - GetDiscount(CurrentTypePrivilege.Price, product.Discount.Percent);
            }
            else
            {
                return GetDiscount(product) - GetDiscount(CurrentTypePrivilege); // учитываем только привилегию
            }
        }
    }
}
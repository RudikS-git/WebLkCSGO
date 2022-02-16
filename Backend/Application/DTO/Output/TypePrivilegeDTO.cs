using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.Account;
using Domain.Purchase;

namespace Application.DTO
{
    public class TypePrivilegeDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string GroupName { get; set; }

        public int Lvl { get; set; }

        public decimal Price { get; set; }

        public decimal? DiscountPrice { get; set; }

        public string ImageSource { get; set; }

        public List<FeatureDTO> Features { get; set; }
    }
}

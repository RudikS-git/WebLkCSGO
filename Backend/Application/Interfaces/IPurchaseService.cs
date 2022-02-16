using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities.Product;
using Qiwi.BillPayments.Model;
using Qiwi.BillPayments.Model.Out;

namespace Application
{
    public interface IPurchaseService
    {
        public Task<string> PayAsync(string steamId, TypePrivilege typePrivilege, string name);
        public Task CheckSuccessAsync(string signature, Notification notification, Func<int, int, Task<int>> Action);
    }
}

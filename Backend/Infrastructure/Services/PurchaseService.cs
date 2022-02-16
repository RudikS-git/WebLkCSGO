using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Domain.Context;
using Domain.Contexts;
using Domain.Entities.Product;
using Domain.Entities.Purchase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Qiwi.BillPayments.Client;
using Qiwi.BillPayments.Model;
using Qiwi.BillPayments.Model.In;
using Qiwi.BillPayments.Model.Out;
using Qiwi.BillPayments.Utils;
using SteamIDs_Engine;
using Domain.Entities.Account;
using Domain.Entities.Privilege;
using DAL = Domain.Entities.Purchase;

namespace Infrastructure.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly BillPaymentsClient client = null;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PurchaseService> _logger;
        private readonly DataContext _dataContext;
        private readonly ITypePrivilegeService _typePrivilegeService;

        public PurchaseService(DataContext dataContext, 
                                IConfiguration configuration, 
                                ILogger<PurchaseService> logger,
                                ITypePrivilegeService typePrivilegeService)
        {
            client = BillPaymentsClientFactory.Create(secretKey: configuration.GetValue<string>("Qiwi:SecretKey"));
            _configuration = configuration;
            _logger = logger;
            _dataContext = dataContext;
            _typePrivilegeService = typePrivilegeService;
        }

        public async Task<string> PayAsync(string steamId, TypePrivilege typePrivilege, string name)
        {
            string billId = Guid.NewGuid().ToString();

            decimal price = await _typePrivilegeService.CalculatePriceAsync(typePrivilege, SteamIDConvert.Steam64ToSteam32(long.Parse(steamId)));

            var response = await client.CreateBillAsync(
                info: new CreateBillInfo
                {
                    BillId = billId,

                    Amount = new MoneyAmount
                    {
                        ValueDecimal = price,
                        CurrencyEnum = CurrencyEnum.Rub
                    },

                    ExpirationDateTime = DateTime.Now.AddDays(1),
                    Comment = $"Оплата доната на аккаунт {name}({steamId}) в размере {price} руб.",
                    SuccessUrl = new Uri(_configuration.GetValue<string>("Domain")) // При localhost будет исключение
                },

                customFields: new CustomFields
                {
                    ThemeCode =_configuration.GetValue<string>("Qiwi:ThemeCode"),
                    SteamId = SteamIDConvert.Steam64ToSteam32(long.Parse(steamId)),
                    TypePrivilegeId = typePrivilege.Id
                }
            );

            return response.PayUrl.ToString();
        }

        public async Task CheckSuccessAsync(string signature, Notification notification, Func<int, int, Task<int>> Action)
        {
            if(BillPaymentsUtils.CheckNotificationSignature(signature, notification, _configuration.GetValue<string>("Qiwi:SecretKey")))
            {
                if (notification.Bill.BillId != null)
                {
                    if (notification.Bill.Status.ValueEnum == BillStatusEnum.Paid && !IsAlreadyIssued(notification.Bill.BillId, notification.Bill.CustomFields.SteamId))
                    {
                        //var billInfo = await client.GetBillInfoAsync(billId: billId);

                        var privilegeId = await Action(notification.Bill.CustomFields.SteamId, notification.Bill.CustomFields.TypePrivilegeId);

                        await _dataContext.Payment.AddAsync(new Payment()
                        {
                            BillId = notification.Bill.BillId,
                            CreationDate = notification.Bill.CreationDateTime,
                            ExpirationDate = notification.Bill.Status.DateTime,
                            Phone = notification.Bill.Customer.Phone,
                            TypePrivilegeId = notification.Bill.CustomFields.TypePrivilegeId,
                            Value = notification.Bill.Amount.ValueDecimal,
                            PrivilegeId = privilegeId,
                        });

                        await _dataContext.SaveChangesAsync();

                        var serializeObject = JsonConvert.SerializeObject(notification);
                        _logger.LogInformation($"Успешная оплата: {serializeObject}");
                    }
                    else
                    {
                        _logger.LogWarning($"Action уже вызывался. Повторный вызов запрещен: {notification.Bill.BillId} {notification.Bill.Comment}");
                    }
                }
            }
            else
            {
                _logger.LogWarning($"Попытка компроментации: {notification.Bill.BillId} {notification.Bill.Comment}");
            }
        }

        /// <summary>
        /// Проверяет выдана ли привилегия данному человеку
        /// </summary>
        /// <param name="billId"></param>
        /// <returns>true - выдано, иначе false</returns>
        public bool IsAlreadyIssued(string billId, int steamId)
        {
            var payment = _dataContext.Payment.Where(it => it.BillId == billId)
                                            .OrderByDescending(it => it.ExpirationDate)
                                            .FirstOrDefault();

            if (payment != null)
            {
                return true;
            }

            return false;
        }
    }
}

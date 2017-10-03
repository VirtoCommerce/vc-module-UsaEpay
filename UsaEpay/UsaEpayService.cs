using System;
using UsaEpay.Extensions;
using System.Linq;
using Common.Logging;
using Newtonsoft.Json;
using VirtoCommerce.Domain.Payment.Model;
using UsaEpay.Integration;
using UsaEpay.Client;

namespace UsaEpay
{
    public class UsaEpayService
    {
        private readonly UsaEpayConfiguration _configuration;
        private readonly ILog _logger;

        public UsaEpayService(UsaEpayConfiguration configuration, ILog logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public UsaEpayProcessResult ProcessCreditCard(ProcessPaymentEvaluationContext context)
        {
            var transactionRequestObject = new TransactionRequestObject();

            try
            {
                var shippingAddress =
                    context.Order.Addresses.FirstOrDefault(
                        a => a.AddressType == VirtoCommerce.Domain.Commerce.Model.AddressType.BillingAndShipping ||
                             a.AddressType == VirtoCommerce.Domain.Commerce.Model.AddressType.Shipping);

                if (shippingAddress == null)
                {
                    throw new Exception("No shipping address available.");
                }

                var billingAddress = context.Order.Addresses.FirstOrDefault(
                                         a => a.AddressType == VirtoCommerce.Domain.Commerce.Model.AddressType
                                                  .BillingAndShipping ||
                                              a.AddressType == VirtoCommerce.Domain.Commerce.Model.AddressType.Billing) ??
                                     shippingAddress;

                transactionRequestObject.CreditCardData = new CreditCardData
                {
                    CardNumber = context.BankCardInfo.BankCardNumber,
                    CardExpiration = context.BankCardInfo.BankCardMonth.ToUsaEpayMonth() + context.BankCardInfo.BankCardYear.ToUsaEpayYear(),
                    CardCode = context.BankCardInfo.BankCardCVV2, // -2 illegible, -9 not on card.
                    AvsStreet = billingAddress.Line1,
                    AvsZip = billingAddress.PostalCode,
                };
                transactionRequestObject.Details = new TransactionDetail
                {
                    Amount = context.Order.Total.ToDollars(),
                    AmountSpecified = true,
                    Description = $"Order from {context.Store.Name}.",
                    OrderID = context.Order.Id,
                };
                transactionRequestObject.CustomerID = context.Order.CustomerId;
                transactionRequestObject.BillingAddress = new Address
                {
                    Street = billingAddress.Line1,
                    Zip = billingAddress.PostalCode,
                    Email = billingAddress.Email
                };
                transactionRequestObject.AccountHolder = billingAddress.FirstName + " " + billingAddress.LastName;
                transactionRequestObject.CustReceipt = false;
            }
            catch (Exception)
            {
                _logger.Error(
                    $"Failed to create UsaEpay payment API request from OrderContext. {JsonConvert.SerializeObject(context)}");
                _logger.Trace($"Failed to create UsaEpay payment API request from OrderContext. {JsonConvert.SerializeObject(context)}");
                throw;
            }

            try
            {
                using (var client = new UsaEpayClient(_configuration))
                {
                    var result = client.RunSale(transactionRequestObject);

                    if (result.ResultCode != "A")
                    {
                        return new UsaEpayProcessResult
                        {
                            Succeeded = false,
                            Error = result.Error
                        };
                    }

                    return new UsaEpayProcessResult
                    {
                        Succeeded = true,
                        Error = string.Empty,
                        PaymentId = result.RefNum
                    };
                }
            }
            catch (Exception exception)
            {
                _logger.Error($"Payment processing through UsaEpay failed. {JsonConvert.SerializeObject(context)}");
                _logger.Trace($"Payment processing through UsaEpay failed. {JsonConvert.SerializeObject(context)}");

                return new UsaEpayProcessResult
                {
                    Succeeded = false,
                    Error = $"{exception.Message}",
                    PaymentId = string.Empty
                };
            }
        }
    }
}

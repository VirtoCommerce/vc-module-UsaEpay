namespace UsaEpay.Integration
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web.Services.Protocols;

    using AutoMapper;

    using UsaEpay.Communication;
    using UsaEpay.Client;
    using UsaEpay.Exceptions;

    public class UsaEpayClient : IDisposable
    {
        private readonly UsaEpayConfiguration configuration;

        private readonly usaepayService service;

        private bool disposed;

        public UsaEpayClient(UsaEpayConfiguration configuration)
        {
            this.configuration = configuration;
            service = new usaepayService(this.configuration.Endpoint);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public TransactionSearchResult SearchTransactions(SearchParam[] searchParam, bool matchAll, string start, string limit, string sort)
        {
            return service.searchTransactions(GetSecurityToken(), searchParam, matchAll, start, limit, sort);
        }

        public TransactionResponse RunSale(TransactionRequestObject transactionRequestObject)
        {
            return service.runSale(GetSecurityToken(), transactionRequestObject);
        }

        public TokenizedCreditCard GetStoredCreditCard(string token)
        {
            var creditCard = service.lookupCardToken(GetSecurityToken(), token);

            return creditCard == null ? null : Mapper.Map<CreditCardToken, TokenizedCreditCard>(creditCard);
        }

        public TokenizedCreditCard StoreCreditCard(AddPaymentMethodRequest request)
        {
            var creditCardData = new CreditCardData
            {
                CardCode = request.CardVerificationValue,
                CardExpiration = request.ExpirationDate.ToString("MMyy"),
                CardNumber = request.AccountIdentifier,
            };
            try
            {
                var token = service.saveCard(GetSecurityToken(), creditCardData);
                return new TokenizedCreditCard
                {
                    CardExpiration = token.CardExpiration,
                    CardNumber = token.CardNumber,
                    CardType = token.CardType,
                    Token = token.CardRef
                };
            }
            catch (SoapHeaderException soapHeaderException)
            {
                var code = long.Parse(soapHeaderException.Message.Split(':')[0].Trim());
                switch (code)
                {
                    case 17:
                        throw new ExpiredCardException();
                    case 12:
                        throw new InvalidCardNumberException();
                    default:
                        throw;
                }
            }
        }

        public TransactionObject GetTransaction(string referenceNumber)
        {
            return service.getTransaction(GetSecurityToken(), referenceNumber);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (service != null)
                    {
                        service.Dispose();
                    }
                }
            }

            disposed = true;
        }

        private ueSecurityToken GetSecurityToken()
        {
            var securityToken = new ueSecurityToken { SourceKey = configuration.SourceKey };

            // securityToken.ClientIP = "11.22.33.44";  // IP address of end user (if applicable)
            var pin = configuration.Pin;
            var hash = new ueHash { Type = "md5", Seed = Guid.NewGuid().ToString() };

            // combine data into single string
            var prehashvalue = string.Concat(securityToken.SourceKey, hash.Seed, pin);

            // generate hash
            hash.HashValue = GenerateHash(prehashvalue);

            // add hash value to token
            securityToken.PinHash = hash;
            return securityToken;
        }
        
        /// </returns>
        private string GenerateHash(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            var md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var hash = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            foreach (byte hashByte in data)
            {
                hash.Append(hashByte.ToString("x2"));
            }

            // Return the hexadecimal string.
            return hash.ToString();
        }
    }
}
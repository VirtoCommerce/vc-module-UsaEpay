using System;
using Common.Logging;
using Microsoft.Practices.Unity;
using UsaEpay.PaymentMethods;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;

namespace UsaEpay
{
    public class Module : ModuleBase
    {
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        public override void PostInitialize()
        {
            var settings = _container.Resolve<ISettingsManager>().GetModuleSettings("UsaEpay");
            var logger = _container.Resolve<ILog>();

            Func<UsaEpayCreditCardPaymentMethod> usaEpayCreditCardPaymentMethod = () => new UsaEpayCreditCardPaymentMethod(logger)
            {
                Name = "USA ePay Credit Card Processing",
                Description = "Process credit cards using USAePay.",
                LogoUrl = "https://github.com/montanehamilton/vc-module-UsaEpay/raw/master/UsaEpay/Content/UsaEpay.png",
                Settings = settings
            };

            _container.Resolve<IPaymentMethodsService>().RegisterPaymentMethod(usaEpayCreditCardPaymentMethod);
        }
    }
}

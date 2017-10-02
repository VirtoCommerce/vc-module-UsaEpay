namespace UsaEpay
{
    using AutoMapper;
    using UsaEpay.Client;
    using UsaEpay.Communication;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PaymentMethod, PaymentMethodDto>();
            CreateMap<CreditCardToken, TokenizedCreditCard>().ForMember(target => target.Token, map => map.MapFrom(source => source.CardRef));
        }
    }
}

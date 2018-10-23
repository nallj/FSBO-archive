using AutoMapper;
using System.Collections.Generic;

using FSBO.WebServices.Models.Dto;
using FSBO.WebServices.Models.Scraping;

namespace FSBO.WebServices.MappingProfiles
{
    public class ServiceToDALModel : Profile
    {
        public ServiceToDALModel()
        {

            CreateMap<ScrapeEvent, DAL.ScrapeEvent>()
                // Implicit 'AreaId' mapping.
                .ForMember(d => d.ScrapeRecords, opt => opt.MapFrom(s => s.Records));

            CreateMap<ScrapeRecord, DAL.ScrapeRecord>()
                .ForMember(d => d.TargetValues, opt => opt.MapFrom(s => s.TargetFieldIdToValueDictionary));

            CreateMap<RegistrationData, DAL.User>()
                // Implicit 'DayToCharge,' 'Email,' 'Phone,' and 'Username' mapping.
                //.ForMember(d => d.UserPaymentMethod, opt => opt.MapFrom(s => s))
                .ForMember(d => d.UserSuggestions, opt => opt.MapFrom(s => new List<RegistrationData>() { s }));

            CreateMap<RegistrationData, DAL.UserPaymentMethod>()
                // Implicit 'ExpMonth' and 'ExpYear' mapping.
                .ForMember(d => d.CreditCardNumber, opt => opt.MapFrom(s => s.CcNumber))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.PaymentName))
                .ForMember(d => d.NameOnCard, opt => opt.MapFrom(s => s.CcName));

            CreateMap<RegistrationData, DAL.UserSuggestion>()
                .ForMember(d => d.ZipCode, opt => opt.MapFrom(s => s.FirstZip));

            CreateMap<User, DAL.User>()
                .ForMember(d => d.UserId, opt => opt.MapFrom(s => s.Id));

            CreateMap<KeyValuePair<int, string>, DAL.TargetValue>()
                .ForMember(d => d.TargetId, opt => opt.MapFrom(s => s.Key))
                .ForMember(d => d.Value, opt => opt.MapFrom(s => s.Value));

        }
    }
}
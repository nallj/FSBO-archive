using AutoMapper;
using System.Linq;
using System.Collections.Generic;

using FSBO.WebServices.Models.Dto;
using FSBO.WebServices.Models.Scraping;

namespace FSBO.WebServices.MappingProfiles
{
    public class DALToServiceModel : Profile
    {
        public DALToServiceModel()
        {

            CreateMap<DAL.Area, AreaWithEntries>()
                // Implied 'AreaId' mapping.
                .ForMember(d => d.AreaType, opt => opt.MapFrom(s => s.AreaType))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Value))
                .ForMember(d => d.EntryPoints, opt => opt.MapFrom(s => s.SourceAreaEntries));

            CreateMap<DAL.AreaType, AreaType>();
            // Implicit 'AreaTypeId', 'Name' mapping.

            CreateMap<DAL.RecordDisqualification, RecordDisqualification>()
                // Implicit 'Parameters' mapping.
                .ForMember(d => d.DisqualificationType, opt => opt.MapFrom(s => (DisqualificationType)s.DisqualificationTypeId));

            CreateMap<DAL.SourceAreaEntry, EntryPoint>()
                .ForMember(d => d.EntryPointUri, opt => opt.MapFrom(s => s.Source.HeadUri + s.EntryPoint))
                .ForMember(d => d.SourceId, opt => opt.MapFrom(s => s.SourceId));

            CreateMap<DAL.Template, TemplateInstructions>()
                // Implicit 'IsTopLevel', 'SourceId', and 'TemplateId' mapping.
                .ForMember(d => d.OrderedSetup, opt => opt.MapFrom(s => s.SetupActions
                    .OrderBy(o => o.OrderIndex)))
                    .ForMember(d => d.Disqualifiers, opt => opt.MapFrom(s => s.RecordDisqualifications))
                .ForMember(d => d.TemplateFields, opt => opt.MapFrom(s => s.TemplateFields
                    .OrderBy(o => o.OrderIndex)));

            CreateMap<DAL.ScrapeAction, ScrapingStep>()
                // Implied 'Parameters' mapping.
                .ForMember(d => d.ScrapingStepId, opt => opt.MapFrom(s => s.ActionId))
                .ForMember(d => d.ActionType, opt => opt.MapFrom(s => (ScrapingActionType)s.ActionTypeId));

            CreateMap<DAL.ScrapeEvent, EventEmailPackage>()
                .ForMember(d => d.AreaName, opt => opt.MapFrom(s => s.Area.Value))
                .ForMember(d => d.Records, opt => opt.MapFrom(s => s.ScrapeRecords
                    .Select(sel => sel.TargetValues)));

            CreateMap<DAL.SetupAction, SetupStep>()
                // Implied 'Parameters' mapping.
                .ForMember(d => d.SetupStepId, opt => opt.MapFrom(s => s.SetupActionId))
                .ForMember(d => d.ActionType, opt => opt.MapFrom(s => (ScrapingActionType)s.ActionTypeId));

            //CreateMap<DAL.Subscriber, Subscriber>();

            CreateMap<ICollection<DAL.TargetValue>, ScrapeRecord>()
                .ForMember(d => d.TargetFieldIdToValueDictionary, opt => opt.MapFrom(s => s
                    .Where(w => !w.TargetField.IsTemporaryField)
                    .ToDictionary(k => k.TargetId, v => v.Value)));

            CreateMap<DAL.TemplateField, TargetTemplateField>()
                // Implied 'TemplateFieldId' mapping.
                .ForMember(d => d.TargetFieldId, opt => opt.MapFrom(s => s.TargetId))
                .ForMember(d => d.IsTemporaryField, opt => opt.MapFrom(s => s.TargetField.IsTemporaryField))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.TargetField.Name))
                .ForMember(d => d.TemplateIndex, opt => opt.MapFrom(s => s.OrderIndex))
                .ForMember(d => d.OrderedScrapingSteps, opt => opt.MapFrom(s => s.ScrapeActions
                    .OrderBy(o => o.OrderIndex)));

            CreateMap<DAL.User, User>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.UserId));

        }
    }
}
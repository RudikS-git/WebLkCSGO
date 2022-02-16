using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Application.DTO;
using Application.DTO.Output;
using AutoMapper;
using Domain;
using Domain.Context;
using SteamIDs_Engine;
using Domain.Entities.Account;
using Domain.Entities.Product;
using Domain.Entities.Server;
using Domain.Entities.Support;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            
            CreateMap<Ticket, TicketDTO>()
                .ForMember(it => it.AccusedUserName,
                    source => source.MapFrom(source => source.AccusedUserStat.Name))

                .ForMember(it => it.SenderUserName,
                    source => source.MapFrom(source => source.SenderUserStat.Name))

                .ForMember(it => it.CheckingUserName,
                    source => source.MapFrom(source => source.CheckingUserStat.Name))

                .ForMember(it => it.AccusedUserStatId,
                    source => source.MapFrom(source =>
                        SteamIDConvert.Steam2ToSteam64(source.AccusedUserStat.SteamAuth2).ToString()))

                .ForMember(it => it.SenderUserStatId,
                    source => source.MapFrom(source =>
                        SteamIDConvert.Steam2ToSteam64(source.SenderUserStat.SteamAuth2).ToString()))

                .ForMember(it => it.CheckingUserStatId,
                    source => source.MapFrom(source =>
                        source.CheckingUserStatId != null
                            ? SteamIDConvert.Steam2ToSteam64(source.CheckingUserStat.SteamAuth2).ToString()
                            : null))
                .ForMember(it => it.CheckingUserName,
                    source => source.MapFrom(source => source.CheckingUserStat.Name))
                .ReverseMap();



            CreateMap<Server, ServerDTO>();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Feature, FeatureDTO>().ReverseMap();
            CreateMap<TypePrivilege, TypePrivilegeDTO>();

            CreateMap<ChatRow, GameChatDTO>()
                .ForMember(it => it.Time,
                    source => source.MapFrom(source => DateTimeOffset.FromUnixTimeSeconds(source.Timestamp).DateTime));
        }
        
    }
}

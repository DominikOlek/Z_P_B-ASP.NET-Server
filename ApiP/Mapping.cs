using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using System.Linq.Expressions;
using ApiP.Models;
using ApiP.Data;
namespace ApiP
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<AddTicketDto, Drivers>();
            CreateMap<AddTicketDto, Tickets>().ForMember(a=>a.Description,c=>c.MapFrom(g=>g.Description))
                .ForMember(a => a.ReasonID, c => c.MapFrom(g => g.ReasonID));
            CreateMap<Drivers, ReturnDriverDto>().ForMember(a=>a.DateOfPassLicense,c=>c.MapFrom(g=>g.DateOfPassLicense.Date))
                .ForMember(a => a.BirthDate, c => c.MapFrom(g => g.BirthDate.Date));
            CreateMap<Tickets, ReturnTicketDto>().ForMember(a => a.Pkt, c => c.MapFrom(g => g.Reason.PointNumber))
                .ForMember(a => a.Title, c => c.MapFrom(g => g.Reason.Title))
                .ForMember(a => a.DateOfTicket, c => c.MapFrom(g => g.DateOfTicket.Date))
                .ForMember(a => a.MonthsOfLost, c => c.MapFrom(g => g.Reason.MonthsOfLost));
            CreateMap<Tickets, ReturnBigMandatDto>().ForMember(a => a.PointsNumber, c => c.MapFrom(g => g.Reason.PointNumber))
                .ForMember(a => a.Title, c => c.MapFrom(g => g.Reason.Title))
                .ForMember(a => a.NameOfRecipient, c => c.MapFrom(g => g.Driver.Name))
                .ForMember(a => a.LastNameOfRecipient, c => c.MapFrom(g => g.Driver.LastName))
                .ForMember(a => a.NameOfCop, c => c.MapFrom(g => g.Cop.Name))
                .ForMember(a => a.LastNameOfCop, c => c.MapFrom(g => g.Cop.LastName))
                .ForMember(a => a.BadgeNumberOfCop, c => c.MapFrom(g => g.Cop.BadgeNumber.ToString()))
                .ForMember(a => a.isCost, c => c.MapFrom(g => g.DateOfPayment == DateTime.MinValue ? false : true)) ;
            CreateMap<RegisterDto, Users>();
            CreateMap<RegisterAppDto, AppUsers>();
            CreateMap<Users, UserInfoDto>();
            CreateMap<Users, ReturnUserDto>().ForMember(a => a.Role, c => c.MapFrom(g => g.Role.RoleName));
            CreateMap<AddTaryfikatorDto, Taryfikator>();
        }
    }
}

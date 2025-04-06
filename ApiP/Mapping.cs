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
            CreateMap<AddMandatDto, Kierowcy>();
            CreateMap<AddMandatDto, Mandaty>().ForMember(a=>a.Opis,c=>c.MapFrom(g=>g.OpisPowodu))
                .ForMember(a => a.PowodID, c => c.MapFrom(g => g.IDPowodu));
            CreateMap<Kierowcy, ReturnKierowcaDto>().ForMember(a=>a.Data_orzymania,c=>c.MapFrom(g=>g.Data_orzymania.Date))
                .ForMember(a => a.Data_ur, c => c.MapFrom(g => g.Data_ur.Date));
            CreateMap<Mandaty, ReturnMandatDto>().ForMember(a => a.PktZaMandat, c => c.MapFrom(g => g.Powod.Liczba_PKT))
                .ForMember(a => a.Tytul, c => c.MapFrom(g => g.Powod.Tytul))
                .ForMember(a => a.DataWydania, c => c.MapFrom(g => g.DataWydania.Date))
                .ForMember(a => a.MWstrzymania, c => c.MapFrom(g => g.Powod.Miesi¹ceWstrzymania));
            CreateMap<Mandaty, ReturnBigMandatDto>().ForMember(a => a.PktZaMandat, c => c.MapFrom(g => g.Powod.Liczba_PKT))
                .ForMember(a => a.Tytul, c => c.MapFrom(g => g.Powod.Tytul))
                .ForMember(a => a.ImieKaranego, c => c.MapFrom(g => g.Kierowcy.Imie))
                .ForMember(a => a.NazwiskoKaranego, c => c.MapFrom(g => g.Kierowcy.Nazwisko))
                .ForMember(a => a.ImieKaraj¹cego, c => c.MapFrom(g => g.Przez.Imie))
                .ForMember(a => a.NazwiskoKaraj¹cego, c => c.MapFrom(g => g.Przez.Nazwisko))
                .ForMember(a => a.Nr_Karaj¹cego, c => c.MapFrom(g => g.Przez.Nr_Sluzbowy.ToString()))
                .ForMember(a => a.oplata, c => c.MapFrom(g => g.DataOplacenia == DateTime.MinValue ? false : true)) ;
            CreateMap<RegisterDto, Users>();
            CreateMap<RegisterAppDto, AppUsers>();
            CreateMap<Users, UserInfoDto>();
            CreateMap<Users, ReturnUserDto>().ForMember(a => a.Rola, c => c.MapFrom(g => g.Rola.Nazwa_Roli));
            CreateMap<AddTaryfikatorDto, Taryfikator>();
        }
    }
}

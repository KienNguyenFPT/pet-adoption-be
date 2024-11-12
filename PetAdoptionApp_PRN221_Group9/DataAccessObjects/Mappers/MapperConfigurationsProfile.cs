using AutoMapper;
using BusinessLogicLayer.ViewModels.AdoptionsDTOs;
using BusinessLogicLayer.ViewModels.DonationDTOs;
using BusinessLogicLayer.ViewModels.EventDTO;
using BusinessLogicLayer.ViewModels.HealthDTO;
using BusinessLogicLayer.ViewModels.PetDTOs;
using BusinessLogicLayer.ViewModels.PetImageDTOs;
using BusinessLogicLayer.ViewModels.ShelterDTOs;
using BusinessLogicLayer.ViewModels.UserDTOs;
using BusinessObjects;


namespace DataAccessObjects.Mappers {
    public class MapperConfigurationsProfile : Profile {
        public MapperConfigurationsProfile() {
            CreateMap<Shelter, ShelterDTO>().ReverseMap();
            CreateMap<User, AuthenticationDTOs>().ReverseMap();
            CreateMap<User, RegistrationDTOs>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UpdateUserDTO>().ReverseMap();
            CreateMap<Pet, PetDTOs>().ReverseMap();
            CreateMap<Pet, GetPetDTOs>().ForMember(dest => dest.ShelterName, opt => opt.MapFrom(src => src.Shelter.ShelterName))
                .ForMember(dest => dest.PetImages, opt => opt.MapFrom(src => src.PetImages)).ReverseMap();
            CreateMap<Donation, DonationDTOs>().ReverseMap();
            CreateMap<PetImage, PetImagesDTOs>().ReverseMap();
            CreateMap<Event, EventDTO>().ReverseMap().ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images)).ReverseMap();
            CreateMap<BasicEventRequestDTO, Event>().ReverseMap();
            CreateMap<BasicEventResponseDTO, Event>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ForMember(dest => dest.Enrollments, opt => opt.MapFrom(src => src.Enrollments))
                .ReverseMap();
            CreateMap<Adoption, AdoptionDTOs>().ReverseMap();
            CreateMap<Adoption, GetAdoptionDTos>().ReverseMap();
            CreateMap<Adoption, CreateAdoptionDTOs>().ReverseMap();
            CreateMap<Donation, GetDonationDTO>().ReverseMap();
            CreateMap<Healths, HealthDTOs>().ReverseMap();
            CreateMap<PetImage, GetPetImageDTOs>().ReverseMap();
            CreateMap<EventImage, EventImageDTO>()
                .ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.EventId))
                .ReverseMap();
            CreateMap<Enrollment, EnrollmentDTO>()
                .ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.EventId))
                .ReverseMap();


            //Mapper history for enrollment
            CreateMap<Enrollment, EventHistoryDTO>()
           .ForMember(dest => dest.EventName, opt => opt.MapFrom(src => src.Event.EventName))
           .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Event.Description))
           .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.Event.StartDate))
           .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.Event.EndDate))
           .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Event.Location))
           .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => src.Event.EventType.ToString()))
           .ForMember(dest => dest.EventStatus, opt => opt.MapFrom(src => src.Event.EventStatus.ToString()));
        }
    }
}

using AutoMapper;

namespace prid_tuto.Models
{
    /*
    Cette classe sert � configurer AutoMapper pour lui indiquer quels sont les mappings possibles
    et, le cas �ch�ant, param�trer ces mappings de mani�re d�clarative (nous verrons des exemples plus tard).
    */
    public class MappingProfile : Profile
    {
        private MsnContext _context;

        /*
        Le gestionnaire de d�pendance injecte une instance du contexte EF dont le mapper peut
        se servir en cas de besoin (ce n'est pas encore le cas).
        */
        public MappingProfile(MsnContext context)
        {
            _context = context;

            CreateMap<Member, MemberDTO>();
            CreateMap<MemberDTO, Member>();
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();

            CreateMap<Member, MemberWithPasswordDTO>();
            CreateMap<MemberWithPasswordDTO, Member>();
            CreateMap<User, UserWithPasswordDTO>();
            CreateMap<UserWithPasswordDTO, User>();
        }
    }
}
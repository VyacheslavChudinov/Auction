using AutoMapper;
using contracts;
using SearchService.Models;

namespace SearchService.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<AuctionCreated, Item>();
    }
}
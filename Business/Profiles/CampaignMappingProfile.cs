using AutoMapper;
using Business.DTOs.Campaign;
using Entities.Concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Profiles
{
    public class CampaignMappingProfile : Profile
    {
        public CampaignMappingProfile()
        {
            CreateMap<Campaign, CampaignDTO>().ReverseMap();
        }
    }
}

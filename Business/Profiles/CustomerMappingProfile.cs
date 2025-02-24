using AutoMapper;
using Business.DTOs.Customer;
using Entities.Concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Profiles
{
    public class CustomerMappingProfile:Profile
    {
        public CustomerMappingProfile()
        {
            CreateMap<Customer,CustomerDTO>().ReverseMap();
        }
    }
}

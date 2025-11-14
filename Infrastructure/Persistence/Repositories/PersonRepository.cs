using Application.Contracts.Repositories;
using Application.Features.Dtos;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class PersonRepository(AppDbContext ctx) : GenericRepository<Person>(ctx), IPersonRepository
    {
      
    }
}

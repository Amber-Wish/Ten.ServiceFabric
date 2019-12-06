using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Timing;

namespace Ten.DDD.EFCore
{
    public class AbpBaseDbContext:AbpDbContext<AbpBaseDbContext>
    {
        public AbpBaseDbContext(DbContextOptions<AbpBaseDbContext> options) : base(options)
        {
        }

    }
}
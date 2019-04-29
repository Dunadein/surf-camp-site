﻿using Microsoft.EntityFrameworkCore;
using SurfLevel.Contracts.Models.DatabaseObjects;

namespace SurfLevel.Repository.DBProviders
{
    public class PackageContext : DbContext
    {
        public PackageContext(DbContextOptions<PackageContext> options)
            : base(options)
        {
        }

        public DbSet<Package> Packages { get; set; }

        public DbSet<PackagePeriodPrice> PackagePrices { get; set; }

        public DbSet<OutOfServicePeriod> StopPeriods { get; set; }
    }
}
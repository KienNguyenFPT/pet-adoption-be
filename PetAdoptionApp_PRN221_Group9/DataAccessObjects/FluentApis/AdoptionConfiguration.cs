﻿using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.FluentApis
{
    public class AdoptionConfiguration : IEntityTypeConfiguration<Adoption>
    {
        public void Configure(EntityTypeBuilder<Adoption> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            /*builder.HasMany(x => x.Pets).WithOne(x => x.Adoption);*/
        }
    }
}

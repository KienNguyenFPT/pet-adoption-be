using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.FluentApis
{
    public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.HasKey(x => new { x.EventId, x.UserId });
            builder.HasOne(x => x.User).WithMany(x => x.Enrollments).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Event).WithMany(x => x.Enrollments).OnDelete(DeleteBehavior.Cascade);
        }
    }
}

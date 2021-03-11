using Microsoft.EntityFrameworkCore;
using System;

namespace diaryofmyself.Data
{
    public abstract partial class Base : IBase
    {
        public virtual int Id { get; set; }

        public virtual DateTimeOffset Created { get; set; }

        public static void OnModelCreating<T>(ModelBuilder builder)
            where T : class, IBase
        {
            builder.Entity<T>().HasKey(entity => entity.Id);
        }
    }
}
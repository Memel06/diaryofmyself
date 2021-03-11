using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace diaryofmyself.Data
{
    public class DOMDbContext : DbContext
    {
        protected IConfiguration _configuration;

        public DOMDbContext()
        {
            _configuration = new ConfigurationBuilder().AddJsonFile(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\config.json").Build();
        }

        private readonly IHttpContextAccessor _httpContextAccessor;

        public DOMDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor)
                    : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DOMDbContext([NotNull] IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(entity => entity.GetInterfaces().Any(_interface => _interface.Equals(typeof(IBase)) && entity.IsClass && entity.IsPublic && !entity.IsAbstract));

            foreach (var type in types)
            {
                var onModelCreatingMethod = type.GetMethods().FirstOrDefault(x => x.Name == "OnModelCreating" && x.IsStatic);

                if (onModelCreatingMethod != null)
                {
                    onModelCreatingMethod.Invoke(type, new object[] { builder });
                }

                if (type.BaseType == null || type.BaseType != typeof(Base))
                {
                    continue;
                }

                var baseOnModelCreatingMethod = type.BaseType.GetMethods().FirstOrDefault(x => x.Name == "OnModelCreating" && x.IsStatic);

                if (baseOnModelCreatingMethod == null)
                {
                    continue;
                }

                var baseOnModelCreatingGenericMethod = baseOnModelCreatingMethod.MakeGenericMethod(new Type[] { type });

                if (baseOnModelCreatingGenericMethod == null)
                {
                    continue;
                }

                baseOnModelCreatingGenericMethod.Invoke(typeof(Base), new object[] { builder });
            }

        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(_configuration["ConnectionStrings:DOMDbContext"]);
        }

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is IBase)
                {
                    entry.Property("Added").CurrentValue = DateTimeOffset.Now;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
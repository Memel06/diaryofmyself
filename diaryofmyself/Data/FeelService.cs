using System;
using diaryofmyself.Data;
using diaryofmyself.Data.Feel;
using diaryofmyself.Services.Feel;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace diaryofmyself.Data
{
    public partial class FeelService : IFeelService
    {
        protected readonly DOMDbContext _context;

        public FeelService([NotNull] DOMDbContext context)
        {
            _context = context;
        }

        public virtual async Task<IEnumerable<Feeling>> GetAll(string userUID)
        {
            var query = _context.Set<Feeling>().AsQueryable().RecentFeels();
            return await query.Where(feeling => feeling.userUID == userUID).ToListAsync();
        }


        public virtual async Task InsertFeel(Feeling feel)
        {
            _context.Add(feel);
            return await _context.SaveChangesAsync();
        }
    }
    public static class FeelServiceHelpers
    {
        public static IQueryable<Feeling> RecentFeels(this IQueryable<Feeling> query)
        {
            return query.Where(feel => feel.Created >= DateTime.Now.AddYears(-2));
        }
    }
}

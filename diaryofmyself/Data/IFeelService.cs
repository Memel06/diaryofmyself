using System;
using diaryofmyself.Data.Feel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diaryofmyself.Services.Feel
{
    public partial interface IFeelService
    {
            Task<IEnumerable<Feeling>> GetAll(string userUID);
            Task<int> InsertFeel(Feeling feeling);
    }
}

using System;

namespace diaryofmyself.Data
{
    public partial interface IBase
    {
        int Id { get; set; }

        DateTimeOffset Created { get; set; }
    }
}
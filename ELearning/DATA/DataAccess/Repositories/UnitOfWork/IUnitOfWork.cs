using DATA.DataAccess.Repositories.IRepositories;
using DATA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.DataAccess.Repositories.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<AppUser> AppUsers { get; }
        IBaseRepository<Language> Languages { get; }
        IBaseRepository<LanguagePreference> LanguagePreferences { get; }
        IBaseRepository<Report> Reports { get; }
        IBaseRepository<Feedback> Feedbacks { get; }
        IBaseRepository<Message> Messages { get; }
        IBaseRepository<Interest> Interests { get; }
        IBaseRepository<UserInterest> UserInterests { get; }
        IBaseRepository<UserMatch> UserMatches { get; }

        Task<int> CommitAsync();

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}

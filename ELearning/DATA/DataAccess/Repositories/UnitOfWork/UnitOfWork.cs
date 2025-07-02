using DATA.DataAccess.Context;
using DATA.DataAccess.Repositories.IRepositories;
using DATA.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace DATA.DataAccess.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction _transaction;

        public IBaseRepository<AppUser> AppUsers { get; private set; }

        public IBaseRepository<Language> Languages { get; private set; }

        public IBaseRepository<LanguagePreference> LanguagePreferences { get; private set; }

        public IBaseRepository<Report> Reports { get; private set; }

        public IBaseRepository<Feedback> Feedbacks { get; private set; }
        public IBaseRepository<Message> Messages { get; private set; }

        public UnitOfWork(AppDbContext context, ILogger<UnitOfWork> logger)
        {
            _context = context;
            AppUsers = new BaseRepository<AppUser>(_context);
            Languages = new BaseRepository<Language>(_context);
            LanguagePreferences = new BaseRepository<LanguagePreference>(_context);
            Reports = new BaseRepository<Report>(_context);
            Feedbacks = new BaseRepository<Feedback>(_context);
            Messages = new BaseRepository<Message>(_context);
        }

        public async Task<int> CommitAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }
        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();

                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
            catch (Exception ex)
            {
                await RollbackTransactionAsync();
                throw;
            }
        }
        public async Task RollbackTransactionAsync()
        {
            try
            {
                if (_transaction != null)
                {
                    await _transaction.RollbackAsync();
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

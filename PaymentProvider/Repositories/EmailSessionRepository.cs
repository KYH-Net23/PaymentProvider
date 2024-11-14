using Microsoft.EntityFrameworkCore;
using PaymentProvider.Contexts;
using PaymentProvider.Models;

namespace PaymentProvider.Repositories
{
    public class EmailSessionRepository(EmailSessionDbContext context)
    {
        private readonly EmailSessionDbContext _context = context;

        public async Task<EmailSession> GetEmailSessionAsync(string sessionId)
        {
            try
            {
                var session = await _context.Sessions.FirstOrDefaultAsync(s => s.SessionId == sessionId);
                if (session == null) return null!;
                return session;
            }
            catch
            {
                return null!;
            }
        }
        public async Task CreateAsync(EmailSession session)
        {
            try
            {
                await _context.Sessions.AddAsync(session);
                await SaveAsync();
            }
            catch { }
        }
        public async Task UpdateAsync(EmailSession session)
        {
            try
            {
                var oldSession = await _context.Sessions.FindAsync(session.Id);
                if (oldSession == null)
                {
                    await CreateAsync(session);
                }
                else
                {
                    _context.Entry(oldSession).CurrentValues.SetValues(session);
                    await SaveAsync();
                }
            }
            catch { }
        }

        public async Task SaveAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch { }
        }
    }
}

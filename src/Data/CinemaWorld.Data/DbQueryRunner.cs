namespace CinemaWorld.Data
{
    using System;
    using System.Threading.Tasks;

    using CinemaWorld.Data.Common;

    using Microsoft.EntityFrameworkCore;

    public class DbQueryRunner : IDbQueryRunner
    {
        public DbQueryRunner(CinemaWorldDbContext context)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public CinemaWorldDbContext Context { get; set; }

        public Task RunQueryAsync(string query, params object[] parameters)
        {
            return this.Context.Database.ExecuteSqlRawAsync(query, parameters);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Context?.Dispose();
            }
        }
    }
}

using MvcApplication1.Models.DBModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MvcApplication1.Repository
{
    public class JobRepository
    {
        dataEntities dbContext = new dataEntities();
        private DbSet<Job> _allJobs;
        public JobRepository()
        {
            dbContext.Configuration.ProxyCreationEnabled = false;
            _allJobs = dbContext.Jobs;
        }


        public DbSet<Job> AllJobs
        {
            get
            {
                if (_allJobs == null)
                    _allJobs = dbContext.Jobs;
                return _allJobs;
            }
        }
        public Task<Job> FindAJob(int id)
        {
            return _allJobs.FirstOrDefaultAsync<Job>(c => c.Id == id);
        }
    }
}

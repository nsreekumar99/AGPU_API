using AGPU_API.Data;
using AGPU_API.Models;
using AGPU_API.Repository.IRepository;

namespace AGPU_API.Repository
{
    public class AGPUMainRepository : AGPURepository<AGPU>, IAGPUMainRepository
    {
        private readonly ApplicationDbContext _db; // applicationdbcontext of this class should be passed to the main repository so we add base(db) in constructor

        public AGPUMainRepository(ApplicationDbContext db) : base(db)  // applicationdbcontext passed to base
        {
            _db = db;
        }

        public async Task<AGPU> UpdateAsync(AGPU entity)
        {
            _db.Update(entity);
            //await _db.SaveChangesAsync(); // since we are already calling save explicitely using unitofwork this is not needed.
            return await Task.FromResult(entity); //used task.fromresult instead of return entity to return the task to comply with the async method.
        }
    }
}

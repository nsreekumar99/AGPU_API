using AGPU_API.Data;
using AGPU_API.Models;
using AGPU_API.Repository.IRepository;

namespace AGPU_API.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        private IAGPURepository<AGPU> _agpuRepository;
        private IAGPUMainRepository _agpuMainRepository;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
        }

        public IAGPURepository<AGPU> AGPUs
        {
            get
            {
                return _agpuRepository ??= new AGPURepository<AGPU>(_db);
            }

        }

        public IAGPUMainRepository AGPUMain
        {
            get
            {
                return _agpuMainRepository ??= new AGPUMainRepository(_db);
            }
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}

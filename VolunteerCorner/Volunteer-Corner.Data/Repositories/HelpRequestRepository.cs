using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volunteer_Corner.Data.Entities;
using Volunteer_Corner.Data.Interfaces;

namespace Volunteer_Corner.Data.Repositories
{
    internal class HelpRequestRepository : IRepository<HelpRequest>
    {
        private ApplicationDbContext _db;

        public HelpRequestRepository(ApplicationDbContext context)
        {
            _db = context;
        }
        public IEnumerable<HelpRequest> GetAll()
        {
            return _db.HelpRequests;
        }

        public HelpRequest Get(int id)
        {
            return _db.HelpRequests.Find(id)!;
        }

        public void Create(HelpRequest item)
        {
            _db.HelpRequests.Add(item);
            _db.SaveChanges();
        }

        public void Update(HelpRequest item)
        {
            _db.Entry(item).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var proposalRequest = _db.HelpRequests.Find(id)!;
            _db.HelpRequests.Remove(proposalRequest);

            _db.SaveChanges();
        }
    }
}

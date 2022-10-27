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
    internal class HelpProposaRepository : IRepository<HelpProposal>
    {
        private ApplicationDbContext _db;

        public HelpProposaRepository(ApplicationDbContext context)
        {
            _db = context;
        }
        public IEnumerable<HelpProposal> GetAll()
        {
            return _db.HelpProposals;
        }

        public HelpProposal Get(int id)
        {
            return _db.HelpProposals.Find(id)!;
        }

        public void Create(HelpProposal item)
        {
            _db.HelpProposals.Add(item);
            _db.SaveChanges();
        }

        public void Update(HelpProposal item)
        {
            _db.Entry(item).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var proposal = _db.HelpProposals.Find(id)!;
            _db.HelpProposals.Remove(proposal);

            _db.SaveChanges();
        }
    }
}

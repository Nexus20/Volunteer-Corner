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
    internal class HelpSeekerRepository : IRepository<HelpSeeker>
    {
        private ApplicationDbContext _db;

        public HelpSeekerRepository(ApplicationDbContext context)
        {
            _db = context;
        }
        public IEnumerable<HelpSeeker> GetAll()
        {
            return _db.HelpSeekers;
        }

        public HelpSeeker Get(int id)
        {
            return _db.HelpSeekers.Find(id)!;
        }

        public void Create(HelpSeeker item)
        {
            _db.HelpSeekers.Add(item);
            _db.SaveChanges();
        }

        public void Update(HelpSeeker item)
        {
            _db.Entry(item).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var helpSeeker = _db.HelpSeekers.Find(id)!;
            _db.HelpSeekers.Remove(helpSeeker);

            _db.SaveChanges();
        }
    }
}

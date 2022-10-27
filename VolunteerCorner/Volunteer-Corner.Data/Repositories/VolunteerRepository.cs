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
    internal class VolunteerRepository : IRepository<Volunteer>
    {
        private ApplicationDbContext _db;

        public VolunteerRepository(ApplicationDbContext context)
        {
            _db = context;
        }
        public IEnumerable<Volunteer> GetAll()
        {
            return _db.Volunteers;
        }

        public Volunteer Get(int id)
        {
            return _db.Volunteers.Find(id)!;
        }

        public void Create(Volunteer item)
        {
            _db.Volunteers.Add(item);
            _db.SaveChanges();
        }

        public void Update(Volunteer item)
        {
            _db.Entry(item).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var volunteer = _db.Volunteers.Find(id)!;
            _db.Volunteers.Remove(volunteer);

            _db.SaveChanges();
        }
    }
}

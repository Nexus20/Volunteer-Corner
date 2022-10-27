using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volunteer_Corner.Data.Entities.Identity;
using Volunteer_Corner.Data.Interfaces;

namespace Volunteer_Corner.Data.Repositories
{
    internal class UserDocumentsRepository : IRepository<UserDocument>
    {
        private ApplicationDbContext _db;

        public UserDocumentsRepository(ApplicationDbContext context)
        {
            _db = context;
        }
        public IEnumerable<UserDocument> GetAll()
        {
            return _db.UserDocuments;
        }

        public UserDocument Get(int id)
        {
            return _db.UserDocuments.Find(id)!;
        }

        public void Create(UserDocument item)
        {
            _db.UserDocuments.Add(item);
            _db.SaveChanges();
        }

        public void Update(UserDocument item)
        {
            _db.Entry(item).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var userDocument = _db.UserDocuments.Find(id)!;
            _db.UserDocuments.Remove(userDocument);

            _db.SaveChanges();
        }
    }
}

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
    internal class HelpRequestDocumentRepository : IRepository<HelpRequestDocument>
    {
        private ApplicationDbContext _db;

        public HelpRequestDocumentRepository(ApplicationDbContext context)
        {
            _db = context;
        }
        public IEnumerable<HelpRequestDocument> GetAll()
        {
            return _db.HelpRequestDocuments;
        }

        public HelpRequestDocument Get(int id)
        {
            return _db.HelpRequestDocuments.Find(id)!;
        }

        public void Create(HelpRequestDocument item)
        {
            _db.HelpRequestDocuments.Add(item);
            _db.SaveChanges();
        }

        public void Update(HelpRequestDocument item)
        {
            _db.Entry(item).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var helpRequestDocument = _db.HelpRequestDocuments.Find(id)!;
            _db.HelpRequestDocuments.Remove(helpRequestDocument);

            _db.SaveChanges();
        }
    }
}

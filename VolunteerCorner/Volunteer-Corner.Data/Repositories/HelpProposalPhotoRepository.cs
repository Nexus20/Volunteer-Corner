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
    internal class HelpProposalPhotoRepository : IRepository<HelpProposalPhoto>
    {
        private ApplicationDbContext _db;

        public HelpProposalPhotoRepository(ApplicationDbContext context)
        {
            _db = context;
        }
        public IEnumerable<HelpProposalPhoto> GetAll()
        {
            return _db.HelpProposalPhotos;
        }

        public HelpProposalPhoto Get(int id)
        {
            return _db.HelpProposalPhotos.Find(id)!;
        }

        public void Create(HelpProposalPhoto item)
        {
            _db.HelpProposalPhotos.Add(item);
            _db.SaveChanges();
        }

        public void Update(HelpProposalPhoto item)
        {
            _db.Entry(item).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var proposalPhoto = _db.HelpProposalPhotos.Find(id)!;
            _db.HelpProposalPhotos.Remove(proposalPhoto);

            _db.SaveChanges();
        }
    }
}

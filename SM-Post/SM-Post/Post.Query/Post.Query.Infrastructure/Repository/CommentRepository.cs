using Arch.EntityFrameworkCore;
using Post.Query.Domain.Domain;
using Post.Query.Domain.Entities;
using Post.Query.Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Query.Infrastructure.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DatabaseContextFactory _databaseContextFactory;

        public CommentRepository(DatabaseContextFactory databaseContextFactory)
        {
            _databaseContextFactory = databaseContextFactory;
        }

        public async Task CreateAsync(CommentEntity comment)
        {
            using DatabaseContext _context = _databaseContextFactory.CreateDbContext();

            _context.CommentEntities.Add(comment);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteAsync(Guid commentId)
        {
            using DatabaseContext _context = _databaseContextFactory.CreateDbContext();
            var comment = await GetByIdAsync(commentId);
            if (comment==null)
                return;
            _context.CommentEntities.Remove(comment);   
        }

        public async Task<CommentEntity> GetByIdAsync(Guid commentId)
        {
            using DatabaseContext _context = _databaseContextFactory.CreateDbContext();
            var comment = await _context.CommentEntities.FirstOrDefaultAsync(a => a.CommentId == commentId);

            return comment;
        }

        public async Task UpdateAsync(CommentEntity comment)
        {
            using DatabaseContext _context = _databaseContextFactory.CreateDbContext();

            _context.CommentEntities.Update(comment);
            await _context.SaveChangesAsync();
        }
    }
}

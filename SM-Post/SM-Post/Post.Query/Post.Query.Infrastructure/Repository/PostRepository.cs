using Microsoft.EntityFrameworkCore;
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
    public class PostRepository : IPostRepository
    {
        private readonly DatabaseContextFactory _databaseContextFactory;

        public PostRepository(DatabaseContextFactory databaseContextFactory)
        {
            _databaseContextFactory = databaseContextFactory;
        }

        public async Task CreateAsync(PostEntity post)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            context.Posts.Add(post);
            _ = await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid postId)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            var post = await GetByIdAsync(postId);
            if (post == null)
                return;
            context.Posts.Remove(post);
           
        }

        public async Task<PostEntity> GetByIdAsync(Guid postId)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            var post = await context.Posts
                            .Include(p=>p.Comments)
                            .FirstOrDefaultAsync(x=>x.PostId==postId);
            return post;
        }

        public async Task<List<PostEntity>> ListAllAsync()
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            var posts = await context.Posts
                             .AsNoTracking()
                            .Include(p => p.Comments)
                            .ToListAsync();
            return posts;
        }

        public async Task<List<PostEntity>> ListAuthorAsync(string author)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            var posts = await context.Posts
                            .AsNoTracking()
                            .Include(p => p.Comments)
                            .Where(a=>a.Author.Contains(author)).ToListAsync();
            return posts;
        }

        public async Task<List<PostEntity>> ListWithLiskesAsync(int numberOfLikes)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            var posts = await context.Posts
                            .AsNoTracking()
                            .Include(p => p.Comments)
                            .Where(a => a.Likes>=numberOfLikes).ToListAsync();
            return posts;
        }

        public async Task<List<PostEntity>> ListWithCommentAsync()
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            var posts = await context.Posts
                            .AsNoTracking()
                            .Include(p => p.Comments)
                            .Where(a => a.Comments!=null && a.Comments.Any()).ToListAsync();
            return posts;
        }

        public async Task UpdateAsync(PostEntity post)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            context.Posts.Update(post);
            _ = await context.SaveChangesAsync();
        }
    }
}

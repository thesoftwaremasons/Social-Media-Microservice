﻿using Post.Query.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Query.Domain.Domain
{
    public interface IPostRepository
    {
        Task CreateAsync(PostEntity post);
        Task UpdateAsync(PostEntity post);
        Task DeleteAsync(Guid postId);
        Task<PostEntity> GetByIdAsync(Guid postId);

        Task<List<PostEntity>> ListAllAsync();
        Task<List<PostEntity>> ListAuthorAsync(string author);
        Task<List<PostEntity>> ListWithLiskesAsync(int numberOfLikes);
       // Task<List<PostEntity>> ListWithPostsAsync();
        Task<List<PostEntity>> ListWithCommentAsync();




    }
}

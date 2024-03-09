using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using classroomLibrary.Data.Models;
using classroomLibrary.ViewModels;
using classroomLibrary.Domain.Response;

namespace classroomLibrary.Services.Interfaces
{
    public interface IPostsService
    {
        IBaseResponse<List<Post>> GetPosts();

        Task<IBaseResponse<PostViewModel>> GetPost(long id);

        Task<IBaseResponse<Post>> Create(PostViewModel post);

        Task<IBaseResponse<bool>> Delete(long id);

        Task<IBaseResponse<Post>> Edit(long id, PostViewModel model);
    }
}

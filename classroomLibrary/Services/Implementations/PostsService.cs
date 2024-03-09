using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using classroomLibrary.Services.Interfaces;
using classroomLibrary.Data.Models;
using classroomLibrary.Data.Interfaces;
using classroomLibrary.ViewModels;
using classroomLibrary.Domain.Response;
using Microsoft.EntityFrameworkCore;
using classroomLibrary.Domain.Enum;

namespace classroomLibrary.Services.Implementations
{
    public class PostsService:IPostsService
    {
        private readonly IPosts _postsRepository;
        public PostsService(IPosts PostRepository)
        {
            _postsRepository = PostRepository;
        }

        public async Task<IBaseResponse<PostViewModel>> GetPost(long id)
        {
            try
            {
                var Post = await _postsRepository.GetAll().Include(v => v.workers).FirstOrDefaultAsync(x => x.id == id);
                if (Post == null)
                {
                    return new BaseResponse<PostViewModel>()
                    {
                        Description = "Город не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                var data = new PostViewModel()
                {
                    title = Post.title,
                    departmentId = Post.departmentId,
                    id = Post.id,
                    department = Post.department,
                    workers = Post.workers, 
                };

                return new BaseResponse<PostViewModel>()
                {
                    StatusCode = StatusCode.OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<PostViewModel>()
                {
                    Description = $"[GetPost] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Post>> Create(PostViewModel model)
        {
            try
            {
                var Post = new Post()
                {
                    title = model.title,
                    departmentId = model.departmentId
                };
                await _postsRepository.Create(Post);

                return new BaseResponse<Post>()
                {
                    StatusCode = StatusCode.OK,
                    Data = Post
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Post>()
                {
                    Description = $"[Create] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<bool>> Delete(long id)
        {
            try
            {
                var Post = await _postsRepository.GetAll().FirstOrDefaultAsync(x => x.id == id);
                if (Post == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "Post not found",
                        StatusCode = StatusCode.UserNotFound,
                        Data = false
                    };
                }

                await _postsRepository.Remove(Post);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[Delete] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Post>> Edit(long id, PostViewModel model)
        {
            try
            {
                var Post = await _postsRepository.GetAll().FirstOrDefaultAsync(x => x.id == id);
                if (Post == null)
                {
                    return new BaseResponse<Post>()
                    {
                        Description = "Post not found",
                        StatusCode = StatusCode.ObjectNotFound
                    };
                }

                Post.title = model.title;
                Post.departmentId = model.departmentId;

                await _postsRepository.Edit(Post);


                return new BaseResponse<Post>()
                {
                    Data = Post,
                    StatusCode = StatusCode.OK,
                };
                // TypeCar
            }
            catch (Exception ex)
            {
                return new BaseResponse<Post>()
                {
                    Description = $"[Edit] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public IBaseResponse<List<Post>> GetPosts()
        {
            try
            {
                var posts = _postsRepository.GetAll().ToList();
                if (!posts.Any())
                {
                    return new BaseResponse<List<Post>>()
                    {
                        Description = "Найдено 0 элементов",
                        Data=posts,
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<List<Post>>()
                {
                    Data = posts,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Post>>()
                {
                    Description = $"[Getposts] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}

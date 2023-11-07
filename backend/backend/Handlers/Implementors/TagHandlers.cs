using AutoMapper;
using Azure;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.Implementors;
using backend.Repositories.IRepositories;
using backend.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace backend.Handlers.Implementors
{
    public class TagHandlers : ITagHandlers
    {
        private readonly IUserRepository _userRepository;
        private readonly IVideoHandlers _videoHandlers;
        private readonly IImageHandlers _imageHandlers;
        private readonly ITagRepository _tagRepository;
        private readonly ICategoryTagRepository _categoryTagRepository;
        private readonly IPostCategoryRepository _postCategoryRepository;
        private readonly IPostTagRepository _postTagRepository;
        private readonly IVotePostRepository _votePostRepository;
        private readonly IMapper _mapper;

        public TagHandlers(IUserRepository userRepository,
                                IVideoHandlers videoHandlers,
                                IImageHandlers imageHandlers,
                                ITagRepository tagRepository,
                                ICategoryTagRepository categoryTagRepository,
                                IPostCategoryRepository postCategoryRepository,
                                IPostTagRepository postTagRepository,
                                IVotePostRepository votePostRepository,
                                IMapper mapper)
        {
            _userRepository = userRepository;
            _videoHandlers = videoHandlers;
            _imageHandlers = imageHandlers;
            _tagRepository = tagRepository;
            _categoryTagRepository = categoryTagRepository;
            _postCategoryRepository = postCategoryRepository;
            _postTagRepository = postTagRepository;
            _votePostRepository = votePostRepository;
            _mapper = mapper;
        }

        public ICollection<TagDTO> GetTags()
        {
            var tags = _tagRepository.GetAllTags();
            List<TagDTO> result = _mapper.Map<List<TagDTO>>(tags);

            //get related data for all tag
            foreach ( var tag in result )
            {
                var getCategories = _mapper.Map<ICollection<CategoryDTO>?>(_categoryTagRepository.GetCategoriesOf(tag.Id));
                tag.Categories = (getCategories is not null && getCategories.Count > 0) ? getCategories : new List<CategoryDTO>();
            }

            return result;
        }

        public ICollection<TagDTO> GetDisableTags()
        {
            var tags = _tagRepository.GetDisableTags();
            return _mapper.Map<List<TagDTO>>(tags);
        }

        public TagDTO? GetTagById(int tagId)
        {
            var tag = _tagRepository.GetTagById(tagId);
            if (tag == null) return null;

            return _mapper.Map<TagDTO>(tag);
        }

        public TagDTO? GetTagByName(string tagName)
        {
            var tag = _tagRepository.GetTagByName(tagName);
            if (tag == null) return null;

            return _mapper.Map<TagDTO>(tag);
        }

        public ICollection<PostDTO>? GetPostsByTag(int tagId)
        {
            var tag = _tagRepository.GetTagById(tagId);
            if (tag == null || tag.Status == false) return null;

            var posts = _tagRepository.GetPostsByTag(tagId);
            if (posts == null || posts.Count == 0) return null;

            //map to list DTO
            List<PostDTO> resultList = _mapper.Map<List<PostDTO>>(posts);

            //get related data for all post
            foreach (var post in resultList)
            {
                var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUserByPostID(post.Id));
                post.User = (getUser is not null && getUser.Status) ? getUser : null;

                var getCategories = _mapper.Map<ICollection<CategoryDTO>?>(_postCategoryRepository.GetCategoriesOf(post.Id));
                post.Categories = (getCategories is not null && getCategories.Count > 0) ? getCategories : new List<CategoryDTO>();

                var getTags = _mapper.Map<ICollection<TagDTO>?>(_postTagRepository.GetTagsOf(post.Id));
                post.Tags = (getTags is not null && getTags.Count > 0) ? getTags : new List<TagDTO>();

                var getImages = _imageHandlers.GetImagesByPost(post.Id);
                post.Images = (getImages is not null && getImages.Count > 0) ? getImages : new List<ImageDTO>();

                var getVideos = _videoHandlers.GetVideosByPost(post.Id);
                post.Videos = (getVideos is not null && getVideos.Count > 0) ? getVideos : new List<VideoDTO>();

                var postUpvote = _votePostRepository.GetAllUsersVotedBy(post.Id);
                post.Upvotes = (postUpvote == null || postUpvote.Count == 0) ? 0 : postUpvote.Count;
            }

            //return posts'list
            return resultList;
        }

        public ICollection<CategoryDTO>? GetCategoriesByTag(int tagId)
        {
            var tag = _tagRepository.GetTagById(tagId);
            if (tag == null || tag.Status == false) return null;

            var categories = _tagRepository.GetCategoriesByTag(tagId);
            return _mapper.Map<List<CategoryDTO>>(categories);
        }


        public TagDTO? CreateTag(int adminId, int categoryId, string tagName)
        {
            // Create a new tag object
            var tag = new Tag()
            {
                AdminId = adminId,
                TagName = tagName,
                CreatedAt = DateTime.Now,
                Status = true,
            };

            // Create tag, and return the mapped tag DTO if succeed.
            if (_tagRepository.CreateTag(tag)) return _mapper.Map<TagDTO>(tag);

            // Otherwise, return null.
            return null;
        }

        public TagDTO? UpdateTag(int currentTagId, string newTagName)
        {
            // Find tag and categoryTag
            var tag = _tagRepository.GetTagById(currentTagId);
            if (tag == null || tag.Status == false) return null;

            // Set new TagName and UpdatedAt
            tag.TagName = newTagName;
            tag.UpdatedAt = DateTime.Now;

            // Return the mapped tag DTO if all updates succeeded, otherwise return null.
            return _tagRepository.UpdateTag(tag) ? _mapper.Map<TagDTO>(tag) : null;
        }

        public TagDTO? EnableTag(int tagId)
        {
            // Find tag and categoryTag
            var tag = _tagRepository.GetTagById(tagId);
            var categoryTags = _categoryTagRepository.GetCategoryTagsByTagId(tag.Id);
            var postTags = _postTagRepository.GetPostTagsByTagId(tag.Id);
            if (tag == null || tag.Status == true || categoryTags == null) return null;

            // Check if all enables succeeded.
            var checkTag = _tagRepository.EnableTag(tag);
            var checkCategoryTag = categoryTags.All(categoryTag => _categoryTagRepository.EnableCategoryTag(categoryTag));
            var checkPostTag = postTags.All(postTag => _postTagRepository.EnablePostTag(postTag));

            // Return the mapped tag DTO if all enables succeeded, otherwise return null.
            return (checkTag && checkCategoryTag && checkPostTag) ? _mapper.Map<TagDTO>(tag) : null;
        }

        public TagDTO? DisableTag(int tagId)
        {
            // Find tag and categoryTag
            var tag = _tagRepository.GetTagById(tagId);
            var categoryTags = _categoryTagRepository.GetCategoryTagsByTagId(tag.Id);
            var postTags = _postTagRepository.GetPostTagsByTagId(tag.Id);
            if (tag == null || tag.Status == false || categoryTags == null) return null;

            // Check if all disables succeeded.
            var checkTag = _tagRepository.DisableTag(tag);
            var checkCategoryTag = categoryTags.All(categoryTag => _categoryTagRepository.DisableCategoryTag(categoryTag));
            var checkPostTag = postTags.All(postTag => _postTagRepository.DisablePostTag(postTag));

            // Return the mapped tag DTO if all disables succeeded, otherwise return null.
            return (checkTag && checkCategoryTag && checkPostTag) ? _mapper.Map<TagDTO>(tag) : null;
        }

        public TagDTO? CreateCategoryTag(TagDTO tag, CategoryDTO category)
        {
            // If relationship exists, then return null.
            var isExists = _categoryTagRepository.GetCategoryTag(tag.Id, category.Id);
            if (isExists != null && isExists.Status) return null;

            // If relationship is disabled, enable it and return it.
            if (isExists != null && !isExists.Status)
            {
                _categoryTagRepository.EnableCategoryTag(isExists);
                return _mapper.Map<TagDTO>(tag);
            }

            // Create a new categoryTag object if isExists is null, or return isExists otherwise.
            var categoryTag = new CategoryTag()
            {
                CategoryId = category.Id,
                TagId = tag.Id,
                Status = true
            };

            // Add relationship
            if (_categoryTagRepository.CreateCategoryTag(categoryTag))
                return _mapper.Map<TagDTO>(tag);

            return null;
        }

        public TagDTO? DisableCategoryTag(int tagId, int categoryId)
        {
            var tag = _tagRepository.GetTagById(tagId);
            var categoryTag = _categoryTagRepository.GetCategoryTag(tagId, categoryId);
            if (categoryTag == null || categoryTag.Status == false) return null;

            if (_categoryTagRepository.DisableCategoryTag(categoryTag))
                return _mapper.Map<TagDTO>(tag);

            return null;
        }

        public TagDTO? CreatePostTag(PostDTO post, TagDTO tag)
        {
            // If relationship exists, then return null.
            var isExists = _postTagRepository.GetPostTag(post.Id, tag.Id);
            if (isExists != null && isExists.Status) return null;

            // If relationship is disabled, enable it and return it.
            if (isExists != null && !isExists.Status)
            {
                _postTagRepository.EnablePostTag(isExists);
                return _mapper.Map<TagDTO>(tag);
            }

            // Create a new postTag object if isExists is null, or return isExists otherwise.
            var postTag = new PostTag()
            {
                PostId = post.Id,
                TagId = tag.Id,
                Status = true
            };

            // Add relationship
            if (_postTagRepository.CreatePostTag(postTag))
                return _mapper.Map<TagDTO>(tag);

            return null;
        }

        public TagDTO? DisablePostTag(int postId, int tagId)
        {
            var tag = _tagRepository.GetTagById(tagId);
            var postTag = _postTagRepository.GetPostTag(postId, tagId);
            if (postTag == null) return null;
            // || postTag.Status == false
            if (_postTagRepository.DisablePostTag(postTag))
                return _mapper.Map<TagDTO>(tag);

            return null;
        }
        public ICollection<TagDTO>? GetTop5Tags()
        {

            var tagList = GetTags();
            if (tagList == null || tagList.Count == 0)
            {
                return null;
            }
            var map = new Dictionary<TagDTO, int>();
            foreach (var tag in tagList)
            {
                if (tag.Status)
                {
                    int topVotePosts = 0;
                    var postList = _tagRepository.GetPostsByTag(tag.Id);
                    if (postList == null || postList.Count == 0)
                    {

                    }
                    else
                    {
                        foreach (var post in postList)
                        {
                            var votePost = _votePostRepository.GetAllUsersVotedBy(post.Id);
                            if (votePost == null)
                            {
                            }
                            if (votePost.Count > topVotePosts)
                            {
                                topVotePosts = votePost.Count;
                            }
                        }
                        map.Add(tag, topVotePosts);
                    }
                }
            }
            var sortedMap = map.OrderByDescending(p => p.Value)
                           .ToDictionary(pair => pair.Key, pair => pair.Value);
            List<TagDTO> keysList = new List<TagDTO>(sortedMap.Keys);
            if (keysList == null || keysList.Count == 0)
            {
                return null;
            }
            foreach(var tag in keysList)
            {
                if (tag.Status)
                {
                    var categories = GetCategoriesByTag(tag.Id);
                    if(categories == null || categories.Count == 0)
                    {
                    }
                    else
                    {
                        tag.Categories = categories;
                    }
                }
            }
            if (keysList.Count > 5)
            {
                keysList = keysList.GetRange(0, 5);
            }
            return keysList;
        }
    }
}

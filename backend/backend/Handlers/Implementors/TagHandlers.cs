using AutoMapper;
using Azure;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.Implementors;
using backend.Repositories.IRepositories;
using backend.Utils;
using Microsoft.EntityFrameworkCore;

namespace backend.Handlers.Implementors
{
    public class TagHandlers : ITagHandlers
    {
        private readonly ITagRepository _tagRepository;
        private readonly ICategoryTagRepository _categoryTagRepository;
        private readonly IPostTagRepository _postTagRepository;
        private readonly IMapper _mapper;

        public TagHandlers(ITagRepository tagRepository,
                           ICategoryTagRepository categoryTagRepository,
                           IPostTagRepository postTagRepository,
                           IMapper mapper)
        {
            _tagRepository = tagRepository;
            _categoryTagRepository = categoryTagRepository;
            _postTagRepository = postTagRepository;
            _mapper = mapper;
        }

        public ICollection<TagDTO> GetTags()
        {
            var categories = _tagRepository.GetAllTags();
            return _mapper.Map<List<TagDTO>>(categories);
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
            return _mapper.Map<List<PostDTO>>(posts);
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
            if (postTag == null || postTag.Status == false) return null;

            if (_postTagRepository.DisablePostTag(postTag))
                return _mapper.Map<TagDTO>(tag);

            return null;
        }
    }
}

using AutoMapper;
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
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryTagRepository _categoryTagRepository;
        private readonly IPostTagRepository _postTagRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly UserRoleConstrant _userRoleConstrant;

        public TagHandlers(ITagRepository tagRepository,
                           ICategoryRepository categoryRepository,
                           ICategoryTagRepository categoryTagRepository,
                           IPostTagRepository postTagRepository,
                           IUserRepository userRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _categoryRepository = categoryRepository;
            _categoryTagRepository = categoryTagRepository;
            _postTagRepository = postTagRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _userRoleConstrant = new();
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
            if (tag == null || tag.Status == false) return null;

            return _mapper.Map<TagDTO>(tag);
        }

        public TagDTO? GetTagByName(string tagName)
        {
            var tag = _tagRepository.GetTagByName(tagName);
            if (tag == null || tag.Status == false) return null;

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
            // Find admin and category
            var admin = _userRepository.GetUser(adminId);
            var adminRole = _userRoleConstrant.GetAdminRole();
            var category = _categoryRepository.GetCategoryById(categoryId);
            // Check if admin and category exist, and if admin has the correct role.
            if (admin == null || !admin.Role.Equals(adminRole) || category == null) return null;

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

        public TagDTO? UpdateTag(string currentTagName, string newTagName)
        {
            // Find tag and categoryTag
            var tag = _tagRepository.GetTagByName(currentTagName);
            var categoryTags = _categoryTagRepository.GetCategoryTagsByTagId(tag.Id);
            if (tag == null || tag.Status == false || categoryTags == null) return null;

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

        public TagDTO? CreateRelationship(int tagId, int categoryId)
        {
            // Check if category and tag is null
            var category = _categoryRepository.GetCategoryById(categoryId);
            var tag = _tagRepository.GetTagById(tagId);
            if (category == null && tag == null) return null;

            // Is there already a relationship
            var isExists = _categoryTagRepository.GetCategoryTag(tagId, categoryId);

            // If relationship exists and is disabled, enable it and return it.
            if (isExists.Status == false && _categoryTagRepository.EnableCategoryTag(isExists))
                return _mapper.Map<TagDTO>(category);

            // Create a new categoryTag object if isExists is null, or return isExists otherwise.
            var categoryTag = isExists ?? new CategoryTag()
            {
                CategoryId = category.Id,
                TagId = tag.Id,
                Status = true
            };

            // Add relationship
            if (_categoryTagRepository.CreateCategoryTag(categoryTag))
                return _mapper.Map<TagDTO>(category);

            return null;
        }
        public TagDTO? DisableRelationship(int tagId, int categoryId)
        {
            var categoryTag = _categoryTagRepository.GetCategoryTag(tagId, categoryId);

            if (categoryTag.Status == false) return null;

            if (_categoryTagRepository.DisableCategoryTag(categoryTag))
                return _mapper.Map<TagDTO>(categoryTag);

            return null;
        }
    }
}

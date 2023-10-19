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
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly UserRoleConstrant _userRoleConstrant;

        public TagHandlers(ITagRepository tagRepository, 
                           ICategoryRepository categoryRepository, 
                           ICategoryTagRepository categoryTagRepository, 
                           IUserRepository userRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _categoryRepository = categoryRepository;
            _categoryTagRepository = categoryTagRepository;
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

            // Find tag by name
            var tagExists = _tagRepository.GetTagByName(tagName);
            // Create a new tag object if tagExists is null, or return tagExists otherwise.
            var tag = tagExists ?? new Tag()
            {
                AdminId = adminId,
                TagName = tagName,
                CreatedAt = DateTime.Now,
                Status = true,
            };

            // Create a new category tag object.
            var categoryTag = new CategoryTag()
            {
                Category = category,
                Tag = tag
            };

            // Create the tag and category tag objects, and return the mapped tag DTO if both operations succeed.
            if (_tagRepository.CreateTag(tag) && _categoryTagRepository.CreateCategoryTag(categoryTag))
            {
                return _mapper.Map<TagDTO>(tag);
            }

            // Otherwise, return null.
            return null;
        }

        public TagDTO? UpdateTag(string currentTagName, string newTagName)
        {
            var tag = _tagRepository.GetTagByName(currentTagName);
            if (tag == null || tag.Status == false) return null;

            // If update succeed then return true, else return false
            tag.TagName = newTagName;
            tag.UpdatedAt = DateTime.Now;

            // If update succeed then return tag, else return null
            if (_tagRepository.UpdateTag(tag))
                return _mapper.Map<TagDTO>(tag);

            return null;
        }

        public TagDTO? EnableTag(int tagId)
        {
            var tag = _tagRepository.GetTagById(tagId);
            if (tag == null || tag.Status == true) return null;

            // If enable succeed then return tag, else return null
            if (_tagRepository.EnableTag(tag))
                return _mapper.Map<TagDTO>(tag);

            return null;
        }

        public TagDTO? DisableTag(int tagId)
        {
            var tag = _tagRepository.GetTagById(tagId);
            if (tag == null || tag.Status == false) return null;

            // If disable succeed then return tag, else return null
            if (_tagRepository.DisableTag(tag))
                return _mapper.Map<TagDTO>(tag);

            return null;
        }
    }
}

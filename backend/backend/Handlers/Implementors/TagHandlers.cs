using AutoMapper;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.Implementors;
using backend.Repositories.IRepositories;

namespace backend.Handlers.Implementors
{
    public class TagHandlers : ITagHandlers
    {
        private readonly ITagRepository _tagRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public TagHandlers(ITagRepository tagRepository, IUserRepository userRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _userRepository = userRepository;
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


        public bool CreateTag(int adminId, int categoryId, string tagName)
        {
            // Find admin
            var admin = _userRepository.GetUser(adminId);
            if (admin == null || admin.Role != "Admin") return false;

            // Find tag by name
            var tagExists = _tagRepository.GetTagByName(tagName);
            if (tagExists == null)
            {
                Tag tag = new()
                {
                    AdminId = adminId,
                    TagName = tagName,
                    CreatedAt = DateTime.Now,
                    Status = true
                };
                // If create succeed then return true, else return false
                return _tagRepository.CreateTag(categoryId, tag); 
            }

            // If tag was disabled, set status to true
            if (tagExists.Status == false)
            {
                tagExists.Status = true;
                return _tagRepository.EnableTag(tagExists);
            }

            return false;
        }

        public bool UpdateTag(string currentTagName, string newTagName)
        {
            var tag = _tagRepository.GetTagByName(currentTagName);
            if (tag == null || tag.Status == false) return false;

            // If update succeed then return true, else return false
            tag.TagName = newTagName;
            tag.UpdatedAt = DateTime.Now;
            return _tagRepository.UpdateTag(tag);
        }

        public bool EnableTag(int tagId)
        {
            var tag = _tagRepository.GetTagById(tagId);
            if (tag == null || tag.Status == true) return true;

            // If enable succeed then return true, else return false
            return _tagRepository.EnableTag(tag);
        }

        public bool DisableTag(int tagId)
        {
            var tag = _tagRepository.GetTagById(tagId);
            if (tag == null || tag.Status == false) return false;

            // If disable succeed then return true, else return false
            return _tagRepository.DisableTag(tag);
        }
    }
}

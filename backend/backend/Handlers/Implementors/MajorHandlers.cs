using AutoMapper;
using Azure;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.Implementors;
using backend.Repositories.IRepositories;
using backend.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace backend.Handlers.Implementors
{
    public class MajorHandlers : IMajorHandlers
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IImageHandlers _imageHandlers;
        private readonly IVideoHandlers _videoHandlers;
        private readonly IMajorRepository _categoryRepository;
        private readonly IMajorSubjectRepository _categorySubjectRepository;
        private readonly IPostMajorRepository _postMajorRepository;
        private readonly IPostSubjectRepository _postSubjectRepository;
        private readonly IVotePostRepository _votePostRepository;
        private readonly IMapper _mapper;

        public MajorHandlers(IUserRepository userRepository,
                                IImageHandlers imageHandlers,
                                IVideoHandlers videoHandlers,
                                IMajorRepository categoryRepository,
                                IMajorSubjectRepository categorySubjectRepository,
                                IPostMajorRepository postMajorRepository,
                                IPostSubjectRepository postSubjectRepository,
                                IVotePostRepository votePostRepository,
                                IMapper mapper,
                                IPostRepository postRepository)
        {
            _userRepository = userRepository;
            _imageHandlers = imageHandlers;
            _videoHandlers = videoHandlers;
            _categoryRepository = categoryRepository;
            _categorySubjectRepository = categorySubjectRepository;
            _postMajorRepository = postMajorRepository;
            _postSubjectRepository = postSubjectRepository;
            _votePostRepository = votePostRepository;
            _mapper = mapper;
            _postRepository = postRepository;
        }

        public ICollection<MajorDTO>? GetMajors()
        {
            var categories = _categoryRepository.GetAllMajors();
            return _mapper.Map<List<MajorDTO>>(categories);
        }

        public ICollection<MajorDTO>? GetDisableMajors()
        {
            var categories = _categoryRepository.GetDisableMajors();
            return _mapper.Map<List<MajorDTO>>(categories);
        }

        public MajorDTO? GetMajorById(int categoryId)
        {
            var category = _categoryRepository.GetMajorById(categoryId);
            if (category == null) return null;

            return _mapper.Map<MajorDTO>(category);
        }

        public MajorDTO? GetMajorByName(string categoryName)
        {
            var category = _categoryRepository.GetMajorByName(categoryName);
            if (category == null) return null;

            return _mapper.Map<MajorDTO>(category);
        }

        public ICollection<PostDTO>? GetPostsByMajor(int categoryId)
        {
            var category = _categoryRepository.GetMajorById(categoryId);
            if (category == null || category.Status == false) return null;

            var posts = _categoryRepository.GetPostsByMajor(categoryId);
            if (posts == null || posts.Count == 0) return null;

            //map to list DTO
            List<PostDTO> resultList = _mapper.Map<List<PostDTO>>(posts);

            //get related data for all post
            foreach (var post in resultList)
            {
                var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUserByPostID(post.Id));
                post.User = (getUser is not null && getUser.Status) ? getUser : null;

                var getMajors = _mapper.Map<ICollection<MajorDTO>?>(_postMajorRepository.GetMajorsOf(post.Id));
                post.Majors = (getMajors is not null && getMajors.Count > 0) ? getMajors : new List<MajorDTO>();

                var getSubjects = _mapper.Map<ICollection<SubjectDTO>?>(_postSubjectRepository.GetSubjectsOf(post.Id));
                post.Subjects = (getSubjects is not null && getSubjects.Count > 0) ? getSubjects : new List<SubjectDTO>();

                var getImages= _imageHandlers.GetImagesByPost(post.Id);
                post.Images = (getImages is not null && getImages.Count > 0) ? getImages : new List<ImageDTO>();

                var getVideos = _videoHandlers.GetVideosByPost(post.Id);
                post.Videos = (getVideos is not null && getVideos.Count > 0) ? getVideos : new List<VideoDTO>();

                var postUpvote = _votePostRepository.GetAllUsersVotedBy(post.Id);
                post.Upvotes = (postUpvote == null || postUpvote.Count == 0) ? 0 : postUpvote.Count;
            }

            //return posts'list
            return resultList;
        }

        public ICollection<SubjectDTO>? GetSubjectsByMajor(int categoryId)
        {
            var category = _categoryRepository.GetMajorById(categoryId);
            if (category == null || category.Status == false) return null;

            var tags = _categoryRepository.GetSubjectsByMajor(categoryId);
            if (tags == null || tags.Count == 0) return null;

            return _mapper.Map<List<SubjectDTO>>(tags);
        }

        public MajorDTO? CreateMajor(int adminId, string categoryName)
        {
            // Create a new tacategory object 
            var category = new Major()
            {
                MajorName = categoryName,
                CreatedAt = DateTime.Now,
                Status = true,
            };

            // Create the category, and return the mapped tag DTO if succeed.
            if (_categoryRepository.CreateMajor(category))
                return _mapper.Map<MajorDTO>(category);

            // Otherwise, return null.
            return null;
        }

        public MajorDTO? UpdateMajor(int currentMajorId, string newMajorName)
        {
            // Find category and categorySubject
            var category = _categoryRepository.GetMajorById(currentMajorId);
            if (category == null || category.Status == false) return null;

            // Set new MajorName and UpdatedAt
            category.MajorName = newMajorName;
            category.UpdatedAt = DateTime.Now;

            // Return the mapped tag DTO if all updates succeeded, otherwise return null.
            return _categoryRepository.UpdateMajor(category) ? _mapper.Map<MajorDTO>(category) : null;
        }

        public MajorDTO? EnableMajor(int categoryId)
        {
            // Find category and categorySubjects and postMajors
            var category = _categoryRepository.GetMajorById(categoryId);
            var categorySubjects = _categorySubjectRepository.GetMajorSubjectsByMajorId(category.Id);
            var postMajors = _postMajorRepository.GetPostMajorsByMajorId(category.Id);
            if (category == null || category.Status == true) return null;

            // Check if all enables succeeded.
            var checkMajor = _categoryRepository.EnableMajor(category);
            var checkMajorSubject = categorySubjects.All(categorySubject => _categorySubjectRepository.EnableMajorSubject(categorySubject));
            var checkPostMajor = postMajors.All(postMajor => _postMajorRepository.EnablePostMajor(postMajor));

            // Return the mapped tag DTO if all enables succeeded, otherwise return null.
            return (checkMajor) ? _mapper.Map<MajorDTO>(category) : null;
        }

        public MajorDTO? DisableMajor(int categoryId)
        {
            // Find category and categorySubjects and postMajors
            var category = _categoryRepository.GetMajorById(categoryId);
            var categorySubjects = _categorySubjectRepository.GetMajorSubjectsByMajorId(category.Id);
            var postMajors = _postMajorRepository.GetPostMajorsByMajorId(category.Id);
            if (category == null || category.Status == false) return null;
            // Check if all disable succeeded.
            var checkMajor = _categoryRepository.DisableMajor(category);
            var checkMajorSubject = categorySubjects.All(categorySubject => _categorySubjectRepository.DisableMajorSubject(categorySubject));
            var checkPostMajor = postMajors.All(postMajor => _postMajorRepository.DisablePostMajor(postMajor));

            // Return the mapped tag DTO if all disable succeeded, otherwise return null.
            return (checkMajor) ? _mapper.Map<MajorDTO>(category) : null;
        }

        public MajorDTO? CreatePostMajor(PostDTO post, MajorDTO category)
        {
            // If relationship exists, then return null.
            var isExists = _postMajorRepository.GetPostMajor(post.Id, category.Id);
            if (isExists != null && isExists.Status) return null;

            // If relationship is disabled, enable it and return it.
            if (isExists != null && !isExists.Status)
            {
                _postMajorRepository.EnablePostMajor(isExists);
                return _mapper.Map<MajorDTO>(category);
            }

            // Create a new postSubject object if isExists is null, or return isExists otherwise.
            var postMajor = new PostMajor()
            {
                PostId = post.Id,
                MajorId = category.Id,
                Status = true
            };

            // Add relationship
            if (_postMajorRepository.CreatePostMajor(postMajor))
                return _mapper.Map<MajorDTO>(category);

            return null;
        }

        public MajorDTO? DisablePostMajor(int postId, int categoryId)
        {
            var category = _categoryRepository.GetMajorById(categoryId);
            var postMajor = _postMajorRepository.GetPostMajor(postId, categoryId);
            if (postMajor == null) return null;
            // || postMajor.Status == false
            if (_postMajorRepository.DisablePostMajor(postMajor))
                return _mapper.Map<MajorDTO>(category);

            return null;
        }

        public ICollection<MajorDTO>? GetTop5Majors()
        {
            var categories = GetMajors().Where(c => c.Status).ToList();

            var topMajors = categories
              .Select(c => new
              {
                  Major = c,
                  VoteCount = _votePostRepository.GetAllUsersVotedBy(c.Id).Count
              })
              .OrderByDescending(x => x.VoteCount)
              .Take(5)
              .Select(x => x.Major)
              .ToList();

            return topMajors;
        }

    }
}

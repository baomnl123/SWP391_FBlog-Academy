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
        private readonly ISubjectRepository _subjectRepository;
        private readonly IUserRepository _userRepository;
        private readonly IImageHandlers _imageHandlers;
        private readonly IVideoHandlers _videoHandlers;
        private readonly IMajorRepository _majorRepository;
        private readonly IMajorSubjectRepository _majorSubjectRepository;
        private readonly IPostMajorRepository _postMajorRepository;
        private readonly IPostSubjectRepository _postSubjectRepository;
        private readonly IVotePostRepository _votePostRepository;
        private readonly IMapper _mapper;

        public MajorHandlers(IUserRepository userRepository,
                                IImageHandlers imageHandlers,
                                IVideoHandlers videoHandlers,
                                IMajorRepository majorRepository,
                                IMajorSubjectRepository majorSubjectRepository,
                                IPostMajorRepository postMajorRepository,
                                IPostSubjectRepository postSubjectRepository,
                                IVotePostRepository votePostRepository,
                                IMapper mapper,
                                IPostRepository postRepository,
                                ISubjectRepository subjectRepository)
        {
            _userRepository = userRepository;
            _imageHandlers = imageHandlers;
            _videoHandlers = videoHandlers;
            _majorRepository = majorRepository;
            _majorSubjectRepository = majorSubjectRepository;
            _postMajorRepository = postMajorRepository;
            _postSubjectRepository = postSubjectRepository;
            _votePostRepository = votePostRepository;
            _mapper = mapper;
            _subjectRepository = subjectRepository;
        }

        public ICollection<MajorDTO>? GetMajors()
        {
            var majors = _majorRepository.GetAllMajors();
            if (majors == null || majors.Count == 0) return null;
            var returnList = new List<MajorDTO>();
            foreach (var major in majors)
            {
                var majorDTO = _mapper.Map<MajorDTO>(major);
                if (!majorDTO.Status) continue;
                var getSubjects = _majorRepository.GetSubjectsByMajor(majorDTO.Id);
                if (getSubjects != null) majorDTO.Subjects = _mapper.Map<List<SubjectDTO>?>(getSubjects);
                returnList.Add(majorDTO);
            }
            return returnList;
        }

        public ICollection<MajorDTO>? GetDisableMajors()
        {
            var majors = _majorRepository.GetDisableMajors();
            return _mapper.Map<List<MajorDTO>>(majors);
        }

        public MajorDTO? GetMajorById(int majorId)
        {
            var major = _majorRepository.GetMajorById(majorId);
            if (major == null) return null;
            var majorDTO = _mapper.Map<MajorDTO>(major);
            var getSubjects = _majorRepository.GetSubjectsByMajor(majorDTO.Id);
            if (getSubjects != null) majorDTO.Subjects = _mapper.Map<List<SubjectDTO>?>(getSubjects);
            return majorDTO;
        }

        public MajorDTO? GetMajorByName(string majorName)
        {
            var major = _majorRepository.GetMajorByName(majorName);
            if (major == null) return null;

            var majorDTO = _mapper.Map<MajorDTO>(major);
            var getSubjects = _majorRepository.GetSubjectsByMajor(majorDTO.Id);
            if (getSubjects != null) majorDTO.Subjects = _mapper.Map<List<SubjectDTO>?>(getSubjects);
            return majorDTO;
        }

        public ICollection<PostDTO>? GetPostsByMajor(int majorId)
        {
            var major = _majorRepository.GetMajorById(majorId);
            if (major == null || major.Status == false) return null;

            var posts = _majorRepository.GetPostsByMajor(majorId);
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

        public ICollection<SubjectDTO>? GetSubjectsByMajor(int majorId)
        {
            var major = _majorRepository.GetMajorById(majorId);
            if (major == null || major.Status == false) return null;

            var subjects = _majorRepository.GetSubjectsByMajor(majorId);
            if (subjects == null || subjects.Count == 0) return null;

            return _mapper.Map<List<SubjectDTO>>(subjects);
        }

        public MajorDTO? CreateMajor(int adminId, string majorName)
        {
            // Create a new tamajor object 
            var major = new Major()
            {
                MajorName = majorName,
                CreatedAt = DateTime.Now,
                Status = true,
            };

            // Create the major, and return the mapped subject DTO if succeed.
            if (_majorRepository.CreateMajor(major))
                return _mapper.Map<MajorDTO>(major);

            // Otherwise, return null.
            return null;
        }

        public MajorDTO? UpdateMajor(int currentMajorId, string newMajorName)
        {
            // Find major and majorSubject
            var major = _majorRepository.GetMajorById(currentMajorId);
            if (major == null || major.Status == false) return null;

            // Set new MajorName and UpdatedAt
            major.MajorName = newMajorName;
            major.UpdatedAt = DateTime.Now;

            // Return the mapped subject DTO if all updates succeeded, otherwise return null.
            return _majorRepository.UpdateMajor(major) ? _mapper.Map<MajorDTO>(major) : null;
        }

        public MajorDTO? EnableMajor(int majorId)
        {
            // Find major and majorSubjects and postMajors
            var major = _majorRepository.GetMajorById(majorId);
            var majorSubjects = _majorSubjectRepository.GetMajorSubjectsByMajorId(major.Id);
            var postMajors = _postMajorRepository.GetPostMajorsByMajorId(major.Id);
            if (major == null || major.Status == true) return null;

            // Check if all enables succeeded.
            var checkMajor = _majorRepository.EnableMajor(major);
            var checkMajorSubject = majorSubjects.All(majorSubject => _majorSubjectRepository.EnableMajorSubject(majorSubject));
            var checkPostMajor = postMajors.All(postMajor => _postMajorRepository.EnablePostMajor(postMajor));

            // Return the mapped subject DTO if all enables succeeded, otherwise return null.
            return (checkMajor) ? _mapper.Map<MajorDTO>(major) : null;
        }

        public MajorDTO? DisableMajor(int majorId)
        {
            // Find major and majorSubjects and postMajors
            var major = _majorRepository.GetMajorById(majorId);
            var majorSubjects = _majorSubjectRepository.GetMajorSubjectsByMajorId(major.Id);
            var postMajors = _postMajorRepository.GetPostMajorsByMajorId(major.Id);
            if (major == null || major.Status == false) return null;
            // Check if all disable succeeded.
            var checkMajor = _majorRepository.DisableMajor(major);
            var checkMajorSubject = majorSubjects.All(majorSubject => _majorSubjectRepository.DisableMajorSubject(majorSubject));
            var checkPostMajor = postMajors.All(postMajor => _postMajorRepository.DisablePostMajor(postMajor));

            // Return the mapped subject DTO if all disable succeeded, otherwise return null.
            return (checkMajor) ? _mapper.Map<MajorDTO>(major) : null;
        }

        public MajorDTO? CreatePostMajor(PostDTO post, MajorDTO major)
        {
            // If relationship exists, then return null.
            var isExists = _postMajorRepository.GetPostMajor(post.Id, major.Id);
            if (isExists != null && isExists.Status) return null;

            // If relationship is disabled, enable it and return it.
            if (isExists != null && !isExists.Status)
            {
                _postMajorRepository.EnablePostMajor(isExists);
                return _mapper.Map<MajorDTO>(major);
            }

            // Create a new postSubject object if isExists is null, or return isExists otherwise.
            var postMajor = new PostMajor()
            {
                PostId = post.Id,
                MajorId = major.Id,
                Status = true
            };

            // Add relationship
            if (_postMajorRepository.CreatePostMajor(postMajor))
                return _mapper.Map<MajorDTO>(major);

            return null;
        }

        public MajorDTO? DisablePostMajor(int postId, int majorId)
        {
            var major = _majorRepository.GetMajorById(majorId);
            var postMajor = _postMajorRepository.GetPostMajor(postId, majorId);
            if (postMajor == null) return null;
            // || postMajor.Status == false
            if (_postMajorRepository.DisablePostMajor(postMajor))
                return _mapper.Map<MajorDTO>(major);

            return null;
        }

        public ICollection<MajorDTO>? GetTop5Majors()
        {
            var majors = GetMajors().Where(c => c.Status).ToList();

            var topMajors = majors
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

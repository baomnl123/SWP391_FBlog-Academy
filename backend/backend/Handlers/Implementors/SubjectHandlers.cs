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
    public class SubjectHandlers : ISubjectHandlers
    {
        private readonly IUserRepository _userRepository;
        private readonly IImageHandlers _imageHandlers;
        private readonly IVideoHandlers _videoHandlers;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IMajorSubjectRepository _majorSubjectRepository;
        private readonly IPostMajorRepository _postMajorRepository;
        private readonly IPostSubjectRepository _postSubjectRepository;
        private readonly IVotePostRepository _votePostRepository;
        private readonly IMapper _mapper;

        public SubjectHandlers(IUserRepository userRepository,
                                IImageHandlers imageHandlers,
                                IVideoHandlers videoHandlers,
                                ISubjectRepository subjectRepository,
                                IMajorSubjectRepository majorSubjectRepository,
                                IPostMajorRepository postMajorRepository,
                                IPostSubjectRepository postSubjectRepository,
                                IVotePostRepository votePostRepository,
                                IMapper mapper)
        {
            _userRepository = userRepository;
            _imageHandlers = imageHandlers;
            _videoHandlers = videoHandlers;
            _subjectRepository = subjectRepository;
            _majorSubjectRepository = majorSubjectRepository;
            _postMajorRepository = postMajorRepository;
            _postSubjectRepository = postSubjectRepository;
            _votePostRepository = votePostRepository;
            _mapper = mapper;
        }

        public ICollection<SubjectDTO> GetSubjects()
        {
            var subjects = _subjectRepository.GetAllSubjects();
            List<SubjectDTO> result = _mapper.Map<List<SubjectDTO>>(subjects);

            //get related data for all subject
            foreach (var subject in result)
            {
                var getMajors = _mapper.Map<ICollection<MajorDTO>?>(_majorSubjectRepository.GetMajorsOf(subject.Id));
                subject.Major = (getMajors is not null && getMajors.Count > 0) ? getMajors : new List<MajorDTO>();
            }

            return result;
        }

        public ICollection<SubjectDTO> GetDisableSubjects()
        {
            var subjects = _subjectRepository.GetDisableSubjects();
            return _mapper.Map<List<SubjectDTO>>(subjects);
        }

        public SubjectDTO? GetSubjectById(int subjectId)
        {
            var subject = _subjectRepository.GetSubjectById(subjectId);
            if (subject == null) return null;

            return _mapper.Map<SubjectDTO>(subject);
        }

        public SubjectDTO? GetSubjectByName(string subjectName)
        {
            var subject = _subjectRepository.GetSubjectByName(subjectName);
            if (subject == null) return null;

            return _mapper.Map<SubjectDTO>(subject);
        }

        public ICollection<PostDTO>? GetPostsBySubject(int subjectId)
        {
            var subject = _subjectRepository.GetSubjectById(subjectId);
            if (subject == null || subject.Status == false) return null;

            var posts = _subjectRepository.GetPostsBySubject(subjectId);
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

        public ICollection<MajorDTO>? GetMajorsBySubject(int subjectId)
        {
            var subject = _subjectRepository.GetSubjectById(subjectId);
            if (subject == null || subject.Status == false) return null;

            var majors = _subjectRepository.GetMajorsBySubject(subjectId);
            return _mapper.Map<List<MajorDTO>>(majors);
        }


        public SubjectDTO? CreateSubject(int adminId, int majorId, string subjectName)
        {
            // Create a new subject object
            var subject = new Subject()
            {
                SubjectName = subjectName,
                CreatedAt = DateTime.Now,
                Status = true,
            };

            // Create subject, and return the mapped subject DTO if succeed.
            if (_subjectRepository.CreateSubject(subject)) return _mapper.Map<SubjectDTO>(subject);

            // Otherwise, return null.
            return null;
        }

        public SubjectDTO? UpdateSubject(int currentSubjectId, string newSubjectName)
        {
            // Find subject and majorSubject
            var subject = _subjectRepository.GetSubjectById(currentSubjectId);
            if (subject == null || subject.Status == false) return null;

            // Set new SubjectName and UpdatedAt
            subject.SubjectName = newSubjectName;
            subject.UpdatedAt = DateTime.Now;

            // Return the mapped subject DTO if all updates succeeded, otherwise return null.
            return _subjectRepository.UpdateSubject(subject) ? _mapper.Map<SubjectDTO>(subject) : null;
        }

        public SubjectDTO? EnableSubject(int subjectId)
        {
            // Find subject and majorSubject
            var subject = _subjectRepository.GetSubjectById(subjectId);
            var majorSubjects = _majorSubjectRepository.GetMajorSubjectsBySubjectId(subject.Id);
            var postSubjects = _postSubjectRepository.GetPostSubjectsBySubjectId(subject.Id);
            if (subject == null || subject.Status == true || majorSubjects == null) return null;

            // Check if all enables succeeded.
            var checkSubject = _subjectRepository.EnableSubject(subject);
            var checkMajorSubject = majorSubjects.All(majorSubject => _majorSubjectRepository.EnableMajorSubject(majorSubject));
            var checkPostSubject = postSubjects.All(postSubject => _postSubjectRepository.EnablePostSubject(postSubject));

            // Return the mapped subject DTO if all enables succeeded, otherwise return null.
            return (checkSubject && checkMajorSubject && checkPostSubject) ? _mapper.Map<SubjectDTO>(subject) : null;
        }

        public SubjectDTO? DisableSubject(int subjectId)
        {
            // Find subject and majorSubject
            var subject = _subjectRepository.GetSubjectById(subjectId);
            var majorSubjects = _majorSubjectRepository.GetMajorSubjectsBySubjectId(subject.Id);
            var postSubjects = _postSubjectRepository.GetPostSubjectsBySubjectId(subject.Id);
            if (subject == null || subject.Status == false || majorSubjects == null) return null;

            // Check if all disables succeeded.
            var checkSubject = _subjectRepository.DisableSubject(subject);
            var checkMajorSubject = majorSubjects.All(majorSubject => _majorSubjectRepository.DisableMajorSubject(majorSubject));
            var checkPostSubject = postSubjects.All(postSubject => _postSubjectRepository.DisablePostSubject(postSubject));

            // Return the mapped subject DTO if all disables succeeded, otherwise return null.
            return (checkSubject && checkMajorSubject && checkPostSubject) ? _mapper.Map<SubjectDTO>(subject) : null;
        }

        public SubjectDTO? CreateMajorSubject(SubjectDTO subject, MajorDTO major)
        {
            // If relationship exists, then return null.
            var isExists = _majorSubjectRepository.GetMajorSubject(subject.Id, major.Id);
            if (isExists != null && isExists.Status) return null;

            // If relationship is disabled, enable it and return it.
            if (isExists != null && !isExists.Status)
            {
                _majorSubjectRepository.EnableMajorSubject(isExists);
                return _mapper.Map<SubjectDTO>(subject);
            }

            // Create a new majorSubject object if isExists is null, or return isExists otherwise.
            var majorSubject = new MajorSubject()
            {
                MajorId = major.Id,
                SubjectId = subject.Id,
                Status = true
            };

            // Add relationship
            if (_majorSubjectRepository.CreateMajorSubject(majorSubject))
                return _mapper.Map<SubjectDTO>(subject);

            return null;
        }

        public SubjectDTO? DisableMajorSubject(int subjectId, int majorId)
        {
            var subject = _subjectRepository.GetSubjectById(subjectId);
            var majorSubject = _majorSubjectRepository.GetMajorSubject(subjectId, majorId);
            if (majorSubject == null || majorSubject.Status == false) return null;

            if (_majorSubjectRepository.DisableMajorSubject(majorSubject))
                return _mapper.Map<SubjectDTO>(subject);

            return null;
        }

        public SubjectDTO? CreatePostSubject(PostDTO post, SubjectDTO subject)
        {
            // If relationship exists, then return null.
            var isExists = _postSubjectRepository.GetPostSubject(post.Id, subject.Id);
            if (isExists != null && isExists.Status) return null;

            // If relationship is disabled, enable it and return it.
            if (isExists != null && !isExists.Status)
            {
                _postSubjectRepository.EnablePostSubject(isExists);
                return _mapper.Map<SubjectDTO>(subject);
            }

            // Create a new postSubject object if isExists is null, or return isExists otherwise.
            var postSubject = new PostSubject()
            {
                PostId = post.Id,
                SubjectId = subject.Id,
                Status = true
            };

            // Add relationship
            if (_postSubjectRepository.CreatePostSubject(postSubject))
                return _mapper.Map<SubjectDTO>(subject);

            return null;
        }

        public SubjectDTO? DisablePostSubject(int postId, int subjectId)
        {
            var subject = _subjectRepository.GetSubjectById(subjectId);
            var postSubject = _postSubjectRepository.GetPostSubject(postId, subjectId);
            if (postSubject == null) return null;
            // || postSubject.Status == false
            if (_postSubjectRepository.DisablePostSubject(postSubject))
                return _mapper.Map<SubjectDTO>(subject);

            return null;
        }

        public ICollection<SubjectDTO>? GetTop5Subjects()
        {

            var subjects = GetSubjects().Where(c => c.Status).ToList();

            var topSubjects = subjects
                .Select(c => new
                {
                    Subject = c,
                    VoteCount = _votePostRepository.GetAllUsersVotedBy(c.Id).Count
                })
                .OrderByDescending(x => x.VoteCount)
                .Take(5)
                .Select(x => x.Subject)
                .ToList();

            return topSubjects;
        }
    }
}

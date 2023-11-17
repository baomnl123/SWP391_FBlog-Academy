using AutoMapper;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.IRepositories;

namespace backend.Handlers.Implementors
{
    public class UserSubjectHandlers : IUserSubjectHandlers
    {
        private readonly IUserHandlers _userHandlers;
        private readonly ISubjectHandlers _subjectHandlers;
        private readonly IUserSubjectRepository _userSubjectRepository;
        private readonly IMapper _mapper;
        public UserSubjectHandlers(IUserHandlers userHandlers, ISubjectHandlers subjectHandlers, IMapper mapper,IUserSubjectRepository userSubjectRepository)
        {
            _userHandlers = userHandlers;
            _subjectHandlers = subjectHandlers;
            _userSubjectRepository = userSubjectRepository;
            _mapper = mapper;
        }
        public UserSubjectDTO? AddUserSubject(int currentUserID, int subjectID)
        {
            //check user
            var getUser = _userHandlers.GetUser(currentUserID);
            if (getUser == null || !getUser.Status) return null;

            //check subject
            var getSubject = _subjectHandlers.GetSubjectById(subjectID);
            if (getSubject == null || !getSubject.Status) return null;

            //check user subject
            var getUserSubject = _userSubjectRepository.GetBy(currentUserID, subjectID);
            if(getUserSubject != null)
            {
                if (getUserSubject.Status) return null; 
                getUserSubject.Status = true;

                if(!_userSubjectRepository.Update(getUserSubject)) return null;

                var userSubjectDTO = _mapper.Map<UserSubjectDTO>(getUserSubject);
                userSubjectDTO.User = getUser;
                userSubjectDTO.Subject = getSubject;

                return userSubjectDTO;
            }
            //init
            var newUserSubject = new UserSubject(){
                UserId = currentUserID,
                SubjectId = subjectID,
                Status = true,
            };
            //add to db
            if(!_userSubjectRepository.Add(newUserSubject)) return null;

            //map to dto
            var newUserSubjectDTO = _mapper.Map<UserSubjectDTO>(newUserSubject);
            newUserSubjectDTO.User = getUser;
            newUserSubjectDTO.Subject = getSubject;

            return newUserSubjectDTO;
        }

        public UserSubjectDTO? DeleteUserSubject(int currentUserID, int subjectID)
        {
            //check user
            var getUser = _userHandlers.GetUser(currentUserID);
            if (getUser == null || !getUser.Status) return null;

            //check subject
            var getSubject = _subjectHandlers.GetSubjectById(subjectID);
            if (getSubject == null || !getSubject.Status) return null;

            //check user subject
            var getUserSubject = _userSubjectRepository.GetBy(currentUserID, subjectID);
            if (getUserSubject == null || !getUserSubject.Status) return null;

            //disable
            getUserSubject.Status = false;

            //update to db
            if(!_userSubjectRepository.Update(getUserSubject)) return null;

            //map to dto
            var userSubjectDTO = _mapper.Map<UserSubjectDTO>(getUserSubject);
            userSubjectDTO.User = getUser;
            userSubjectDTO.Subject = getSubject;

            return userSubjectDTO;
        }

        public ICollection<SubjectDTO>? GetSubjectsOf(int userID)
        {
            //check user
            var getUser = _userHandlers.GetUser(userID);
            if (getUser == null || !getUser.Status) return null;

            var getSubjects = _userSubjectRepository.GetSubjectsOf(userID);
            if (getSubjects == null || getSubjects.Count == 0) return null;

            var listDTO = new List<SubjectDTO>();

            foreach (var subject in getSubjects)
            {
                if (!subject.Status) continue;
                var subjectDTO = _subjectHandlers.GetSubjectById(subject.Id);
                if (subjectDTO == null || !subjectDTO.Status) continue;
                listDTO.Add(subjectDTO);
            }
            if (listDTO.Count == 0) return null;
            return listDTO;
        }

        public ICollection<UserDTO>? GetUsersOf(int subjectID)
        {
            //check subject
            var getSubject = _subjectHandlers.GetSubjectById(subjectID);
            if (getSubject == null || !getSubject.Status) return null;

            var getUsers = _userSubjectRepository.GetUsersOf(subjectID);
            if (getUsers == null || getUsers.Count == 0) return null;

            var listDTO = new List<UserDTO>();

            foreach(var user in getUsers)
            {
                if (!user.Status) continue;
                var userDTO = _userHandlers.GetUser(user.Id);
                if (userDTO == null || !userDTO.Status) continue;
                listDTO.Add(userDTO);
            }
            if (listDTO.Count == 0) return null;
            return listDTO;
        }
    }
}

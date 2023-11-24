using AutoMapper;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.Implementors;
using backend.Repositories.IRepositories;

namespace backend.Handlers.Implementors
{
    public class UserSubjectHandlers : IUserSubjectHandlers
    {
        private readonly IUserHandlers _userHandlers;
        private readonly ISubjectHandlers _subjectHandlers;
        private readonly IUserSubjectRepository _userSubjectRepository;
        private readonly IMapper _mapper;
        public UserSubjectHandlers(IUserHandlers userHandlers, ISubjectHandlers subjectHandlers, IMapper mapper, IUserSubjectRepository userSubjectRepository)
        {
            _userHandlers = userHandlers;
            _subjectHandlers = subjectHandlers;
            _userSubjectRepository = userSubjectRepository;
            _mapper = mapper;
        }
        public ICollection<UserSubjectDTO>? AddUserSubject(int currentUserID, int[] subjectIDs)
        {
            //check user
            var getUser = _userHandlers.GetUser(currentUserID);
            if (getUser == null || !getUser.Status) return null;

            //check subject
            var getSubjects = _userSubjectRepository.GetSubjectsOf(currentUserID);
            if (getSubjects == null) return null;

            var returnList = new List<UserSubjectDTO>();

            foreach (var subjectid in subjectIDs)
            {
                var getSubject = _subjectHandlers.GetSubjectById(subjectid);
                if (getSubject == null || !getSubject.Status) continue;
                var getUserSubject = _userSubjectRepository.GetBy(currentUserID, subjectid);
                if (getSubjects.Any(item => item.Id == subjectid)){
                    var existedUserSubjectDTO = _mapper.Map<UserSubjectDTO>(getUserSubject);
                    existedUserSubjectDTO.User = getUser;
                    existedUserSubjectDTO.Subject = getSubject;

                    returnList.Add(existedUserSubjectDTO);
                    continue;
                }
                //check user subject
                
                if (getUserSubject != null)
                {
                    if (getUserSubject.Status) continue;
                    getUserSubject.Status = true;

                    if (!_userSubjectRepository.Update(getUserSubject)) continue;

                    var userSubjectDTO = _mapper.Map<UserSubjectDTO>(getUserSubject);
                    userSubjectDTO.User = getUser;
                    userSubjectDTO.Subject = getSubject;

                    returnList.Add(userSubjectDTO);
                }
                else
                {
                    //init
                    var newUserSubject = new UserSubject()
                    {
                        UserId = currentUserID,
                        SubjectId = subjectid,
                        Status = true,
                    };
                    //add to db
                    if (!_userSubjectRepository.Add(newUserSubject)) continue;

                    var newUserSubjectDTO = _mapper.Map<UserSubjectDTO>(newUserSubject);
                    newUserSubjectDTO.User = getUser;
                    newUserSubjectDTO.Subject = getSubject;

                    returnList.Add(newUserSubjectDTO);
                }
            }

            return returnList;
        }

        public ICollection<UserSubjectDTO>? DeleteUserSubject(int currentUserID, int[] subjectIDs)
        {
            //check user
            var getUser = _userHandlers.GetUser(currentUserID);
            if (getUser == null || !getUser.Status) return null;

            //check subject
            var getSubjects = _userSubjectRepository.GetSubjectsOf(currentUserID);
            if (getSubjects == null) return null;

            var returnList = new List<UserSubjectDTO>();

            foreach (var subjectID in subjectIDs)
            {
                var getSubject = _subjectHandlers.GetSubjectById(subjectID);
                if (getSubject == null || !getSubject.Status) continue;
                var getUserSubject = _userSubjectRepository.GetBy(currentUserID, subjectID);
                //check user subject
                if (getUserSubject != null)
                {
                    if (!getUserSubject.Status) continue;

                    getUserSubject.Status = false;

                    var userSubjectDTO = _mapper.Map<UserSubjectDTO>(getUserSubject);
                    userSubjectDTO.User = getUser;
                    userSubjectDTO.Subject = getSubject;

                    if (!_userSubjectRepository.Update(getUserSubject)) continue;

                    returnList.Add(userSubjectDTO);
                }
            }
            return returnList;
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

            foreach (var user in getUsers)
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

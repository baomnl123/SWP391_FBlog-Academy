using AutoMapper;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.IRepositories;

namespace backend.Handlers.Implementors
{
    public class UserMajorHandlers : IUserMajorHandlers
    {
        private readonly IUserHandlers _userHandlers;
        private readonly IMajorHandlers _majorHandlers;
        private readonly IUserMajorRepository _userMajorRepository;
        private readonly IMapper _mapper;
        public UserMajorHandlers(IUserHandlers userHandlers , IMajorHandlers majorHandlers,IUserMajorRepository userMajorRepository, IMapper mapper)
        {
            _userHandlers = userHandlers;
            _majorHandlers = majorHandlers;
            _userMajorRepository = userMajorRepository;
            _mapper = mapper;
        }
        public UserMajorDTO? AddUserMajor(int currentUserID, int majorID)
        {
            //check user
            var getUser = _userHandlers.GetUser(currentUserID);
            if (getUser == null || !getUser.Status) return null;

            //check major
            var getMajor = _majorHandlers.GetMajorById(majorID);
            if (getMajor == null || !getMajor.Status) return null;

            //check user major
            var getUserMajor = _userMajorRepository.GetBy(currentUserID,majorID);
            if(getUserMajor != null)
            {
                if (getUserMajor.Status) return null;
                getUserMajor.Status = true;
                if (!_userMajorRepository.Update(getUserMajor)) return null;
                
                var userMajorDTO = _mapper.Map<UserMajorDTO>(getUserMajor);
                userMajorDTO.User = getUser;
                userMajorDTO.Major = getMajor;

                return userMajorDTO;
            }
            var newUserMajor = new UserMajor()
            {
                UserId = currentUserID,
                MajorId = majorID,
                Status = true,
            };
            if(!_userMajorRepository.Add(newUserMajor)) return null;

            var newUserMajorDTO = _mapper.Map<UserMajorDTO>(newUserMajor);
            newUserMajorDTO.User = getUser;
            newUserMajorDTO.Major = getMajor;

            return newUserMajorDTO;
        }

        public UserMajorDTO? DeleteUserMajor(int currentUserID, int majorID)
        {
            //check user
            var getUser = _userHandlers.GetUser(currentUserID);
            if (getUser == null || !getUser.Status) return null;

            //check major
            var getMajor = _majorHandlers.GetMajorById(majorID);
            if (getMajor == null || !getMajor.Status) return null;

            //check user major
            var getUserMajor = _userMajorRepository.GetBy(currentUserID, majorID);
            if (getUserMajor == null || !getUserMajor.Status) return null;

            getUserMajor.Status = false;
            if (!_userMajorRepository.Update(getUserMajor)) return null;

            var userMajorDTO = _mapper.Map<UserMajorDTO>(getUserMajor);
            userMajorDTO.User = getUser;
            userMajorDTO.Major = getMajor;

            return userMajorDTO;
        }

        public ICollection<MajorDTO>? GetMajorsOf(int userID)
        {
            //check user
            var getUser = _userHandlers.GetUser(userID);
            if (getUser == null || !getUser.Status) return null;

            var getMajors = _userMajorRepository.GetMajorsOf(userID);
            if (getMajors == null || getMajors.Count == 0) return null;

            var listDTO = new List<MajorDTO>();

            foreach(var major in getMajors)
            {
                if (!major.Status) continue;

                var majorDTO = _majorHandlers.GetMajorById(major.Id);
                if (majorDTO == null || !majorDTO.Status) continue;
                listDTO.Add(majorDTO);
            }
            if (listDTO.Count == 0) return null;
            return listDTO;
        }

        public ICollection<UserDTO>? GetUsersOf(int majorID)
        {
            //check major
            var getMajor = _majorHandlers.GetMajorById(majorID);
            if (getMajor == null || !getMajor.Status) return null;

            var getUsers = _userMajorRepository.GetUsersOf(majorID);
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

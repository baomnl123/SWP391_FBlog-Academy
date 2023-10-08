using AutoMapper;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.IRepositories;
using backend.Utils;

namespace backend.Handlers.Implementors
{
    public class UserHandlers : IUserHandlers
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly UserRoleConstrant _userRoleConstrant;
        private HashingString _hashingString;
        public UserHandlers(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userRoleConstrant = new();
            _hashingString = new();
        }
        public UserDTO? CreateLecturer(string name, string email, string password)
        {
            //make sure that info is not null
            if (name == null || email == null || password == null)
            {
                return null;
            }
            //get lecture role
            var lectureRole = _userRoleConstrant.GetLecturerRole();
            //check if existed
            if (_userRepository.isExisted(email))
            {
                //get existed user
                var existedUser = _userRepository.GetUser(email);
                //check if user is enable
                if (existedUser.Status)
                {
                    return null;
                }
                //if user is disable then update the user into enable status
                existedUser.Name = name;
                existedUser.Password = password;
                existedUser.Status = true;
                existedUser.UpdatedAt = DateTime.Now;
                if (!_userRepository.UpdateUser(existedUser))
                {
                    return null;
                }
                //return
                return _mapper.Map<UserDTO>(existedUser);
            }
            //add new user
            User newUser = new()
            {
                Name = name,
                Email = email,
                Password = _hashingString.HashString(password),
                Role = lectureRole,
                CreatedAt = DateTime.Now,
                Status = true,
                IsAwarded = false,
            };
            //add to db
            if (!_userRepository.CreateUser(newUser))
            {
                return null;
            }
            //return
            return _mapper.Map<UserDTO>(newUser);
        }

        public UserDTO? CreateUser(string name, string email, string password)
        {
            //make sure that info is not null
            if (name == null || email == null || password == null)
            {
                return null;
            }
            //get student role
            var studentRole = _userRoleConstrant.GetStudentRole();
            //check if existed
            if (_userRepository.isExisted(email))
            {
                //get existed user
                var existedUser = _userRepository.GetUser(email);
                //check if user is enable
                if (existedUser.Status)
                {
                    return null;
                }
                //if user is disable then update the user into enable status
                existedUser.Name = name;
                existedUser.Password = password;
                existedUser.Status = true;
                existedUser.UpdatedAt = DateTime.Now;
                if (!_userRepository.UpdateUser(existedUser))
                {
                    return null;
                }
                //return
                return _mapper.Map<UserDTO>(existedUser);
            }
            //add new user
            User newUser = new()
            {
                Name = name,
                Email = email,
                Password = _hashingString.HashString(password),
                Role = studentRole,
                CreatedAt = DateTime.Now,
                Status = true,
                IsAwarded = false,
            };
            //add to db
            if (!_userRepository.CreateUser(newUser))
            {
                return null;
            }
            //return
            return _mapper.Map<UserDTO>(newUser);
        }

        public UserDTO? DemoteStudent(int userID)
        {
            //check if existed
            if (!_userRepository.isExisted(userID))
            {
                return null;
            }
            //get user info
            var user = _userRepository.GetUser(userID);
            //get status
            var studentRole = _userRoleConstrant.GetStudentRole();
            var moderatorRole = _userRoleConstrant.GetModeratorRole();
            //check if already demoted
            if (!user.Role.Trim().Contains(moderatorRole))
            {
                return null;
            }
            //demote user
            user.Role = studentRole;
            //update
            if (!_userRepository.UpdateUser(user))
            {
                return null;
            }
            //return
            return _mapper.Map<UserDTO>(user);
        }

        public UserDTO? DisableUser(int userID)
        {
            //get user info
            var user = _userRepository.GetUser(userID);
            //if user is not found or disabled
            if (user == null || !user.Status)
            {
                return null;
            }
            //update user status
            user.Status = false;
            user.UpdatedAt = DateTime.Now;
            if (!_userRepository.UpdateUser(user))
            {
                return null;
            }
            //return
            return _mapper.Map<UserDTO>(user);

        }

        public ICollection<UserDTO>? GetAllUsers()
        {
            //init list dto
            var listDTO = new List<UserDTO>();
            //get user list
            var list = _userRepository.GetAllUsers();
            //check if list is null of empty
            if (list == null || list.Count == 0)
            {
                return null;
            }
            foreach (var user in list)
            {
                //check user status then map to dto
                if (user.Status)
                {
                    listDTO.Add(_mapper.Map<UserDTO>(user));
                }
            }
            //return
            return listDTO;
        }

        public ICollection<UserDTO>? GetLecturers()
        {
            //init dto list
            var listDTO = new List<UserDTO>();
            //get lecture role
            var lectureRole = _userRoleConstrant.GetLecturerRole();
            //get lecture list
            var list = _userRepository.GetUsersByRole(lectureRole);
            //if list is null
            if (list == null)
            {
                return null;
            }
            foreach (var user in list)
            {
                //map if user status is true
                if (user.Status)
                {
                    listDTO.Add(_mapper.Map<UserDTO>(user));
                }
            }
            //return
            return listDTO;
        }

        public ICollection<UserDTO>? GetStudentsAndModerator()
        {
            //init dto list
            var listDTO = new List<UserDTO>();

            //get student role
            var studentRole = _userRoleConstrant.GetStudentRole();
            //get student list
            var studentList = _userRepository.GetUsersByRole(studentRole);
            //if list is null
            if (studentList != null)
            {
                foreach (var user in studentList)
                {
                    //map if user status is true
                    if (user.Status)
                    {
                        listDTO.Add(_mapper.Map<UserDTO>(user));
                    }
                }
            }

            //get student role
            var moderatorRole = _userRoleConstrant.GetModeratorRole();
            //get student list
            var moderatorList = _userRepository.GetUsersByRole(moderatorRole);
            //if list is null
            if (moderatorList != null)
            {
                foreach (var user in moderatorList)
                {
                    //map if user status is true
                    if (user.Status)
                    {
                        listDTO.Add(_mapper.Map<UserDTO>(user));
                    }
                }
            }
            if(listDTO.Count == 0)
            {
                return null;
            }
            //return
            return listDTO;
        }
        public UserDTO? GetUser(int userID)
        {
            //get user info
            var user = _userRepository.GetUser(userID);
            if (user == null || !user.Status)
            {
                return null;
            }
            return _mapper.Map<UserDTO>(user);
        }
        public ICollection<UserDTO>? GetAllDisableUsers()
        {
            var userList = _userRepository.GetAllDisableUser();
            if(userList == null || userList.Count == 0)
            {
                return null;
            }
            return _mapper.Map<List<UserDTO>>(userList);
        }

        public UserDTO? PromoteStudent(int userID)
        {
            //check if existed
            if (!_userRepository.isExisted(userID))
            {
                return null;
            }
            //get user info
            var user = _userRepository.GetUser(userID);
            //get status
            var studentRole = _userRoleConstrant.GetStudentRole();
            var moderatorRole = _userRoleConstrant.GetModeratorRole();
            //check if already demoted
            if (!user.Role.Trim().Contains(studentRole))
            {
                return null;
            }
            //demote user
            user.Role = moderatorRole;
            //update
            if (!_userRepository.UpdateUser(user))
            {
                return null;
            }
            //return
            return _mapper.Map<UserDTO>(user);
        }

        public UserDTO? UpdateUser(int userID, string name, string email, string password)
        {
            //get user info
            var user = _userRepository.GetUser(userID);
            //check if user is null and is not empty
            if (user == null || !user.Status)
            {
                return null;
            }
            //update
            {
                user.Name = name;
                user.Email = email;
                user.Password = _hashingString.HashString(password);
                user.UpdatedAt = DateTime.Now;
            }
            //update to db
            if (!_userRepository.UpdateUser(user))
            {
                return null;
            }
            //return result
            return _mapper.Map<UserDTO>(user);
        }
        public UserDTO? GetUserByEmail(string email)
        {
            //get user info
            var user = _userRepository.GetUser(email);
            //return userdto info
            return _mapper.Map<UserDTO>(user);
        }
    }
}

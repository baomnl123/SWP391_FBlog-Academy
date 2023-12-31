﻿using AutoMapper;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.Implementors;
using backend.Repositories.IRepositories;
using backend.Utils;

namespace backend.Handlers.Implementors
{
    public class UserHandlers : IUserHandlers
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IReportPostRepository _reportPostRepository;
        private readonly IFollowUserRepository _followUserRepository;
        private readonly IPostHandlers _postHandlers;
        private readonly IUserMajorRepository _userMajorRepository;
        private readonly IUserSubjectRepository _userSubjectRepository;
        private readonly UserRoleConstrant _userRoleConstrant;
        private HashingString _hashingString;
        public UserHandlers(IUserRepository userRepository, IMapper mapper, IReportPostRepository reportPostRepository, IFollowUserRepository followUserRepository, IPostHandlers postHandlers, IUserMajorRepository userMajorRepository, IUserSubjectRepository userSubjectRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _reportPostRepository = reportPostRepository;
            _followUserRepository = followUserRepository;
            _postHandlers = postHandlers;
            _userRoleConstrant = new();
            _hashingString = new();
            _userMajorRepository = userMajorRepository;
            _userSubjectRepository = userSubjectRepository;
        }
        public UserDTO? CreateLecturer(string name, string avatarURL, string email, string password)
        {
            //make sure that info is not null
            if (name == null || email == null) return null;
            
            //get lecture role
            var lectureRole = _userRoleConstrant.GetLecturerRole();
            //check if existed
            if (_userRepository.isExisted(email))
            {
                //get existed user
                var existedUser = _userRepository.GetUser(email);
                //check if user is enable
                if (existedUser == null) return null;
                if (existedUser.Status) return null;
                //if user is disable then update the user into enable status
                existedUser.Name = name;
                existedUser.Role = lectureRole;
                existedUser.IsAwarded = false;
                //check if password is null
                if (password != null) existedUser.Password = _hashingString.HashString(password);
                //check if avatar is null
                if (avatarURL == null) existedUser.AvatarUrl = string.Empty;
                else existedUser.AvatarUrl = avatarURL;
                existedUser.Status = true;
                existedUser.CreatedAt = DateTime.Now;
                existedUser.UpdatedAt = null;
                if (!_userRepository.UpdateUser(existedUser)) return null;
                //return
                return _mapper.Map<UserDTO>(existedUser);
            }
            //init new user
            User newUser = new()
            {
                Name = name,
                Email = email,
                AvatarUrl = avatarURL,
                Role = lectureRole,
                CreatedAt = DateTime.Now,
                Status = true,
                IsAwarded = false,
            };
            //if avatar is null
            if (newUser.AvatarUrl == null) newUser.AvatarUrl = string.Empty;
            if (password != null) newUser.Password = _hashingString.HashString(password);
            //add to db
            if (!_userRepository.CreateUser(newUser)) return null;
            
            //return
            return _mapper.Map<UserDTO>(newUser);
        }

        public UserDTO? CreateUser(string name, string avatarURL, string email, string password)
        {
            //make sure that info is not null
            if (name == null || email == null)
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
                if (existedUser == null) return null;
                if (existedUser.Status) return null;
                //if user is disable then update the user into enable status
                existedUser.Role = studentRole;
                existedUser.IsAwarded = false;
                existedUser.Name = name;
                if (password != null)
                {
                    existedUser.Password = _hashingString.HashString(password);
                }
                if (avatarURL == null)
                {
                    existedUser.AvatarUrl = string.Empty;
                }
                else
                {
                    existedUser.AvatarUrl = avatarURL;
                }
                existedUser.Status = true;
                existedUser.CreatedAt = DateTime.Now;
                existedUser.UpdatedAt = null;
                if (!_userRepository.UpdateUser(existedUser))
                {
                    return null;
                }
                //return
                var existedUserDTO = _mapper.Map<UserDTO>(existedUser);
                return existedUserDTO;
            }
            //add new user
            User newUser = new()
            {
                Name = name,
                Email = email,
                AvatarUrl = avatarURL,
                Role = studentRole,
                CreatedAt = DateTime.Now,
                Status = true,
                IsAwarded = false,
            };
            //if avatar is null
            if (newUser.AvatarUrl == null)
            {
                newUser.AvatarUrl = string.Empty;
            }
            if (password != null)
            {
                newUser.Password = _hashingString.HashString(password);
            }
            //add to db
            if (!_userRepository.CreateUser(newUser))
            {
                return null;
            }
            //return
            var userDTO = _mapper.Map<UserDTO>(newUser);
            return userDTO;
        }

        public UserDTO? DemoteStudent(int userID)
        {
            //get user info
            var user = _userRepository.GetUser(userID);
            if (user == null)
            {
                return null;
            }
            //get status
            var moderatorRole = _userRoleConstrant.GetModeratorRole();
            //check if already demoted
            if (!user.Role.Trim().Contains(moderatorRole))
            {
                return null;
            }
            //demote user
            var studentRole = _userRoleConstrant.GetStudentRole();
            user.Role = studentRole;
            user.UpdatedAt = DateTime.Now;
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
            //get user list
            var list = _userRepository.GetAllUsers();
            //check if list is null of empty
            if (list == null || list.Count == 0) return null;
            
            //init list dto
            var listDTO = new List<UserDTO>();
            foreach (var user in list)
            {
                if (!user.Status) continue;
                //check user status then map to dto
                var userDTO = this.GetUser(user.Id);
                if (userDTO == null || !userDTO.Status) continue;

                listDTO.Add(userDTO);
            }
            listDTO.OrderByDescending(u => u.followerNumber).ThenByDescending(u => u.postNumber);
            //return
            return listDTO;
        }

        public ICollection<UserDTO>? GetLecturers()
        {
            //get lecture role
            var lectureRole = _userRoleConstrant.GetLecturerRole();
            //get lecture list
            var list = _userRepository.GetUsersByRole(lectureRole);
            //if list is null
            if (list == null)
            {
                return null;
            }
            //init dto list
            var listDTO = new List<UserDTO>();
            foreach (var user in list)
            {
                //map if user status is true
                if (!user.Status) continue;

                var userDTO = this.GetUser(user.Id);

                if (userDTO == null || !userDTO.Status) return null;

                listDTO.Add(userDTO);
            }
            if (listDTO.Count == 0) return null;
            //return
            return listDTO;
        }

        public ICollection<UserDTO>? GetStudentsAndModerator()
        {
            //get students and moderators list
            var list = _userRepository.GetStudentsAndModerators();
            if (list == null || list.Count == 0)
            {
                return null;
            }
            //init dto list
            var listDTO = new List<UserDTO>();
            //if list is null
            foreach (var user in list)
            {
                //map if user status is true
                if (!user.Status) continue;
                
                var userDTO = this.GetUser(user.Id);
                if (userDTO == null || !userDTO.Status) return null;

                listDTO.Add(userDTO);
            }
            if (listDTO.Count == 0) return null;
            
            listDTO.OrderByDescending(u => u.followerNumber).ThenByDescending(u => u.postNumber);
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
            var userDTO = _mapper.Map<UserDTO>(user);

            var getMajors = _userMajorRepository.GetMajorsOf(userDTO.Id);
            if(getMajors != null)
            {
                userDTO.majorsList = _mapper.Map<ICollection<MajorDTO>?>(getMajors);
                userDTO.majorsNumber = getMajors.Count();
            }

            var getSubjects = _userSubjectRepository.GetSubjectsOf(userDTO.Id);
            if (getSubjects != null)
            {
                userDTO.subjectsList = _mapper.Map<ICollection<SubjectDTO>?>(getSubjects);
                userDTO.subjectsNumber = getSubjects.Count();
            }

            var getReports = _reportPostRepository.GetApprovedReportsAbout(userDTO.Id);
            if (getReports != null)
            {
                userDTO.successReportedTimes = getReports.Count;
            }

            var getFollowers = _followUserRepository.GetAllFollowerUsers(user);
            if (getFollowers != null)
            {
                //userDTO.followersList = _mapper.Map<ICollection<UserDTO>?>(getFollowers);
                userDTO.followerNumber = getFollowers.Count();
            }

            var getFollowings = _followUserRepository.GetAllFollowingUsers(user);
            if (getFollowings != null)
            {
                //userDTO.followingsList = _mapper.Map<ICollection<UserDTO>?>(getFollowings);
                userDTO.followingNumber = getFollowings.Count();
            }

            var getPosts = _postHandlers.SearchPostByUserId(userDTO.Id);
            if (getPosts != null)
            {
                userDTO.postsList = _mapper.Map<ICollection<PostDTO>?>(getPosts);
                userDTO.postNumber = getPosts.Count();
            }


            return userDTO;
        }

        public ICollection<UserDTO>? GetAllDisableUsers()
        {
            var userList = _userRepository.GetAllDisableUser();
            if (userList == null || userList.Count == 0)
            {
                return null;
            }
            return _mapper.Map<ICollection<UserDTO>>(userList);
        }

        public UserDTO? PromoteStudent(int userID)
        {
            //get user info
            var user = _userRepository.GetUser(userID);
            if (user == null || !user.Status)
            {
                return null;
            }
            //get status
            var studentRole = _userRoleConstrant.GetStudentRole();
            //check if already demoted
            if (!user.Role.Trim().Contains(studentRole))
            {
                return null;
            }
            //demote user
            var moderatorRole = _userRoleConstrant.GetModeratorRole();
            user.Role = moderatorRole;
            user.UpdatedAt = DateTime.Now;
            //update
            if (!_userRepository.UpdateUser(user))
            {
                return null;
            }
            //return
            return _mapper.Map<UserDTO>(user);
        }

        public UserDTO? UpdateUser(int userID, string name, string avatarURL, string password)
        {
            //get user info
            var user = _userRepository.GetUser(userID);
            //check if user is null and is not empty
            if (user == null || !user.Status)
            {
                return null;
            }
            //update
            user.Name = name;
            if (avatarURL == null)
            {
                user.AvatarUrl = string.Empty;
            }
            else
            {
                user.AvatarUrl = avatarURL;
            }
            user.UpdatedAt = DateTime.Now;
            //update password
            if (password != null)
            {
                user.Password = _hashingString.HashString(password);
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
            if (user == null || !user.Status) return null;
            //return userdto info
            var userDTO = this.GetUser(user.Id);

            if (userDTO == null || !userDTO.Status) return null;

            return userDTO;
        }

        public UserDTO? GiveAward(int userID)
        {
            //get user
            var existedUser = _userRepository.GetUser(userID);
            //check if user is unavailable
            if (existedUser == null || !existedUser.Status) return null;
            //check if user is awarded
            if (existedUser.IsAwarded) return null;
            //update user
            existedUser.IsAwarded = true;
            //update to db
            if (!_userRepository.UpdateUser(existedUser))
            {
                return null;
            }

            var userDTO = this.GetUser(existedUser.Id);

            if (userDTO == null || !userDTO.Status) return null;

            return userDTO;
        }

        public UserDTO? RemoveAward(int userID)
        {
            //get user
            var existedUser = _userRepository.GetUser(userID);
            //check if user is unavailable
            if (existedUser == null || !existedUser.Status) return null;
            //check if user is awarded
            if (!existedUser.IsAwarded) return null;
            //update user
            existedUser.IsAwarded = false;
            //update to db
            if (!_userRepository.UpdateUser(existedUser))
            {
                return null;
            }
            var userDTO = this.GetUser(existedUser.Id);

            if (userDTO == null || !userDTO.Status) return null;

            return userDTO;
        }

        public UserDTO? UnbanUser(int userID)
        {
            var getUser = _userRepository.GetUser(userID);
            if (getUser == null)
            {
                return null;
            }
            if (getUser.Status)
            {
                return null;
            }
            getUser.Status = true;
            getUser.UpdatedAt = DateTime.Now;
            if (!_userRepository.UpdateUser(getUser))
            {
                return null;
            }
            var userDTO = this.GetUser(getUser.Id);

            if (userDTO == null || !userDTO.Status) return null;

            return userDTO;
        }

        public ICollection<UserDTO>? GetBannedUsers()
        {
            var getUsers = _userRepository.GetBannedUser();
            if (getUsers == null || getUsers.Count == 0)
            {
                return null;
            }
            var listDTO = new List<UserDTO>();
            foreach (var user in getUsers)
            {
                var userDTO = _mapper.Map<UserDTO>(user);
                var getReports = _reportPostRepository.GetApprovedReportsAbout(userDTO.Id);
                if (getReports != null)
                {
                    userDTO.successReportedTimes = getReports.Count();
                }
                listDTO.Add(userDTO);
            }
            if (listDTO.Count == 0) return null;
            return listDTO;
        }

        public ICollection<UserDTO>? GetStudents()
        {
            var getUsers = _userRepository.GetStudents();
            if (getUsers == null || getUsers.Count == 0)
            {
                return null;
            }
            var listDTO = new List<UserDTO>();
            foreach (var user in getUsers)
            {
                if (!user.Status) continue;
                
                var userDTO = this.GetUser(user.Id);

                if (userDTO == null || !userDTO.Status) return null;

                listDTO.Add(userDTO);
            }
            if (listDTO.Count == 0) return null;

            listDTO.OrderByDescending(u => u.followerNumber).ThenByDescending(u => u.postNumber);
            return listDTO;
        }

        public UserDTO? GetBannedUser(int userID)
        {
            var getUser = _userRepository.GetUser(userID);
            if (getUser == null) return null;
            var userDTO = _mapper.Map<UserDTO>(getUser);
            return userDTO;
        }
    } 
}

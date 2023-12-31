﻿using backend.DTO;
using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IUserRepository
    {
        //Get Users
        public ICollection<User>? GetAllUsers();
        public ICollection<User>? GetAllDisableUser();
        public User? GetUser(int userID);
        public User? GetUser(string email);
        public ICollection<User>? GetUsersByUsername(string username);
        public ICollection<User>? GetUsersByRole(string role);
        public ICollection<User>? GetStudents();
        public ICollection<User>? GetStudentsAndModerators();
        public User? GetUserByPostID(int postID);
        public User? GetUserByCommentID(int commentID);
        public ICollection<User>? GetBannedUser();
        //CRUD Users
        public bool CreateUser(User user);
        public bool UpdateUser(User user);
        //public bool DisableUser(User user);
        //Check Exists
        public bool isExisted(User user);
        public bool isExisted(int userID);
        public bool isExisted(string email);
    }
}

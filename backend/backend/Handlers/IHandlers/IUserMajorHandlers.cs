﻿using backend.DTO;

namespace backend.Handlers.IHandlers
{
    public interface IUserMajorHandlers
    {
        public UserMajorDTO? AddUserMajor(int currentUserID, int majorID);
        public UserMajorDTO? DeleteUserMajor(int currentUserID, int majorID);
        public ICollection<MajorDTO>? GetMajorsOf(int userID);
        public ICollection<UserDTO>? GetUsersOf(int majorID);
    }
}
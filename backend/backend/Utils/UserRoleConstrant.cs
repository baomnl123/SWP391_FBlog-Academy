namespace backend.Utils
{
    public class UserRoleConstrant
    {
        private readonly ConfigHelper _configHelper;
        private string _student;
        private string _moderator;
        private string _lecturer;
        private string _admin;

        public UserRoleConstrant()
        {
            this._configHelper = new();
            // Initialize _student here after _configHelper is assigned
            _student = _configHelper.configuration["Roles:Student"];
            _moderator = _configHelper.configuration["Roles:Moderator"];
            _lecturer = _configHelper.configuration["Roles:Lecturer"];
            _admin = _configHelper.configuration["Roles:Admin"];
        }

        public string GetStudentRole()
        {
            return this._student;
        }
        public string GetModeratorRole()
        {
            return this._moderator;
        }
        public string GetLecturerRole()
        {
            return this._lecturer;
        }
        public string GetAdminRole()
        {
            return this._admin;
        }
    }

}

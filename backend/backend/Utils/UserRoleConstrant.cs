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
        }

        public string GetStudentRole()
        {
            _student = _configHelper.config["Roles:Student"];
            return this._student;
        }
        public string GetModeratorRole()
        {
            _moderator = _configHelper.config["Roles:Moderator"];
            return this._moderator;
        }
        public string GetLecturerRole()
        {
            _lecturer = _configHelper.config["Roles:Lecturer"];
            return this._lecturer;
        }
        public string GetAdminRole()
        {
            _admin = _configHelper.config["Roles:Admin"];
            return this._admin;
        }
    }

}

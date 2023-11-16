namespace backend.Utils
{
    public class SpecialMajors
    {
        private readonly ConfigHelper _configHelper;
        private string _onlyStudents;

        public SpecialMajors()
        {
            this._configHelper = new ConfigHelper();
        }

        public string GetOnlyStudent()
        {
            _onlyStudents = _configHelper.config["SpecialMajor:OnlyStudent"];
            return _onlyStudents;
        }
    }
}

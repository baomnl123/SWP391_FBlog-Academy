namespace backend.Utils
{
    public class SpecialCategories
    {
        private readonly ConfigHelper _configHelper;
        private string _onlyStudents;

        public SpecialCategories()
        {
            this._configHelper = new ConfigHelper();
        }

        public string GetOnlyStudent()
        {
            _onlyStudents = _configHelper.config["SpecialCategories:OnlyStudent"];
            return _onlyStudents;
        }
    }
}

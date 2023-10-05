namespace backend.Utils
{
    public class ReportStatusConstrant
    {
        private readonly ConfigHelper _configHelper;
        private string _pending;
        private string _declined;
        private string _approved;
        private string _disable;

        public ReportStatusConstrant()
        {
            this._configHelper = new ConfigHelper();
        }

        public string GetPendingStatus()
        {
            _pending = _configHelper.config["ReportPostStatus:Pending"];
            return _pending;
        }
        public string GetDeclinedStatus()
        {
            _declined = _configHelper.config["ReportPostStatus:Declined"];
            return this._declined;
        }
        public string GetApprovedStatus()
        {
            _approved = _configHelper.config["ReportPostStatus:Approved"];
            return this._approved;
        }
        public string GetDisableStatus()
        {
            _disable = _configHelper.config["ReportPostStatus:Disable"];
            return this._disable;
        }
    }
}

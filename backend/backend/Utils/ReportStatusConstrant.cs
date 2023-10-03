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
            this._configHelper = new();
            // Initialize _student here after _configHelper is assigned
            _pending = _configHelper.configuration["ReportPostStatus:Pending"];
            _declined = _configHelper.configuration["ReportPostStatus:Declined"];
            _approved = _configHelper.configuration["ReportPostStatus:Approved"];
            _disable = _configHelper.configuration["ReportPostStatus:Disable"];
        }

        public string GetPendingStatus()
        {
            return this._pending;
        }
        public string GetDeclinedStatus()
        {
            return this._declined;
        }
        public string GetApprovedStatus()
        {
            return this._approved;
        }
        public string GetDisableStatus()
        {
            return this._disable;
        }
    }
}

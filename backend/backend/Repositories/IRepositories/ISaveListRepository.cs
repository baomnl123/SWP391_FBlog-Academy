using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface ISaveListRepository
    {
        //Get Save List
        public ICollection<SaveList> GetAllSaveLists(User user);
        public ICollection<SaveList> GetSaveListsByListName(User user, string listname);
        //CRUD Save List
        public bool CreateSaveList(SaveList savelist);
        public bool UpdateSaveList(SaveList savelist);
        public bool DisableSaveList(SaveList savelist);
        //Check Existed
        public bool isExisted(SaveList savelist);
    }
}

using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface ISaveListRepository
    {
        //Get Save List
        public ICollection<SaveList>? GetAllSaveLists(int userID);
        public SaveList? GetSaveListBySaveListID(int saveListID);
        //CRUD Save List
        public bool CreateSaveList(SaveList savelist);
        public bool UpdateSaveList(SaveList savelist);
        public bool DisableSaveList(int saveListID);
        //Check Existed
        public bool isExisted(int saveListID);
    }
}

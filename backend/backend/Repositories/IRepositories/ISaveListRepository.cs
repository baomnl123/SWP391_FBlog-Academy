using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface ISaveListRepository
    {
        //Get Save List
        public ICollection<SaveList>? GetAllSaveLists(int userID);
        public ICollection<SaveList>? GetAllDisableSaveLists(int userID);
        public SaveList? GetSaveListBySaveListID(int saveListID);
        public SaveList? GetSaveListByUserIDAndName(int userID, string listName);
        //CRUD Save List
        public bool CreateSaveList(SaveList savelist);
        public bool UpdateSaveList(SaveList savelist);
        //Check Existed
        public bool isExisted(int saveListID);
        public bool isExisted(int userID, string listName);
    }
}

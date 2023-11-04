using backend.DTO;
using backend.Models;

namespace backend.Handlers.IHandlers
{
    public interface ISaveListHandlers
    {
        public ICollection<SaveListDTO>? GetAllActiveSaveList(int userID);
        public ICollection<SaveListDTO>? GetAllDisableSaveList(int userID);
        public SaveListDTO? AddSaveList(int userID, string name);
        public SaveListDTO? DisableSaveList(int saveListID);
        public SaveListDTO? UpdateSaveListName(int saveListID,string name);
        public SaveListDTO? GetSaveList(int saveListID);
    }
}

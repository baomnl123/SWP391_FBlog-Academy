using backend.DTO;
using backend.Models;

namespace backend.Handlers.IHandlers
{
    public interface ISaveListHandlers
    {
        public ICollection<SaveList> GetAllSaveList();
        public SaveList AddSaveList(string name);
        public bool DisableSaveList(int saveListID);
        public SaveList UpdateSaveList(int saveListID);
    }
}

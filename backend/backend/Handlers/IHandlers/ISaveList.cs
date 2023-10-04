using backend.DTO;
using backend.Models;

namespace backend.Handlers.IHandlers
{
    public interface ISaveList
    {
        public ICollection<SaveListDTO> GetAllSaveList();
        public ICollection<SaveListDTO> GetSaveListByName(string saveListName);
        public SaveListDTO AddSaveList();
        public void DisableSaveList(int saveListID);
        public SaveListDTO UpdateSaveList(int saveListID);
    }
}

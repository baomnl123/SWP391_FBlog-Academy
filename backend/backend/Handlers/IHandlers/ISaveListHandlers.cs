using backend.DTO;
using backend.Models;

namespace backend.Handlers.IHandlers
{
    public interface ISaveListHandlers
    {
        public ICollection<SaveListDTO> GetAllSaveList();
        public SaveListDTO AddSaveList(string name);
        public bool DisableSaveList(int saveListID);
        public SaveListDTO UpdateSaveList(int saveListID);
    }
}

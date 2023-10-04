using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;

namespace backend.Handlers.Implementors
{
    public class SaveListHandlers : ISaveListHandlers
    {
        public SaveListDTO AddSaveList(string name)
        {
            throw new NotImplementedException();
        }

        public bool DisableSaveList(int saveListID)
        {
            throw new NotImplementedException();
        }

        public ICollection<SaveListDTO> GetAllSaveList()
        {
            throw new NotImplementedException();
        }

        public SaveListDTO UpdateSaveList(int saveListID)
        {
            throw new NotImplementedException();
        }
    }
}

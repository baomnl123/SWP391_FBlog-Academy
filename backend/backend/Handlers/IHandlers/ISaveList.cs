using backend.Models;

namespace backend.Handlers.IHandlers
{
    public interface ISaveList
    {
        public ICollection<SaveList> GetAllSaveList(User user);
        public SaveList AddSaveList(User user, SaveList savelist);
        public void DisableSaveList(User user,SaveList savelist);
        public SaveList UpdateSaveList(User user,SaveList savelist);
        

    }
}

using backend.Models;
using backend.Repositories.IRepositories;

namespace backend.Repositories.Implementors
{
    public class SaveListRepository : ISaveListRepository
    {
        private readonly FBlogAcademyContext _fblogacademycontext;
        public SaveListRepository()
        {
            this._fblogacademycontext = new();
        }
        public bool CreateSaveList(SaveList savelist)
        {
            _fblogacademycontext.SaveLists.Add(savelist);
            if (_fblogacademycontext.SaveChanges() != 0) return true;
            else return false;
        }

        public bool DisableSaveList(SaveList savelist)
        {
            if (!this.isExisted(savelist))
            {
                return false;
            }
            else
            {
                savelist.Status = false;
                if (this.UpdateSaveList(savelist)) return true;
                else return false;
            }
        }

        public ICollection<SaveList> GetAllSaveLists(User user)
        {
            var list = _fblogacademycontext.SaveLists.ToList();
            return list;
        }

        public ICollection<SaveList> GetSaveListsByListName(User user, string listname)
        {
            var list = _fblogacademycontext.SaveLists.Where(u => u.Name.Equals(listname)).ToList();
            return list;
        }

        public bool isExisted(SaveList savelist)
        {
            var checkSaveList = _fblogacademycontext.SaveLists.FirstOrDefault(u => u.Id.Equals(savelist));
            if (checkSaveList == null) return false;
            else return true;
        }

        public bool UpdateSaveList(SaveList savelist)
        {
            _fblogacademycontext.SaveLists.Update(savelist);
            if (_fblogacademycontext.SaveChanges() != 0) return true;
            else return false;
        }
    }

}

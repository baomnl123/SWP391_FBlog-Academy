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
            try
            {
                _fblogacademycontext.SaveLists.Add(savelist);
                if (_fblogacademycontext.SaveChanges() == 0)
                {
                    return false;
                }
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public SaveList? GetSaveListBySaveListID(int saveListID)
        {
            try
            {
                var saveList = _fblogacademycontext.SaveLists.FirstOrDefault(u => u.Id.Equals(saveListID));
                return saveList;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public ICollection<SaveList>? GetAllSaveLists(int userID)
        {
            try
            {
                var list = _fblogacademycontext.SaveLists.Where(u => u.UserId.Equals(userID)).OrderBy(u => u.Name).ToList();
                if (list == null || list.Count == 0)
                {
                    return null;
                }
                return list;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public ICollection<SaveList>? GetAllDisableSaveLists(int userID)
        {
            try
            {
                var list = _fblogacademycontext.SaveLists.Where(u => u.UserId.Equals(userID) 
                                                                  && u.Status == false).OrderBy(u => u.Name).ToList();
                if (list == null || list.Count == 0)
                {
                    return null;
                }
                return list;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
        public SaveList? GetSaveListByUserIDAndName(int userID, string listName)
        {
            try
            {
                var saveList = _fblogacademycontext.SaveLists.FirstOrDefault(u => u.UserId.Equals(userID)
                                                                               && u.Name.Trim().Equals(listName.Trim()));
                return saveList;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public bool isExisted(int saveListID)
        {

            try
            {
                var checkSaveList = _fblogacademycontext.SaveLists.FirstOrDefault(u => u.Id.Equals(saveListID));

                if (checkSaveList == null)
                {
                    return false;
                }
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }

        }
        public bool isExisted(int userID, string listName)
        {
            try
            {
                var checkSaveList = GetSaveListByUserIDAndName(userID, listName);
                if (checkSaveList == null)
                {
                    return false;
                }
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public bool UpdateSaveList(SaveList savelist)
        {
            try
            {
                _fblogacademycontext.SaveLists.Update(savelist);
                if (_fblogacademycontext.SaveChanges() == 0)
                {
                    return false;
                }
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
    }
}

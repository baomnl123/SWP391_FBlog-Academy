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
            if (_fblogacademycontext.SaveChanges() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool DisableSaveList(int saveListID)
        {
            if (!this.isExisted(saveListID))
            {
                return false;
            }
            else
            {
                var savelist = GetSaveListBySaveListID(saveListID);

                if(savelist == null)
                {
                    return false;
                }
                else
                {
                    savelist.Status = false;
                    if (!this.UpdateSaveList(savelist))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    } 
                }
                
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
                var list = _fblogacademycontext.SaveLists.Where(u => u.UserId.Equals(userID) && u.Status == true).ToList();
                if(list.Count == 0)
                {
                    return null;
                }
                else
                {
                    return list;
                }
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public bool isExisted(int saveListID)
        {
            var checkSaveList = _fblogacademycontext.SaveLists.FirstOrDefault(u => u.Id.Equals(saveListID));

            if (checkSaveList == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool UpdateSaveList(SaveList savelist)
        {
            _fblogacademycontext.SaveLists.Update(savelist);
            if (_fblogacademycontext.SaveChanges() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

}

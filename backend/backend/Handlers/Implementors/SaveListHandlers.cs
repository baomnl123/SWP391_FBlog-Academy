using AutoMapper;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.IRepositories;

namespace backend.Handlers.Implementors
{
    public class SaveListHandlers : ISaveListHandlers
    {
        private readonly ISaveListRepository _saveListRepository;
        private readonly IMapper _mapper;
        public SaveListHandlers(ISaveListRepository saveListRepository, IMapper mapper)
        {
            _saveListRepository = saveListRepository;
            _mapper = mapper;
        }
        public SaveListDTO? AddSaveList(int userID, string listName)
        {
            //Check If Existed
            if (_saveListRepository.isExisted(userID, listName))
            {
                //Check Status
                var checkSaveList = _saveListRepository.GetSaveListByUserIDAndName(userID, listName);
                if (checkSaveList.Status == true)
                {
                    return null;
                }
                //Reactivate the savelist
                checkSaveList.Status = true;
                checkSaveList.CreatedAt = DateTime.Now;
                if (!_saveListRepository.UpdateSaveList(checkSaveList))
                {
                    return null;
                }
                return _mapper.Map<SaveListDTO>(checkSaveList);
            }
            //Init new Savelist
            SaveList newSaveList = new()
            {
                UserId = userID,
                Name = listName,
                CreatedAt = DateTime.Now,
                Status = true,
            };
            if (!_saveListRepository.CreateSaveList(newSaveList))
            {
                return null;
            }
            return _mapper.Map<SaveListDTO>(newSaveList);
        }

        public SaveListDTO? DisableSaveList(int saveListID)
        {
            //get savelist information
            var saveList = _saveListRepository.GetSaveListBySaveListID(saveListID);
            //if savelist is unavaiable
            if(saveList == null || saveList.Status == false)
            {
                return null;
            }
            saveList.Status = false;
            saveList.UpdateAt = DateTime.Now;
            //disable save list
            if (!_saveListRepository.UpdateSaveList(saveList))
            {
                return null;
            }
            return _mapper.Map<SaveListDTO>(saveList);
        }

        public ICollection<SaveListDTO>? GetAllActiveSaveList(int userID)
        {
            //init list
            List<SaveListDTO> saveListsDTO = new();
            //get savelist information
            var saveLists = _saveListRepository.GetAllSaveLists(userID);
            //if savelist is unavailable then return nothing
            if (saveLists == null || saveLists.Count == 0)
            {
                return null;
            }
            foreach (var saveList in saveLists)
            {
                //Map to DTO
                if(saveList.Status)
                {
                    saveListsDTO.Add(_mapper.Map<SaveListDTO>(saveList));
                }
            }
            return saveListsDTO;
        }

        public ICollection<SaveListDTO>? GetAllDisableSaveList(int userID)
        {
            var list = _saveListRepository.GetAllDisableSaveLists(userID);
            if(list == null || list.Count == 0)
            {
                return null;
            }
            return _mapper.Map<List<SaveListDTO>>(list);
        }

        public SaveListDTO? UpdateSaveListName(int saveListID,string listName)
        {
            //Check if list name is null
            if(listName == null)
            {
                return null;
            }
            //Check if savelist is not exist
            if (!_saveListRepository.isExisted(saveListID))
            {
                return null;
            }
            //Get savelist
            var saveList = _saveListRepository.GetSaveListBySaveListID(saveListID);
            //Update savelist
            saveList.Name = listName;
            saveList.UpdateAt = DateTime.Now;
            //Update DB
            if (!_saveListRepository.UpdateSaveList(saveList))
            {
                return null;
            }
            //Return result
            return _mapper.Map<SaveListDTO>(saveList);
        }
    }
}

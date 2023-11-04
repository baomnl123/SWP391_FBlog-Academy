using AutoMapper;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.Implementors;
using backend.Repositories.IRepositories;

namespace backend.Handlers.Implementors
{
    public class SaveListHandlers : ISaveListHandlers
    {
        private readonly IUserRepository _userRepository;
        private readonly ISaveListRepository _saveListRepository;
        private readonly IMapper _mapper;
        public SaveListHandlers(ISaveListRepository saveListRepository, IMapper mapper, IUserRepository userRepository)
        {
            _saveListRepository = saveListRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public SaveListDTO? AddSaveList(int userID, string listName)
        {
            //get user
            var user = _userRepository.GetUser(userID);

            //check if user is not available
            if(user == null || !user.Status)
            {
                return null;
            }

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
                checkSaveList.UpdateAt = null;
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

            var newSaveListDTO = _mapper.Map<SaveListDTO>(newSaveList);

            //add UserDTO to savelistDTO
            var getUser = _mapper.Map<UserDTO>(user);
            newSaveListDTO.User = (getUser is not null && getUser.Status) ? getUser : null;

            return newSaveListDTO;
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
            var user = _userRepository.GetUser(userID);

            //check if user is unavailable then return
            if(user == null || !user.Status)
            {
                return null;
            }

            //get savelist information
            var saveLists = _saveListRepository.GetAllSaveLists(userID);
            //if savelist is unavailable then return nothing
            if (saveLists == null || saveLists.Count == 0)
            {
                return null;
            }
            //init list
            var saveListsDTOList = new List<SaveListDTO>();
            foreach (var saveList in saveLists)
            {
                //Map to DTO
                if(saveList.Status)
                {
                    var saveListDTO = _mapper.Map<SaveListDTO>(saveList);

                    //add UserDTO to savelistDTO
                    var getUser = _mapper.Map<UserDTO>(user);
                    saveListDTO.User = (getUser is not null && getUser.Status) ? getUser : null;

                    saveListsDTOList.Add(saveListDTO);
                }
            }
            return saveListsDTOList;
        }

        public ICollection<SaveListDTO>? GetAllDisableSaveList(int userID)
        {
            var user = _userRepository.GetUser(userID);

            //check if user is unavailable then return
            if (user == null || !user.Status)
            {
                return null;
            }

            var list = _saveListRepository.GetAllDisableSaveLists(userID);

            if(list == null || list.Count == 0)
            {
                return null;
            }

            var listDTO = new List<SaveListDTO>();

            foreach(var saveList in list)
            {
                var saveListDTO = _mapper.Map<SaveListDTO>(saveList);

                //add UserDTO to savelistDTO
                var getUser = _mapper.Map<UserDTO>(user);
                saveListDTO.User = (getUser is not null && getUser.Status) ? getUser : null;

                listDTO.Add(saveListDTO);
            } 

            return listDTO;
        }

        public SaveListDTO? UpdateSaveListName(int saveListID,string listName)
        {
            //Check if list name is null
            if(listName == null)
            {
                return null;
            }

            //Get savelist
            var saveList = _saveListRepository.GetSaveListBySaveListID(saveListID);

            //check available
            if(saveList == null || !saveList.Status)
            {
                return null;
            }

            //Update savelist
            saveList.Name = listName;
            saveList.UpdateAt = DateTime.Now;
            //Update DB
            if (!_saveListRepository.UpdateSaveList(saveList))
            {
                return null;
            }

            var saveListDTO = _mapper.Map<SaveListDTO>(saveList);

            //add UserDTO to savelistDTO
            var getUser = _mapper.Map<UserDTO>(saveList.UserId);
            saveListDTO.User = (getUser is not null && getUser.Status) ? getUser : null;

            //Return result
            return saveListDTO;
        }

        public SaveListDTO? GetSaveList(int saveListID)
        {
            //get save list
            var saveList = _saveListRepository.GetSaveListBySaveListID(saveListID);

            //check save list condition
            if(saveList == null || !saveList.Status)
            {
                return null;
            }

            //
            var user = _userRepository.GetUser(saveList.UserId);
            if (user == null || !user.Status)
            {
                return null;
            }
            //
            var saveListDTO = _mapper.Map<SaveListDTO>(saveList);

            //add UserDTO to savelistDTO
            var getUser = _mapper.Map<UserDTO>(user);
            saveListDTO.User = (getUser is not null && getUser.Status) ? getUser : null;

            return saveListDTO;
        }

    }
}

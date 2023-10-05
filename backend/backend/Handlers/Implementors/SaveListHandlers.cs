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
        public SaveListDTO? AddSaveList(int userID,string name)
        {
            throw new NotImplementedException();
        }

        public bool DisableSaveList(int saveListID)
        {
            if (!_saveListRepository.DisableSaveList(saveListID))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public ICollection<SaveListDTO>? GetAllSaveList(int userID)
        {
            List<SaveListDTO> saveListsDTO = new();
            var saveLists = _saveListRepository.GetAllSaveLists(userID);
            if(saveLists == null || saveLists.Count == 0)
            {
                return null;
            }
            else
            {
                foreach (var saveList in saveLists)
                {
                        saveListsDTO.Add(_mapper.Map<SaveListDTO>(saveList));
                }
                return saveListsDTO;
            }
        }

        public SaveListDTO? UpdateSaveList(int saveListID)
        {
            throw new NotImplementedException();
        }
    }
}

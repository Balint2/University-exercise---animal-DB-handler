using C2INIS_HSZF_2024251.Persistence.MsSql;

namespace C2INIS_HSZF_2024251.Application
{
    public interface IMainService
    {
        IActivityService ActivityService { get; set; }
        IAnimalService AnimalService { get; set; }
        IFoodService FoodService { get; set; }
        bool wasCreated { get; }

        void DeleteAll();
        void FolderCreate();
        void Init(IAnimalService animalService, IFoodService foodServcie, IActivityService acivityService, IAnimalDataProvider animalDataProvider, IFoodDataProvider foodDataProvider, IActivityDataProvider activityDataProvider, bool read);
        void Reset();
    }
}
using C2INIS_HSZF_2024251.Model;
using Newtonsoft.Json;
using C2INIS_HSZF_2024251.Persistence.MsSql;
using System.Xml.Serialization;
using C2INIS_HSZF_2024251.Persistence;

namespace C2INIS_HSZF_2024251.Application
{
    public class MainService : IMainService
    {
        public IAnimalService AnimalService { get; set; }
        public IFoodService FoodService { get; set; }
        public IActivityService ActivityService { get; set; }
        private readonly IMainDataProvider mainDataProvider;

        public bool wasCreated { get { return mainDataProvider.wasCreated; } }

        const string mainDirectory = "Riports";
        //const string animalDirectory = "Animals";
        //const string keeperDirectory = "Keepers";

        public MainService(IMainDataProvider mainDataProvider)
        {
            this.mainDataProvider = mainDataProvider;
        }

        public void Init(IAnimalService animalService, IFoodService foodServcie, IActivityService acivityService, IAnimalDataProvider animalDataProvider, IFoodDataProvider foodDataProvider, IActivityDataProvider activityDataProvider, bool read)
        {
            AnimalService = animalService;
            FoodService = foodServcie;
            ActivityService = acivityService;
            mainDataProvider.Init(animalDataProvider, foodDataProvider, activityDataProvider, read);
            FolderCreate();
        }




        public void FolderCreate()
        {
            Directory.CreateDirectory(mainDirectory);
            foreach (var item in AnimalService.ListEntities())
            {
                Directory.CreateDirectory(Path.Combine(mainDirectory, item.Id + "_" + item.Name));
            }

            //Directory.CreateDirectory(Path.Combine(mainDirectory, keeperDirectory));
        }

        public void Reset()
        {
            mainDataProvider.Reset();
        }

        public void DeleteAll()
        {
            mainDataProvider.Delete();
        }

    }
}

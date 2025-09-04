namespace C2INIS_HSZF_2024251.Persistence.MsSql
{
    public interface IMainDataProvider
    {
        IActivityDataProvider ActivityDataProvider { get; set; }
        IAnimalDataProvider AnimalDataProvider { get; set; }
        IFoodDataProvider FoodDataProvider { get; set; }
        //AppDbContext ctx { get; set; }

        bool wasCreated { get; }

        void Delete();
        void Init(IAnimalDataProvider animalDataProvider, IFoodDataProvider foodDataProvider, IActivityDataProvider activityDataProvider, bool read);
        void ReadFromJSON();
        void Reset();
    }
}
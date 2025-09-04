using C2INIS_HSZF_2024251.Model;

namespace C2INIS_HSZF_2024251.Application
{
    public interface IFoodService
    {
        Food CreateEntity(Food newKeeper);
        Food DeleteEntity(Food keeper);
        IEnumerable<Food> ListEntities();
        void MakeRiport();
        void Reduce(Food food);
        Food SearchById(int id);
        Food UpdateEntity(Food oldKeeper, Food newKeeper);
        void Init(string path);
    }
}
using C2INIS_HSZF_2024251.Model;

namespace C2INIS_HSZF_2024251.Persistence.MsSql
{
    public interface IFoodDataProvider
    {
        void CreateEntity(Food keeper);
        Food DeleteEntity(Food keeperToDelete);
        IEnumerable<Food> ReadAllEntity();
        void ReadFromJSON();
        void Reduce(Food food);
        Food SearchById(int id);
        Food Update(Food oldKeeper, Food newKkeper);
    }
}
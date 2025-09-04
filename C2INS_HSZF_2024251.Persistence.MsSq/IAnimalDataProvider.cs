using C2INIS_HSZF_2024251.Model;

namespace C2INIS_HSZF_2024251.Persistence.MsSql
{
    public interface IAnimalDataProvider
    {
        void CreateEntity(Animal animal);
        Animal DeleteEntity(Animal animalToDelete);
        IEnumerable<Animal> ReadAllEntity();
        void ReadFromJSON();
        Animal SearchById(int id);
        Animal Update(Animal oldAnimal, Animal newAnimal);
    }
}
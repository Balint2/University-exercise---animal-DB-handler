using C2INIS_HSZF_2024251.Model;

namespace C2INIS_HSZF_2024251.Application
{
    public interface IAnimalService
    {
        Animal CreateEntity(Animal newAnimal);
        Animal DeleteEntity(Animal animal);
        IEnumerable<Animal> ListEntities();
        void MakeRiport(Animal animal);
        Animal SearchById(int id);
        Animal UpdateEntity(Animal oldAnimal, Animal newAnimal);
        void Init(string path);
    }
}
using C2INIS_HSZF_2024251.Model;

namespace C2INIS_HSZF_2024251.Persistence.MsSql
{
    public interface IActivityDataProvider
    {
        void CreateEntity(Activity activity);
        Activity DeleteEntity(Activity activityToDelete);
        IEnumerable<Activity> ReadAllEntity();
        void ReadFromJSON();
        Activity SearchById(int id);
        Activity Update(Activity oldActivity, Activity newActivity);

        public IEnumerable<Activity> SearchByName(string keyWord);

        public IEnumerable<Activity> ReOrder<T>(Func<Activity, T> keySelector, bool ascending = true);
    }
}
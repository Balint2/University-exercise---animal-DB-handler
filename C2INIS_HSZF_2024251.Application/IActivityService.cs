using C2INIS_HSZF_2024251.Model;

namespace C2INIS_HSZF_2024251.Application
{
    public interface IActivityService
    {
        Activity CreateEntity(Activity newActivity);
        Activity DeleteEntity(Activity activity);
        IEnumerable<Activity> ListEntities();
        IEnumerable<Activity> ReOrder<T>(Func<Activity, T> keySelector, bool ascending = true);
        Activity SearchById(int id);
        IEnumerable<Activity> SearchByName(string keyWord);
        Activity UpdateEntity(Activity oldActivity, Activity newActivity);
    }
}
using C2INIS_HSZF_2024251.Model;
using C2INIS_HSZF_2024251.Persistence.MsSql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2INIS_HSZF_2024251.Application
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityDataProvider activityDataProvider;

        public ActivityService(IActivityDataProvider activityDataProvider)
        {
            this.activityDataProvider = activityDataProvider;
        }

        public Activity CreateEntity(Activity newActivity)
        {
            activityDataProvider.CreateEntity(newActivity);
            return newActivity;
        }
        public Activity DeleteEntity(Activity activity)
        {
            return activityDataProvider.DeleteEntity(activity);
        }
        public Activity UpdateEntity(Activity oldActivity, Activity newActivity)
        {
            //Activity newActivity = new Activity(date, comment, name, animalId, keeperId);
            return activityDataProvider.Update(oldActivity, newActivity);
        }

        public IEnumerable<Activity> ListEntities()
        {
            return activityDataProvider.ReadAllEntity();
        }
        public IEnumerable<Activity> SearchByName(string keyWord)
        {
            //return DBManager.ActivityDBManager.SearchByKeeper(keyWord);
            /*IEnumerable<Activity> collection = ListEntities();
            return collection.Where(x => x.Name.Contains(keyWord));*/

            return activityDataProvider.SearchByName(keyWord);
        }
        public IEnumerable<Activity> ReOrder<T>(Func<Activity, T> keySelector, bool ascending = true)
        {
            return activityDataProvider.ReOrder<T>(keySelector, ascending);
            /*IEnumerable<Activity> collection = ListEntities();
            return ascending
                ? collection.OrderBy(keySelector).ToList()
                : collection.OrderByDescending(keySelector).ToList();*/

        }

        public Activity SearchById(int id)
        {

            return activityDataProvider.SearchById(id);
        }

        /*public bool LessThanThree(Activity newActivity)
        {
            /*var day = newActivity.Date.Date;
            Food actKeeper = newActivity.Food;
            int activityCount = actKeeper.Activities.Count(x => x.Date.Date == newActivity.Date.Date);
            if (activityCount > 10)
            {
                return true;
            }
            else
            {
                return false;
            }
            return false;
        }*/
    }
}

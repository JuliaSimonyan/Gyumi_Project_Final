using Gyumri.Common.ViewModel.Activity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyumri.App.Interfaces
{
    public interface IActivityService
    {
        Task<List<ActivityViewModel>> GetAllActivities();
        Task<ActivityViewModel> GetActivityById(int id);
        Task AddActivity(ActivityViewModel activityVm);
        Task EditActivity(ActivityViewModel activityVm);
        Task DeleteActivity(int id);
    }
}

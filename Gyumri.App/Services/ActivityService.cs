using Gyumri.App.Interfaces;
using Gyumri.Common.ViewModel.Activity;
using Gyumri.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyumri.App.Services
{
    public class ActivityService : IActivityService
    {
        private readonly ApplicationContext _context;

        public ActivityService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<List<ActivityViewModel>> GetAllActivities()
        {
            return await _context.Activities
                .Select(a => new ActivityViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    OpeningHours = a.OpeningHours,
                    PriceMin = a.PriceMin,
                    PriceMax = a.PriceMax,
                    ActivityType = a.ActivityType,
                    Image = a.Image,
                    Rating = a.Rating
                })
                .ToListAsync();
        }

        public async Task<ActivityViewModel> GetActivityById(int id)
        {
            var a = await _context.Activities.FindAsync(id);
            if (a == null) return null;

            return new ActivityViewModel
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                OpeningHours = a.OpeningHours,
                PriceMin = a.PriceMin,
                PriceMax = a.PriceMax,
                ActivityType = a.ActivityType,
                Image = a.Image,
                Rating = a.Rating
            };
        }

        public async Task AddActivity(ActivityViewModel activityVm)
        {
            var activity = new Activity
            {
                Name = activityVm.Name,
                Description = activityVm.Description,
                OpeningHours = activityVm.OpeningHours,
                PriceMin = activityVm.PriceMin,
                PriceMax = activityVm.PriceMax,
                ActivityType = activityVm.ActivityType,
                Image = activityVm.Image,
                Rating = activityVm.Rating
            };

            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();
        }

        public async Task EditActivity(ActivityViewModel activityVm)
        {
            var activity = await _context.Activities.FindAsync(activityVm.Id);
            if (activity == null) return;

            activity.Name = activityVm.Name;
            activity.Description = activityVm.Description;
            activity.OpeningHours = activityVm.OpeningHours;
            activity.PriceMin = activityVm.PriceMin;
            activity.PriceMax = activityVm.PriceMax;
            activity.ActivityType = activityVm.ActivityType;
            activity.Image = activityVm.Image;
            activity.Rating = activityVm.Rating;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteActivity(int id)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity == null) return;

            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
        }
    }
    }

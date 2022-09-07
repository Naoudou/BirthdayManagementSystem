using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BirthdayManagementSystem.Models
{


    public class JobScheduler
    {
       
        public static async System.Threading.Tasks.Task StartAsync()
        {
            anniv_dbEntities db = new anniv_dbEntities();
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<Jobclass>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInHours(24)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(10,19))
                  )
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}
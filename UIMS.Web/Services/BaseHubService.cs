﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models;

namespace UIMS.Web.Services
{
    public class BaseHubService
    {
        protected const string SUCCESSFUL_MESSAGE = "اطلاع رسانی با موفقیت انجام شد";

        protected readonly SemesterService _SemesterService;
        protected readonly UserService _userService;
        protected int UserId { get; set; }
        protected List<string> Roles { get; set; }
        //protected List<string> Roles => Context.User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role").Value.Split(',').ToList();


        public void InitializeToken(int userId,List<string> roles)
        {
            UserId = userId;
            Roles = roles;

        }

        protected async Task<AppUser> GetCurrentUser()
        {
            return await _userService.GetAsync(x => x.Id == UserId);
        }

        protected async Task<int> GetCurrentSemesterId()
        {
            return (await _SemesterService.GetCurrentAsycn()).Id;
        }

        public BaseHubService(SemesterService semesterService, UserService userService)
        {
            _SemesterService = semesterService;
            _userService = userService;

        }





        protected List<NotificationReceiver> GetReceiversByIds(params string[] ids)
        {
            List<NotificationReceiver> receivers = new List<NotificationReceiver>();
            foreach (var id in ids)
            {
                receivers.Add(new NotificationReceiver() { UserId = int.Parse(id) });
            }
            return receivers;
        }

        protected List<NotificationReceiver> GetReceiversByIds(params int[] ids)
        {
            List<NotificationReceiver> receivers = new List<NotificationReceiver>();
            foreach (var id in ids)
            {
                receivers.Add(new NotificationReceiver() { UserId = id });
            }
            return receivers;
        }

        protected List<NotificationReceiver> GetReceiversByIds(IEnumerable<int> ids)
        {
            List<NotificationReceiver> receivers = new List<NotificationReceiver>();
            foreach (var id in ids)
            {
                receivers.Add(new NotificationReceiver() { UserId = id });
            }
            return receivers;
        }

        protected async Task<Tuple<Notification, AppUser>> GetNotificationAsync(string content, string title, int typeId, List<NotificationReceiver> receivers, string subtitle = "")
        {
            var semesterId = await GetCurrentSemesterId();
            var user = await _userService.GetAsync(x => x.Id == UserId);
            return new Tuple<Notification, AppUser>(new Notification()
            {
                Content = content,
                NotificationTypeId = typeId,
                Title = title,
                Subtitle = subtitle,
                SemesterId = semesterId,
                SenderId = user.Id,
                Receivers = receivers
            },
                user);
        }
    }
}

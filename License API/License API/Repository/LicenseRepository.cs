﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using License_API.Models;

namespace License_API.Repository
{
    public class LicenseRepository
    {
        private DataBaseContext context;
        public LicenseRepository()
        {
            context = new DataBaseContext();
        }
        public LICENSE_USER GetUser(string loginID)
        {
            LICENSE_USER user = context.LICENSE_USER.Where(w => w.LOGIN_ID == loginID).FirstOrDefault();
            return user;
        }

        public bool SaveLogin(LicenseInputModel user)
        {
            #region update existing Login record 

            List<LICENSE_LOG_HISTORY> lst = context.LICENSE_LOG_HISTORY.
                            Where(t => t.LOGIN_ID == user.Login_ID &&
                                       t.IP_ADDRESS == user.IPAddress &&
                                       t.DEVICE_TYPE == user.DeviceType &&
                                       t.BROWSER_INFO == user.BrowserInfo &&
                                       t.LOGOFFTIME == null).ToList();

            if (lst != null && lst.Count > 0)
            {
                foreach (LICENSE_LOG_HISTORY l_log in lst)
                {
                    l_log.LOGOFFTIME = DateTime.Now;
                }
                context.SaveChanges();
            }
            #endregion

            LICENSE_LOG_HISTORY l_logHistory = new LICENSE_LOG_HISTORY();
            l_logHistory.LOGIN_ID = user.Login_ID;
            l_logHistory.IP_ADDRESS = user.IPAddress;
            l_logHistory.DEVICE_TYPE = user.DeviceType;
            l_logHistory.BROWSER_INFO = user.BrowserInfo;
            l_logHistory.LOGINTIME = DateTime.Now;
            context.LICENSE_LOG_HISTORY.Add(l_logHistory);
            context.Entry(l_logHistory).State = EntityState.Added;
            context.SaveChanges();

            return true;
        }

        public bool LogOff(LicenseInputModel user)
        {

            var existing_data = context.LICENSE_LOG_HISTORY.
                            Where(t => t.LOGIN_ID == user.Login_ID &&
                                       t.IP_ADDRESS == user.IPAddress &&
                                       t.DEVICE_TYPE == user.DeviceType &&
                                       t.BROWSER_INFO == user.BrowserInfo &&
                                       t.LOGOFFTIME == null).FirstOrDefault<LICENSE_LOG_HISTORY>();
            if (existing_data != null)
            {
                existing_data.LOGOFFTIME = DateTime.Now;
                context.SaveChanges();
            }
            return true;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using License_API.Models;
using License_API.Models.License;

namespace License_API.Repository
{
    public class LicenseRepository
    {
        private DataBaseContext context;
        public int LicenseNoOfUsers;
        public int SessionIntraval;
        public int NoOfLoggins;
        public string ProductKey = string.Empty;

        public LicenseRepository(string l_ProductKey)
        {
            context = new DataBaseContext();
            LICENSE_CONFIG cofig = context.LICENSE_CONFIG.Where(r => r.PRODUCT_KEY == l_ProductKey).FirstOrDefault();
            if (cofig != null)
            {
                LicenseNoOfUsers = cofig.NUMBER_OF_USERS;
                SessionIntraval = cofig.SESSION_TIME_INTRAVAL;
                NoOfLoggins = cofig.NUMBER_OF_LOGGING;
                ProductKey = cofig.PRODUCT_KEY;
            }
        }
        public LICENSE_USER GetUser(string loginID)
        {
            LICENSE_USER user = context.LICENSE_USER.Where(w => w.LOGIN_ID == loginID && w.Product_Key == ProductKey).FirstOrDefault();
            return user;
        }

        public List<LICENSE_LOG_HISTORY> GetAllActiveLoggedUser()
        {
            return context.LICENSE_LOG_HISTORY.Where(r => r.Product_Key == ProductKey && r.LOGOFFTIME == null).ToList();
        }

        public bool SaveLogin(LicenseInputModel user)
        {
            #region update existing Login record 

            List<LICENSE_LOG_HISTORY> lst = context.LICENSE_LOG_HISTORY.
                            Where(t => t.Product_Key == user.Product_Key &&
                                       t.LOGIN_ID == user.Login_ID &&
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
            l_logHistory.Product_Key = user.Product_Key;
            l_logHistory.LOGINTIME = DateTime.Now;
            l_logHistory.SESSION_RENEW = DateTime.Now;
            context.LICENSE_LOG_HISTORY.Add(l_logHistory);
            context.Entry(l_logHistory).State = EntityState.Added;
            context.SaveChanges();
            return true;
        }

        public bool LogOff(LicenseInputModel user)
        {
            var existing_data = context.LICENSE_LOG_HISTORY.
                            Where(t => t.Product_Key == user.Product_Key &&
                                       t.LOGIN_ID == user.Login_ID &&
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

        public bool KeepAlive(LicenseInputModel user)
        {
            var existing_data = context.LICENSE_LOG_HISTORY.
                            Where(t => t.Product_Key == user.Product_Key &&
                                       t.LOGIN_ID == user.Login_ID &&
                                       t.IP_ADDRESS == user.IPAddress &&
                                       t.DEVICE_TYPE == user.DeviceType &&
                                       t.BROWSER_INFO == user.BrowserInfo &&
                                       t.LOGOFFTIME == null).FirstOrDefault<LICENSE_LOG_HISTORY>();
            if (existing_data != null)
            {
                existing_data.LOGINTIME = DateTime.Now;
                context.SaveChanges();
            }
            return true;
        }
        //Session Expired users will be logged off
        public bool SessionExpire(out int NoOfUsersExpired)
        {
            NoOfUsersExpired = 0;
            DateTime SessionExpire = DateTime.Now.AddSeconds(SessionIntraval);
            var sessionExpiredUsers = context.LICENSE_LOG_HISTORY.
                            Where(t => t.Product_Key == ProductKey &&
                                       t.SESSION_RENEW < SessionExpire &&
                                       t.LOGOFFTIME == null).ToList();
            if (sessionExpiredUsers != null && sessionExpiredUsers.Count > 0)
            {
                NoOfUsersExpired = sessionExpiredUsers.Count;
                foreach (LICENSE_LOG_HISTORY l in sessionExpiredUsers)
                {
                    l.LOGOFFTIME = DateTime.Now;
                    context.SaveChanges();
                }
            }
            return true;
        }
    }
}
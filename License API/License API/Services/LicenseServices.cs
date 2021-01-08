using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using License_API.Models;
using License_API.Models.License;
using License_API.Repository;
using License_API.UnitofWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace License_API.Services
{
    public class LicenseServices
    {
        private LicenseRepository l_repo;
        private LicenseResponseModel response = new LicenseResponseModel();
        public object Login(LicenseInputModel licenseInputModel)
        {
            l_repo = new LicenseRepository(licenseInputModel.Product_Key);
            string ErrMsg = string.Empty;
            if (l_repo.ProductKey != "")
            {
                if (validateCrdential(licenseInputModel.Login_ID, licenseInputModel.Password, out ErrMsg))
                {
                    if (validateLicense(licenseInputModel, out ErrMsg))
                    {
                        l_repo.SaveLogin(licenseInputModel);
                        response.status_Description = "Successfully Logged In";
                        response.status_code = "1";
                    }
                    else
                    {
                        response.status_Description = ErrMsg;
                        response.status_code = "0";
                    }
                }
                else
                {
                    response.status_Description = ErrMsg;
                    response.status_code = "0";
                }
            }
            else
            {
                response.status_Description = "Invalid Product Key";
                response.status_code = "0";
            }
            return Result(response);
        }

        public object LogOff(LicenseInputModel licenseInputModel)
        {
            string ErrMsg = string.Empty;
            l_repo = new LicenseRepository(licenseInputModel.Product_Key);
            if (l_repo.ProductKey != "")
            {
                l_repo.LogOff(licenseInputModel);
                response.status_Description = "Successfuly Logged Off";
                response.status_code = "1";
            }
            else
            {
                response.status_Description = "Invalid Product Key";
                response.status_code = "0";
            }
            return Result(response);
        }

        public object KeepAlive(LicenseInputModel licenseInputModel)
        {
            string ErrMsg = string.Empty;
            l_repo = new LicenseRepository(licenseInputModel.Product_Key);
            if (l_repo.ProductKey != "")
            {
                l_repo.LogOff(licenseInputModel);
                response.status_Description = "Successfuly Session Extended";
                response.status_code = "1";
            }
            else
            {
                response.status_Description = "Invalid Product Key";
                response.status_code = "0";
            }
            return Result(response);
        }

        public object SessionExpire(string Product_Key)
        {
            string ErrMsg = string.Empty;
            int NoOfUsersExpired;
            l_repo = new LicenseRepository(Product_Key);
            if (l_repo.ProductKey != "")
            {
                l_repo.SessionExpire(out NoOfUsersExpired);
                response.status_Description = "Successfuly session expired users(" + NoOfUsersExpired.ToString() + ") are killed";
                response.status_code = "1";
            }
            else
            {
                response.status_Description = "Invalid Product Key";
                response.status_code = "0";
            }
            return Result(response);
        }

        public object GetAllLggedUser(string Product_Key)
        {
            string ErrMsg = string.Empty;
            l_repo = new LicenseRepository(Product_Key);
            if (l_repo.ProductKey != "")
            {
                var allUser = l_repo.GetAllActiveLoggedUser();
                if (allUser != null && allUser.Count > 0)
                {
                    return allUser.Select(r => new
                    {
                        Login_ID = r.LOGIN_ID,
                        Password = "",
                        IPAddress = r.IP_ADDRESS,
                        DeviceType = r.DEVICE_TYPE,
                        BrowserInfo = r.BROWSER_INFO,
                        LogginTime = r.LOGINTIME
                    });
                     
                }
                else
                {
                    //response.status_code = "1";
                    //response.status_Description = "No Records";
                    return new JArray();
                }
            }
            else
            {
                response.status_Description = "Invalid Product Key";
                response.status_code = "0";
            }
            return Result(response);
        }

        public object GetAllUser(string Product_Key)
        {
            string ErrMsg = string.Empty;
            l_repo = new LicenseRepository(Product_Key);
            if (l_repo.ProductKey != "")
            {
                var allUser = l_repo.GetAllUser();
                if (allUser != null && allUser.Count > 0)
                {
                    return allUser;
                }
                else
                {
                    return new JArray();
                }
            }
            else
            {
                response.status_Description = "Invalid Product Key";
                response.status_code = "0";
            }
            return Result(response);
        }

        
        public object AddUser(LICENSE_USER lICENSE_USER)
        {
            l_repo = new LicenseRepository(lICENSE_USER.Product_Key);
            string ErrMsg = string.Empty;
            if (l_repo.ProductKey != "")
            {
                if (ValidateUser(lICENSE_USER.LOGIN_ID, out ErrMsg))
                {
                        l_repo.AddUser(lICENSE_USER);
                        response.status_Description = "Successfully User Created";
                        response.status_code = "1";
                 }
                else
                {
                    response.status_Description = ErrMsg;
                    response.status_code = "0";
                }
            }
            else
            {
                response.status_Description = "Invalid Product Key";
                response.status_code = "0";
            }
            return Result(response);
        }

        private bool ValidateUser(string loginID, out string ErrorMsg)
        {
            LICENSE_USER l_user = l_repo.GetUser(loginID);
            ErrorMsg = string.Empty;
            if (l_user != null)
            {
                ErrorMsg = "User Name already exists";
                return false;
            }
            return true;
        }

        private bool validateCrdential(string loginId, string Password, out string ErrorMsg)
        {
            LICENSE_USER l_user = l_repo.GetUser(loginId);
            ErrorMsg = string.Empty;

            if (l_user == null)
            {
                ErrorMsg = "Invalid User Name";
                return false;
            }
            else
            {
                if (l_user.PASSWORD != Password)
                {
                    ErrorMsg = "Invalid Password";
                    return false;
                }
            }
            return true;
        }

        private bool validateLicense(LicenseInputModel licenseInputModel, out string ErrorMsg)
        {
            ErrorMsg = string.Empty;
            List<LICENSE_LOG_HISTORY> lst = new List<LICENSE_LOG_HISTORY>();
            lst = l_repo.GetAllLoggedUser(licenseInputModel);
            if (lst.Count < l_repo.LicenseNoOfUsers)
            {
                if (l_repo.ISMULTI_lOGGIN == "N")
                {
                    //neee to check
                }
                return true;
            }
            else
            {
                ErrorMsg = "Total number of logged users reached License. Kindly contact admin.";
                return false;
            }
        }

        public JObject Result(LicenseResponseModel res)
        {
            JObject resObj = new JObject();
            resObj.Add("status_code", res.status_code);
            resObj.Add("status_description", res.status_Description);
            return resObj;
        }

        
    }
}
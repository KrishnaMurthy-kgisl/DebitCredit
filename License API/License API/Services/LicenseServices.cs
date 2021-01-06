using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using License_API.Models;
using License_API.Repository;
using License_API.UnitofWork;

namespace License_API.Services
{
    public class LicenseServices
    {
        LicenseRepository l_repo = new LicenseRepository();
        public string Login(LicenseInputModel licenseInputModel)
        {
            string ErrMsg = validateCrdential(licenseInputModel.Login_ID, licenseInputModel.Password);
            if (ErrMsg == String.Empty)
            {
                l_repo.SaveLogin(licenseInputModel);
            }
            return ErrMsg;
        }

        public string LogOff(LicenseInputModel licenseInputModel)
        {
            l_repo.LogOff(licenseInputModel);
            return "Successfully Logged Out";
        }

        private string validateCrdential(string loinId, string Password)
        {
            LICENSE_USER l_user = l_repo.GetUser(loinId);
            string ErrorMsg = string.Empty;

            //Need to check the Username validd

            //Need to check the Password

            return ErrorMsg;
        }

        private bool validateCrdential(LicenseInputModel licenseInputModel)
        {
            return true;
        }

    }
}
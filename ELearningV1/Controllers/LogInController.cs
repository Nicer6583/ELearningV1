﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ELearningV1.Models;
using ELearningV1.Models.ViewModel;

namespace ELearningV1.Controllers
{
    public class LogInController : Controller
    {
        // GET: LogIn
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LogInAccount(string unum, string upass)
        {
            DAL SQLcon = new DAL();
            bool stats = false;
            string path = "http://trecnetkiosk:94/UploadedImages/";
            try
            {
                var logInData = SQLcon.KioskLogInUserData(unum, upass).SingleOrDefault();
                if (logInData != null)
                {
                    Session["EmployeeNumber"] = logInData.EmployeeNumber.ToString();
                    Session["EmployeeDeptName"] = logInData.Department.ToString();
                    Session["EmployeePositionName"] = logInData.Position.ToString();
                    Session["EmployeeReportTo"] = logInData.ReportTo.ToString();
                    var data1 = SQLcon.FBLoadLeftPanel(logInData.EmployeeNumber).Select(x => new VMELearnEmpData
                    {
                        EmpName = x.EmpName,
                        EmpImage = (x.EmpImage != "") ? x.EmpImage : "avatar3.png"
                    }).SingleOrDefault();
                    Session["EmployeeName"] = data1.EmpName;
                    Session["UserProfilePic"] = path + data1.EmpImage;
                    stats = SQLcon.SaveLogInLogHistory(logInData.EmployeeNumber.ToString(), DateTime.Now.ToString("MM/dd/yyyy"));
                }
                else
                {
                    Session.Clear();
                    Session.Abandon();
                    Session.RemoveAll();
                    stats = false;
                }
            }
            catch (Exception ex)
            {
                Session.Clear();
                Session.Abandon();
                Session.RemoveAll();
                stats = false;
            }
            var response = new JsonResult();
            response.Data = new
            {
                result = stats
            };
            return response;
        }


        public ActionResult LogOutAccount()
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            return Json(new { res = true });
        }

    }
}
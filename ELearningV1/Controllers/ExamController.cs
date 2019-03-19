﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ELearningV1.Models;
using ELearningV1.Models.ViewModel;
using KioskVersion3.Models.ViewModel;
//
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ELearningV1.Controllers
{
    public class ExamController : Controller
    {
        DAL SQLcon = new DAL();
        
        string QuestOrder = "";

        #region Done by boss Tiqius
        [HttpGet]
        public ActionResult ExamList(string CourseID)
        {
            VMViewCourses courseData = new VMViewCourses();
            try
            {
                courseData = SQLcon.ViewCoursesByID(CourseID).SingleOrDefault();
            }
            catch (Exception ex) { }

            return View(courseData);
        }

        public ActionResult SaveVideoData(CourseImage model, string CID, string Title)
        {
            var result = false;
            var file = model.ImageFile;
            if (file != null)
            {
                if (!System.IO.File.Exists(Server.MapPath("/UploadedFiles/" + file.FileName)))
                {
                    file.SaveAs(Server.MapPath("/UploadedFiles/" + file.FileName));
                    try
                    {
                        DAL SQLcon = new DAL();
                        result = SQLcon.UploadNewFile(Title, "Video", "/UploadedFiles/" + file.FileName, CID);
                    }
                    catch (Exception ex) { }
                }
                else
                {
                    result = false;
                }
            }
            var response = new JsonResult();
            response.Data = new
            {
                res = result,
                vName = "/UploadedFiles/" + file.FileName
            };
            return response;
        }

        public ActionResult SavePPTData(CourseImage model, string CID, string Title)
        {
            var result = false;
            var file = model.ImageFile;
            if (file != null)
            {
                if (!System.IO.File.Exists(Server.MapPath("/UploadedFiles/" + file.FileName)))
                {
                    file.SaveAs(Server.MapPath("/UploadedFiles/" + file.FileName));
                    try
                    {
                        DAL SQLcon = new DAL();
                        result = SQLcon.UploadNewFile(Title, "PDF", "/UploadedFiles/" + file.FileName, CID);
                    }
                    catch (Exception ex) { }
                }
                else
                {
                    result = false;
                }
            }
            var response = new JsonResult();
            response.Data = new
            {
                res = result,
                pName = "/UploadedFiles/" + file.FileName
            };
            return response;
        }

        public JsonResult LoadSectionData(DataTablesParam param, string courseID)
        {
            List<VMCourseSection> sectionData = new List<VMCourseSection>();
            int pageNo = 1;
            int totalCount = 0;

            if (param.iDisplayStart >= param.iDisplayLength)
            { pageNo = (param.iDisplayStart / param.iDisplayLength) + 1; }


            totalCount = SQLcon.ViewCourseSectionByID(courseID).Count();
            sectionData = SQLcon.ViewCourseSectionByID(courseID).OrderBy(x => x.OrderSec).Select(x => new VMCourseSection
            {
                ID = x.ID,
                Title = x.Title,
                Type = x.Type,
                SrcFile = x.SrcFile,
                CourseID = x.CourseID,
                OrderSec = x.OrderSec
            }).AsEnumerable().ToList();

            return Json(new
            {
                aaData = sectionData,
                eEcho = param.sEcho,
                iTotalDisplayRecords = totalCount,
                iTotalRecords = sectionData.Count()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateSectionOrderingByID(string SID, string OrderVal)
        {
            var result = false;
            try
            {
                DAL SQLcon = new DAL();
                result = SQLcon.UpdateSectionOrdering(SID, OrderVal);
            }
            catch (Exception ex)
            {
                result = false;
            }
            var response = new JsonResult();
            response.Data = new
            {
                res = result
            };
            return response;
        }

        public ActionResult DeleteSectionByID(string SID)
        {
            var result = false;
            try
            {
                DAL SQLcon = new DAL();
                result = SQLcon.DeleteSectionById(SID);
            }
            catch (Exception ex)
            {
                result = false;
            }
            var response = new JsonResult();
            response.Data = new
            {
                res = result
            };
            return response;
        }
        #endregion

        #region Creating Test Name and Question
        public ActionResult CreateExam()
        {
            return View();
        }

        public ActionResult ExamQuestionList()
        {
            return View();
        }

        public ActionResult SaveTestTitle(string CID, string Title)
        {
            var result = "";
            var user = Session["EmployeeNumber"].ToString();
            try
            {
                DAL SQLcon = new DAL();
                result = SQLcon.SaveTestName(Title, "Test", CID);
            }
            catch (Exception ex)
            {
                result = "false";
            }
            var response = new JsonResult();
            response.Data = new
            {
                res = result
            };
            return response;
        }

        public ActionResult saveQuestion(string Question, string QuestionType, string Answer1, string Answer2, string Answer3, string Answer4, string CorrectAns1, string CourseSecID, string CourseID)
        {

            string EmployeeNumber = Session["EmployeeNumber"].ToString();
            if (Answer1 == "" || Answer1 == null)
            {

            }
            else if (Answer2 == "" || Answer2 == null)
            {

            }
            else if (Answer3 == "" || Answer3 == null)
            {

            }
            else if (Answer4 == "" || Answer4 == null)
            {

            }

            string result = SQLcon.SaveExamAndQuestion(Question, QuestionType, Answer1, Answer2, Answer3, Answer4, CorrectAns1, EmployeeNumber, CourseSecID, CourseID);

            var response = new JsonResult();
            response.Data = new
            {
                res = result
            };
            return response;
        }

        public ActionResult deleteQuestion(string ID)
        {
            var result = "";
            try
            { result = SQLcon.DeleteQuestion(ID); }
            catch (Exception ex)
            { result = "false"; }

            var response = new JsonResult();
            response.Data = new
            { res = result };
            return response;
        }

        public ActionResult updateQuestion(string ID, string Question, string C1, string C2, string C3, string C4, string CorAns)
        {
            var result = "";
            try
            { result = SQLcon.UpdateQuestion(ID, Question, C1, C2, C3, C4, CorAns); }
            catch (Exception ex)
            { result = "false"; }

            var response = new JsonResult();
            response.Data = new
            { res = result };
            return response;
        }

        public ActionResult updateQuestionOrderNumber(string ID, string OrderNo)
        {
            var result = "";
            try
            { result = SQLcon.UpdateQuestionOrderNo(ID, OrderNo); }
            catch (Exception ex)
            { result = "false"; }

            var response = new JsonResult();
            response.Data = new
            { res = result };
            return response;
        }

        public ActionResult updateTestName(string ID, string TestName)
        {
            var result = "";
            try
            { result = SQLcon.UpdateTestName(ID, TestName); }
            catch (Exception ex)
            { result = "false"; }

            var response = new JsonResult();
            response.Data = new
            { res = result };
            return response;
        }

        public JsonResult getQuestionList(DataTablesParam param, string CourSecID)
        {
            List<VMGetExamQuestions> QuestList = new List<VMGetExamQuestions>();
            int pageNo = 1;
            int totalCount = 0;

            if (param.iDisplayStart >= param.iDisplayLength)
            { pageNo = (param.iDisplayStart / param.iDisplayLength) + 1; }

            totalCount = SQLcon.getQuestionList(CourSecID).Count();
            QuestList = SQLcon.getQuestionList(CourSecID).OrderBy(x => x.OrderNumber).Select(x => new VMGetExamQuestions
            {
                ID = x.ID,
                Question = x.Question,
                QuestionType = x.QuestionType,
                C1 = x.C1,
                C2 = x.C2,
                C3 = x.C3,
                C4 = x.C4,
                CAnswer = x.CAnswer,
                CourseSectionID = x.CourseSectionID,
                CourseID = x.CourseID,
                OrderNumber = x.OrderNumber,
            }).AsEnumerable().ToList();

            return Json(new
            {
                aaData = QuestList,
                eEcho = param.sEcho,
                iTotalDisplayRecords = totalCount,
                iTotalRecords = QuestList.Count()
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Exam
        public ActionResult Exam(string CourseID)
        {
            DAL SQLcon = new DAL();
            List<VMGetExamQuestion> cList = SQLcon.getQuest(CourseID).Select(x => new VMGetExamQuestion
            {
                ID = x.ID,
                CourseID = x.CourseID,
                CourseSectionID = x.CourseSectionID,
                OrderNumber = x.OrderNumber,
                Question = x.Question,
                QuestionType = x.QuestionType,
                C1 = x.C1,
                C2 = x.C2,
                C3 = x.C3,
                C4 = x.C4,
                CAnswer = x.CAnswer
            }).ToList();

            return View("Exam", cList);
        }

        public ActionResult GetDataToLoad(string CourseID, int PreviousOrderSec)
        {
            var loadData = "";
            var CourseSectionOrder = "";
            var dataCount = SQLcon.getDataToLoad(CourseID, PreviousOrderSec).Count();

            if(dataCount > 0)
            {
                loadData = SQLcon.getDataToLoad(CourseID, PreviousOrderSec).OrderBy(x => x.OrderSec).Select(x => x.Type).FirstOrDefault().ToString();
                CourseSectionOrder = SQLcon.getDataToLoad(CourseID, PreviousOrderSec).OrderBy(x => x.OrderSec).Select(x => x.OrderSec).FirstOrDefault().ToString();
            }
            else
            {   loadData = ""; CourseSectionOrder = ""; }

            var response = new JsonResult();
            response.Data = new
            {   res = loadData,
                _currentSectionOrder = CourseSectionOrder
            };
            return response;
        }

        public JsonResult loadVideo(string CourseID)
        {
            var vidTitle = SQLcon.getVideoPath(CourseID).Select(x => x.Title).FirstOrDefault().ToString();
            var vidPath = SQLcon.getVideoPath(CourseID).Select(x => x.VideoPath).FirstOrDefault().ToString();

            var responseVideo = new JsonResult();
            responseVideo.Data = new
            {
                resTitleVideo = vidTitle,
                resVideo = vidPath,
            };
            return responseVideo;
        }

        public JsonResult loadPDF(string CourseID)
        {
            var PDFTitle = SQLcon.getPDFPath(CourseID).Select(x => x.Title).FirstOrDefault().ToString();
            var PDFpath = SQLcon.getPDFPath(CourseID).Select(x => x.PDFPath).FirstOrDefault().ToString();

            var responsePDF = new JsonResult();
            responsePDF.Data = new
            {
                resTitlePDF = PDFTitle,
                resPDF = PDFpath,
            };
            return responsePDF;
        }

        public ActionResult SaveEmployeeAnswers(string[] QuestionID, string[] Answers)
        {
            var user = Session["EmployeeNumber"].ToString();
            var AnswerfromDB = "";
            string[] AnswersfromDB = new string[] { };

            var QuestionIDfromDB = "";
            string[] QuestionIDsfromDB = new string[] { };
            var result = "";

            foreach (var qID in QuestionID)
            {
                AnswerfromDB = SQLcon.getAnsList(qID).Select(x => x.Answers).FirstOrDefault().ToString();
                AnswersfromDB = AnswerfromDB.Split(',');
                foreach (var ans in Answers)
                {
                    if (AnswersfromDB.Contains(ans))
                    {   result = SQLcon.saveAnswers(qID,user,ans,"Y");   }
                    else
                    {   result = SQLcon.saveAnswers(qID, user, ans, "N");}
                }
            }

            /**
            foreach (var quest in QuestionID)
            {
                foreach (var ans in Answers)
                {
                    try
                    { result = SQLcon.saveAnswers(quest, user, ans, isCorrect); }
                    catch (Exception ex)
                    { result = "false"; }
                    break;
                }
            }
            **/
            var response = new JsonResult();
            response.Data = new
            {
                res = result
            };
            return response;
        }
        
        public ActionResult getScoreofEmployeeExam(string CourseID, string CourseSectionID)
        {
            List<VMGetAnswers> GetAnswerFromDB = new List<VMGetAnswers>();
            List<VMGetQuestionID> QuestionID = SQLcon.getQuestListByCourseSec(CourseSectionID).Select(x => new VMGetQuestionID{   QuestionID = x.QuestionID  }).ToList();

            List<string> QuestIDFromDB = new List<string>();
            List<string> AnsFromDB = new List<string>();

            //Get the list of QUESTION ID
            foreach (var data in QuestionID)
            {   QuestIDFromDB.Add(data.QuestionID.ToString());  }

            //Get the CORRECT ANSWER from DB
            foreach (var questID in QuestIDFromDB)
            {   GetAnswerFromDB = SQLcon.getAnsList(questID).Select(x => new VMGetAnswers { Answers = x.Answers }).ToList();
                foreach (var a in GetAnswerFromDB)
                {
                    var result = a.Answers.Split(',');
                    foreach(var result2 in result)
                    {   AnsFromDB.Add(result2); }
                    //break;
                }
            }

            var responseScore = new JsonResult();
            responseScore.Data = new
            {
            };
            return responseScore;
        }
        /**
        public JsonResult loadQuestionaire(string CourseID)
        {
            var qID = "";
            var qCourseID = "";
            var qCourseSectionID = "";
            var OrderNumber = "";
            var Question = "";
            var QuestionType = "";
            var C1 = "";
            var C2 = "";
            var C3 = "";
            var C4 = "";
            var CAnswer = "";

            if (QuestOrder == "" || QuestOrder == null || QuestOrder == "0" || QuestOrder == 0.ToString())
            {
                qID = SQLcon.getQuest(CourseID).Select(x => x.ID).FirstOrDefault().ToString();
                qCourseID = SQLcon.getQuest(CourseID).Select(x => x.CourseID).FirstOrDefault().ToString();
                qCourseSectionID = SQLcon.getQuest(CourseID).Select(x => x.CourseSectionID).FirstOrDefault().ToString();
                OrderNumber = SQLcon.getQuest(CourseID).Select(x => x.OrderNumber).FirstOrDefault().ToString();
                Question = SQLcon.getQuest(CourseID).Select(x => x.Question).FirstOrDefault().ToString();
                QuestionType = SQLcon.getQuest(CourseID).Select(x => x.QuestionType).FirstOrDefault().ToString();
                C1 = SQLcon.getQuest(CourseID).Select(x => x.C1).FirstOrDefault().ToString();
                C2 = SQLcon.getQuest(CourseID).Select(x => x.C2).FirstOrDefault().ToString();
                C3 = SQLcon.getQuest(CourseID).Select(x => x.C3).FirstOrDefault().ToString();
                C4 = SQLcon.getQuest(CourseID).Select(x => x.C4).FirstOrDefault().ToString();
                CAnswer = SQLcon.getQuest(CourseID).Select(x => x.CAnswer).FirstOrDefault().ToString();

                Session["QuestOrder"] = OrderNumber;
            }

            else
            {
                qID = SQLcon.getQuest(CourseID).Select(x => x.ID).FirstOrDefault().ToString();
                qCourseID = SQLcon.getQuest(CourseID).Select(x => x.CourseID).FirstOrDefault().ToString();
                qCourseSectionID = SQLcon.getQuest(CourseID).Select(x => x.CourseSectionID).FirstOrDefault().ToString();
                OrderNumber = SQLcon.getQuest(CourseID).Select(x => x.OrderNumber).FirstOrDefault().ToString();
                Question = SQLcon.getQuest(CourseID).Select(x => x.Question).FirstOrDefault().ToString();
                QuestionType = SQLcon.getQuest(CourseID).Select(x => x.QuestionType).FirstOrDefault().ToString();
                C1 = SQLcon.getQuest(CourseID).Select(x => x.C1).FirstOrDefault().ToString();
                C2 = SQLcon.getQuest(CourseID).Select(x => x.C2).FirstOrDefault().ToString();
                C3 = SQLcon.getQuest(CourseID).Select(x => x.C3).FirstOrDefault().ToString();
                C4 = SQLcon.getQuest(CourseID).Select(x => x.C4).FirstOrDefault().ToString();
                CAnswer = SQLcon.getQuest(CourseID).Select(x => x.CAnswer).FirstOrDefault().ToString();
            }

            var responsePDF = new JsonResult();
            responsePDF.Data = new
            {
                _QuestionID = qID,
                _CourseID = qCourseID,
                _CourseSectionID = qCourseSectionID,
                _OrderNo = OrderNumber,
                _Question = Question,
                _QuestionType = QuestionType,
                _C1 = C1,
                _C2 = C2,
                _C3 = C3,
                _C4 = C4,
                _CAnswer = CAnswer
            };
            return responsePDF;
        }
        **/
        #endregion
    }
}
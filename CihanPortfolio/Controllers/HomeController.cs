using CihanPortfolio.Entities;
using Dapper;
using CihanPortfolio.Extensions;
using CihanPortfolio.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI.WebControls;
using System.Web.WebSockets;
using System.ComponentModel;

namespace CihanPortfolio.Controllers
{
    public class HomeController : Controller
    {


        [HttpGet]
        public ActionResult Index()
        {

            //Burada sql bağlantısı oluşturuluyor
            SqlConnection connection = new SqlConnection("server=.\\SQLExpress; database=CvDbCihan; integrated security =true");


            var about = connection.QueryFirst<About>(sql: "select * from Abouts");

            var skills = connection.Query<Skill>(sql: "ap_ListSkill", commandType: System.Data.CommandType.StoredProcedure);
            var services = connection.Query<CihanPortfolio.Entities.Service>(sql: "select * from Services");

            var serviceSlogan = connection.QuerySingle<Slogan>(commandType: System.Data.CommandType.StoredProcedure, sql: "ap_ListSlogan", param: new
            {
                @sectionName = "Services"
            });

            var skillSlogan = connection.QuerySingle<Slogan>(sql: "ap_ListSlogan", param: new
            {
                @sectionName = "Skills"
            }, commandType: System.Data.CommandType.StoredProcedure);

            var reviews = connection.Query<CustomerReview>(sql: "ap_ListCustomerReviews", commandType: System.Data.CommandType.StoredProcedure);

            var viewModel = new IndexViewModel();

            //Ekranda gösterilecek property'ler viewModele eklendi.

            viewModel.About = about;
            viewModel.Skills = skills;
            viewModel.Services = services;
            viewModel.ServiceSlogan = serviceSlogan;
            viewModel.SkillSlogan = skillSlogan;
            viewModel.CustomerReviews = reviews;

            return View(viewModel);
        }

         //Sayfadan girilen iletişim bilgilerinin post edildiği method 
        [HttpPost]
        public ActionResult Contact(Contact contact)
        {
            SqlConnection connection = new SqlConnection("server=.\\SQLExpress; database=CvDbCihan; integrated security =true");
            var affectedRows = connection.Execute(sql: "ap_CreateContact", commandType: System.Data.CommandType.StoredProcedure, param: contact);

            return RedirectToAction("Index");
        }       
        //iletişim sayfasını ekrana getirir
        [HttpGet]
        public ActionResult Contact()
        {

            return View();
        }
        //portfolio sayfasını ekrana getirir
        [HttpGet]
        public ActionResult Portfolio()
        {
            SqlConnection connection = new SqlConnection("server=.\\SQLExpress; database=CvDbCihan; integrated security =true");
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "ap_ListSlogan";


            command.Parameters.Add("@sectionName", "Skills");

            connection.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var title = reader.GetString(2);
                var description = reader.GetString(3);
            }
            connection.Close();
            reader.Close();
            return View();
        }

    }
}
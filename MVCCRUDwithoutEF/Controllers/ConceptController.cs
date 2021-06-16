using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MVCCRUDwithoutEF.Data;
using MVCCRUDwithoutEF.Models;

namespace MVCCRUDwithoutEF.Controllers
{
    public class ConceptController : Controller
    {
        private readonly IConfiguration _configuration;

        public ConceptController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        // GET: Concept
        public IActionResult Index()
        {
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("ConceptViewAll", sqlConnection);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.Fill(dtbl);
            }
            return View(dtbl);
        }

       

        // GET: Concept/AddOrEdit/
        public IActionResult AddOrEdit(int? id)
        {
           ConceptViewModel ConceptViewModel = new ConceptViewModel();
            if (id > 0)
                ConceptViewModel = FetchConceptByID(id);
           return View(ConceptViewModel);
        }

        // POST: Concept/AddOrEdit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit(int id, [Bind("ConceptID,Title,Author,Price")] ConceptViewModel ConceptViewModel)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection sqlConnection= new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCmd = new SqlCommand("ConceptAddOrEdit", sqlConnection);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("ConceptID", ConceptViewModel.ConceptID);
                    sqlCmd.Parameters.AddWithValue("Title", ConceptViewModel.Title);
                    sqlCmd.Parameters.AddWithValue("Author", ConceptViewModel.Author);
                    sqlCmd.Parameters.AddWithValue("Price", ConceptViewModel.Price);
                    sqlCmd.ExecuteNonQuery();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ConceptViewModel);
        }

        // GET: Concept/Delete/5
        public IActionResult Delete(int? id)
        {
            ConceptViewModel ConceptViewModel = FetchConceptByID(id);
            return View(ConceptViewModel);
        }

        // POST: Concept/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlCommand sqlCmd = new SqlCommand("ConceptDeleteByID", sqlConnection);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("ConceptID",id);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction(nameof(Index));
        }

        [NonAction]
        public ConceptViewModel FetchConceptByID(int? id) 
        {
            ConceptViewModel ConceptViewModel = new ConceptViewModel();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                DataTable dtbl = new DataTable();
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("ConceptViewByID", sqlConnection);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("ConceptID", id);
                sqlDa.Fill(dtbl);
                if(dtbl.Rows.Count == 1)
                {
                    ConceptViewModel.ConceptID =Convert.ToInt32(dtbl.Rows[0]["ConceptID"].ToString());
                    ConceptViewModel.Title = dtbl.Rows[0]["Title"].ToString();
                    ConceptViewModel.Author = dtbl.Rows[0]["Author"].ToString();
                    ConceptViewModel.Price = Convert.ToInt32(dtbl.Rows[0]["Price"].ToString());
                }
                return ConceptViewModel;
            }
        }
    }
}

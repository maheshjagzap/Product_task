using Machinetest.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Machinetest.Controllers
{
    public class HomeController : Controller
    {

        MachineContext m1 = new MachineContext();   
        
        // GET: Home
        public ActionResult Index()
        {
         
            return View();
        }

        public ActionResult category()
        {

            return View();
        }


        [HttpPost]  
         public ActionResult category(Category c1)
        {
            m1.Categories.Add(c1);  
            m1.SaveChanges();

            Session["kk"] = "Record saved successfully.";


            return RedirectToAction("Index");   
        }

        public ActionResult Product()
        {
            var categories = m1.Categories.Select(x => new SelectListItem
            {
                Value = x.CategoryId.ToString(),
                Text = x.CategoryName
            }).ToList();

            ViewBag.CategoryList = categories;

            return View();  
        }

        [HttpPost]
      
        public ActionResult Product(Product p)
        {
            if (ModelState.IsValid)
            {
                m1.Products.Add(p);
                m1.SaveChanges();
                return RedirectToAction("ProductList");
            }

          return View();
        }

       [HttpGet]
        public ActionResult ProductList(int page = 1, int pageSize = 10)
        {
            int skipRecords = (page - 1) * pageSize;

            var products = m1.Products
                           .Include("Category")  // String-based Include
                           .OrderBy(p => p.ProductId) // Ensure ordering for consistent paging
                             .Skip(skipRecords)
                             .Take(pageSize)
                             .ToList();

            ViewBag.CurrentPage = page;
            return View(products);
        }

        public ActionResult Delete(int id)
        {
           var b=  m1.Products.Find(id);

            m1.Products.Remove(b);
            m1.SaveChanges();


            return RedirectToAction("ProductList");
        }

        public ActionResult Edit(int id)
        {
            var categories = m1.Categories.Select(p => new SelectListItem
            {
                Value = p.CategoryId.ToString(),
                Text = p.CategoryName
            }).ToList();

            ViewBag.CategoryList = categories;


            var x =   m1.Products.Find(id);
            return View(x);
        }

        [HttpPost]
        public ActionResult UData(Product p1)
        {
            // Retrieve the existing product from the database using the ProductId
            var existingProduct = m1.Products.Find(p1.ProductId);

            if (existingProduct != null)
            {
                // Update the properties of the existing product
                existingProduct.ProductName = p1.ProductName;
                existingProduct.CategoryId = p1.CategoryId;

                // Save the changes to the database
                m1.SaveChanges();
            }
            else
            {
                // Handle the case where the product does not exist
                ModelState.AddModelError("", "Product not found.");
                return View(p1); // Return to the view with the error
            }

            return RedirectToAction("ProductList");
        }











    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentDetails.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StudentDetails.Controllers
{
    public class ProductController : Controller
    {
        string dbconnectionStr = "Data Source = DESKTOP-6CON5KJ;Initial Catalog = AspNetCoreTest; Integrated Security = True";
        
        public IEnumerable<Product> Product()
            {
                List<Product> productList = new List<Product>();
              //  List productList = new List();
            var dbconfig = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json").Build();
            try
            {
                dbconnectionStr = dbconfig["ConnectionStrings:Myconnection"];
                string sql = "SP_Get_ProductList";
                using (SqlConnection connection = new SqlConnection(dbconnectionStr))
                {
                    SqlCommand command = new SqlCommand(sql, connection);
                    connection.Open();
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Product product = new Product();
                            product.Id = Convert.ToInt32(dataReader["Id"]);
                            product.ProductName = Convert.ToString(dataReader["ProductName"]);
                            product.ProductDescription = Convert.ToString(dataReader["ProductDescription"]);
                            product.ProductCost = Convert.ToDecimal(dataReader["ProductCost"]);
                            product.Stock = Convert.ToInt32(dataReader["Stock"]);
                            productList.Add(product);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return (IEnumerable<Product>)View(productList);
        }
        public IActionResult ProductCreate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ProductCreate(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dbconfig = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json").Build();
                    if (!string.IsNullOrEmpty(dbconfig.ToString()))
                    {
                        dbconnectionStr = dbconfig["ConnectionStrings:Myconnection"];
                        using (SqlConnection connection = new SqlConnection(dbconnectionStr))
                        {
                            string sql = "SP_Add_New_Product";
                            using (SqlCommand cmd = new SqlCommand(sql, connection))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                                cmd.Parameters.AddWithValue("@ProductDescription", product.ProductDescription);
                                cmd.Parameters.AddWithValue("@ProductCost", product.ProductCost);
                                cmd.Parameters.AddWithValue("@Stock", product.Stock);
                                connection.Open();
                                cmd.ExecuteNonQuery();
                                connection.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return RedirectToAction("Product");
        }
        public IActionResult ProductUpdate(int id)
        {
            var dbconfig = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json").Build();
            dbconnectionStr = dbconfig["ConnectionStrings:Myconnection"];
            Product product = new Product();
            using (SqlConnection connection = new SqlConnection(dbconnectionStr))
            {
                string sql = "SP_Get_Product_By_Id";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            product.Id = Convert.ToInt32(dataReader["Id"]);
                            product.ProductName = Convert.ToString(dataReader["ProductName"]);
                            product.ProductDescription = Convert.ToString(dataReader["ProductDescription"]);
                            product.ProductCost = Convert.ToDecimal(dataReader["ProductCost"]);
                            product.Stock = Convert.ToInt32(dataReader["Stock"]);
                        }
                    }
                }
                connection.Close();
            }
            return View(product);
        }
        [HttpPost]
        public IActionResult ProductUpdate(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dbconfig = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json").Build();
                    if (!string.IsNullOrEmpty(dbconfig.ToString()))
                    {
                        dbconnectionStr = dbconfig["ConnectionStrings:Myconnection"];
                        using (SqlConnection connection = new SqlConnection(dbconnectionStr))
                        {
                            string sql = "SP_Update_Product";
                            using (SqlCommand cmd = new SqlCommand(sql, connection))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@id", product.Id);
                                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                                cmd.Parameters.AddWithValue("@ProductDescription", product.ProductDescription);
                                cmd.Parameters.AddWithValue("@ProductCost", product.ProductCost);
                                cmd.Parameters.AddWithValue("@Stock", product.Stock);
                                connection.Open();
                                cmd.ExecuteNonQuery();
                                connection.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return RedirectToAction("Product");
        }
        [HttpPost]
        public IActionResult ProductDelete(int id)
        {
            var dbconfig = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json").Build();
            dbconnectionStr = dbconfig["ConnectionStrings:Myconnection"];
            using (SqlConnection connection = new SqlConnection(dbconnectionStr))
            {
                string sql = "SP_Delete_Product_By_Id";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {

                    }
                    connection.Close();
                }
            }
            return RedirectToAction("Product");
        }
    }
}

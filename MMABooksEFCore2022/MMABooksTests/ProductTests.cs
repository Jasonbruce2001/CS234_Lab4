using System.Collections.Generic;
using System.Linq;
using System;

using NUnit.Framework;
using MMABooksEFClasses.MarisModels;
using Microsoft.EntityFrameworkCore;

namespace MMABooksTests
{
    [TestFixture]
    public class ProductTests
    {
        MMABooksContext dbContext;
        Products? p;
        List<Products>? products;

        [SetUp]
        public void Setup()
        {
            dbContext = new MMABooksContext();
            dbContext.Database.ExecuteSqlRaw("call usp_testingResetData()");
        }

        [Test]
        public void GetAllTest()
        {
            products = dbContext.Products.OrderBy(p => p.ProductCode).ToList();
            Assert.AreEqual(16, products.Count);
            Assert.AreEqual("A4CS", products[0].ProductCode);
            PrintAll(products);
        }

        [Test]
        public void GetByPrimaryKeyTest()
        {
            p = dbContext.Products.Find("A4CS");
            Assert.IsNotNull(p);
            Assert.AreEqual(4637, p.OnHandQuantity);
            Console.WriteLine(p);
        }

        [Test]
        public void GetUsingWhere()
        {
            products = dbContext.Products.Where(p => p.ProductCode.StartsWith("A")).OrderBy(p => p.ProductCode).ToList();
            Assert.AreEqual(4, products.Count);
            Assert.AreEqual(4637, products[0].OnHandQuantity);
            PrintAll(products);
        }

        /*[Test]
        public void GetWithCustomersTest()
        {
            p = dbContext.States.Include("Customers").Where(s => s.StateCode == "OR").SingleOrDefault();
            Assert.IsNotNull(s);
            Assert.AreEqual("Ore", s.StateName);
            Assert.AreEqual(5, s.Customers.Count);
            Console.WriteLine(s);
        }*/

        [Test]
        public void DeleteTest()
        {
            //remove invoice
            p = dbContext.Products.Include("InvoiceLineItem").Where(p => p.ProductCode = "A4CS").SingleOrDefault();

            //remove product
            p = dbContext.Products.Find("A4CS");
            Assert.NotNull(p);

            dbContext.Products.Remove(p);
            dbContext.SaveChanges();
            Assert.IsNull(dbContext.Products.Find("A4CS"));
        }

        [Test]
        public void CreateTest()
        {
            p = new Products();
            p.ProductCode = "HI32";
            p.Description = "Hawaii Book";
            p.OnHandQuantity = 10;
            p.UnitPrice = 100;
            dbContext.Products.Add(p);
            dbContext.SaveChanges();
            Assert.IsNotNull(dbContext.Products.Find("HI32"));
        }

        [Test]
        public void UpdateTest()
        {
            p = dbContext.Products.Find("CRFC");
            p.OnHandQuantity = 100;

            dbContext.Products.Update(p);
            dbContext.SaveChanges();

            p = dbContext.Products.Find("CRFC");

            Assert.AreEqual(100, p.OnHandQuantity);
        }

        public void PrintAll(List<Products> products)
        {
            foreach (Products p in products)
            {
                Console.WriteLine(p);
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System;

using NUnit.Framework;
using MMABooksEFClasses.MarisModels;
using Microsoft.EntityFrameworkCore;

namespace MMABooksTests
{
    [TestFixture]
    public class CustomerTests
    {
        
        MMABooksContext dbContext;
        Customer? c;
        List<Customer>? customers;

        [SetUp]
        public void Setup()
        {
            dbContext = new MMABooksContext();
            dbContext.Database.ExecuteSqlRaw("call usp_testingResetData()");
        }

        [Test]
        public void GetAllTest()
        {
            customers = dbContext.Customers.OrderBy(c => c.CustomerId).ToList();
            Assert.AreEqual(696, customers.Count);
            Assert.AreEqual("Molunguri, A", customers[0].Name);
            //PrintAll(customers);
        }

        
        [Test]
        public void GetByPrimaryKeyTest()
        {
            c = dbContext.Customers.Find(1);
            Assert.IsNotNull(c);
            Assert.AreEqual("Molunguri, A", c.Name);
            Console.WriteLine(c);
        }

        [Test]
        public void GetUsingWhere()
        {
            customers = dbContext.Customers.Where(c => c.Name.StartsWith("A")).OrderBy(c => c.Name).ToList();
            Assert.AreEqual(37, customers.Count);
            Assert.AreEqual("Abeyatunge, Derek", customers[0].Name);
        }

        
        [Test]
        public void GetWithInvoicesTest()
        {
            c = dbContext.Customers.Include("Invoices").Where(c => c.CustomerId == 20).SingleOrDefault();

            Assert.IsNotNull(c);
            Assert.AreEqual("Doraville", c.City);
            Assert.AreEqual(3, c.Invoices.Count);
            Console.WriteLine(c);
        }

        [Test]
        public void DeleteTest()
        {
            c = dbContext.Customers.Find(1);
            dbContext.Customers.Remove(c);
            dbContext.SaveChanges();
            Assert.IsNull(dbContext.Customers.Find(1));
        }

        [Test]
        public void CreateTest()
        {
            c = new Customer();
            c.CustomerId = 99999;
            c.Name = "Test Customer";
            c.Address = "123 My Address";
            c.City = "Palo Alto";
            c.StateCode = "CA";
            c.ZipCode = "94025";

            dbContext.Customers.Add(c);
            dbContext.SaveChanges();
            Assert.IsNotNull(dbContext.Customers.Find(99999));
        }

        
        [Test]
        public void UpdateTest()
        {
            c = dbContext.Customers.Find(3);
            c.Name = "Tony the tiger";

            dbContext.Customers.Update(c);
            dbContext.SaveChanges();

            c = dbContext.Customers.Find(3);

            Assert.AreEqual("Tony the tiger", c.Name);
        }

        public void PrintAll(List<Customer> customers)
        {
            foreach (Customer c in customers)
            {
                Console.WriteLine(c);
            }
        }
    }
}
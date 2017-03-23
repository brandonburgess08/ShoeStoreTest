
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using ShoeStoreTest.Pages;

namespace SpecSeleniumShoeStorePractice
{
    [TestFixture(typeof(ChromeDriver))]
    [TestFixture(typeof(InternetExplorerDriver))]
    [TestFixture(typeof(FirefoxDriver))]

    public class ShoeStoreTests<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private const string URL = "http://shoestore-manheim.rhcloud.com/";

        [SetUp]
        public void CreateDriver()
        {
            _driver = new TWebDriver();
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(URL);
        }

        [Test]
        public void CheckMonthlyShoeDisplay()
        {
            HomePage page = new HomePage(_driver);

            string selectedMonth;

            for (int index = 0; index < page.Months.Count; index++)
            {
                var month = page.Months[index];
                selectedMonth = month.Text;
                month.Click();
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                var isTitleMatchingMonth = wait.Until(ExpectedConditions.TitleContains(selectedMonth));
                Assert.True(isTitleMatchingMonth);
                StorePage storePage = new StorePage(_driver);
               

                var shoeList = storePage.ShoeResults;
                //if there are shoe results for the month then verify everything else
                if (shoeList.Count > 0)
                {
                    foreach (var shoe in shoeList)
                    {
                        var releaseMonth = shoe.FindElement(By.ClassName("shoe_release_month")).Text;
                        Assert.True(selectedMonth == releaseMonth, "Selected month does not equal Release Month");

                        //verify that there is a description
                        var shoeDescription = shoe.FindElement(By.ClassName("shoe_description"));
                        Assert.True(shoeDescription.Displayed && shoeDescription.Text !="", "Shoe description did not display");

                        //verify image of shoe is present
                        var shoeImage = shoe.FindElement(By.ClassName("shoe_image"));
                        Assert.True(shoeImage.Displayed && shoeImage.GetAttribute("src")!="", "Image for the shoe did not display");

                        //check that there is a price
                        var shoePrice = shoe.FindElement(By.ClassName("shoe_price"));
                        Assert.True(shoePrice.Displayed && shoePrice.Text !="", "Shoe price did not display");
                    }
                }
                else
                {
                    Assert.Fail("There was not a description for the shoe in the month of {0}", selectedMonth);
                }
            }
        }

        [Test]
        public void SubmitEmailAddressForReminder()
        {
            HomePage page = new HomePage(_driver);

            string emailAddress = "testCustomer@gmail.com";

            page.EmailField.SendKeys(emailAddress);
            page.SubmitButton.Click();
          
            string expectedMessage = string.Format("Thanks! We will notify you of our new shoes at this email: {0}",emailAddress);
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            IWebElement successMessage = wait.Until(ExpectedConditions.ElementExists(By.ClassName("notice")));
            Assert.True(successMessage.Text == expectedMessage, "Message displayed not equal to expected message");
          
        }


        [Test]
        public void SubmitInvalidEmailAddress()
        {
            HomePage page = new HomePage(_driver);

            //Negative Test. Invalid email not allowed. 
            string emailAddress = "111111";

            page.EmailField.SendKeys(emailAddress);
           
            page.SubmitButton.Click();
            //Test will fail due to invalid email being accepted.
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            IWebElement successMessage = wait.Until(ExpectedConditions.ElementExists(By.ClassName("notice")));
            Assert.True(!successMessage.Displayed,"Invalid email accepted. Success message displayed");

        }

        [TearDown]
        public void TestFixtureTearnDown()
        {
            try
            {
                _driver.Quit();
            }
            catch (Exception randomException)
            {
                Console.WriteLine(randomException.Message,"Failed to quit the driver.");
            }
            finally
            {
                _driver.Dispose();
            }

        }
    }
}

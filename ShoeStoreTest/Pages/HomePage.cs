using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace ShoeStoreTest.Pages
{
    class HomePage
    {
        public HomePage(IWebDriver driver)
        {
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.TagName, Using = "li")]
        public IList<IWebElement> Months { get; set; }

        [FindsBy(How = How.Id, Using = "remind_email_input")]
        public IWebElement EmailField { get; set; }

        [FindsBy(How = How.CssSelector, Using = "div.left input:not([name='email'])")]
        public IWebElement SubmitButton { get; set; }

    }
}

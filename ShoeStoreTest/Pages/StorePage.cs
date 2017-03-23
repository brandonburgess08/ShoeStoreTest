using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace ShoeStoreTest.Pages
{
    class StorePage
    {
        public StorePage(IWebDriver driver)
        {
            PageFactory.InitElements(driver, this);
        }

         [FindsBy(How = How.ClassName, Using = "shoe_result")]
          public IList<IWebElement> ShoeResults { get; set; }
    }
}

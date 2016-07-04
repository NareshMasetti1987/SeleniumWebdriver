using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace SeleniumWebdriver.ComponentHelper
{
    public class CheckBoxHelper
    {
        private static IWebElement element;

        public static void CheckedCheckBox(By locator)
        {
            element = GenericHelper.GetElement(locator);
            element.Click();
        }

        public static bool IsCheckBoxChecked(By locator)
        {
            element = GenericHelper.GetElement(locator);
            string flag = element.GetAttribute("checked");

            if (flag == null)
                return false;
            else
                return flag.Equals("true") || flag.Equals("checked");
        }
    }
}


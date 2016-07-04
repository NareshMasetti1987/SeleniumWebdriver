using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using SeleniumWebdriver.Settings;

namespace SeleniumWebdriver.ComponentHelper
{
    public class GenericHelper
    {
        private static Func<IWebDriver, bool> WaitForWebElementFunc(By locator)
        {
            return ((x) =>
            {
                if (x.FindElements(locator).Count == 1)
                    return true;
                return false;
            });
        }

        private static Func<IWebDriver, IWebElement> WaitForWebElementInPageFunc(By locator)
        {
            return ((x) =>
            {
                if (x.FindElements(locator).Count == 1)
                    return x.FindElement(locator);
                return null;
            });
        }
        public static WebDriverWait GetWebdriverWait(TimeSpan timeout)
        {
            ObjectRepository.Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1));
            WebDriverWait wait = new WebDriverWait(ObjectRepository.Driver,timeout)
            {
                PollingInterval = TimeSpan.FromMilliseconds(500),
            };
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(ElementNotVisibleException));
            return wait;
        }  
        public static bool IsElemetPresent(By locator)
        {
            try
            {
                return ObjectRepository.Driver.FindElements(locator).Count == 1;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        public static IWebElement GetElement(By locator)
        {
            if (IsElemetPresent(locator))
                return ObjectRepository.Driver.FindElement(locator);
            else
                throw new NoSuchElementException("Element Not Found : " + locator.ToString());
        }

        public static void TakeScreenShot(string filename = "Screen")
        {
            var screen = ObjectRepository.Driver.TakeScreenshot();
            if (filename.Equals("Screen"))
            {
                filename = filename + DateTime.UtcNow.ToString("yyyy-MM-dd-mm-ss") + ".jpeg";
                screen.SaveAsFile(filename,ImageFormat.Jpeg);
                return;
            }
            screen.SaveAsFile(filename, ImageFormat.Jpeg);
        }

        public static bool WaitForWebElement(By locator,TimeSpan timeout) 
        {
            ObjectRepository.Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1));
            var wait = GetWebdriverWait(timeout);
            var flag = wait.Until(WaitForWebElementFunc(locator));
            ObjectRepository.Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(ObjectRepository.Config.GetElementLoadTimeOut()));
            return flag;
        }

        public static IWebElement WaitForWebElementInPage(By locator, TimeSpan timeout)
        {
            ObjectRepository.Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1));
            var wait = GetWebdriverWait(timeout);
            var flag = wait.Until(WaitForWebElementInPageFunc(locator));
            ObjectRepository.Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(ObjectRepository.Config.GetElementLoadTimeOut()));
            return flag;
        }

        

    }
}

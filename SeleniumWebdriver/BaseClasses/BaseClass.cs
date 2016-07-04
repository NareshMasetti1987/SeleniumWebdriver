using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;
using SeleniumWebdriver.ComponentHelper;
using SeleniumWebdriver.Configuration;
using SeleniumWebdriver.CustomException;
using SeleniumWebdriver.Settings;
using TechTalk.SpecFlow;

namespace SeleniumWebdriver.BaseClasses
{
    [TestClass]
    [DeploymentItem(@"Resources", "Resources")]
    public class BaseClass
    {

        private static FirefoxProfile GetFirefoxptions()
        {
            FirefoxProfile profile = new FirefoxProfile();
            FirefoxProfileManager manager = new FirefoxProfileManager();
            //profile = manager.GetProfile("default");
            return profile;
        }
        private static ChromeOptions GetChromeOptions()
        {
            ChromeOptions option = new ChromeOptions();
            option.AddArgument("start-maximized");
            //option.AddExtension(@"C:\Users\rahul.rathore\Desktop\Cucumber\extension_3_0_12.crx");
            return option;
        }

        private static InternetExplorerOptions GetIEOptions()
        {
            InternetExplorerOptions options = new InternetExplorerOptions();
            options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
            options.EnsureCleanSession = true;
            options.ElementScrollBehavior = InternetExplorerElementScrollBehavior.Bottom;
            return options;
        }

        
        private static FirefoxDriver GetFirefoxDriver()
        {
            FirefoxDriver driver = new FirefoxDriver(GetFirefoxptions());
            return driver;
        }

        private static ChromeDriver GetChromeDriver()
        {
            ChromeDriver driver = new ChromeDriver(GetChromeOptions());
            return driver;
        }

        private static InternetExplorerDriver GetIEDriver()
        {
            InternetExplorerDriver driver = new InternetExplorerDriver(GetIEOptions());
            return driver;
        }

        private static PhantomJSDriver GetPhantomJsDriver()
        {
            PhantomJSDriver driver = new PhantomJSDriver(GetPhantomJsptions());
           
            return driver;
        }

        private static PhantomJSOptions GetPhantomJsptions()
        {
            PhantomJSOptions option = new PhantomJSOptions();
            option.AddAdditionalCapability("handlesAlerts", true);
            return option;
        }

        private static PhantomJSDriverService GetPhantomJsDrvierService()
        {
            PhantomJSDriverService service = PhantomJSDriverService.CreateDefaultService();
            service.LogFile = "TestPhantomJS.log";
            service.HideCommandPromptWindow = false;
            service.LoadImages = true;
            return service;
        }
      

        [AssemblyInitialize]
        //[BeforeFeature()]
        public static void InitWebdriver(TestContext tc)
        {
            ObjectRepository.Config = new AppConfigReader();

            switch (ObjectRepository.Config.GetBrowser())
            {
                case BrowserType.Firefox:
                    ObjectRepository.Driver = GetFirefoxDriver();
                    break;

                case BrowserType.Chrome:
                    ObjectRepository.Driver = GetChromeDriver();
                    break;

                case BrowserType.IExplorer:
                    ObjectRepository.Driver = GetIEDriver();
                    break;

                case BrowserType.PhantomJs:
                    ObjectRepository.Driver = GetPhantomJsDriver();
                    break;

                default:
                    throw new NoSutiableDriverFound("Driver Not Found : " + ObjectRepository.Config.GetBrowser().ToString()); 
            }
            ObjectRepository.Driver.Manage()
                .Timeouts()
                .SetPageLoadTimeout(TimeSpan.FromSeconds(ObjectRepository.Config.GetPageLoadTimeOut()));
            ObjectRepository.Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(ObjectRepository.Config.GetElementLoadTimeOut()));
            BrowserHelper.BrowserMaximize();
        }


        [AssemblyCleanup]
        //[AfterScenario()]
        public static void TearDown()
        {
            if (ObjectRepository.Driver != null)
            {
               ObjectRepository.Driver.Close();
               ObjectRepository.Driver.Quit();
            }
        }
    }
}

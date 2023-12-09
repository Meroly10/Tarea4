using System;
using System.Collections.Generic;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        try
        {
            var op = new EdgeOptions();
            var Dv = new EdgeDriver(op);

            var screenshoot = Directory.GetFiles(Directory.GetCurrentDirectory(), ".png");

            Dv.Manage().Window.Maximize();

            // Buscar la página.
            Dv.Navigate().GoToUrl("https://boxpaq-online.iplus.com.do/lg-es/ut/Login.aspx?c=1");

            System.Threading.Thread.Sleep(4000);

            // Capturar captura de pantalla después de cargar la página
            CaptureScreenshot(Dv, "pagina_inicial");

            System.Threading.Thread.Sleep(2000);

            var gmail = Dv.FindElement(By.Id("cphCuerpo_UsuarioID"));
            gmail.SendKeys("BP-056092");

            System.Threading.Thread.Sleep(1000);

            // Capturar captura de pantalla después de ingresar el usuario
            CaptureScreenshot(Dv, "usuario_ingresado");

            System.Threading.Thread.Sleep(1000);

            var Contra = Dv.FindElement(By.Id("cphCuerpo_UsuarioPW"));
            Contra.SendKeys("06b844b623");

            System.Threading.Thread.Sleep(1000);

            // Capturar captura de pantalla después de ingresar la contraseña
            CaptureScreenshot(Dv, "contrasena_ingresada");

            System.Threading.Thread.Sleep(1000);

            var loginButton = Dv.FindElement(By.Id("cphCuerpo_bIniciarSesion"));
            ExecuteJavaScriptClick(Dv, loginButton);

            System.Threading.Thread.Sleep(1000);

            // Capturar captura de pantalla después de hacer clic en el botón de inicio de sesión
            CaptureScreenshot(Dv, "inicio_sesion");


            //Pre-Alerta
            System.Threading.Thread.Sleep(5000);
            Dv.Navigate().GoToUrl("https://boxpaq-online.iplus.com.do/lg-es/co/PreAlertasHistorico.aspx");
            System.Threading.Thread.Sleep(5000);
            CaptureScreenshot(Dv, "Pre-Alerta");


            //Pos-Alerta
            Dv.Navigate().GoToUrl("https://boxpaq-online.iplus.com.do/lg-es/tr/Declaracion.aspx");
            System.Threading.Thread.Sleep(5000);
            CaptureScreenshot(Dv, "Pos-Alerta");

            //Facturacion
            System.Threading.Thread.Sleep(5000);
            Dv.Navigate().GoToUrl("https://boxpaq-online.iplus.com.do/lg-es/co/Estado.aspx");
            System.Threading.Thread.Sleep(5000);
            CaptureScreenshot(Dv, "Facturacion");

        }

        catch (Exception x)
        {
            Console.WriteLine("Error en", x.Message);
        }
        finally
        {
            CreateHtmlReport();
        }


    }



    static void ExecuteJavaScriptClick(IWebDriver Dv, IWebElement element)
    {
        IJavaScriptExecutor js = (IJavaScriptExecutor)Dv;
        js.ExecuteScript("arguments[0].click();", element);
    }

    static void CaptureScreenshot(IWebDriver driver, string screenshotName)
    {
        // Tomar captura de pantalla y guardarla en la carpeta actual
        var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
        screenshot.SaveAsFile($"{screenshotName}.png", ScreenshotImageFormat.Png);
    }

    static void CreateHtmlReport()
    {
        // Obtener la lista de archivos de capturas de pantalla en la carpeta actual
        var screenshots = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.png");

        // Crear el contenido HTML del informe
        var htmlContent = "<html><body>";
        foreach (var screenshot in screenshots)
        {
            htmlContent += $"<img src=\"{screenshot}\" alt=\"screenshot\"><br>";
        }
        htmlContent += "</body></html>";

        // Guardar el contenido HTML en un archivo
        File.WriteAllText("report.html", htmlContent);

        // Obtener la ruta completa del informe HTML
        var reportPath = Path.GetFullPath("report.html");

        // Abrir automáticamente el informe HTML usando el programa predeterminado asociado
        System.Diagnostics.Process.Start(new ProcessStartInfo(reportPath) { UseShellExecute = true });
    }
}

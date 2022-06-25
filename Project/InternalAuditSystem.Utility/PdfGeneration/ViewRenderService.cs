using InternalAuditSystem.Utility.Constants;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.IO;
using System.Threading.Tasks;

namespace InternalAuditSystem.Utility.PdfGeneration
{
    public class ViewRenderService : IViewRenderService
    {
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        private IHostingEnvironment _hostingEnvironment;
        public ViewRenderService(IRazorViewEngine razorViewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider,
            IHostingEnvironment hostingEnvironment)
        {
            _razorViewEngine = razorViewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Method for converting file content to string 
        /// </summary>
        /// <param name="viewName">File name to be converted</param>
        /// <param name="model">Model to be passed in cshtml</param>
        /// <param name="fileName">File name of html</param>
        /// <returns>Converts file content to string and create html of cshtml</returns>
        public async Task<string> RenderToStringAsync(string viewName, object model, string fileName)
        {
            var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            using (var sw = new StringWriter())
            {
                var viewResult = _razorViewEngine.FindView(actionContext, viewName, false);

                if (viewResult.View == null)
                {
                    throw new ArgumentNullException($"{viewName}{StringConstant.NotMatchedViewText}");
                }

                var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = model
                };

                var viewContext = new ViewContext(
                    actionContext,
                    viewResult.View,
                    viewDictionary,
                    new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                    sw,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                var uploadFilesPath = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + "\\" + Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Name, StringConstant.PuppeteerText);
                if (!Directory.Exists(uploadFilesPath))
                    Directory.CreateDirectory(uploadFilesPath);
                string destPath = Path.Combine(uploadFilesPath, fileName + StringConstant.FileExtension);
                File.WriteAllText(destPath, sw.ToString());
                //Delete generate file from server
                string[] files = Directory.GetFiles(uploadFilesPath);
                foreach (string deleteFile in files)
                {
                    System.IO.File.Delete(deleteFile);
                }
                return sw.ToString();
            }
        }
    }
}

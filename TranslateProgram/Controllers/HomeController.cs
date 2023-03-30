using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using TranslateProgram.Models;
using Translator;

namespace TranslateProgram.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public string key = "42a90f4327c14a60a2f177085a095cc6";
        public string end_point = "https://api.cognitive.microsofttranslator.com/";
        public string region = "switzerlandnorth";


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /*[HttpPost]
        public async Task<IActionResult> Translated(string text)
        {
            if (ModelState.IsValid)
            {
                await translate.Translate(text);
                return RedirectToAction("Index");
            }

            return View();
        }*/
        [HttpPost]
        public async Task<IActionResult> Translated()
        {
            string inputText = Request.Form["sourcetext"];
            string lan = Request.Form["targetlang"];
            string source = Request.Form["source-lang"];
            string key = "42a90f4327c14a60a2f177085a095cc6";
            string end_point = "https://api.cognitive.microsofttranslator.com/";
            string region = "switzerlandnorth";
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
                {

                    httpRequestMessage.Method = HttpMethod.Post;
                    httpRequestMessage.RequestUri = new Uri(end_point + $"translate?api-version=3.0&to={lan}");
                    httpRequestMessage.Headers.Add("Ocp-Apim-Subscription-Key", key);
                    httpRequestMessage.Headers.Add("Ocp-Apim-Subscription-Region", region);

                    object[] body = new object[] { new { Text = inputText } };

                    string req = JsonConvert.SerializeObject(body);

                    httpRequestMessage.Content = new StringContent(req, Encoding.Unicode, "application/json");

                    HttpResponseMessage responseMessage = await httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false);

                    string responsText = await responseMessage.Content.ReadAsStringAsync();
                    Console.WriteLine(responsText);



                    Root[] param = JsonConvert.DeserializeObject<Root[]>(responsText);
                    foreach (Root item in param)
                    {

                        foreach (Translation translation in item.translations)
                        {
                            var texttranslate = translation.text;
                            ViewBag.Text = texttranslate;
                        }
                    }
                }
            }

            return View("Index");
        }
        public async Task<IActionResult> ImageAnalis()
        {
            string inputText = Request.Form["textimage"];
            string end_point = "https://computerzeer.cognitiveservices.azure.com/";
            string key = "e947edb7d8294c6b8f0ed8a3ccaf007b";
            ComputerVisionClient computerVisionClient = new ComputerVisionClient(new ApiKeyServiceClientCredentials(key)) { Endpoint = end_point };

            string path_img = inputText;

            List<VisualFeatureTypes?> visualFeatures = Enum.GetValues(typeof(VisualFeatureTypes)).OfType<VisualFeatureTypes?>().ToList();
            var result = "";
            ImageAnalysis image = await computerVisionClient.AnalyzeImageAsync(path_img, visualFeatures);
            var text = await computerVisionClient.RecognizePrintedTextAsync(true, path_img);
            foreach (var regions in text.Regions)
            {
                foreach (var lines in regions.Lines)
                {
                    foreach (var words in lines.Words)
                    {
                        result += words.Text + " ";
                    }
                }
                
            }

            string lan = Request.Form["targetlang1"];
            string source = Request.Form["source-lang"];
            string keyTranslate = "42a90f4327c14a60a2f177085a095cc6";
            string end_pointTranslate = "https://api.cognitive.microsofttranslator.com/";
            string region = "switzerlandnorth";
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
                {

                    httpRequestMessage.Method = HttpMethod.Post;
                    httpRequestMessage.RequestUri = new Uri(end_pointTranslate + $"translate?api-version=3.0&to={lan}");
                    httpRequestMessage.Headers.Add("Ocp-Apim-Subscription-Key", keyTranslate);
                    httpRequestMessage.Headers.Add("Ocp-Apim-Subscription-Region", region);

                    object[] body = new object[] { new { Text = result } };

                    string req = JsonConvert.SerializeObject(body);

                    httpRequestMessage.Content = new StringContent(req, Encoding.Unicode, "application/json");

                    HttpResponseMessage responseMessage = await httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false);

                    string responsText = await responseMessage.Content.ReadAsStringAsync();
                    Console.WriteLine(responsText);



                    Root[] param = JsonConvert.DeserializeObject<Root[]>(responsText);
                    foreach (Root item in param)
                    {

                        foreach (Translation translation in item.translations)
                        {
                            var texttranslate = translation.text;
                            ViewBag.TextAnalisis = texttranslate;
                        }
                    }
                }
            }



            return View("Index");
        }


            public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
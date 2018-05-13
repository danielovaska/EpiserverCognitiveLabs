using System.Web.Mvc;
using EpiserverSite4.Models.Pages;
using EpiserverSite4.Models.ViewModels;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using Microsoft.ProjectOxford.Face;
using System.Collections.Generic;
using System.IO;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using EPiServer;
using EpiserverSite4.Models.Media;
using EPiServer.Core;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using Mogul.Interceptor.Cache;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;

namespace EpiserverSite4.Controllers
{
    public class StartPageController : PageControllerBase<StartPage>
    {
        const string subscriptionKey = "539643c82def441199c9bfe0c6d066e8";
        const string uriBase = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0/detect";

        private readonly IContentRepository _contentRepository;
        private readonly INewsRepository _newsRepository;
        public StartPageController(IContentRepository contentRepository, INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
            _contentRepository = contentRepository;
        }
        public ActionResult Index(StartPage currentPage)
        {
            var model = PageViewModel.Create(currentPage);
            var result = _newsRepository.GetNews("Test");
            if (SiteDefinition.Current.StartPage.CompareToIgnoreWorkID(currentPage.ContentLink)) // Check if it is the StartPage or just a page of the StartPage type.
            {
                //Connect the view models logotype property to the start page's to make it editable
                var editHints = ViewData.GetEditHints<PageViewModel<StartPage>, StartPage>();
                editHints.AddConnection(m => m.Layout.Logotype, p => p.SiteLogotype);
                editHints.AddConnection(m => m.Layout.ProductPages, p => p.ProductPageLinks);
                editHints.AddConnection(m => m.Layout.CompanyInformationPages, p => p.CompanyInformationPageLinks);
                editHints.AddConnection(m => m.Layout.NewsPages, p => p.NewsPageLinks);
                editHints.AddConnection(m => m.Layout.CustomerZonePages, p => p.CustomerZonePageLinks);
            }

            // var imageFile = _contentRepository.Get<ImageFile>(new ContentReference(113));
            //if (imageFile == null)
            //    return;

            //var clonedImage = (ImageFile)imageFile.CreateWritableClone();

            //get image as bitmap

            //MakeAnalysisRequest();
            //InvokeContentTrainingService().Wait();
            //RunTextAnalysis();
            return View(model);
        }
        public void RunTextAnalysis()
        {
            ITextAnalyticsAPI client = new TextAnalyticsAPI();
            client.AzureRegion = AzureRegions.Westus;
            client.SubscriptionKey = "07dc127ac4d14c9a9a4760cdf0562ebe";

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Extracting language
            Console.WriteLine("===== LANGUAGE EXTRACTION ======");

            LanguageBatchResult result = client.DetectLanguage(
                    new BatchInput(
                        new List<Input>()
                        {
                          new Input("1", "This is a document written in English."),
                          new Input("2", "Este es un document escrito en Español."),
                          new Input("3", "这是一个用中文写的文件")
                        }));

            // Printing language results.
            foreach (var document in result.Documents)
            {
                Console.WriteLine("Document ID: {0} , Language: {1}", document.Id, document.DetectedLanguages[0].Name);
            }

            // Getting key-phrases
            Console.WriteLine("\n\n===== KEY-PHRASE EXTRACTION ======");

            KeyPhraseBatchResult result2 = client.KeyPhrases(
                    new MultiLanguageBatchInput(
                        new List<MultiLanguageInput>()
                        {
                          new MultiLanguageInput("ja", "1", "猫は幸せ"),
                          new MultiLanguageInput("de", "2", "Fahrt nach Stuttgart und dann zum Hotel zu Fu."),
                          new MultiLanguageInput("en", "3", "My cat is stiff as a rock."),
                          new MultiLanguageInput("es", "4", "A mi me encanta el fútbol!")
                        }));


            // Printing keyphrases
            foreach (var document in result2.Documents)
            {
                Console.WriteLine("Document ID: {0} ", document.Id);

                Console.WriteLine("\t Key phrases:");

                foreach (string keyphrase in document.KeyPhrases)
                {
                    Console.WriteLine("\t\t" + keyphrase);
                }
            }

            // Extracting sentiment
            Console.WriteLine("\n\n===== SENTIMENT ANALYSIS ======");

            SentimentBatchResult result3 = client.Sentiment(
                    new MultiLanguageBatchInput(
                        new List<MultiLanguageInput>()
                        {
                          new MultiLanguageInput("en", "0", "I had the best day of my life."),
                          new MultiLanguageInput("en", "1", "This was a waste of my time. The speaker put me to sleep."),
                          new MultiLanguageInput("es", "2", "No tengo dinero ni nada que dar..."),
                          new MultiLanguageInput("it", "3", "L'hotel veneziano era meraviglioso. È un bellissimo pezzo di architettura."),
                        }));


            // Printing sentiment results
            foreach (var document in result3.Documents)
            {
                Console.WriteLine("Document ID: {0} , Sentiment Score: {1:0.00}", document.Id, document.Score);
            }

        }
        /// <summary>
        /// Gets the analysis of the specified image file by using the Computer Vision REST API.
        /// </summary>
        /// <param name="imageFilePath">The image file.</param>
        public void MakeAnalysisRequest()
        {
            var client = new HttpClient();

            // Request headers.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            // Request parameters. A third optional parameter is "details".
            string requestParameters = "returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=smile";

            // Assemble the URI for the REST API Call.
            string uri = uriBase + "?" + requestParameters;

            HttpResponseMessage response;
            var imageFile = _contentRepository.Get<ImageFile>(new ContentReference(104));
            // Request body. Posts a locally stored JPEG image.
            var imageStream = imageFile.BinaryData.OpenRead();
            var binaryReader = new BinaryReader(imageStream);

            byte[] byteData = binaryReader.ReadBytes((int)imageStream.Length); ; //GetImageAsByteArray(imageFilePath);

            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json" and "multipart/form-data".
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                // Execute the REST API call.
                response = client.PostAsync(uri, content).Result;

                // Get the JSON response.
                string contentString = response.Content.ReadAsStringAsync().Result;
                // Display the JSON response.
            }
        }


        /// <summary>
        /// Returns the contents of the specified file as a byte array.
        /// </summary>
        /// <param name="imageFilePath">The image file to read.</param>
        /// <returns>The byte array of the image data.</returns>
        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }


        /// <summary>
        /// Formats the given JSON string by adding line breaks and indents.
        /// </summary>
        /// <param name="json">The raw JSON string to format.</param>
        /// <returns>The formatted JSON string.</returns>
        static string JsonPrettyPrint(string json)
        {
            if (string.IsNullOrEmpty(json))
                return string.Empty;

            json = json.Replace(Environment.NewLine, "").Replace("\t", "");

            StringBuilder sb = new StringBuilder();
            bool quote = false;
            bool ignore = false;
            int offset = 0;
            int indentLength = 3;

            foreach (char ch in json)
            {
                switch (ch)
                {
                    case '"':
                        if (!ignore) quote = !quote;
                        break;
                    case '\'':
                        if (quote) ignore = !ignore;
                        break;
                }

                if (quote)
                    sb.Append(ch);
                else
                {
                    switch (ch)
                    {
                        case '{':
                        case '[':
                            sb.Append(ch);
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', ++offset * indentLength));
                            break;
                        case '}':
                        case ']':
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', --offset * indentLength));
                            sb.Append(ch);
                            break;
                        case ',':
                            sb.Append(ch);
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', offset * indentLength));
                            break;
                        case ':':
                            sb.Append(ch);
                            sb.Append(' ');
                            break;
                        default:
                            if (ch != ' ') sb.Append(ch);
                            break;
                    }
                }
            }

            return sb.ToString().Trim();
        }
        static async Task InvokeRequestResponseService()
        {
            using (var client = new HttpClient())
            {
                var scoreRequest = new
                {
                    Inputs = new Dictionary<string, List<Dictionary<string, string>>>() {
                        {
                            "input1",
                            new List<Dictionary<string, string>>(){new Dictionary<string, string>(){
                                            {
                                                "symboling", "3"
                                            },
                                            {
                                                "normalized-losses", "1"
                                            },
                                            {
                                                "make", "alfa-romero"
                                            },
                                            {
                                                "fuel-type", "gas"
                                            },
                                            {
                                                "aspiration", "std"
                                            },
                                            {
                                                "num-of-doors", "two"
                                            },
                                            {
                                                "body-style", "convertible"
                                            },
                                            {
                                                "drive-wheels", "rwd"
                                            },
                                            {
                                                "engine-location", "front"
                                            },
                                            {
                                                "wheel-base", "88.6"
                                            },
                                            {
                                                "length", "168.8"
                                            },
                                            {
                                                "width", "64.1"
                                            },
                                            {
                                                "height", "48.8"
                                            },
                                            {
                                                "curb-weight", "2548"
                                            },
                                            {
                                                "engine-type", "dohc"
                                            },
                                            {
                                                "num-of-cylinders", "four"
                                            },
                                            {
                                                "engine-size", "130"
                                            },
                                            {
                                                "fuel-system", "mpfi"
                                            },
                                            {
                                                "bore", "3.47"
                                            },
                                            {
                                                "stroke", "2.68"
                                            },
                                            {
                                                "compression-ratio", "9"
                                            },
                                            {
                                                "horsepower", "111"
                                            },
                                            {
                                                "peak-rpm", "5000"
                                            },
                                            {
                                                "city-mpg", "21"
                                            },
                                            {
                                                "highway-mpg", "27"
                                            },
                                            {
                                                "price", "13495"
                                            },
                                }
                            }
                        },
                    },
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };

                const string apiKey = "2qRDDQmhG/nWAoECpSOs3Ol+yXfZzQ5Lc2DrFo0fVnMNrNOiQczHMFwNc9C5piygSRw6HbaTX3iHDDQcgmxWXg=="; // Replace this with the API key for the web service
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/24006bc353544700b7387d3c9af2d007/services/0fc80ef595524c089dd2f93aade4129b/execute?api-version=2.0&format=swagger");

                // WARNING: The 'await' statement below can result in a deadlock
                // if you are calling this code from the UI thread of an ASP.Net application.
                // One way to address this would be to call ConfigureAwait(false)
                // so that the execution does not attempt to resume on the original context.
                // For instance, replace code such as:
                //      result = await DoSomeTask()
                // with the following:
                //      result = await DoSomeTask().ConfigureAwait(false)

                HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Result: {0}", result);
                }
                else
                {
                    Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));

                    // Print the headers - they include the requert ID and the timestamp,
                    // which are useful for debugging the failure
                    Console.WriteLine(response.Headers.ToString());

                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);
                }


            }
        }
            static async Task InvokeContentTrainingService()
            {
                using (var client = new HttpClient())
                {
                    var scoreRequest = new
                    {
                        Inputs = new Dictionary<string, List<Dictionary<string, string>>>() {
                        {
                            "input1",
                            new List<Dictionary<string, string>>(){new Dictionary<string, string>(){
                                            {
                                                "City", "Uppsala"
                                            },
                                            {
                                                "Age", "17"
                                            },
                                            {
                                                "Gender", "M"
                                            },
                                            {
                                                "BikesVG", "BikesUnknown"
                                            },
                                            {
                                                "CarsVG", "CarsUnknown"
                                            },
                                            {
                                                "Choice", "1"
                                            },
                                }
                            }
                        },
                    },
                        GlobalParameters = new Dictionary<string, string>()
                        {
                        }
                    };

                    const string apiKey = "CyAkdzKdFik/lbEpnVcldgyyDB7xVYYx01EYVYILEopX8LprVcDEHyBNFdCO581ad52S7ldRcC4CYLompeZgcA=="; // Replace this with the API key for the web service
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                    client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/24006bc353544700b7387d3c9af2d007/services/e76679b1dda54d868219e14030075dce/execute?api-version=2.0&format=swagger");

                    // WARNING: The 'await' statement below can result in a deadlock
                    // if you are calling this code from the UI thread of an ASP.Net application.
                    // One way to address this would be to call ConfigureAwait(false)
                    // so that the execution does not attempt to resume on the original context.
                    // For instance, replace code such as:
                    //      result = await DoSomeTask()
                    // with the following:
                    //      result = await DoSomeTask().ConfigureAwait(false)

                    HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest).ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Result: {0}", result);
                    }
                    else
                    {
                        Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));

                        // Print the headers - they include the requert ID and the timestamp,
                        // which are useful for debugging the failure
                        Console.WriteLine(response.Headers.ToString());

                        string responseContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(responseContent);
                    }
                }
            }
        }
    public interface INewsRepository
    {
        [Cache(10,"News")]
        IEnumerable<string> GetNews(string query);

    }
    public class NewsRepository : INewsRepository
    {
        public IEnumerable<string> GetNews(string query)
        {
            Thread.Sleep(1000);
            return new [] { "News1", "News2"};
        }
    }
   
    

}


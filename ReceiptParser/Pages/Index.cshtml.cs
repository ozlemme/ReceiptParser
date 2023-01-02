using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ReceiptParser.Entities;

namespace ReceiptParser.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        public List<string> PrintList { get; set; }
        public IndexModel(ILogger<IndexModel> logger, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        public void OnGet()
        {
            var jsonData = GetJsonData();

            PrintList = GetTextToPrint(DeserializeJson(jsonData));
        }

        private string GetJsonData()
        {
            var path = Path.Combine(_hostingEnvironment.WebRootPath, "response.json");
            return System.IO.File.ReadAllText(path);
        }

        private List<ReceiptDetail> DeserializeJson(string jsonData)
        {
            return JsonConvert.DeserializeObject<List<ReceiptDetail>>(jsonData);
        }

        private int GetSmallestX(List<Vertice> verticeList)
        {
            return verticeList.Min(x => x.X);
        }

        private int GetLargestX(List<Vertice> verticeList)
        {
            return verticeList.Max(x => x.X);
        }

        private List<string> GetTextToPrint(List<ReceiptDetail> receiptDetails)
        {
            var previousLargestX = 0;
            var indexOfList = 0;
            var printList = new List<string>();

            foreach (var item in receiptDetails)
            {
                if (!string.IsNullOrEmpty(item.Locale))
                    continue;

                var smallestX = GetSmallestX(item.BoundingPoly.Vertices);

                if (smallestX > previousLargestX)
                {
                    if (!printList.Any())
                        printList.Insert(indexOfList, item.Description);
                    else
                        printList[indexOfList] = printList[indexOfList] + " " + item.Description;
                }
                else
                {
                    indexOfList++;
                    printList.Insert(indexOfList, item.Description);
                }

                previousLargestX = GetLargestX(item.BoundingPoly.Vertices); ;
            }

            return printList;
        }

    }
}
using Newtonsoft.Json;
using System.Text;

var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");

var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir);

var salesFiles = FindFiles(storesDirectory);

var salesTotal = CalculateSalesTotal(salesFiles);

GenerateSalesSummary(salesFiles, salesTotal, salesTotalDir);

IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        var extension = Path.GetExtension(file);
        if (extension == ".json")
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}

double CalculateSalesTotal(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;

    // Loop over each file path in salesFiles
    foreach (var file in salesFiles)
    {
        // Read the contents of the file
        string salesJson = File.ReadAllText(file);

        // Parse the contents as JSON
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);

        // Add the amount found in the Total field to the salesTotal variable
        salesTotal += data?.Total ?? 0;
    }

    return salesTotal;
}

void GenerateSalesSummary(IEnumerable<string> salesFiles, double salesTotal, string salesTotalDir)
{
StringBuilder report = new StringBuilder();

    report.AppendLine($"Sales Summary Report");
    report.AppendLine($"====================");
    report.AppendLine($"Total Sales: {salesTotal:C}");
    report.AppendLine();
    report.AppendLine($"Sales Files Processed:");
    foreach (var file in salesFiles)
    {
        report.AppendLine($"- {file}");
    }

    string summaryFilePath = Path.Combine(salesTotalDir, "SalesSummary.txt");
    File.WriteAllText(summaryFilePath, report.ToString());
}

record SalesData(double Total);
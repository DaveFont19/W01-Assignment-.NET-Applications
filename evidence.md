## Part 1 — Evidence from *Web API with ASP.NET*

The completed project is in `ContosoPizza/` (`Models/Pizza.cs`, `Services/PizzaService.cs`,
`Controllers/PizzaController.cs`). The API was run locally with:

```
dotnet run    PORT: http://localhost:5260
```

### 1. Existing content — `GET /pizza`

```
http://localhost:5260/pizza/get

[{"id":1,"name":"Classic Italian","isGlutenFree":false},{"id":2,"name":"Veggie","isGlutenFree":true}]
```
### 2. Additional record — `POST /pizza`

```
POST Request
{
    "id":  3,
    "name":  "Hawaiian",
    "isGlutenFree":  false
}
```

The controller returned `201 Created` via `CreatedAtAction`, and `PizzaService.Add` assigned
`Id = 3` from the `nextId` counter.

### 3. Existing content **+** the additional record — `GET /pizza`

```
Get Request

[{"id":1,"name":"Classic Italian","isGlutenFree":false},{"id":2,"name":"Veggie","isGlutenFree":true},{"id":3,"name":"Hawaiian","isGlutenFree":false}]
```

The list now contains the two original records plus the "Hawaiian" record that was added and later changed.
The full set of requests (GET all, GET by id, POST, PUT, DELETE) is also saved in
`ContosoPizza/ContosoPizza.http`.

---

## Part 2 — Working sales summary

From `mslearn-dotnet-files/Program.cs`. `GenerateSalesSummary` is the function that produces the
report; `CalculateSalesTotal` is included because it supplies the total it prints.

```
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
```

It is called from the top-level statements as:

```
var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir);

var salesFiles  = FindFiles(storesDirectory);
var salesTotal  = CalculateSalesTotal(salesFiles);

GenerateSalesSummary(salesFiles, salesTotal, salesTotalDir);
```

### Output produced — `mslearn-dotnet-files/salesTotalDir/SalesSummary.txt`

```
Sales Summary Report
====================
Total Sales: $2,012.20

Sales Files Processed:
- ...\mslearn-dotnet-files\stores\sales.json
- ...\mslearn-dotnet-files\stores\201\sales.json
- ...\mslearn-dotnet-files\stores\201\salestotals.json
- ...\mslearn-dotnet-files\stores\202\sales.json
- ...\mslearn-dotnet-files\stores\202\salestotals.json
- ...\mslearn-dotnet-files\stores\203\sales.json
- ...\mslearn-dotnet-files\stores\203\salestotals.json
- ...\mslearn-dotnet-files\stores\204\sales.json
- ...\mslearn-dotnet-files\stores\204\salestotals.json
```

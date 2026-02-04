using CsvHelper;
using CsvHelper.Configuration;
using SharePointLargeListApp.Models;
using System.Globalization;

namespace SharePointLargeListApp.Utilities
{
    public class CsvExporter
    {
        public static void ExportFailedItems(List<FailedItem> failedItems, string filePath)
        {
            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                };

                using var writer = new StreamWriter(filePath);
                using var csv = new CsvWriter(writer, config);

                csv.WriteRecords(failedItems);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to export CSV: {ex.Message}", ex);
            }
        }

        public static string GenerateFailedItemsReport(ProcessResult result, string outputDirectory = "Reports")
        {
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            string fileName = $"FailedItems_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            string filePath = Path.Combine(outputDirectory, fileName);

            ExportFailedItems(result.Errors, filePath);

            return filePath;
        }
    }
}

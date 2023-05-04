using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace LaunchAVDPDF
{
    class Program
    {
        static void Main(string[] args)
        {

                string destinationDirectory = ConfigurationManager.AppSettings["destinationDirectory"];

                if (!Directory.Exists(destinationDirectory))
                {
                    Console.WriteLine("The specified directory does not exist.");
                    return;
                }

                // Get all PDF files in the directory and order them by the last write time (descending)
                var pdfFiles = Directory.GetFiles(destinationDirectory, "*.pdf")
                    .Select(f => new FileInfo(f))
                    .OrderByDescending(fi => fi.LastWriteTime)
                    .ToList();

                if (pdfFiles.Count == 0)
                {
                    Console.WriteLine("No PDF files were found in the specified directory.");
                    return;
                }

                // Get the latest PDF file
                FileInfo latestPdf = pdfFiles.First();

                try
                {
                    // Open the PDF file with the default PDF viewer
                    Process.Start(new ProcessStartInfo(latestPdf.FullName) { UseShellExecute = true });
                    Console.WriteLine($"Opened the latest PDF file: {latestPdf.FullName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error opening the PDF file: {ex.Message}");
                }

        }
    }
}

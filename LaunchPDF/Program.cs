using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace LaunchPDF
{
    class Program
    {
        static void Main(string[] args)
        {

                if (args.Length == 0)
                {
                    Console.WriteLine("No file path provided.");
                    return;
                }

                string sourceFilePath = args[0];
                string destinationDirectory = ConfigurationManager.AppSettings["destinationDirectory"];

                if (!File.Exists(sourceFilePath))
                {
                    Console.WriteLine($"File not found: {sourceFilePath}");
                    return;
                }

                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                }
                else
                {
                    // Delete all existing files in the destination directory
                    foreach (string file in Directory.GetFiles(destinationDirectory))
                    {
                        File.Delete(file);
                    }
                }

                string fileName = Path.GetFileName(sourceFilePath);
                string destinationFilePath = Path.Combine(destinationDirectory, fileName);

                File.Copy(sourceFilePath, destinationFilePath, true);

                Console.WriteLine($"File copied to: {destinationFilePath}");

                // Check if the PDF file exists
                if (File.Exists(destinationFilePath))
                {
                    // Launch the PDF file using the default PDF viewer
                    //Process.Start(new ProcessStartInfo(destinationFilePath) { UseShellExecute = true });
                    Console.WriteLine("Launching URI");

                    string workspaceId = ConfigurationManager.AppSettings["workspaceId"];
                    string resourceId = ConfigurationManager.AppSettings["resourceId"];
                    string domainName = ConfigurationManager.AppSettings["domainName"];

                    string domainUser = Environment.UserName;

                    string[] splitDomainUser = domainUser.Split('\\');

                Console.WriteLine(domainUser);


                // Get the username part
                string username;
                    if (splitDomainUser.Length > 1)
                    {
                        username = splitDomainUser[1];
                    }
                    else
                    {
                        username = splitDomainUser[0];
                    }

                // Combine the username with the new domain


                string newUser = username + domainName;

                string uri = $"ms-avd:connect?workspaceId={workspaceId}&resourceid={resourceId}&username={newUser}&version=0";

                Console.WriteLine(uri);

                Process.Start(new ProcessStartInfo
                        {
                            FileName = uri,
                            UseShellExecute = true // Use the operating system shell to start the process
                        });


                }
                else
                {
                    Console.WriteLine($"PDF file not found at: {destinationFilePath}");
                }


        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
namespace MFA_assignment.Models
{
    public class CodeFileManager
    {
        private readonly string filePath;

        public CodeFileManager(string filePath)
        {
            this.filePath = filePath;
        }

        public void SaveCode(string phone, string code)
        {
            // Prepare the data to be saved in the file
            var data = $"{phone},{code}";

            // Append the data to the file (you can handle file locking for concurrent writes)
            File.AppendAllLines(filePath, new List<string> { data });
        }

        public Dictionary<string, string> ReadCodes()
        {
            var codes = new Dictionary<string, string>();

            try
            {
                // Read all lines from the file
                var lines = File.ReadAllLines(filePath);

                // Parse each line to extract phone and code
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 2)
                    {
                        var phone = parts[0].Trim();
                        var code = parts[1].Trim();
                        codes[phone] = code;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exception that might occur while reading the file
                Console.WriteLine("Error reading codes file: " + ex.Message);
            }

            return codes;
        }
    }

}
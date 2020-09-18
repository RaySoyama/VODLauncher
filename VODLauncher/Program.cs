using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace VODLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = "VODLauncher.json";

            string filePath = Directory.GetCurrentDirectory() + $"\\{fileName}";

            List<string> gameDirectories = new List<string>();


            if (!File.Exists(filePath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    gameDirectories.Add("GamePath1");
                    gameDirectories.Add("GamePath2");
                    sw.WriteLine(JsonConvert.SerializeObject(gameDirectories, Formatting.Indented));
                }

                Console.WriteLine("No valid game paths in configs");
                Console.ReadKey();
                return;
            }

            try
            {
                gameDirectories = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(filePath));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to Deserialize File\n{e.ToString()}");
                Console.ReadKey();
                return;
            }

            foreach (string gamePath in gameDirectories)
            {
                if (gamePath.Contains("GamePath") || gamePath == "" || gamePath == null || !File.Exists(gamePath))
                {
                    Console.WriteLine("Invalid game paths in configs");
                    Console.ReadKey();
                }
                else
                {
                    try
                    {
                        Process.Start(gamePath);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Invalid game paths in configs. Shortcuts don't work for whatever fucking reason");
                        Console.ReadKey();
                    }
                }
            }
        }
    }
}

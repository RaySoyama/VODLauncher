using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace VODLauncher
{
    class Program
    {
        struct PathArg
        {
            public string path;
            public string arg;
            public int timeToSleep;
        }

        static void Main(string[] args)
        {
            string fileName = "VODLauncher.json";

            string filePath = Directory.GetCurrentDirectory() + $"\\{fileName}";



            List<PathArg> gameDirectories = new List<PathArg>();


            if (!File.Exists(filePath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    gameDirectories.Add(new PathArg() { path = "GamePath1", arg = "" });
                    gameDirectories.Add(new PathArg() { path = "GamePath2", arg = "" });
                    sw.WriteLine(JsonConvert.SerializeObject(gameDirectories, Formatting.Indented));
                }

                Console.WriteLine("No valid game paths in configs");
                Console.ReadKey();
                return;
            }

            try
            {
                gameDirectories = JsonConvert.DeserializeObject<List<PathArg>>(File.ReadAllText(filePath));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to Deserialize File\n{e.ToString()}");
                Console.ReadKey();
                return;
            }

            foreach (PathArg gamePath in gameDirectories)
            {
                Console.WriteLine($"Parsing...{gamePath.path}\nArg...{gamePath.arg}");

                if (gamePath.path.Contains("GamePath") || gamePath.path == "" || gamePath.path == null || !File.Exists(gamePath.path))
                {
                    Console.WriteLine($"Invalid game paths in configs\n{gamePath.path}");
                    Console.ReadKey();
                }
                else
                {
                    try
                    {
                        Process.Start(gamePath.path, gamePath.arg);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"Invalid game paths in configs. Shortcuts don't work for whatever fucking reason\n{gamePath.path}");
                        Console.ReadKey();
                    }
                }

                System.Threading.Thread.Sleep(gamePath.timeToSleep);
            }
        }
    }
}

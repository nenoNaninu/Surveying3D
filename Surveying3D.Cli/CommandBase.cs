using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MicroBatchFramework;
using Surveying3D.Core;
using Utf8Json;

namespace Surveying3D.Cli
{
    public class CommandBase : BatchBase
    {
        [Command("test", "check one object")]
        public void Test([Option(0)] string path)
        {
            var results = Surveyor.Survey(path);
            var bytes = JsonSerializer.Serialize(results);

            Console.WriteLine("====result====");
            Console.WriteLine(JsonSerializer.PrettyPrint(bytes));
        }

        [Command("list")]
        public void List([Option(0)] string listPath,
            [Option("o", "output dir, The default output is not a file, but the console")]
            string outputDir = null)
        {
            var pathList = new List<string>(256);
            using var streamReader = new StreamReader(listPath);
            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                pathList.Add(line);
            }

            var surveyResults = pathList.AsParallel().Select(Surveyor.Survey).ToList();

            var jsonBytes = JsonSerializer.Serialize(surveyResults);
            var prettyJson = JsonSerializer.PrettyPrint(jsonBytes);
            Console.WriteLine(prettyJson);

            if (outputDir == null) return;
            
            Write2File(jsonBytes,outputDir);
        }

        [Command("search")]
        public void Search(
            [Option("d", "search under root dir")] string rootDirectory,
            [Option("e", "extension to search")] string extension = "obj",
            [Option("o", "output dir, The default output is not a file, but the console")]
            string outputDir = null)
        {
            string[] zipFilePaths = Directory.GetFiles(rootDirectory, $"*.{extension}", SearchOption.AllDirectories);

            Dictionary<string, SurveyResults> surveyResults = zipFilePaths.AsParallel()
                .Select(x => (Result: Surveyor.Survey(x), modelPath: x))
                .ToDictionary(x => x.modelPath, x => x.Result);

            var jsonBytes = JsonSerializer.Serialize(surveyResults);
            var prettyJson = JsonSerializer.PrettyPrint(jsonBytes);
            Console.WriteLine(prettyJson);

            if (outputDir == null) return;

            Write2File(jsonBytes, outputDir);
        }

        private static void Write2File(byte[] jsonBytes, string outputDir)
        {
            if (outputDir != null)
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                using var streamWriter =
                    new BinaryWriter(new FileStream(Path.Join(outputDir, $"result{DateTime.Now:yyyyMMddHHmmss}.json"), FileMode.Create));
                streamWriter.Write(jsonBytes);
            }
        }
    }
}
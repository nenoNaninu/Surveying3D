using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
            
            string[] absolutePath = pathList.Select(Path.GetFullPath).ToArray();

            Dictionary<string, SurveyResults> surveyResults = absolutePath.AsParallel()
                .Select(x => (Result: Surveyor.Survey(x), ModelPath: x))
                .ToDictionary(x => x.ModelPath, x => x.Result);

            var jsonBytes = JsonSerializer.Serialize(surveyResults);
            var prettyJson = JsonSerializer.PrettyPrint(jsonBytes);
            Console.WriteLine(prettyJson);

            if (outputDir == null) return;

            Write2File(jsonBytes, outputDir);
        }

        [Command("search")]
        public async Task Search(
            [Option("d", "search under root dir")] string rootDirectory,
            [Option("e", "extension to search")] string extension = "obj",
            [Option("o", "output dir, The default output is not a file, but the console")]
            string outputDir = null)
        {
            string[] relativePath = Directory.GetFiles(rootDirectory, $"*.{extension}", SearchOption.AllDirectories);
            string[] absolutePath = relativePath.Select(Path.GetFullPath).ToArray();

            var surveyTasks = absolutePath
                .Select(x => Task.Run(() => (Result: Surveyor.Survey(x), ModelPath: x)));

            var surveyResults = await Task.WhenAll(surveyTasks);
            var surveyDictionary = surveyResults.ToDictionary(x => x.ModelPath, x => x.Result);

            var jsonBytes = JsonSerializer.Serialize(surveyDictionary);
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
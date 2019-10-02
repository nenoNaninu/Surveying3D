using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MicroBatchFramework;
using Utf8Json;

namespace Surveying3D.Cli
{
    public class CommandBase : BatchBase
    {
        [Command("test", "check one object")]
        public async Task Test([Option(0, "3d object path")] string path,
            [Option("o", "output dir, The default output is not a file, but the console")]
            string outputDir = null,
            [Option("n", "output file name")] string outputFileName = "result.json")
        {
            var results = Surveyor.Survey(path);
            var bytes = JsonSerializer.Serialize(results);

            Console.WriteLine("====result====");
            Console.WriteLine(JsonSerializer.PrettyPrint(bytes));

            if (outputDir == null) return;

            await Write2File(bytes, outputDir, outputFileName, this.Context.CancellationToken);
        }

        [Command("list", @"The path of the file describing the list of obj (for example, created with [find `pwd` -name *.obj])")]
        public async Task List([Option(0, "Text path that describes the model file path.")]
            string listPath,
            [Option("o", "output dir, The default output is not a file, but the console")]
            string outputDir = null,
            [Option("n", "")] string outputFileName = "result.json")
        {
            var pathList = new List<string>(256);
            using var streamReader = new StreamReader(listPath);
            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                pathList.Add(line);
            }

            string[] absolutePath = pathList.Select(Path.GetFullPath).ToArray();

            Dictionary<string, SurveyResults> surveyDictionary = absolutePath.AsParallel()
                .Select(x => (Result: Surveyor.Survey(x), ModelPath: x))
                .ToDictionary(x => x.ModelPath, x => x.Result);

            var jsonBytes = JsonSerializer.Serialize(surveyDictionary);
            var prettyJson = JsonSerializer.PrettyPrint(jsonBytes);
            Console.WriteLine(prettyJson);

            if (outputDir == null) return;

            await Write2File(jsonBytes, outputDir, outputFileName, this.Context.CancellationToken);
        }

        [Command("search", "Search and measure models with the specified extension under the specified directory.")]
        public async Task Search(
            [Option("d", "search under root dir")] string rootDirectory,
            [Option("o", "output dir, The default output is not a file, but the console")]
            string outputDir = null,
            [Option("e", "extension to search")] string extension = "obj",
            [Option("n", "")] string outputFileName = "result.json")
        {
            string[] relativePath = Directory.GetFiles(rootDirectory, $"*.{extension}", SearchOption.AllDirectories);
            string[] absolutePath = relativePath.Select(Path.GetFullPath).ToArray();

            var surveyDictionary = absolutePath.AsParallel()
                .Select(x => (Result: Surveyor.Survey(x), ModelPath: x))
                .ToDictionary(x => x.ModelPath, x => x.Result);

            var jsonBytes = JsonSerializer.Serialize(surveyDictionary);
            var prettyJson = JsonSerializer.PrettyPrint(jsonBytes);
            Console.WriteLine(prettyJson);

            if (outputDir == null) return;

            await Write2File(jsonBytes, outputDir, outputFileName, this.Context.CancellationToken);
        }

        private async Task Write2File(byte[] jsonBytes, string outputDir, string fileName, CancellationToken cancellationToken)
        {
            if (outputDir != null)
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                var outputPath = Path.Join(outputDir, fileName);
                if (File.Exists(outputPath))
                {
                    Console.WriteLine($"{outputPath} is already exists!! ");
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                    var extension = Path.GetExtension(fileName);
                    outputPath = Path.Join(outputDir, $"{fileNameWithoutExtension}{DateTime.Now:yyyyMMddHHmmss}{extension}");
                    Console.WriteLine($"Instead, set the output destination to {outputPath}");
                }
                else
                {
                    Console.WriteLine($"output to {outputPath}");
                }

                await using var fileStream = new FileStream(outputPath, FileMode.Create);

                await fileStream.WriteAsync(jsonBytes, cancellationToken);
            }
        }
    }
}
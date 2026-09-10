using PKHeX.Core;

const string defaultInputDirectory = "input";
const string defaultOutputDirectory = "output";

var inputDirectory = args.Length > 0 ? args[0] : defaultInputDirectory;
var outputDirectory = args.Length > 1 ? args[1] : defaultOutputDirectory;

if (args.Length > 2)
{
    Console.Error.WriteLine("Usage: dotnet run -- [input-directory] [output-directory]");
    return 2;
}

if (!Directory.Exists(inputDirectory))
{
    Console.Error.WriteLine($"Input directory does not exist: {inputDirectory}");
    return 1;
}

Directory.CreateDirectory(outputDirectory);

var inputFiles = Directory
    .EnumerateFiles(inputDirectory)
    .Where(path => string.Equals(Path.GetExtension(path), ".pk7", StringComparison.OrdinalIgnoreCase))
    .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
    .ToArray();

if (inputFiles.Length == 0)
{
    Console.WriteLine($"No .pk7 files found in {inputDirectory}.");
    return 0;
}

var failures = 0;
foreach (var inputPath in inputFiles)
{
    var fileName = Path.GetFileName(inputPath);
    var outputPath = Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(inputPath) + ".pk8");

    try
    {
        var source = new PK7(File.ReadAllBytes(inputPath));
        var target = new PK8();
        var isCompatible = EntityConverter.TryMakePKMCompatible(source, target, out var result, out var converted);

        if (!isCompatible || converted is not PK8 pk8 || !result.IsSuccess)
        {
            failures++;
            Console.Error.WriteLine($"FAILED {fileName}: {result}");
            continue;
        }

        File.WriteAllBytes(outputPath, pk8.Data.ToArray());
        Console.WriteLine($"OK      {fileName} -> {Path.GetFileName(outputPath)}");
    }
    catch (Exception exception)
    {
        failures++;
        Console.Error.WriteLine($"FAILED {fileName}: {exception.Message}");
    }
}

Console.WriteLine($"Processed {inputFiles.Length} file(s): {inputFiles.Length - failures} succeeded, {failures} failed.");
return failures == 0 ? 0 : 1;
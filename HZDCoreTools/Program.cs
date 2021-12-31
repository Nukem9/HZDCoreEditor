namespace HZDCoreTools;

using System;
using CommandLine;
using Decima;

public class Program
{
    public static void Main(string[] args)
    {
        RTTI.SetGameMode(GameType.HZD);

        var types = new Type[]
        {
                typeof(Archive.PackArchiveCommand),
                typeof(Archive.ExtractArchiveCommand),
                typeof(Localize.ExportLocalizationCommand),
                typeof(Localize.ImportLocalizationCommand),
                typeof(ArchiveIndex.ExportIndexFilesCommand),
                typeof(ArchiveIndex.RebuildIndexFilesCommand),
                typeof(Misc.ExportAllStringsCommand),
                typeof(Misc.ExportEntryPointNamesCommand),
                typeof(Misc.RebuildPrefetchFileCommand),
        };

        var parser = new Parser(with => with.HelpWriter = Console.Error);

        parser.ParseArguments(args, types)
            .WithParsed(Run)
            .WithNotParsed(errs => Console.WriteLine("Unable to parse command line: {0}", string.Join(" ", args)));
    }

    private static void Run(object options)
    {
        switch (options)
        {
            case Archive.PackArchiveCommand c:
                Archive.PackArchive(c);
                break;

            case Archive.ExtractArchiveCommand c:
                Archive.ExtractArchive(c);
                break;

            case Localize.ExportLocalizationCommand c:
                Localize.ExportLocalization(c);
                break;

            case Localize.ImportLocalizationCommand c:
                Localize.ImportLocalization(c);
                break;

            case ArchiveIndex.ExportIndexFilesCommand c:
                ArchiveIndex.ExportIndexFiles(c);
                break;

            case ArchiveIndex.RebuildIndexFilesCommand c:
                ArchiveIndex.RebuildIndexFiles(c);
                break;

            case Misc.ExportAllStringsCommand c:
                Misc.ExportAllStrings(c);
                break;

            case Misc.ExportEntryPointNamesCommand c:
                Misc.ExportEntryPointNames(c);
                break;

            case Misc.RebuildPrefetchFileCommand c:
                Misc.RebuildPrefetchFile(c);
                break;
        }
    }
}

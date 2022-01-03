namespace HZDCoreTools;

using System;
using System.Linq;
using CommandLine;
using Decima;

public class Program
{
    public static void Main(string[] args)
    {
        // Determine the game type from the first arg if it was supplied. Default to HZD otherwise.
        GameType targetGameType = GameType.Invalid;

        if (args.Length > 0)
        {
            switch (args[0].ToLower())
            {
                case "--horizonzerodawn":
                    targetGameType = GameType.HZD;
                    break;

                case "--deathstranding":
                    targetGameType = GameType.DS;
                    break;
            }

            // No longer needed. Remove this index.
            if (targetGameType != GameType.Invalid)
                args = args.Skip(1).ToArray();
        }

        if (targetGameType == GameType.Invalid)
            targetGameType = GameType.HZD;

        RTTI.SetGameMode(targetGameType);

        // Handle the rest of the arguments
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

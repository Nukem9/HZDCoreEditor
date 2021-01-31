using System;
using CommandLine;
using Decima;

namespace HZDCoreTools
{
    public class CmdOptions
    {
        [Option('e', "extract", HelpText = "Extract folder or file")]
        public string Extract { get; set; }

        [Option('g', "game", HelpText = "Director is the game directory (ignores unknown packs)")]
        public bool GameDir { get; set; }

        [Value(0, Required = true, HelpText = "Output folder")]
        public string OutputFolder { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var cmds = new CmdOptions();
            var parser = new Parser(with => with.HelpWriter = Console.Error);
            parser.ParseArguments<CmdOptions>(args)
                .WithParsed(o => cmds = o)
                .WithNotParsed(errs => Console.WriteLine("Unable to parse command line: {0}", String.Join(" ", args)));

            RTTI.SetGameMode(GameType.HZD);

            var ex = new Extractor()
            {
                OutputDir = cmds.OutputFolder
            };
            ex.Extract(cmds.Extract, cmds.GameDir);

            while (true)
                Console.ReadLine();
        }
    }
}

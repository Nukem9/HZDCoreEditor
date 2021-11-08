namespace HZDCoreEditorUI
{
    using System;
    using System.Windows.Forms;
    using CommandLine;
    using Decima;

    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            RTTI.SetGameMode(GameType.HZD);

            var cmds = new CmdOptions();
            var parser = new Parser(with => with.HelpWriter = Console.Error);

            parser.ParseArguments<CmdOptions>(args)
                .WithParsed(o => cmds = o)
                .WithNotParsed(errs => MessageBox.Show("Unable to parse command line: {0}", string.Join(" ", args)));

            Application.Run(new UI.FormCoreView(cmds));
        }

        public class CmdOptions
        {
            [Option('s', "search", HelpText = "Search for text")]
            public string Search { get; set; }

            [Option('o', "object", HelpText = "Highlight object by id")]
            public string ObjectId { get; set; }

            [Value(0)]
            public string File { get; set; }
        }
    }
}

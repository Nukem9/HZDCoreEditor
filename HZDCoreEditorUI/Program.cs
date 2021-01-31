using Decima;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommandLine;

namespace HZDCoreEditorUI
{
    public class CmdOptions
    {
        [Option('s', "search", HelpText = "Search for text")]
        public string Search { get; set; }

        [Option('o', "object", HelpText = "Highlight object by id")]
        public string ObjectId { get; set; }

        [Value(0)]
        public string File { get; set; }
    }

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            RTTI.SetGameMode(GameType.HZD);

            var cmds = new CmdOptions();
            var parser = new Parser(with => with.HelpWriter = Console.Error);
            parser.ParseArguments<CmdOptions>(args)
                .WithParsed(o => cmds = o)
                .WithNotParsed(errs => MessageBox.Show("Unable to parse command line: {0}", String.Join(" ", args)));

            Application.Run(new UI.FormCoreView(cmds));
            
        }
    }
}

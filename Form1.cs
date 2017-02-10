using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceWar
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var files = Directory.GetFiles(@"..\..\Classes");

            foreach (var file in files)
            {
                if (file.Contains("API"))
                {
                    continue;
                }
                
                var content = File.ReadAllText(file);
                const string pattern = "^.*namespace SpaceWar.Classes\r\n{(.*)}$";
                var match = Regex.Match(content, pattern, RegexOptions.Singleline);

                textBox1.Text += match.Groups[1].Value;
            }
        }
    }
}

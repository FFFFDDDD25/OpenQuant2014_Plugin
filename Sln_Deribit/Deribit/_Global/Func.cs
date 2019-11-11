using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Global
{
    static public class Func
    {
        static public string InputBox(string title, string defalutAnswer)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox(title, "", defalutAnswer);
            return input;
        }

    }
}




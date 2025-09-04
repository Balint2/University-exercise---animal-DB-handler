using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2INIS_HSZF_2024251.Console
{
    public struct MenuItem
    {
        public MenuItem(string text, Action function)
        {
            Text = text;
            Function = function;
        }

        public MenuItem(string text, Action function, int id) : this(text, function)
        {
            Id = id;
        }
        
        public string Text { get; private set; }
        public Action Function { get; private set; }
        public int Id { get;  private set; }
    }
}

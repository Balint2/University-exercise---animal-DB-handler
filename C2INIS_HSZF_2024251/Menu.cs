using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2INIS_HSZF_2024251.Console
{
    public class Menu
    {
        protected string title;
        protected string comment;
        protected MenuItem[] buttons;
        protected int counter = 0;

        public Menu(string title, string comment, MenuItem[] buttons)
        {
            this.buttons = buttons;
            this.title = title;
            this.comment = comment;
            if (this.GetType() == typeof(Menu))
            {
                Write();
            }
        }




        void Write()
        {
            System.Console.Clear();
            System.Console.ForegroundColor = ConsoleColor.Magenta;
            System.Console.WriteLine(title);
            System.Console.WriteLine();
            for (int i = 0; i < buttons.Length; i++)
            {
                if (i == counter)
                {
                    System.Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    System.Console.ForegroundColor = ConsoleColor.Yellow;
                }
                System.Console.WriteLine(buttons[i].Text);
            }
            System.Console.ForegroundColor = ConsoleColor.White;

            if (comment != "")
            {
                System.Console.WriteLine();
                System.Console.ForegroundColor = ConsoleColor.White;
                System.Console.WriteLine(comment);
            }
        }

        virtual public MenuItem Loop()
        {
            ConsoleKeyInfo consoleKey;
            do
            {
                consoleKey = System.Console.ReadKey(true);
                if (consoleKey.Key == ConsoleKey.UpArrow)
                {
                    counter--;
                    if (counter < 0)
                        counter = buttons.Length - 1;

                }
                else if (consoleKey.Key == ConsoleKey.DownArrow)
                {
                    counter++;
                    if (counter >= buttons.Length)
                        counter = 0;
                }
                int currentScrollPosition = System.Console.WindowTop;
                Write();
                System.Console.WindowTop = currentScrollPosition;
            } while (consoleKey.Key != ConsoleKey.Enter);

            return(buttons[counter]);
        }


    }
}

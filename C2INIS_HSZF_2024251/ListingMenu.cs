using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2INIS_HSZF_2024251.Console
{
    public class ListingMenu : Menu
    {

        int page;
        const int maxItemsOnScreen = 10;
        int maxPage;
        MenuItem[] navigateButtons;
        int rowHeight;

        public ListingMenu(string title, string comment, MenuItem[] entityButtons, MenuItem[] navigateButtons, int rowHeight = 1) : base(title, comment, entityButtons)
        {
            page = 0;
            maxPage = entityButtons.Length / maxItemsOnScreen;
            this.navigateButtons = navigateButtons;
            this.rowHeight = rowHeight;

            Write();
        }

        void Write()
        {
            System.Console.Clear();

            System.Console.ForegroundColor = ConsoleColor.Magenta;
            System.Console.WriteLine(title);
            System.Console.WriteLine();


            for (int i = page*maxItemsOnScreen; i < buttons.Length && i < (page + 1) * maxItemsOnScreen; i++)
            {
                if (i == counter + page*maxItemsOnScreen)
                {
                    System.Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    System.Console.ForegroundColor = ConsoleColor.Yellow;
                }
                System.Console.WriteLine(buttons[i].Text);
            }

            for (int i = 0; i < navigateButtons.Length; i++)
            {
                if (i + Math.Min(maxItemsOnScreen, buttons.Length - page*maxItemsOnScreen) == counter)
                {
                    System.Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    System.Console.ForegroundColor = ConsoleColor.Yellow;
                }
                System.Console.WriteLine(navigateButtons[i].Text);
            }
            System.Console.ForegroundColor = ConsoleColor.White;


            if (comment != "")
            {
                System.Console.WriteLine();
                System.Console.ForegroundColor = ConsoleColor.White;
                System.Console.WriteLine(comment);
            }


            System.Console.WindowTop = rowHeight * Math.Min(counter, Math.Min(maxItemsOnScreen, buttons.Length - page * maxItemsOnScreen));

            /*if (Math.Min(buttons.Length - page*maxItemsOnScreen, maxItemsOnScreen) > 3)
                System.Console.WindowTop = rowHeight * Math.Min(counter, maxItemsOnScreen);
            else
                System.Console.WindowTop = 0;*/
        }

        /*void Refresh(int lastPos)
        {
            int difference = (lastPos-counter)*rowHeight;
            int actPos = System.Console.CursorTop;
            if (counter <= maxItemsOnScreen)
            {
            }
            else
            { 
            }

        }*/

        override public MenuItem Loop()
        {
            ConsoleKeyInfo consoleKey;
            do
            {
                consoleKey = System.Console.ReadKey(true);
                int lastPos = counter;
                bool newSite = false;
                switch (consoleKey.Key)
                {
                    case ConsoleKey.UpArrow:
                        {
                            counter--;
                            if (counter < 0)
                                counter = Math.Min(buttons.Length + navigateButtons.Length - 1 - page * maxItemsOnScreen, maxItemsOnScreen + navigateButtons.Length - 1);
                            break;
                        }
                    case ConsoleKey.DownArrow:
                        {
                            counter++;
                            if (counter >= buttons.Length - page * maxItemsOnScreen + navigateButtons.Length || counter >= maxItemsOnScreen + navigateButtons.Length)
                                counter = 0;
                            break;
                        }
                    case ConsoleKey.RightArrow:
                        {
                            newSite = true;
                            page++;
                            if (page > maxPage)
                                page = 0;
                            if (counter >= buttons.Length - page * maxItemsOnScreen + navigateButtons.Length || counter >= maxItemsOnScreen + navigateButtons.Length)
                                counter = buttons.Length - page * maxItemsOnScreen - 1 + navigateButtons.Length;
                            break;
                        }
                    case ConsoleKey.LeftArrow:
                        {
                            newSite = true;
                            page--;
                            if (page < 0)
                                page = maxPage;
                            if (counter >= buttons.Length - page * maxItemsOnScreen + navigateButtons.Length || counter >= maxItemsOnScreen + navigateButtons.Length)
                                counter = buttons.Length - page * maxItemsOnScreen - 1 + navigateButtons.Length;
                            break;
                        }
                }
                //Refresh(lasPos);
                Write();
                /*
                int currentScrollPosition = System.Console.WindowTop;
                Write();
                if (!newSite)
                    System.Console.WindowTop = currentScrollPosition;*/
            } while (consoleKey.Key != ConsoleKey.Enter/* && consoleKey.Key != ConsoleKey.Backspace*/);
            int quantity = Math.Min(maxItemsOnScreen, buttons.Length-page*maxItemsOnScreen);
            if (counter < Math.Min(maxItemsOnScreen, quantity))
            {
                return (buttons[page*maxItemsOnScreen + counter]);
            }
            else
            {
                return (navigateButtons[counter-quantity]);
            }
        }


    }
}

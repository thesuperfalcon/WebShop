using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop
{
    internal class WindowUI
    {
        public class Window
        {
            public string Header { get; set; }
            public int Left { get; set; }
            public int Top { get; set; }
            public List<string> TextRows { get; set; }

            public Window(string header, int left, int top, List<string> textRows)
            {
                Header = header;
                Left = left;
                Top = top;
                TextRows = textRows;
            }

            public void Draw()
            {
                var width = TextRows.OrderByDescending(s => s.Length).FirstOrDefault().Length;

                // Kolla om Header är längre än det längsta ordet i listan
                if (width < Header.Length + 4)
                {
                    width = Header.Length + 4;
                };

                // Rita Header
                Console.SetCursorPosition(Left, Top);
                if (Header != "")
                {
                    Console.Write('┌' + " ");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(Header);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" " + new String('─', width - Header.Length) + '┐');
                }
                else
                {
                    Console.Write('┌' + new String('─', width + 2) + '┐');
                }

                // Rita raderna i sträng-Listan
                for (int j = 0; j < TextRows.Count; j++)
                {
                    Console.SetCursorPosition(Left, Top + j + 1);
                    Console.WriteLine('│' + " " + TextRows[j] + new String(' ', width - TextRows[j].Length + 1) + '│');
                }

                // Rita undre delen av fönstret
                Console.SetCursorPosition(Left, Top + TextRows.Count + 1);
                Console.Write('└' + new String('─', width + 2) + '┘');


                // Kolla vilket som är den nedersta posotion, i alla fönster, som ritats ut
                if (Lowest.LowestPosition < Top + TextRows.Count + 2)
                {
                    Lowest.LowestPosition = Top + TextRows.Count + 2;
                }

                Console.SetCursorPosition(0, Lowest.LowestPosition);
            }
            public void DrawMenu(List<string> menuOptions)
            {
                int menuWidth = menuOptions.Max(option => option.Length) + 4;

                Console.SetCursorPosition(Left, Top + TextRows.Count + 2);
                Console.Write('┌' + new String('─', menuWidth + 1) + '┐');

                for (int j = 0; j < menuOptions.Count; j++)
                {
                    Console.SetCursorPosition(Left, Top + TextRows.Count + j + 3);
                    Console.WriteLine('│' + " " + j + ". " + menuOptions[j] + new String(' ', menuWidth - menuOptions[j].Length + -3) + '│');
                }

                Console.SetCursorPosition(Left, Top + TextRows.Count + menuOptions.Count + 3);
                Console.Write('└' + new String('─', menuWidth +1) + '┘');
            }

            public void DrawMessage(string message)
            {
                Console.SetCursorPosition(Left, Top);
                Console.Write('┌' + new String('─', message.Length + 1) + '┐');
                Console.SetCursorPosition(Left, Top + 1);
                Console.WriteLine('│' + " " + message + '│');
                Console.SetCursorPosition(Left, Top + 2);
                Console.Write('└' + new String('─', message.Length + 1) + '┘');
                Lowest.LowestPosition = Top + 3; 
            }
        }

        public static class Lowest
        {
            public static int LowestPosition { get; set; }
        }

    }
}

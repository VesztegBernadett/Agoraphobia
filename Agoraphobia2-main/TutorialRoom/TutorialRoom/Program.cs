using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorialRoom
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(200, 45);

            Console.SetCursorPosition(75, 25);

            Console.WriteLine(@"Statok:
                                                                            Életerő: 100
                                                                            Erő: 2
                                                                            Józanság: 50
                                                                            Védekezés: 3
                                                                            Energia: 3");

            Console.SetCursorPosition(72, 0);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(@"|
                                                                        |
                                                                        |
                                                                        |
                                                                        |
                                                                        |
                                                                        |
                                                                        |
                                                                        |
                                                                        |
                                                                        |
                                                                        |
                                                                        |
                                                                        |
                                                                        |
                                                                        |
                                                                        |
                                                                        |
                                                                        |
                                                                        |
                                                                        |
                                                                        |
                                                                        |
                                                                        |
                                                                        |
                                                                        |
                                                                        |
                                                                        |
                                                                        |
                                                                        |
                                                                        |
                                                                        |
                                                                        |
                                                                        |
                                                                        |
                                                                        |");
            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(88, 0);

            Console.WriteLine("Eszköztár");

            Console.SetCursorPosition(75, 20);

            Console.WriteLine("? megvizsgál | + használ | - eldob");

            Console.SetCursorPosition(25, 1);

            Console.WriteLine(@"    -------
                            |* * *  |
                            |  *  * |
                            |*  ** *|
                            | * * **|
                             -------
                            |popcorn|
                            |\_____/|
                            |_______|
                    ");

            Console.SetCursorPosition(2, 1);

            Console.WriteLine(@"         \    /\
            )  ( ')
            (  /  )
             \(__)|");


            Console.SetCursorPosition(60, 12);

            Console.WriteLine(@"    /
                                                               /
                                                              /
                                                             /
                                                            /
");

            Console.SetCursorPosition(2, 13);

            Console.WriteLine(@"__
 (`/\
 `=\/\ __...--~~~~~-._   _.-~~~~~--...__
  `=\/\               \ /               \\
   `=\/                V                 \\
   //_\___--~~~~~~-._  |  _.-~~~~~~--...__\\
  //  ) (..----~~~~._\ | /_.~~~~----.....__\\
 ===( INK )==========\\|//====================
__ejm\___/________dwb`---`____________________________________________");


            Console.SetCursorPosition(0, 23);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(@"_________________________________________________________________________________________________________________");
            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(1, 25);

            Console.WriteLine(@"John Merta, világszerte ismert író. Egy teljesen átlagos éjszakán
 miután álomra húnyta szemeit, John egy üres szobában találta magát.

 >> Körbenéz

 Ezt látod:
 >> Könyv és bot a földön
 >> Gyanús macska a szoba sarkában
 >> Popcorn-t csináló popcorn-gép");

        }
    }
}

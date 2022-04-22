using System;
using System.Threading;

/*      Snake Game (School project)
 *  github: 
 *  
 *  starting date: 4.21.
 *  finish date: 4.22.
 *  
 *  notes:
 *  - not used to c# so the code might be messy
 */

namespace Snake_Game
{
    class Snake
    {
        public static int height = 20; // velikost
        public static int width = 40;

        public int[] X = new int[800]; // maximalni velikost hada
        public int[] Y = new int[800];

        public int selected = 0; // zvoleny prvek v gui
        public int body; // pocet bunek
        public int speed; // rychlost
        public bool border_col; // kolize hranice
        public bool snake_col; // kolize tela hada
        public int best_score = 0; // nejlepsi skore
        public int body_start; // startovni pocet bunek hada

        public string output_border = "Yes"; // output veci
        public string output_snake = "Yes";

        public int scene = 0; // zvolene gui
        
        public int score = 0; // skore

        public int appleX; // pozice jablicka
        public int appleY;

        ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();
        public char key = 'd'; // startovni smer hada (lze vymnenit s 'D' aby pockal na hracuv input po startu)

        Random rnd = new Random();

        Snake() // defaultni hodnoty pri startu
        {
            X[0] = 5; // pozice hada
            Y[0] = 5;
            Console.CursorVisible = false;
            appleX = rnd.Next(2, (width - 2)); // urceni polohy jablicka
            appleY = rnd.Next(2, (height - 2));
        }

        public void RenderBoard() // vykreslovani plochy
        {
            Snake.Clear(1, 1, width + 1, height + 1);
            Console.ForegroundColor = ConsoleColor.DarkGray;

            Console.SetCursorPosition(0, 0);
            Console.Write("╔");
            for (int i = 1; i <= width + 1; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("═");
            }
            Console.Write("╗");

            Console.SetCursorPosition(0, height + 2);
            Console.Write("╚");
            for (int i = 1; i <= width + 1; i++)
            {
                Console.SetCursorPosition(i, height + 2);
                Console.Write("═");
            }
            Console.Write("╝");

            for (int i = 1; i <= height + 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("║");
            }

            for (int i = 1; i <= height + 1; i++)
            {
                Console.SetCursorPosition(width + 2, i);
                Console.Write("║");
            }
            Console.SetCursorPosition(5, height + 3); // vykreslovani skore
            Console.Write("Score: ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(score);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(25, height + 3);
            Console.Write("Best Score: ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(best_score);
            Console.ForegroundColor = ConsoleColor.DarkGray;
        }

        public void Input() // ziskavani klavesnice
        {
            if(Console.KeyAvailable)
            {
                keyInfo = Console.ReadKey(true); // ziska klavesu
                if (keyInfo.KeyChar == 'w' || keyInfo.KeyChar == 's' || keyInfo.KeyChar == 'd' || keyInfo.KeyChar == 'a')
                {
                    // prevence aby nemohl hrac zatocit opposite smerem nez jede
                    if(key == 'w' && keyInfo.KeyChar != 's')
                        key = keyInfo.KeyChar;
                    if (key == 'a' && keyInfo.KeyChar != 'd')
                        key = keyInfo.KeyChar;
                    if (key == 's' && keyInfo.KeyChar != 'w')
                        key = keyInfo.KeyChar;
                    if (key == 'd' && keyInfo.KeyChar != 'a')
                        key = keyInfo.KeyChar;
                }
                if (keyInfo.Key == ConsoleKey.Escape && scene != 0) // prepisovani escapu
                {
                    key = '0';
                }
                if (keyInfo.Key == ConsoleKey.Enter && scene != 2) // prepisovani entru
                {
                    key = '1';
                }
            }
        }

        public void PointPos(int x, int y, int type) // vykreslovani bunky
        {

            Console.SetCursorPosition(x, y); // nastaveni pozice
            if (x > 0 && x < width + 2 && y > 0 && y < height + 2) // prevence aby se bunky nevykreslovali za hranici
            {
                if (type == 0) // hlava
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write('0');
                }
                if (type == 1) // telo
                {
                    if (!(X[0] == x && Y[0] == y)) // prevence aby se hlava vykreslovala vzdy navrchu tela
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write('O');
                    }
                }
            }
            if (type == 2) // jablicko
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write('o');
            }
        }

        public void Logic()
        {
            Snake snake = new Snake();

            if (snake_col) // kolize tela hada
            {
                for (int i = 1; i <= body; i++)
                {
                    if (X[0] == X[i] && Y[0] == Y[i])
                    {
                        Init(3, best_score, body_start, speed, border_col, snake_col);
                    }
                }
            }

            if (border_col) // kolize s hranici
            {
                if(X[0] == width + 2 || Y[0] == height + 2 || X[0] == 0 || Y[0] == 0)
                    Init(3, best_score, body_start, speed, border_col, snake_col);
            }
            else // presunuti hada na druhou stranu
            {
                if(X[0] == width + 1 && key == 'd')
                {
                    X[0] = 0;
                }
                if (X[0] == 1 && key == 'a')
                {
                    X[0] = width + 2;
                }
                if (Y[0] == height + 1 && key == 's')
                {
                    Y[0] = 0;
                }
                if (Y[0] == 1 && key == 'w')
                {
                    Y[0] = height + 2;
                }
            }

            if(score > best_score) // nejlepsi score
                best_score = score;

            if(X[0] == appleX) // pridavani bunek hada kdyz je na jablicku
            {
                if(Y[0] == appleY)
                {
                    body++;
                    score++;

                    appleX = rnd.Next(2, (width - 2)); // generace noveho jablicka
                    appleY = rnd.Next(2, (height - 2));

                    for (int i = 0; i < body; i++) // prevence generace jablicka v hadovo tele
                    {
                        if(X[i] == appleX && Y[i] == appleY)
                        {
                            appleX = rnd.Next(2, (width - 2));
                            appleY = rnd.Next(2, (height - 2));
                            i = 0;
                        }
                    }
                }
            }

            for(int i = body; i > 1; i--) // urcovani polohy bunek za hadem
            {
                X[i - 1] = X[i - 2];
                Y[i - 1] = Y[i - 2];
            }

            switch(key) // smerovaci mechanismus
            {
                case 'w':
                    if (scene == 2)
                        Y[0]--;
                    break;
                case 's':
                    if (scene == 2)
                        Y[0]++;
                    break;
                case 'd':
                    if (scene == 2)
                        X[0]++;
                    break;
                case 'a':
                    if (scene == 2)
                        X[0]--;
                    break;
                case '0':
                    Init(0, best_score, body_start, speed, border_col, snake_col);
                    break;
            }

            for (int i = 0; i <= (body - 1); i++) // vykreslovani charakteru
            {
                if (i == 0)
                    PointPos(X[i], Y[i], 0); // snake head
                else
                    PointPos(X[i], Y[i], 1); // snake body

                PointPos(appleX, appleY, 2); // apple
            }

            switch(speed) // rychlost hry
            {
                case 1:
                    Thread.Sleep(300);
                    break;
                case 2:
                    Thread.Sleep(250);
                    break;
                case 3:
                    Thread.Sleep(200);
                    break;
                case 4:
                    Thread.Sleep(150);
                    break;
                case 5:
                    Thread.Sleep(100);
                    break;
                case 6:
                    Thread.Sleep(90);
                    break;
                case 7:
                    Thread.Sleep(80);
                    break;
                case 8:
                    Thread.Sleep(60);
                    break;
                case 9:
                    Thread.Sleep(50);
                    break;
                case 10:
                    Thread.Sleep(40);
                    break;
            }
        }

        public void Menu()
        {
            Snake snake = new Snake();

            Snake.Clear(0, 0, 43, 25);

            // gui
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\t   _____             _        ");
            Console.WriteLine("\t  / ____|           | |       ");
            Console.WriteLine("\t | (___  _ __   __ _| | _____ ");
            Console.WriteLine("\t  \\___ \\| '_ \\ / _` | |/ / _ \\");
            Console.WriteLine("\t  ____) | | | | (_| |   <  __/");
            Console.WriteLine("\t |_____/|_| |_|\\__,_|_|\\_\\___|");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("═══════════════════════════════════════════");
            Console.ForegroundColor = ConsoleColor.DarkGray;

            if(selected == 0)
                Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\tPlay"); // 0
            if (selected == 0)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(" <<");
            }
            Console.ForegroundColor = ConsoleColor.DarkGray;

            if (selected == 1)
                Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\n\tSettings"); // 1
            if (selected == 1)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(" <<");
            }
            Console.ForegroundColor = ConsoleColor.DarkGray;

            if (selected == 2)
                Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\n\tQuit"); // 2
            if (selected == 2)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(" <<");
            }

            snake.Input(); // ceka na input

            // pohybovani v menu
            if (snake.keyInfo.Key == ConsoleKey.W)
            {
                selected--;
                if (selected < 0)
                    selected = 2;
            }

            if (snake.keyInfo.Key == ConsoleKey.S)
            {
                selected++;
                if (selected > 2)
                    selected = 0;
            }

            // zavolani funkci
            if (snake.keyInfo.Key == ConsoleKey.Enter)
            {
                if (selected == 0)
                {
                    scene = 2;
                }
                if (selected == 1)
                {
                    scene = 1;
                    selected = 0;
                }
                if (selected == 2)
                    Environment.Exit(0);
            }
            Thread.Sleep(100);
        }

        public void GameOver()
        {
            Snake snake = new Snake();

            Snake.Clear(0, 0, 43, 25);

            // gui
            Console.SetCursorPosition(5, 2);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(">>>>\t");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Game Over!");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("\t<<<<");

            Console.SetCursorPosition(9, 4);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Press Enter to Continue");

            Console.SetCursorPosition(14, 6);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("Final Score: ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(score);

            snake.Input(); // ceka na input

            if (snake.keyInfo.Key == ConsoleKey.Enter)
            {
                Init(0, best_score, body_start, speed, border_col, snake_col); // zavola menu
            }

            Thread.Sleep(100);
        }

        public void Settings()
        {
            Snake snake = new Snake();

            Snake.Clear(0, 0, 43, 25);

            // gui
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("═══════════════  Settings  ════════════════");
            Console.ForegroundColor = ConsoleColor.DarkGray;

            if (selected == 0)
                Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\tBody Length at Start: "); // 0
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(body_start);
            Console.ForegroundColor = ConsoleColor.DarkGray;

            if (selected == 1)
                Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\n\tSpeed: "); // 1
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(speed);
            Console.ForegroundColor = ConsoleColor.DarkGray;

            if (selected == 2)
                Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"\n\tBorder Collision: "); // 2
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(output_border);
            Console.ForegroundColor = ConsoleColor.DarkGray;

            if (selected == 3)
                Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"\n\tBody Collision: "); // 3
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(output_snake);
            Console.ForegroundColor = ConsoleColor.DarkGray;

            if (selected == 4)
                Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\n\tBack"); // 4

            snake.Input(); // ziskava input

            // resi zvoleny prvek v gui
            if (snake.keyInfo.Key == ConsoleKey.W)
            {
                selected--;
                if (selected < 0)
                    selected = 4;
            }

            if (snake.keyInfo.Key == ConsoleKey.S)
            {
                selected++;
                if (selected > 4)
                    selected = 0;
            }

            // funcke prvnku
            if (selected == 0)
            {
                if (snake.keyInfo.Key == ConsoleKey.D && body_start < 10)
                    body_start++;
                if (snake.keyInfo.Key == ConsoleKey.A && body_start > 1)
                    body_start--;
            }

            if (selected == 1)
            {
                if (snake.keyInfo.Key == ConsoleKey.D && speed < 10)
                {
                    speed++;
                }
                if (snake.keyInfo.Key == ConsoleKey.A && speed > 1)
                {
                    speed--;
                }
            }

            if (snake.keyInfo.Key == ConsoleKey.Enter)
            {
                if (selected == 2)
                {
                    border_col = !border_col;
                    if (border_col)
                        output_border = "Yes";
                    else
                        output_border = "No";
                }

                if (selected == 3)
                {
                    snake_col = !snake_col;
                    if (snake_col)
                        output_snake = "Yes";
                    else
                        output_snake = "No";
                }

                if (selected == 4)
                {
                    Init(0, best_score, body_start, speed, border_col, snake_col);
                }
            }

            if (snake.keyInfo.Key == ConsoleKey.Escape)
            {
                Init(0, best_score, body_start, speed, border_col, snake_col);
            }
            Thread.Sleep(100);
        }

        // cisteni konzole
        static void Clear(int x, int y, int width, int height)
        {
            Snake snake = new Snake();
            int curTop = Console.CursorTop;
            int curLeft = Console.CursorLeft;
            for (; height > 0;)
            {
                Console.SetCursorPosition(x, y + --height);
                Console.Write(new string(' ', width));
            }   
            Console.SetCursorPosition(curLeft, curTop);
        }

        public void Init(int start, int best, int body_start, int speed, bool border_c, bool snake_c)
        {
            Snake snake = new Snake();

            // nastavovani deafultnich vars
            snake.speed = speed;
            snake.body = body_start;
            snake.scene = start;
            snake.best_score = best;
            snake.border_col = border_c;
            snake.snake_col = snake_c;
            snake.body_start = body_start;

            // gui
            while (true)
            {
                switch (snake.scene)
                {
                    case 0:
                        snake.Menu();
                        break;
                    case 1:
                        snake.Settings();
                        break;
                    case 2:
                        snake.RenderBoard();
                        snake.Input();
                        snake.Logic();
                        break;
                    case 3:
                        GameOver();
                        break;
                }
            }
        }

        static void Main(string[] args)
        {
            Console.SetWindowSize(43, 25); // urceni velikosti okna
            Snake snake = new Snake();

            snake.Init(0, 0, 3, 5, true, true);
        }
    }
}
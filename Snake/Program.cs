using System;
using System.Threading;

/*      TODO:
 * - border
 * - crash
 * - score
 * - color
 */


namespace Snake_Game
{
    class Snake
    {
        static int height = 20;
        static int width = 40;

        int[] X = new int[800];
        int[] Y = new int[800];

        int appleX;
        int appleY;

        int selected = 0;

        int body = 3;
        int speed = 100;
        bool border_col;
        bool snake_col;

        ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();
        char key = 'W';

        Random rnd = new Random();

        Snake()
        {
            X[0] = 5;
            Y[0] = 5;
            Console.CursorVisible = false;
            appleX = rnd.Next(2, (width - 2));
            appleY = rnd.Next(2, (height - 2));
        }

        public void RenderBoard()
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
        }

        public void Input()
        {
            if(Console.KeyAvailable)
            {
                keyInfo = Console.ReadKey(true);
                key = keyInfo.KeyChar;
            }
        }

        public void WritePoint(int x, int y, int type)
        {
            Console.SetCursorPosition(x, y);
            if (type == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write('0');
            }
            if (type == 1)
            {
                if (!(X[0] == x && Y[0] == y))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write('O');
                }
            }
            if (type == 2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write('a');
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        public void Logic()
        {
            if(X[0] == appleX)
            {
                if(Y[0] == appleY)
                {
                    body++;
                    appleX = rnd.Next(2, (width - 2));
                    appleY = rnd.Next(2, (height - 2));
                }
            }

            for(int i = body; i > 1; i--)
            {
                X[i - 1] = X[i - 2];
                Y[i - 1] = Y[i - 2];
            }

            switch(key)
            {
                case 'w':
                    Y[0]--;
                    break;
                case 's':
                    Y[0]++;
                    break;
                case 'd':
                    X[0]++;
                    break;
                case 'a':
                    X[0]--;
                    break;
            }

            for (int i = 0; i <= (body - 1); i++)
            {
                if (i == 0)
                    WritePoint(X[i], Y[i], 0); // snake head
                else
                    WritePoint(X[i], Y[i], 1); // snake body
                WritePoint(appleX, appleY, 2); // apple
            }

            Thread.Sleep(100);
        }

        public int Menu()
        {
            Snake snake = new Snake();

            Snake.Clear(0, 0, 43, 25);

            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("   _____             _        ");
            Console.WriteLine("  / ____|           | |       ");
            Console.WriteLine(" | (___  _ __   __ _| | _____ ");
            Console.WriteLine("  \\___ \\| '_ \\ / _` | |/ / _ \\");
            Console.WriteLine("  ____) | | | | (_| |   <  __/");
            Console.WriteLine(" |_____/|_| |_|\\__,_|_|\\_\\___|");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("═══════════════════════════════");
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

            snake.Input();

            if (snake.keyInfo.Key == ConsoleKey.W)
            {
                selected--;
                if (selected < 0)
                    selected = 2;
                return 0;
            }

            if (snake.keyInfo.Key == ConsoleKey.S)
            {
                selected++;
                if (selected > 2)
                    selected = 0;
                return 0;
            }

            if (snake.keyInfo.Key == ConsoleKey.Enter)
            {
                if (selected == 0)
                    return 1;
                if (selected == 1)
                {
                    selected = 0;
                    return 2;
                }
                if (selected == 2)
                    Environment.Exit(0);
            }
            Thread.Sleep(100);

            return 0;
        }

        public int Settings()
        {
            Snake snake = new Snake();

            Snake.Clear(0, 0, 43, 25);

            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("═══════════════  Settings  ════════════════");
            Console.ForegroundColor = ConsoleColor.DarkGray;

            if (selected == 0)
                Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\tBody Length at Start: "); // 0
            Console.ForegroundColor = ConsoleColor.DarkGray;

            if (selected == 1)
                Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\n\tSpeed: "); // 1
            Console.ForegroundColor = ConsoleColor.DarkGray;

            if (selected == 2)
                Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"\n\tBorder Collision: {snake.border_col}"); // 2
            Console.ForegroundColor = ConsoleColor.DarkGray;

            if (selected == 3)
                Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"\n\tBody Collision: {snake.snake_col}"); // 3
            Console.ForegroundColor = ConsoleColor.DarkGray;

            if (selected == 4)
                Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\n\tBack"); // 4

            snake.Input();

            if (snake.keyInfo.Key == ConsoleKey.W)
            {
                selected--;
                if (selected < 0)
                    selected = 4;
                return 0;
            }

            if (snake.keyInfo.Key == ConsoleKey.S)
            {
                selected++;
                if (selected > 4)
                    selected = 0;
                return 0;
            }

            if (snake.keyInfo.Key == ConsoleKey.Enter)
            {
                //if (selected == 0)

                //if (selected == 1)

                if (selected == 2)
                {
                    snake.border_col = snake.border_col;
                    return 0;
                }

                if (selected == 3)
                {
                    snake.snake_col = snake.snake_col;
                }
                     

                if (selected == 4)
                {
                    selected = 0;
                    return 5;
                }


            }
            Thread.Sleep(100);

            return 0;
        }

        static void Clear(int x, int y, int width, int height)
        {
            int curTop = Console.CursorTop;
            int curLeft = Console.CursorLeft;
            for (; height > 0;)
            {
                Console.SetCursorPosition(x, y + --height);
                Console.Write(new string(' ', width));
            }
            Console.SetCursorPosition(curLeft, curTop);
        }

        static void Main(string[] args)
        {
            Console.SetWindowSize(43, 25);

            Snake snake = new Snake();

            bool start = false;
            bool settings = false;

            while (true)
            {
                if (!start)
                {
                    if (!settings)
                    {
                        int go = snake.Menu();
                        if (go == 1)
                            start = !start;
                        if (go == 2)
                            settings = !settings;
                    }
                    else
                    {
                        int go = snake.Settings();
                        if (go == 5)
                            settings = !settings;
                    }
                }

                if (start)
                {
                    snake.RenderBoard();
                    snake.Input();
                    snake.Logic();
                }
            }
        }
    }
}
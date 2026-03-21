using Raylib_cs;
using CellInfo;
using System.ComponentModel.DataAnnotations;

bool firstTime = true;
int screenHeight = 500;
int screenWidth = 800;
int cellSize = 50;
int cellNumber = 0;

List<CellInfoClass> cellInfoList = [];


Raylib.InitWindow(screenWidth, screenHeight, "Roligt TD spel");
Raylib.SetTargetFPS(60);
while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.Green);
    for (int y = 0; y < screenHeight; y += cellSize)
    {
        for (int x = 0; x < screenWidth; x += cellSize)
        {
            cellNumber++;
            Raylib.DrawRectangleLines(x, y, cellSize, cellSize, Color.White);
            if (firstTime)
            {
                cellInfoList.Add(new CellInfoClass(x, y, cellNumber, Color.Green));
            }
            else
            {
                Raylib.DrawRectangle(x, y, cellSize, cellSize, cellInfoList[cellNumber - 1].CellColor);
            }
        }
    }
    Raylib.EndDrawing();
    cellNumber = 0;
    firstTime = false;
}
static int CordToCellNumberConverter(int xCord, int yCord, int cellSize, List<CellInfoClass> cellInfoList)
{
    for (int i = 0; i < cellInfoList.Count; i++)
    {
        if (cellInfoList[i].X <= xCord && cellInfoList[i].X + cellSize > xCord)
        {
            if (cellInfoList[i].Y <= yCord && cellInfoList[i].Y + cellSize > yCord)
            {
                return (cellInfoList[i].cellNumber);
            }
        }
    }
    return (0);
}






List<(int x, int y)> path = new List<(int x, int y)>
{
    (0,3), (1,3), (2,3),
    (3,3), (3,4), (4,4),
    (5,4), (5,5), (5,6)
};
List<string> listOfMenuItem = ["hem", "in", "r"];
Menu(0, 2, listOfMenuItem);
static void Menu(int minLevel, int maxLevel, List<String> listOfMenuItem)
{
    int level = 0;
    MenuPrinter(listOfMenuItem, level);
    while (true)
    {
        ConsoleKey pressKey = Console.ReadKey(true).Key;
        level = LevelSwitcher(pressKey, minLevel, maxLevel, level);
        Console.Clear();
        MenuPrinter(listOfMenuItem, level);
    }
}
static int LevelSwitcher(ConsoleKey pressKey, int minLevel, int maxLevel, int level)
{
    switch (pressKey)
    {
        case ConsoleKey.DownArrow:
            if (level < maxLevel)
            {
                level++;
            }
            break;
        case ConsoleKey.UpArrow:
            if (level > minLevel)
            {
                level--;
            }
            break;
    }
    return (level);
}
static void MenuPrinter(List<string> listOfMenuItem, int level)
{
    for (int i = 0; i < listOfMenuItem.Count; i++)
    {
        if (i == level)
        {
            Console.WriteLine($">{listOfMenuItem[i]}<");
        }
        else
        {
            Console.WriteLine($" {listOfMenuItem[i]}");
        }
    }
}
// kod som kanske ska användas men kom på att det är jätte dump för man vill inte
// bara se en inzomad skit av banan vilket det ser ut som.
// static (int, int) CordinateCalkylator(List<(int x, int y)> path)
// {
//     List<int> pathX = [];
//     List<int> pathY = [];

//     for (int i = 0; i < path.Count; i++)
//     {
//         pathX.Add(path[i].x);
//         pathY.Add(path[i].y);
//     }
//     int maxCordinateX = pathX.Max();
//     int maxCordinateY = pathY.Max();
//     return (maxCordinateX, maxCordinateY);
// }

// static void BoardBuilder(List<(int, int)> path)
// {
//     int[,] board = new int[10, 15];

//     for (int y = 0; y <= board.GetLength(1); y++)
//     {
//         for (int x = 0; x <= board.GetLength(0); x++)
//         {
//             if (path.Contains((x, y)))
//             {
//                 Console.BackgroundColor = ConsoleColor.Yellow;
//             }
//             else
//             {
//                 Console.BackgroundColor = ConsoleColor.Blue;
//             }
//             Console.Write("x");
//         }
//         Console.WriteLine();
//     }
// }





Console.ReadLine();
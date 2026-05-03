using Raylib_cs;
using CellInfo;
using BasicEnemy;
using System.Numerics;
using Tower;
//skärm relaterade datatyper
bool firstTime = true;
int screenHeight = 500;
int screenWidth = 800;
//cell relaterade datatyper
int cellSize = 50;
int cellNumber = -1;
List<CellInfoClass> cellInfoList = [];
List<(int, int)> pathEasy1 = [(150, 0), (150, 50), (150, 100), (150, 150), (150, 200), (200, 200), (250, 200), (300, 200), (350, 200), (400, 200), (450, 200), (500, 200), (500, 150), (500, 100), (500, 50), (550, 50), (600, 50), (650, 50), (650, 100), (650, 150), (650, 200), (650, 250), (650, 300), (650, 350), (700, 350), (750, 350), (800, 350)];
//mus relaterade datatyper
int oldMouseCell = 0;
//fiende grejor
List<BasicEnemyClass> basicEnemy = [];
basicEnemy.Add(new BasicEnemyClass(100, 1000, new Vector2(175, -25)));
basicEnemy.Add(new BasicEnemyClass(100, 1000, new Vector2(175, -25)));
basicEnemy.Add(new BasicEnemyClass(100, 1000, new Vector2(175, -25)));
//torn relaterade datatyper
List<TowerStats> towerStatsList = [];


Raylib.InitWindow(screenWidth, screenHeight, "Roligt TD spel");
Raylib.SetTargetFPS(60);
while (!Raylib.WindowShouldClose())
{
    int mousePosX = Raylib.GetMouseX();
    int mousePosY = Raylib.GetMouseY();
    int wichCellMouseOn = CordToCellNumberConverter(mousePosX, mousePosY, 50, cellInfoList);
    blockHigheLighter(cellInfoList, ref oldMouseCell, wichCellMouseOn);
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.White);
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
    TowerController(basicEnemy, towerStatsList);
    EnemyController(basicEnemy, pathEasy1);
    Raylib.EndDrawing();
    cellNumber = 0;
    firstTime = false;
    for (int i = 0; i < pathEasy1.Count - 1; i++)
    {
        int tempCellNumber = CordToCellNumberConverter(pathEasy1[i].Item1, pathEasy1[i].Item2, 50, cellInfoList);
        cellInfoList[tempCellNumber].CellColor = Color.Brown;
    }
    ClickChecker(cellInfoList, wichCellMouseOn, ref oldMouseCell, towerStatsList, pathEasy1);
}
static void EnemyController(List<BasicEnemyClass> basicEnemy, List<(int, int)> pathEasy1)
{
    for (int i = 0; i < basicEnemy.Count; i++)
    {
        if (!basicEnemy[i].Enemymover(pathEasy1))
        {
            basicEnemy.RemoveAt(i);
            i--;
        }
    }
}
static void TowerController(List<BasicEnemyClass> basicEnemy, List<TowerStats> towerStatsList)
{
    for (int i = 0; i < towerStatsList.Count; i++)
    {
        towerStatsList[i].TowerShoter(basicEnemy);
    }
}
static int CordToCellNumberConverter(int xCord, int yCord, int cellSize, List<CellInfoClass> cellInfoList)
{
    for (int i = 0; i < cellInfoList.Count; i++)
    {
        if (cellInfoList[i].X <= xCord && cellInfoList[i].X + cellSize > xCord)
        {
            if (cellInfoList[i].Y <= yCord && cellInfoList[i].Y + cellSize > yCord)
            {
                return (cellInfoList[i].CellNumber);
            }
        }
    }
    return (0);
}
static (float, float) CellNumberToCordConverter(List<CellInfoClass> cellInfoList, int cellNumber)
{
    return (cellInfoList[cellNumber].X, cellInfoList[cellNumber].Y);
}
static void blockHigheLighter(List<CellInfoClass> cellInfoList, ref int oldMouseCell, int wichCellMouseOn)
{

    if (oldMouseCell != wichCellMouseOn)
    {
        cellInfoList[wichCellMouseOn].CellColor = Raylib.ColorAlpha(cellInfoList[wichCellMouseOn].CellColor, 0.5f);
        cellInfoList[oldMouseCell].CellColor = Raylib.ColorAlpha(cellInfoList[oldMouseCell].CellColor, 1);
        oldMouseCell = wichCellMouseOn;
    }

}
static void ClickChecker(List<CellInfoClass> cellInfoList, int wichCellMouseOn, ref int oldMouseCell, List<TowerStats> towerStatsList, List<(int, int)> pathEasy1)
{
    bool haveMouseBenPressed;
    haveMouseBenPressed = Raylib.IsMouseButtonPressed(MouseButton.Left);
    if (haveMouseBenPressed)
    {
        TowerPlacer(cellInfoList, wichCellMouseOn, pathEasy1, towerStatsList);
    }
}
static void TowerPlacer(List<CellInfoClass> cellInfoList, int wichCellMouseOn, List<(int, int)> pathEasy1, List<TowerStats> towerStatsList)
{
    if (PlaceIsOcupied(cellInfoList, wichCellMouseOn, pathEasy1, towerStatsList) == false)
    {
        cellInfoList[wichCellMouseOn].CellColor = Color.Red;
        towerStatsList.Add(new TowerStats(new Vector2(CellNumberToCordConverter(cellInfoList, wichCellMouseOn).Item1, CellNumberToCordConverter(cellInfoList, wichCellMouseOn).Item2), 400, 10, 20));
    }
}
static bool PlaceIsOcupied(List<CellInfoClass> cellInfoList, int wichCellMouseOn, List<(int, int)> pathEasy1, List<TowerStats> towerStatsList)
{
    for (int i = 0; i < towerStatsList.Count; i++)
    {
        if (wichCellMouseOn == CordToCellNumberConverter((int)towerStatsList[i].Pos.X, (int)towerStatsList[i].Pos.Y, 50, cellInfoList))
        {
            return true;
        }
    }
    for (int i = 0; i < pathEasy1.Count; i++)
    {
        if (wichCellMouseOn == CordToCellNumberConverter(pathEasy1[i].Item1, pathEasy1[i].Item2, 50, cellInfoList))
        {
            return true;
        }
    }
    return false;
}
static void WaveMaker()
{
    
}

// static void Menu(int minLevel, int maxLevel, List<String> listOfMenuItem)
// {
//     int level = 0;
//     MenuPrinter(listOfMenuItem, level);
//     while (true)
//     {
//         ConsoleKey pressKey = Console.ReadKey(true).Key;
//         level = LevelSwitcher(pressKey, minLevel, maxLevel, level);
//         Console.Clear();
//         MenuPrinter(listOfMenuItem, level);
//     }
// }
// static int LevelSwitcher(ConsoleKey pressKey, int minLevel, int maxLevel, int level)
// {
//     switch (pressKey)
//     {
//         case ConsoleKey.DownArrow:
//             if (level < maxLevel)
//             {
//                 level++;
//             }
//             break;
//         case ConsoleKey.UpArrow:
//             if (level > minLevel)
//             {
//                 level--;
//             }
//             break;
//     }
//     return (level);
// }
// static void MenuPrinter(List<string> listOfMenuItem, int level)
// {
//     for (int i = 0; i < listOfMenuItem.Count; i++)
//     {
//         if (i == level)
//         {
//             Console.WriteLine($">{listOfMenuItem[i]}<");
//         }
//         else
//         {
//             Console.WriteLine($" {listOfMenuItem[i]}");
//         }
//     }
// }




Console.ReadLine();

using Raylib_cs;
using CellInfo;
using BasicEnemy;
using System.Numerics;

//skärm relaterade datatyper
bool firstTime = true;
int screenHeight = 500;
int screenWidth = 800;
//cell relaterade datatyper
int cellSize = 50;
int cellNumber = -1;
List<CellInfoClass> cellInfoList = [];
List<(int, int)> pathEasy1 = [(150, 0), (150, 50), (150, 100), (150, 150), (150, 200), (200, 200), (250, 200), (300, 200), (350, 200), (400, 200), (450, 200), (500, 200), (500, 150), (500, 50), (500, 100), (550, 50), (600, 50), (650, 50), (650, 100), (650, 150), (650, 200), (650, 250), (650, 300), (650, 350), (700, 350), (750, 350)];
//mus relaterade datatyper
bool haveMouseBenPressed = false;
int oldMouseCell = 0;
//fiende grejor

List<BasicEnemyClass> basicEnemy = [];
basicEnemy.Add(new BasicEnemyClass(100, 10, new Vector2(150 , 0)));

Raylib.InitWindow(screenWidth, screenHeight, "Roligt TD spel");
Raylib.SetTargetFPS(60);
while (!Raylib.WindowShouldClose())
{
    int mousePosX = Raylib.GetMouseX();
    int mousePosY = Raylib.GetMouseY();
    int wichCellMouseOn = CordToCellNumberConverter(mousePosX, mousePosY, 50, cellInfoList);
    blockHigheLighter(cellInfoList, ref oldMouseCell, mousePosX, mousePosY, wichCellMouseOn, haveMouseBenPressed);
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
    foreach (var enemy in basicEnemy)
    {
        enemy.Test(pathEasy1);
    }
    Raylib.EndDrawing();
    cellNumber = 0;
    firstTime = false;
    for (int i = 0; i < pathEasy1.Count; i++)
    {
        int tempCellNumber = CordToCellNumberConverter(pathEasy1[i].Item1, pathEasy1[i].Item2, 50, cellInfoList);
        cellInfoList[tempCellNumber].CellColor = Color.Brown;
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
static (int, int) CellNumberToCordConverter(List<CellInfoClass> cellInfoList, int cellNumber)
{
    return (cellInfoList[cellNumber].X, cellInfoList[cellNumber].Y);
}
static void blockHigheLighter(List<CellInfoClass> cellInfoList, ref int oldMouseCell, int mousePosX, int mousePosY, int wichCellMouseOn, bool haveMouseBenPressed)
{
    wichCellMouseOn = Math.Max(wichCellMouseOn, 0);
    oldMouseCell = Math.Max(oldMouseCell, 0);
    ClickChecker(haveMouseBenPressed, cellInfoList, wichCellMouseOn, mousePosX, mousePosY, ref oldMouseCell);
    if (oldMouseCell != wichCellMouseOn)
    {
        cellInfoList[wichCellMouseOn].CellColor = Raylib.ColorAlpha(cellInfoList[wichCellMouseOn].CellColor, 0.5f);
        cellInfoList[oldMouseCell].CellColor = Raylib.ColorAlpha(cellInfoList[oldMouseCell].CellColor, 1);
        oldMouseCell = wichCellMouseOn;
    }
}
static void ClickChecker(bool haveMouseBenPressed, List<CellInfoClass> cellInfoList, int wichCellMouseOn, int mousePosX, int mousePosY, ref int oldMouseCell)
{
    haveMouseBenPressed = Raylib.IsMouseButtonPressed(MouseButton.Left);
    if (haveMouseBenPressed)
    {
        TowerPlacer(cellInfoList, wichCellMouseOn, mousePosX, mousePosY, ref oldMouseCell);
    }
}
static void TowerPlacer(List<CellInfoClass> cellInfoList, int wichCellMouseOn, int mousePosX, int mousePosY, ref int oldMouseCell)
{
    cellInfoList[wichCellMouseOn].CellColor = Color.Brown;
    oldMouseCell = wichCellMouseOn;
    Console.WriteLine(wichCellMouseOn);
}


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




Console.ReadLine();
using Raylib_cs;
using CellInfo;
using BasicEnemy;
using System.Numerics;
using Tower;
//skärm relaterade datatyper
bool firstTime = true;
int screenWidth = 800;
int screenHeight = 500;
//cell relaterade datatyper
int cellSize = 50;
int cellNumber = -1;
List<CellInfoClass> cellInfoList = [];
List<List<(int, int)>> path = [];
List<(int, int)> pathMedium = [(150, 0), (150, 50), (150, 100), (150, 150), (150, 200), (200, 200), (250, 200), (300, 200), (350, 200), (400, 200), (450, 200), (500, 200), (500, 150), (500, 100), (500, 50), (550, 50), (600, 50), (650, 50), (650, 100), (650, 150), (650, 200), (650, 250), (650, 300), (650, 350), (700, 350), (750, 350), (800, 350)];
List<(int, int)> pathEsay = [(0, 150), (50, 150), (100, 150), (100, 200), (100, 250), (100, 300), (100, 350), (150, 350), (200, 350), (200, 300), (200, 250), (200, 200), (200, 150), (200, 100), (250, 100), (300, 100), (350, 100), (400, 100), (450, 100), (500, 100), (500, 150), (500, 200), (500, 250), (500, 300), (500, 350), (550, 350), (600, 350), (650, 350), (700, 350), (700, 300), (700, 250), (700, 200), (700, 150), (700, 100), (700, 50), (700, 0), (700, -50)];
List<(int, int)> pathHard = [(0, 200), (50, 200), (100, 200), (150, 200), (200, 200), (250, 200), (300, 200), (350, 200), (400, 200), (450, 200) ,(500, 200), (550, 200), (600, 200), (650, 200), (700, 200), (750, 200), (800, 200)];
path.Add(pathEsay);
path.Add(pathMedium);
path.Add(pathHard);
//mus relaterade datatyper
int wichCellMouseOn;
int oldMouseCell = 0;
int mousePosY;
int mousePosX;
//fiende grejor
List<BasicEnemyClass> basicEnemy = []; //valde lista för då kan man nå med ett index plus att man kan ta bort och lägga till index värden
basicEnemy.Add(new BasicEnemyClass(100, 1000, new Vector2(175, -25)));
int health = 100;
//torn relaterade datatyper
List<TowerStats> towerStatsList = []; //jag valde en lista för jag vill att den ska kuna nå med ett index och att man kan läga till mer index platser.
int money = 100;
//våg relaterade datatyper
int waveCount = 0;
//startskärm relaterade datatyper
bool showStartMenu = true;
Dictionary<string, Rectangle> startMenuButtons = new Dictionary<string, Rectangle>(); //alla knappar
startMenuButtons.Add("start", new Rectangle(50, 250, 200, 100));
startMenuButtons.Add("changeMode", new Rectangle(550, 250, 200, 100));
startMenuButtons.Add("info", new Rectangle(300, 125, 200, 100));
startMenuButtons.Add("easy", new Rectangle(275, 50, 200, 100));
startMenuButtons.Add("medium", new Rectangle(275, 200, 200, 100));
startMenuButtons.Add("hard", new Rectangle(275, 350, 200, 100));
bool showDifficultyOptions = false;
bool showInfo = false;
int difficulty = 2;
bool changeDifficulty = true;

int temp = 0;
Raylib.InitWindow(screenWidth, screenHeight, "Roligt TD spel");
Raylib.SetTargetFPS(60);
while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();
    if (showStartMenu) { StartMenu(startMenuButtons, ref showStartMenu, ref showDifficultyOptions, ref difficulty, ref showInfo, ref changeDifficulty); }
    else
    {
        Raylib.ClearBackground(Color.White);
        mousePosX = Raylib.GetMouseX();
        mousePosY = Raylib.GetMouseY();
        wichCellMouseOn = CordToCellNumberConverter(mousePosX, mousePosY, 50, cellInfoList);
        BlockHigheLighter(cellInfoList, ref oldMouseCell, wichCellMouseOn);
        CellPainter(cellInfoList, screenWidth, screenHeight, cellSize, cellNumber, firstTime);
        PathChanger(changeDifficulty, path, cellInfoList, difficulty);
        TowerController(basicEnemy, towerStatsList);
        EnemyController(basicEnemy, path[difficulty]);
        cellNumber = 0;
        firstTime = false;
        ClickChecker(cellInfoList, wichCellMouseOn, towerStatsList, path[difficulty]);
        temp++;
        if (temp == 10)
        {
            temp = 0;
            basicEnemy.Add(new BasicEnemyClass(100, 1000, new Vector2(path[difficulty][0].Item1 + 25, path[difficulty][0].Item2 + 25)));
        }
    }
    Raylib.EndDrawing();
}
static void StartMenu(Dictionary<string, Rectangle> startMenuButtons, ref bool showStartMenu, ref bool showDifficultyOptions, ref int difficulty, ref bool showInfo, ref bool changeDifficulty)
{
    Raylib.ClearBackground(Color.Black);
    if (showDifficultyOptions)
    {
        PrintDifficultyMenu(startMenuButtons);
    }
    else if (showInfo)
    {
        PrintInfoMenu();
    }
    else
    {
        PrintStartMenu(startMenuButtons);
    }
    if (Raylib.IsMouseButtonPressed(MouseButton.Left))
    {
        MouseButtonChecker(startMenuButtons, ref showStartMenu, ref showDifficultyOptions, ref difficulty, ref showInfo, ref changeDifficulty);
    }
}
static void PrintInfoMenu()
{
    Raylib.DrawCircle(52, 25, 6, Color.Red);
    Raylib.DrawText("Fiendrna: du får pengar om du dödar dem.", 80, 15, 20, Color.White);
    Raylib.DrawRectangle(40, 50, 25, 25, Color.Green);
    Raylib.DrawText("Blank, här kan du plasera torn genom vänster klick.", 80, 55, 20, Color.White);
    Raylib.DrawRectangle(40, 100, 25, 25, Color.Brown);
    Raylib.DrawText("Path, visar hur fienderna kommer åka.", 80, 105, 20, Color.White);
    Raylib.DrawRectangle(40, 150, 25, 25, Color.Red);
    Raylib.DrawText("Torn, gör skada på fiender inom en viss radie.", 80, 155, 20, Color.White);
    Raylib.DrawText("Om fienderna kommer till pathens ände så förlorar du liv.", 35, 190, 20, Color.White);
    Raylib.DrawText("kostar $50 att säta ut ett torn du får $1 per fiende dödad.", 35, 215, 20, Color.White);
    Raylib.DrawText("Spelet är cell baserat men radien för tornen är kordiant baserart.", 35, 240, 20, Color.White);

}
static void PrintDifficultyMenu(Dictionary<string, Rectangle> startMenuButtons)
{
    Raylib.DrawRectangleRec(startMenuButtons["easy"], Color.White);
    Raylib.DrawRectangleRec(startMenuButtons["medium"], Color.White);
    Raylib.DrawRectangleRec(startMenuButtons["hard"], Color.White);
    Raylib.DrawText("Easy", 300, 75, 40, Color.Black);
    Raylib.DrawText("Medium", 300, 225, 40, Color.Black);
    Raylib.DrawText("Hard", 300, 375, 40, Color.Black);
}
static void PrintStartMenu(Dictionary<string, Rectangle> startMenuButtons)
{
    Raylib.DrawRectangleRec(startMenuButtons["start"], Color.White);
    Raylib.DrawRectangleRec(startMenuButtons["changeMode"], Color.White);
    Raylib.DrawRectangleRec(startMenuButtons["info"], Color.White);
    Raylib.DrawText("Välkomen till ett bra TD spel", 250, 50, 20, Color.White);
    Raylib.DrawText("Start", 85, 275, 40, Color.Black);
    Raylib.DrawText("Info", 355, 150, 40, Color.Black);
    Raylib.DrawText("Difficulty", 560, 275, 40, Color.Black);
}
static void MouseButtonChecker(Dictionary<string, Rectangle> startMenuButtons, ref bool showStartMenu, ref bool showDifficultyOptions, ref int difficulty, ref bool showInfo, ref bool changeDifficulty)
{
    foreach (var item in startMenuButtons)
    {
        if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), item.Value))
        {
            if (!showDifficultyOptions)
                switch (item.Key)
                {
                    case "start":
                        showStartMenu = false;
                        break;
                    case "changeMode":
                        showDifficultyOptions = true;
                        break;
                    case "info":
                        showInfo = true;
                        break;
                }
            else if (showDifficultyOptions)
            {
                switch (item.Key)
                {
                    case "easy":
                        difficulty = 0;
                        changeDifficulty = true;
                        showDifficultyOptions = false;
                        break;
                    case "medium":
                        difficulty = 1;
                        changeDifficulty = true;
                        showDifficultyOptions = false;
                        break;
                    case "hard":
                        difficulty = 2;
                        changeDifficulty = true;
                        showDifficultyOptions = false;
                        break;
                }

            }
        }

    }
}
static void PathChanger(bool changeDifficulty, List<List<(int, int)>> path, List<CellInfoClass> cellInfoList, int difficulty)
{
    if (changeDifficulty)
    {
        for (int i = 0; i < path[difficulty].Count - 1; i++)
        {
            int tempCellNumber = CordToCellNumberConverter(path[difficulty][i].Item1, path[difficulty][i].Item2, 50, cellInfoList);
            cellInfoList[tempCellNumber].CellColor = Color.Brown;
        }
    }
}
static void CellPainter(List<CellInfoClass> cellInfoList, int screenWidth, int screenHeight, int cellSize, int cellNumber, bool firstTime)
{
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
}
static void EnemyController(List<BasicEnemyClass> basicEnemy, List<(int, int)> path)
{
    for (int i = 0; i < basicEnemy.Count; i++)
    {
        if (!basicEnemy[i].Enemymover(path))
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
                return cellInfoList[i].CellNumber;
            }
        }
    }
    return 0;
}
static (float, float) CellNumberToCordConverter(List<CellInfoClass> cellInfoList, int cellNumber)
{
    return (cellInfoList[cellNumber].X, cellInfoList[cellNumber].Y);
}
static void BlockHigheLighter(List<CellInfoClass> cellInfoList, ref int oldMouseCell, int wichCellMouseOn)
{
    if (oldMouseCell != wichCellMouseOn)
    {
        cellInfoList[wichCellMouseOn].CellColor = Raylib.ColorAlpha(cellInfoList[wichCellMouseOn].CellColor, 0.5f);
        cellInfoList[oldMouseCell].CellColor = Raylib.ColorAlpha(cellInfoList[oldMouseCell].CellColor, 1);
        oldMouseCell = wichCellMouseOn;
    }

}
static void ClickChecker(List<CellInfoClass> cellInfoList, int wichCellMouseOn, List<TowerStats> towerStatsList, List<(int, int)> path)
{
    bool haveMouseBenPressed;
    haveMouseBenPressed = Raylib.IsMouseButtonPressed(MouseButton.Left);
    if (haveMouseBenPressed)
    {
        TowerPlacer(cellInfoList, wichCellMouseOn, path, towerStatsList);
    }
}
static void TowerPlacer(List<CellInfoClass> cellInfoList, int whichCellMouseOn, List<(int, int)> path, List<TowerStats> towerStatsList)
{
    if (PlaceIsOcupied(cellInfoList, whichCellMouseOn, path, towerStatsList) == false)
    {
        cellInfoList[whichCellMouseOn].CellColor = Color.Red;
        towerStatsList.Add(new TowerStats(new Vector2(CellNumberToCordConverter(cellInfoList, whichCellMouseOn).Item1, CellNumberToCordConverter(cellInfoList, whichCellMouseOn).Item2), 400, 10, 20));
    }
}
static bool PlaceIsOcupied(List<CellInfoClass> cellInfoList, int wichCellMouseOn, List<(int, int)> path, List<TowerStats> towerStatsList)
{
    for (int i = 0; i < towerStatsList.Count; i++)
    {
        if (wichCellMouseOn == CordToCellNumberConverter((int)towerStatsList[i].Pos.X, (int)towerStatsList[i].Pos.Y, 50, cellInfoList))
        {
            return true;
        }
    }
    for (int i = 0; i < path.Count; i++)
    {
        if (wichCellMouseOn == CordToCellNumberConverter(path[i].Item1, path[i].Item2, 50, cellInfoList))
        {
            return true;
        }
    }
    return false;
}
static void WaveMaker(int waveCount, List<BasicEnemyClass> basicEnemy)
{
    int enemeis = 10 * (int)Math.Pow(1.2, waveCount);
    for (int i = 0; i < enemeis; i++)
    {

    }
}

Console.ReadLine();

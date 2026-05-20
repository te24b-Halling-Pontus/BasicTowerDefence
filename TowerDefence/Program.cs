using Raylib_cs;
using CellInfo;
using BasicEnemy;
using System.Numerics;
using Tower;
//skärm relaterade datatyper
bool firstTime = true; //om dt är görsta gången
int screenWidth = 800;
int screenHeight = 500;
//cell relaterade datatyper
int cellSize = 50;
int cellNumber = -1;
List<CellInfoClass> cellInfoList = []; //har information om celler 
List<List<(int, int)>> path = []; // en lista med listor så man kan med int difficult välja vilken bana
List<(int, int)> pathMedium = [(0, 100), (50, 100), (100, 100), (150, 100), (150, 150), (150, 200), (200, 200), (250, 200), (300, 200), (350, 200), (400, 200), (450, 200), (500, 200), (500, 150), (500, 100), (500, 50), (550, 50), (600, 50), (650, 50), (650, 100), (650, 150), (650, 200), (650, 250), (650, 300), (650, 350), (700, 350), (750, 350), (800, 350)];
List<(int, int)> pathEsay = [(0, 150), (50, 150), (100, 150), (100, 200), (100, 250), (100, 300), (100, 350), (150, 350), (200, 350), (200, 300), (200, 250), (200, 200), (200, 150), (200, 100), (250, 100), (300, 100), (350, 100), (400, 100), (450, 100), (500, 100), (500, 150), (500, 200), (500, 250), (500, 300), (500, 350), (550, 350), (600, 350), (650, 350), (700, 350), (700, 300), (700, 250), (700, 200), (700, 150), (700, 100), (700, 50), (700, 0), (700, -50)];
List<(int, int)> pathHard = [(0, 200), (50, 200), (100, 200), (150, 200), (200, 200), (250, 200), (300, 200), (350, 200), (400, 200), (450, 200), (500, 200), (550, 200), (600, 200), (650, 200), (700, 200), (750, 200), (800, 200)];
path.Add(pathEsay);
path.Add(pathMedium);
path.Add(pathHard);
//mus relaterade datatyper
int wichCellMouseOn; //vilken cell musen är på
int oldMouseCell = 0;
int mousePosY;
int mousePosX;
//fiende grejor
List<BasicEnemyClass> basicEnemy = []; //valde lista för då kan man nå med ett index plus att man kan ta bort och lägga till index värden
int health = 100;
int totalEnemyCount = 0;
//torn relaterade datatyper
List<TowerStats> towerStatsList = []; //jag valde en lista för jag vill att den ska kuna nå med ett index och att man kan läga till mer index platser.
int money = 150;
//startskärm relaterade datatyper
bool showStartMenu = true;
Dictionary<string, Rectangle> startMenuButtons = new Dictionary<string, Rectangle>(); //alla knappar
startMenuButtons.Add("start", new Rectangle(50, 250, 200, 100));
startMenuButtons.Add("changeMode", new Rectangle(550, 250, 200, 100));
startMenuButtons.Add("info", new Rectangle(300, 125, 200, 100));
startMenuButtons.Add("easy", new Rectangle(275, 50, 200, 100));
startMenuButtons.Add("medium", new Rectangle(275, 200, 200, 100));
startMenuButtons.Add("hard", new Rectangle(275, 350, 200, 100));
bool showDifficultyOptions = false; //alla de här tre hanterar vad som ska visas i menun
bool showInfo = false;
bool changeDifficulty = true;
int difficulty = 1; //hanterar vilken bana det är

int temp = 0; //hanterar när enemis ska spawna
Raylib.InitWindow(screenWidth, screenHeight, "Roligt TD spel");
Raylib.SetTargetFPS(60); //seter max FPS
while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();
    if (showStartMenu) //resetar många värden så man t.ex inte startar med mycket pengar eller inget HP
    {
        StartMenu(startMenuButtons, ref showStartMenu, ref showDifficultyOptions, ref difficulty, ref showInfo, ref changeDifficulty, ref money, ref health, ref totalEnemyCount); //vissar start menyn samt resetar vissa värden
        health = 100; // gör bara så när man dör att hp blir 100
        towerStatsList.Clear(); //alla tre clerar alla listor som det blir som en ny start
        basicEnemy.Clear();
        cellInfoList.Clear();
        firstTime = true; // gör så det resetas samma med resten
        changeDifficulty = true;
        cellNumber = -1;

    }
    else
    {
        Raylib.ClearBackground(Color.White); // gör så backrunden blir vit
        mousePosX = Raylib.GetMouseX(); //får mus infromationen
        mousePosY = Raylib.GetMouseY();
        wichCellMouseOn = CordToCellNumberConverter(mousePosX, mousePosY, 50, cellInfoList); // får vilken cell musen är på, vilket möjlirör så man kan setta ut torn smat ser en highlight på den cellen man är på
        CellPainter(cellInfoList, screenWidth, screenHeight, cellSize, cellNumber, firstTime); // ritar ut cellerna
        BlockHigheLighter(cellInfoList, ref oldMouseCell, wichCellMouseOn);// gör så man får en veta vilken ruta man är över (den blir lite ljusare)
        PathChanger(changeDifficulty, path, cellInfoList, difficulty); //ändrar färgen på till brun så man ser vart vägen går 
        TowerController(basicEnemy, towerStatsList); //gör så tornet sjuter
        EnemyController(basicEnemy, path[difficulty], ref money, ref health); // hanterar fiender
        cellNumber = 0; 
        firstTime = false;
        ClickChecker(cellInfoList, wichCellMouseOn, towerStatsList, path[difficulty], ref money); //hanterar hur man säter ut torn
        Raylib.DrawText("$" + money, 700, 10, 25, Color.Black); // visar hur mycket pengar man har
        Raylib.DrawText("HP: " + health, 600, 10, 25, Color.Black); //visar HP;et
        if (health <= 0) { showStartMenu = true; } //gör så man kan dö
        temp++;
        if (temp == 10) //hanterar hur enemys spawnar
        {
            totalEnemyCount += (int)EnemyMaker(totalEnemyCount, basicEnemy, path, difficulty);
            temp = 0;
        }
    }
    Raylib.EndDrawing();
}
static void StartMenu(Dictionary<string, Rectangle> startMenuButtons, ref bool showStartMenu, ref bool showDifficultyOptions, ref int difficulty, ref bool showInfo, ref bool changeDifficulty, ref int money, ref int health, ref int totalEnemyCount) //här kan användaren ändra svårighets grad, se lite hur seplet fungerar och starta själva seplet
{
    money = 100;
    totalEnemyCount = 0;
    Raylib.ClearBackground(Color.Black);
    if (showDifficultyOptions)
    {
        PrintDifficultyMenu(startMenuButtons);
    }
    else if (showInfo)
    {
        showInfo = PrintInfoMenu();
    }
    else
    {
        PrintStartMenu(startMenuButtons);
    }
    if (Raylib.IsMouseButtonPressed(MouseButton.Left)) //kollar om vänster kanppen är tryckt
    {
        MouseButtonChecker(startMenuButtons, ref showStartMenu, ref showDifficultyOptions, ref difficulty, ref showInfo, ref changeDifficulty); //kollar vilken knapp som trycks
    }
}


static bool PrintInfoMenu()//printar bara lite infromation om hur seplat funkar
{
    Raylib.DrawText("[Enter] för att lämna", 530, 10, 25, Color.Red);
    Raylib.DrawCircle(52, 25, 6, Color.Red);
    Raylib.DrawText("Fiendrna: du får pengar om du dödar dem.", 80, 15, 20, Color.White);
    Raylib.DrawRectangle(40, 50, 25, 25, Color.Green);
    Raylib.DrawText("Blank, här kan du plasera torn genom vänster klick.", 80, 55, 20, Color.White);
    Raylib.DrawRectangle(40, 100, 25, 25, Color.Brown);
    Raylib.DrawText("Path, visar hur fienderna kommer åka.", 80, 105, 20, Color.White);
    Raylib.DrawRectangle(40, 150, 25, 25, Color.Red);
    Raylib.DrawText("Torn, gör skada på fiender inom en viss radie.", 80, 155, 20, Color.White);
    Raylib.DrawText("Om fienderna kommer till pathens ände så förlorar du liv.", 35, 190, 20, Color.White);
    Raylib.DrawText("kostar $100 att säta ut ett torn du får $1 per fiende dödad.", 35, 215, 20, Color.White);
    Raylib.DrawText("Spelet är cell baserat men radien för tornen är kordiant baserart.", 35, 240, 20, Color.White);
    if (Raylib.IsKeyPressed(KeyboardKey.Enter)) //escape fungerar inte :(
    {
        return false; //går till backa
    }
    return true;
}
static void PrintDifficultyMenu(Dictionary<string, Rectangle> startMenuButtons)//vissar Difficulty menyn, där man med hjälp av MouseButtonChecker() kan göra så man väljer svårighetsgrad
{
    Raylib.DrawRectangleRec(startMenuButtons["easy"], Color.White);
    Raylib.DrawRectangleRec(startMenuButtons["medium"], Color.White);
    Raylib.DrawRectangleRec(startMenuButtons["hard"], Color.White);
    Raylib.DrawText("Easy", 300, 75, 40, Color.Black);
    Raylib.DrawText("Medium", 300, 225, 40, Color.Black);
    Raylib.DrawText("Hard", 300, 375, 40, Color.Black);
}
static void PrintStartMenu(Dictionary<string, Rectangle> startMenuButtons) //printar vanliga start menyn där man trycker man kan trycka på på kanppar för att starta spelet, ändra svårighets graden samt se lite information om spelet
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
        if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), item.Value)) //kollar vilken knapp som musen koliderar med samt gört så man kan få ut nyckens värde, vilket gör så man kan navigera i de olika menyerna, den ändrar även dificultin om man trycker på hard, easy eller medium vilket ändrar vilken bana man sen kör på när man trycker på start
        {
            if (!showDifficultyOptions && !showInfo) //kollar så den bara funkar när rätt meny är uppe
                switch (item.Key) //nycken värde
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
            else if (showDifficultyOptions && !showInfo) //-II-
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
    if (changeDifficulty) //kollar om dificulen har ändras 
    {
        for (int i = 0; i < path[difficulty].Count - 1; i++)//Repiterar lika många gånger som listor som finns
        {
            cellInfoList[CordToCellNumberConverter(path[difficulty][i].Item1, path[difficulty][i].Item2, 50, cellInfoList)].CellColor = Color.Brown; //ändrar färg på genom att få ut cell numret
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
                cellInfoList.Add(new CellInfoClass(x, y, cellNumber, Color.Green)); //läger in cellern i cell listan
            }
            else
            {
                Raylib.DrawRectangle(x, y, cellSize, cellSize, cellInfoList[cellNumber - 1].CellColor); //rittar ruttorna
            }
        }
    }
}
static void EnemyController(List<BasicEnemyClass> basicEnemy, List<(int, int)> path, ref int money, ref int health)
{
    for (int i = 0; i < basicEnemy.Count; i++)
    {
        (bool, string) temp = basicEnemy[i].EnemyMover(path);
        if (!temp.Item1) //kollar om fienden ska dö
        {
            basicEnemy.RemoveAt(i); //tar bort fienden
            i--; //gör så inte listan bli index out off range pga att en tas bort
            if (temp.Item2 == "killed") //kollar orsaken av döden
            {
                money++;//ger pengar om tornen döda
            }
            else
            {
                health--; // förlorar liv om den går till sita path tilen
            }
        }
    }
}
static void TowerController(List<BasicEnemyClass> basicEnemy, List<TowerStats> towerStatsList) 
{
    for (int i = 0; i < towerStatsList.Count; i++) //går igenom alla torn och gör så de sjuter
    {
        towerStatsList[i].TowerShooter(basicEnemy);
    }
}
static int CordToCellNumberConverter(int xCord, int yCord, int cellSize, List<CellInfoClass> cellInfoList) //tar in kordinaterna och spotar ut cell numret, vilket används t.ex när man har musens x och y cordinater men vill ha cell numret istället som när man skapar torn
{
    for (int i = 0; i < cellInfoList.Count; i++)
    {
        if (cellInfoList[i].X <= xCord && cellInfoList[i].X + cellSize > xCord) //kollar vilken cell som x kordnaten är i
        {
            if (cellInfoList[i].Y <= yCord && cellInfoList[i].Y + cellSize > yCord) //kollar vilken cell som x kordnaten är i
            {
                return cellInfoList[i].CellNumber;
            }
        }
    }
    return 0;
}
static (float, float) CellNumberToCordConverter(List<CellInfoClass> cellInfoList, int cellNumber) //tar in cell numret och spotar ut kordinaterna, används när man har cell numret men vill ha x och y kordinaterna används.
{                                                                                                 //t.ex för torn för efftersom rangen som tornen har är i kordinater så behöver den inte i cell numer så behöver den utgå från en plats
    return (cellInfoList[cellNumber].X, cellInfoList[cellNumber].Y); //effter som cellInfoList har alla kordinaterna så är det bara att ta ut dem
}
static void BlockHigheLighter(List<CellInfoClass> cellInfoList, ref int oldMouseCell, int wichCellMouseOn)
{
    if (oldMouseCell != wichCellMouseOn) //kollar om musen har byt cell
    {
        cellInfoList[wichCellMouseOn].CellColor = Raylib.ColorAlpha(cellInfoList[wichCellMouseOn].CellColor, 0.5f); //gör så cellen som musen är på blir mer transparant vilket gör den ljusare
        cellInfoList[oldMouseCell].CellColor = Raylib.ColorAlpha(cellInfoList[oldMouseCell].CellColor, 1); //gör så gamla cellen blir orginal färgen
        oldMouseCell = wichCellMouseOn;
    }

}
//registrerar när du klickar på vänster klick, vilket senare instansierar ett torn i towerStatsList vilket, senare kan sjuta ner fienderna
static void ClickChecker(List<CellInfoClass> cellInfoList, int wichCellMouseOn, List<TowerStats> towerStatsList, List<(int, int)> path, ref int money)
{
    if (Raylib.IsMouseButtonPressed(MouseButton.Left))
    {
        TowerPlacer(cellInfoList, wichCellMouseOn, path, towerStatsList, ref money);
    }
}
//instansierar torn i torn som senare kan sjuta på fiender
static void TowerPlacer(List<CellInfoClass> cellInfoList, int whichCellMouseOn, List<(int, int)> path, List<TowerStats> towerStatsList, ref int money)
{
    if (PlaceIsOcupied(cellInfoList, whichCellMouseOn, path, towerStatsList) == false && money >= 100) //kollar om tilen är uptagen och att du har alla pengar
    {
        money -= 100; // drar pengarna 
        cellInfoList[whichCellMouseOn].CellColor = Color.Red; //ändrar färgen till röd
        towerStatsList.Add(new TowerStats(new Vector2(CellNumberToCordConverter(cellInfoList, whichCellMouseOn).Item1, CellNumberToCordConverter(cellInfoList, whichCellMouseOn).Item2), 400, 10, 10)); //intansierar tornet
    }
}
//kollor om cellen är uptagen av en path tile eller om det redan är ett torn där. Om det är något på den tilen skickar den tillbacka true medans om tilen är fri sickar den true vilket gör så ett torn placeras ut (om du har 100$).
static bool PlaceIsOcupied(List<CellInfoClass> cellInfoList, int wichCellMouseOn, List<(int, int)> path, List<TowerStats> towerStatsList) 
{
    for (int i = 0; i < towerStatsList.Count; i++)
    {
        if (wichCellMouseOn == CordToCellNumberConverter((int)towerStatsList[i].Pos.X, (int)towerStatsList[i].Pos.Y, 50, cellInfoList)) //kollar om det är ett torn på den cellen
        {
            return true;
        }
    }
    for (int i = 0; i < path.Count; i++)
    {
        if (wichCellMouseOn == CordToCellNumberConverter(path[i].Item1, path[i].Item2, 50, cellInfoList)) // kollar om det är på pathen
        {
            return true;
        }
    }
    return false;
}static int EnemyMaker(int totalEnemyCount, List<BasicEnemyClass> basicEnemy, List<List<(int, int)>> path, int difficulty) //skaper nya enemys och ger tillbaka hur många den har skapat så effter ett tag kan de spwna mer beroende på hur mycket den totala enemy skapande har varit
{
    for (int i = 0; i < (int)totalEnemyCount / 150 + 1; i++) //kontrollerar hur manga fiender det ska skapas
    {
        basicEnemy.Add(new BasicEnemyClass(100, 1000, new Vector2(path[difficulty][0].Item1 - 25, path[difficulty][0].Item2 + 25))); //instaniserar fienden
    }
    return totalEnemyCount / 150 + 1; // skickar tillbacka hur många fiender som skapades
}




List<(int x, int y)> path = new List<(int x, int y)>
{
    (0,3), (1,3), (2,3),
    (3,3), (3,4), (4,4),
    (5,4), (5,5), (5,6)
};

int[,] board = new int[Math.Max(path.x), 6];


for (int y = 0; y < board.GetLength(1); y++)
{
    for (int x = 0; x < board.GetLength(0); x++)
    {
        if (path.Contains((x, y)))
        {
            Console.BackgroundColor = ConsoleColor.Yellow;
        }
        else
        {
            Console.BackgroundColor = ConsoleColor.Blue;
        }
        Console.Write(" ");
    }
    Console.WriteLine();
}
Console.ReadLine();
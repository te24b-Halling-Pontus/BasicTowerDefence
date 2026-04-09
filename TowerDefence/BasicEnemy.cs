using System.Numerics;
using Raylib_cs;
namespace BasicEnemy;



class BasicEnemyClass
{
    public int Health;
    public int Speed;
    int PathPos = 0;
    Vector2 Pos;
    public BasicEnemyClass(int Health, int Speed, Vector2 Pos)
    {
        this.Health = Health;
        this.Speed = Speed;
        this.Pos = Pos;
    }


    public void Test(List<(int, int)> path)
    {
        Vector2 nextPos = new Vector2(path[PathPos].Item1, path[PathPos].Item2);
        Vector2 diretion = nextPos - Pos;
        if (diretion.Length() > 1f)
        {
            diretion = Vector2.Normalize(diretion);
            Pos = diretion * Speed / 60;
        }
        Raylib.DrawCircleV(Pos, 25, Color.Red);
        Console.WriteLine("funkar ish");
    }
}




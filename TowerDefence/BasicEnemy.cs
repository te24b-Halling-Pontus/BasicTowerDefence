namespace BasicEnemy;

using System.Numerics;
using Raylib_cs;


class BasicEnemyClass
{
    public int Health;
    public int Speed;
    public int PathPos = 0;
    public Vector2 Pos;
    public BasicEnemyClass(int health, int speed, Vector2 pos)
    {
        this.Health = health;
        this.Speed = speed;
        this.Pos = pos;
    }

    public (bool ,string) Enemymover(List<(int, int)> path)
    {
        Vector2 nextPos = new Vector2(path[PathPos].Item1 + 25, path[PathPos].Item2 + 25); //+25 för då är den i miten av kvadraten
        Vector2 diretion = nextPos - Pos;
        if (diretion.Length() > 1f) // kollar så fienden är mer en en kordinat ifrån målet.
        {
            diretion = Vector2.Normalize(diretion);
            Pos += diretion * Speed / 60; // 60 är lite onödigt men jag tänkte att man skulle dela på framsen men man kan ju också bara säka speeden.
        }
        else //nyt mål
        {
            Pos = nextPos;
            PathPos++;
        }
        Raylib.DrawCircleV(Pos, 25, Color.Red);
        return EnemyKiller(path, PathPos, Health);
    }
    static (bool, string) EnemyKiller(List<(int, int)> path, int PathPos, int Health)
    {
        if (PathPos + 1 == path.Count) // gör så den försviner vid slutet och när den blir sjuten ger även hur den dog
        {
            string deathBy = "End of path"; 
            return (false, deathBy);
        }
        else if (Health <= 0)
        {
            string deathBy = "killed";
            return (false, deathBy);
        }
        return (true, "");
    }
}
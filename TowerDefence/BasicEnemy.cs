namespace BasicEnemy;
using System.Numerics;
using Raylib_cs;


class BasicEnemyClass
{
    public bool IsAlive = true;
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

    public bool Enemymover(List<(int, int)> path)
    {
        if (PathPos + 1 == path.Count) // gör så den försviner vid slutet
        {
            IsAlive = false;
            return (IsAlive);
        }
        else if (Health <= 0)
        {
            IsAlive = false;
            return (IsAlive);
        }
        else
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
        }
        return (IsAlive);
    }
}
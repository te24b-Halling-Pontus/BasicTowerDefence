namespace Tower;

using System.Numerics;
using CellInfo;
class TowerStats
{
    public Vector2 Pos;
    public int Range;
    public int Damage;
    public TowerStats(Vector2 Pos, int Range, int Damage)
    {
        this.Pos = Pos;
        this.Range = Range;
        this.Damage = Damage;
    }

    public void TowerShoter()
    {
        
    }
}
namespace Tower;

using System.Numerics;
using CellInfo;
using BasicEnemy;

class TowerStats
{
    public Vector2 Pos;
    public int Range;
    public int Damage;
    public int hitspeed; // hit speed är beroende på frames 
    int EnemyNumber = 0;
    List<int> PosilbleTragets = [];

    public TowerStats(Vector2 Pos, int Range, int Damage)
    {
        this.Pos = Pos;
        this.Range = Range;
        this.Damage = Damage;
    }

    public void TowerShoter(List<BasicEnemyClass> basicEnemy)
    {
        foreach (var enemy in basicEnemy)
        {
            float distanceBetwen = Vector2.Distance(Pos, enemy.Pos);
            if (distanceBetwen < Range)
            {
                PosilbleTragets.Add(EnemyNumber);
            }
            EnemyNumber++;
        }
    }
    static int WhoIsFirst()
    {
        
        return 1;
    }
}
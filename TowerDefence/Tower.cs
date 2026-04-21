namespace Tower;

using System.Numerics;
using CellInfo;
using BasicEnemy;

class TowerStats
{
    public Vector2 Pos;
    public int Range;
    public int Damage;
    public int Hitspeed; // hit speed är beroende på frames 
    int enemyNumber = 0;
    int target;
    List<int> posilbleTragets = [];

    public TowerStats(Vector2 pos, int range, int damage, int hitspeed)
    {
        this.Pos = pos;
        this.Range = range;
        this.Damage = damage;
        this.Hitspeed = hitspeed;
    }

    public void TowerShoter(List<BasicEnemyClass> basicEnemy)
    {
        foreach (var enemy in basicEnemy)
        {
            float distanceBetwen = Vector2.Distance(Pos, enemy.Pos); // kollar distansen mellan dem
            if (distanceBetwen < Range) // kollar om den är inom range
            {
                posilbleTragets.Add(enemyNumber);
            }
            enemyNumber++;
        }
        target = WhoIsFirst(basicEnemy, posilbleTragets);
        basicEnemy[target].IsAlive = false;
    }
    int WhoIsFirst(List<BasicEnemyClass> basicEnemy, List<int> PosilbleTragets) // försöker kolla vilken fiende som är först
    {
        int maxTemp = 0;
        int temp;
        for (int i = 0; i < PosilbleTragets.Count; i++) // loopar genom och kollar vilken som är först
        {
            temp = basicEnemy[PosilbleTragets[i]].PathPos; 
            if (maxTemp < temp)
            {
                target = PosilbleTragets[i]; 
                maxTemp = temp;
            }
        }
        return target;
    }
}
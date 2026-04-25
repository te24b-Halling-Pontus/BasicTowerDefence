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
        int enemyNumber = 0;
        posilbleTragets.Clear();
        bool targetInRange = false;
        foreach (var enemy in basicEnemy)
        {
            float distanceBetwen = Vector2.Distance(Pos, enemy.Pos); // kollar distansen mellan dem
            if (distanceBetwen <= Range) // kollar om den är inom range
            {
                posilbleTragets.Add(enemyNumber);
                targetInRange = true;
            }
            enemyNumber++;
        }
        if (targetInRange)
        {
            target = WhoIsFirst(basicEnemy, posilbleTragets);
            basicEnemy[target].Health -= Damage;
        }
    }
    int WhoIsFirst(List<BasicEnemyClass> basicEnemy, List<int> PosilbleTragets) // försöker kolla vilken fiende som är först
    {
        int maxTemp = 0;
        int temp;
        if (posilbleTragets.Count > 0)
        {
            for (int i = 0; i <= PosilbleTragets.Count - 1; i++) // loopar genom och kollar vilken som är först
            {
                temp = basicEnemy[PosilbleTragets[i]].PathPos;
                if (maxTemp < temp)
                {
                    target = PosilbleTragets[i];
                    maxTemp = temp;
                }
            }
        }
        return target;
    }
}
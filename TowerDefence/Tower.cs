namespace Tower;

using System.Numerics;
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

    public void TowerShoter(List<BasicEnemyClass> basicEnemy) //gör så tornen sjuter
    {
        int enemyNumber = 0;
        posilbleTragets.Clear(); //tar bort alla värden i posibleTargets
        bool targetInRange = false; //omställer så den inte förblir true 
        foreach (var enemy in basicEnemy)
        {
            float distanceBetwen = Vector2.Distance(Pos, enemy.Pos); // kollar distansen mellan dem
            if (distanceBetwen <= Range) // kollar om den är inom range
            {
                posilbleTragets.Add(enemyNumber);// lägger till fienderna som är i range
                targetInRange = true; // säger att det finns targets i range
            }
            enemyNumber++;
        }
        if (targetInRange) //kolar om det är något i range
        {
            target = FirstChecker(basicEnemy, posilbleTragets); //kollar vilken fiende som är först
            basicEnemy[target].Health -= Damage; //gör skada på fienden
        }
    }
    int FirstChecker(List<BasicEnemyClass> basicEnemy, List<int> PosilbleTragets) // försöker kolla vilken fiende som är först
    {
        int maxTemp = 0;
        if (posilbleTragets.Count > 0)
        {
            for (int i = 0; i < PosilbleTragets.Count; i++) // loopar genom och kollar vilken som är först
            {
                if (maxTemp <= basicEnemy[PosilbleTragets[i]].PathPos)
                {
                    target = PosilbleTragets[i];
                    maxTemp = basicEnemy[PosilbleTragets[i]].PathPos;
                }
            }
        }
        return target;
    }
}
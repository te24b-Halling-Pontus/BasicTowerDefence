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

    public TowerStats(Vector2 pos, int range, int damage, int hitspeed)// ändar variablerna till de som säts in när den instansieras
    {
        this.Pos = pos;
        this.Range = range;
        this.Damage = damage;
        this.Hitspeed = hitspeed;
    }

    //gör så tornen sjuter och dödar fiender genom att dra ner hp hos fiender till noll vilket sen fienden skickar till program.cs som sen dödar den genom att ta bort från listan
    public void TowerShooter(List<BasicEnemyClass> basicEnemy)
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
    //skickar till TowerShoter vem som är först som är inom range så den sen kan göra skada den
    int FirstChecker(List<BasicEnemyClass> basicEnemy, List<int> posilbleTragets) // försöker kolla vilken fiende som är först
    {
        int maxTemp = 0;
        if (posilbleTragets.Count > 0)
        {
            for (int i = 0; i < posilbleTragets.Count; i++) // loopar genom och kollar vilken som är först
            {
                if (maxTemp <= basicEnemy[posilbleTragets[i]].PathPos)
                {
                    target = posilbleTragets[i];
                    maxTemp = basicEnemy[posilbleTragets[i]].PathPos;
                }
            }
        }
        return target;
    }
}
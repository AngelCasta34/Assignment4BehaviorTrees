using System.Linq;
using UnityEngine;


public class CountAlliesQuery : BehaviorTree
{
    private int minCount;
    private float range;

    public CountAlliesQuery(int minCount, float range) : base()
    {
        this.minCount = minCount;
        this.range = range;
    }

    public override Result Run()
    {
        int nearby = GameManager.Instance
                         .GetEnemiesInRange(agent.transform.position, range)
                         .Where(go => go != agent.gameObject)
                         .Count();
        return nearby >= minCount ? Result.SUCCESS : Result.FAILURE;
    }

    public override BehaviorTree Copy()
    {
        return new CountAlliesQuery(minCount, range);
    }
}

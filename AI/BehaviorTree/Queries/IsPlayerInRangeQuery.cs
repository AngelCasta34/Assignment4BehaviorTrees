using UnityEngine;

public class IsPlayerInRangeQuery : BehaviorTree
{
    private float range;
    public IsPlayerInRangeQuery(float range) : base()
    {
        this.range = range;
    }

    public override Result Run()
    {
        float dist = Vector3.Distance(agent.transform.position,
                                      GameManager.Instance.player.transform.position);
        return dist <= range ? Result.SUCCESS : Result.FAILURE;
    }

    public override BehaviorTree Copy()
    {
        return new IsPlayerInRangeQuery(range);
    }
}

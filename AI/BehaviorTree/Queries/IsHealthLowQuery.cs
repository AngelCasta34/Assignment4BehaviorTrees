using UnityEngine;

public class IsHealthLowQuery : BehaviorTree
{
    private float threshold;
    public IsHealthLowQuery(float threshold) => this.threshold = threshold;

    public override Result Run()
    {
        var h = agent.hp;
        float frac = h.hp / (float)h.max_hp;
        return frac < threshold
            ? Result.SUCCESS
            : Result.FAILURE;
    }

    public override BehaviorTree Copy()
    {
        return new IsHealthLowQuery(threshold);
    }
}

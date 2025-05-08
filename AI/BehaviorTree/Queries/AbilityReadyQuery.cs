using UnityEngine;

public class AbilityReadyQuery : BehaviorTree
{
    private string ability;

    public AbilityReadyQuery(string ability) : base()
    {
        this.ability = ability;
    }

    public override Result Run()
    {
        var act = agent.GetAction(ability);
        if (act == null)
        {
            return Result.FAILURE;
        }
        return act.Ready() ? Result.SUCCESS : Result.FAILURE;
    }

    public override BehaviorTree Copy()
    {
        return new AbilityReadyQuery(ability);
    }
}

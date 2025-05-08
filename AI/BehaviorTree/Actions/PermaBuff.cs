// Assets/Scripts/AI/BehaviorTree/Actions/PermaBuff.cs
using UnityEngine;

public class PermaBuff : BehaviorTree
{
    public override Result Run()
    {
        var targetGO = GameManager.Instance.GetClosestOtherEnemy(agent.gameObject);
        if (targetGO == null)
            return Result.FAILURE;

        EnemyAction act = agent.GetAction("permabuff");
        if (act == null)
            return Result.FAILURE;

        bool success = act.Do(targetGO.transform);
        return success ? Result.SUCCESS : Result.FAILURE;
    }

    public PermaBuff() : base() { }

    public override BehaviorTree Copy()
    {
        return new PermaBuff();
    }
}

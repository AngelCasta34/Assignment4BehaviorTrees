// Assets/Scripts/AI/BehaviorTree/Actions/Buff.cs
using UnityEngine;

public class Buff : BehaviorTree
{
    public override Result Run()
    {
        // Find the closest other enemy
        var targetGO = GameManager.Instance.GetClosestOtherEnemy(agent.gameObject);
        if (targetGO == null)
            return Result.FAILURE;

        EnemyAction act = agent.GetAction("buff");
        if (act == null)
            return Result.FAILURE;

        bool success = act.Do(targetGO.transform);
        return success ? Result.SUCCESS : Result.FAILURE;
    }

    public Buff() : base() { }

    public override BehaviorTree Copy()
    {
        return new Buff();
    }
}

// Assets/Scripts/AI/BehaviorTree/Actions/Heal.cs
using UnityEngine;

public class Heal : BehaviorTree
{
    public override Result Run()
    {
        var targetGO = GameManager.Instance.GetClosestOtherEnemy(agent.gameObject);
        if (targetGO == null)
            return Result.FAILURE;

        EnemyAction act = agent.GetAction("heal");
        if (act == null)
            return Result.FAILURE;

        bool success = act.Do(targetGO.transform);
        return success ? Result.SUCCESS : Result.FAILURE;
    }

    public Heal() : base() { }

    public override BehaviorTree Copy()
    {
        return new Heal();
    }
}

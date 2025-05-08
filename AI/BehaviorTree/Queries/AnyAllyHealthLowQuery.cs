using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class AnyAllyHealthLowQuery : BehaviorTree
{
    private float threshold;
    public AnyAllyHealthLowQuery(float threshold) => this.threshold = threshold;

    public override Result Run()
    {
        // Use the warlock’s heal range
        float healRange = agent.GetAction("heal").range;

        // Find all other enemies in that radius
        IEnumerable<GameObject> allies = GameManager.Instance
            .GetEnemiesInRange(agent.transform.position, healRange)
            .Where(go => go != agent.gameObject);

        foreach (var go in allies)
        {
            var ctrl = go.GetComponent<EnemyController>();
            var h = ctrl.hp;
            float frac = h.hp / (float)h.max_hp;
            if (frac < threshold)
                return Result.SUCCESS;
        }

        return Result.FAILURE;
    }

    public override BehaviorTree Copy()
    {
        return new AnyAllyHealthLowQuery(threshold);
    }
}

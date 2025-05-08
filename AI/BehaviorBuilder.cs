using UnityEngine;

public class BehaviorBuilder
{
    public static BehaviorTree MakeTree(EnemyController agent)
    {
        BehaviorTree result;

        // support backline
        if (agent.monster == "warlock")
        {
            result = new Selector(
                // Emergency heal if any ally below 50%
                new Sequence(new BehaviorTree[] {
                    new AbilityReadyQuery("heal"),
                    new AnyAllyHealthLowQuery(0.5f),
                    new Heal()
                }),
                // Temporary strength buff
                new Sequence(new BehaviorTree[] {
                    new AbilityReadyQuery("buff"),
                    new Buff()
                }),
                // Permanent strength buff
                new Sequence(new BehaviorTree[] {
                    new AbilityReadyQuery("permaBuff"),
                    new PermaBuff()
                }),
                //  Attack if player in range
                new Sequence(new BehaviorTree[] {
                    new IsPlayerInRangeQuery(agent.GetAction("attack").range),
                    new Attack()
                }),
                // Fallback move to congregation point
                new GoTo(AIWaypointManager.Instance.GetCongregationPoint(), agent.GetAction("attack").range)
            );
        }
        // front line meat shield
        else if (agent.monster == "zombie")
        {
            result = new Selector(
                //  Rush & attack if very close
                new Sequence(new BehaviorTree[] {
                    new IsPlayerInRangeQuery(agent.GetAction("attack").range),
                    new MoveToPlayer(agent.GetAction("attack").range),
                    new Attack()
                }),
                //  Gather if fewer than 4 allies nearby
                new Sequence(new BehaviorTree[] {
                    new CountAlliesQuery(4, 6f),
                    new GoTo(AIWaypointManager.Instance.GetCongregationPoint(), agent.GetAction("attack").range)
                }),
                //  Default advance & attack
                new Sequence(new BehaviorTree[] {
                    new MoveToPlayer(agent.GetAction("attack").range),
                    new Attack()
                })
            );
        }
        // hit and run striker
        else if (agent.monster == "skeleton")
        {
            result = new Selector(
                //  Attack if player is in range
                new Sequence(new BehaviorTree[] {
                    new IsPlayerInRangeQuery(agent.GetAction("attack").range),
                    new Attack()
                }),
                //  Retreat if health is low (<40%)
                new Sequence(new BehaviorTree[] {
                    new IsHealthLowQuery(0.4f),
                    new GoTo(AIWaypointManager.Instance.GetCongregationPoint(), agent.GetAction("attack").range)
                }),
                //  Strike when backed by at least 3 allies
                new Sequence(new BehaviorTree[] {
                    new CountAlliesQuery(3, 5f),
                    new MoveToPlayer(agent.GetAction("attack").range),
                    new Attack()
                }),
                //  Fallback 
                new GoTo(AIWaypointManager.Instance.GetCongregationPoint(), agent.GetAction("attack").range)
            );
        }
        else
        {
            result = new Sequence(new BehaviorTree[] {
                new MoveToPlayer(agent.GetAction("attack").range),
                new Attack()
            });
        }

        // Assign the agent reference to every node
        foreach (var n in result.AllNodes())
        {
            n.SetAgent(agent);
        }

        return result;
    }
}

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
                    new NearbyEnemiesQuery(1, 5f),
                    new AnyAllyHealthLowQuery(0.5f),
                    new Heal()
                }),
                // Temporary strength buff
                new Sequence(new BehaviorTree[] {
                    new AbilityReadyQuery("buff"),
                    new NearbyEnemiesQuery(1, 5f),
                    new Buff()
                }),
                // Permanent strength buff
                new Sequence(new BehaviorTree[] {
                    new AbilityReadyQuery("permaBuff"),
                    new NearbyEnemiesQuery(1, 5f),
                    new PermaBuff()
                }),
                //  Attack if player in range
                new Sequence(new BehaviorTree[] {
                    new IsPlayerInRangeQuery(agent.GetAction("attack").range),
                    new Attack()
                }),
                //Move and attack if there are more than 6 enemies in range
                new Sequence(new BehaviorTree[] {
                    new CountAlliesQuery(6, 10f),
                    new MoveToPlayer(agent.GetAction("attack").range),
                    new Attack()
                }),
                //Group at nearest waypoint if there are less than 9 allies in range
                //new Sequence(new BehaviorTree[]{
                //    new GoTo(AIWaypointManager.Instance.GetClosest())
                //}),
                // Fallback move to congregation point
                new GoTowards(AIWaypointManager.Instance.GetCongregationPoint(), 10, agent.GetAction("attack").range)
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
                //  Attack if 6 or more allies nearby
                new Sequence(new BehaviorTree[] {
                    new CountAlliesQuery(6, 10f),
                    new MoveToPlayer(agent.GetAction("attack").range),
                    new Attack()
                }),
                //  Congregate
                new GoTowards(AIWaypointManager.Instance.GetCongregationPoint(), 10, agent.GetAction("attack").range)
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
                //  Strike when backed by at least 6 allies
                new Sequence(new BehaviorTree[] {
                    new CountAlliesQuery(6, 10f),
                    new MoveToPlayer(agent.GetAction("attack").range),
                    new Attack()
                }),
                //  Retreat if health is low (<40%)
                /*new Sequence(new BehaviorTree[] {
                    new IsHealthLowQuery(0.4f),
                    new GoTo(AIWaypointManager.Instance.GetCongregationPoint(), agent.GetAction("attack").range)
                }),*/
                //  Fallback 
                new GoTowards(AIWaypointManager.Instance.GetCongregationPoint(), 10, agent.GetAction("attack").range)
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

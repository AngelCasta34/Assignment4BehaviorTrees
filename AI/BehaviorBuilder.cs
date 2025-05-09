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
                // Permanent strength buff
                new Sequence(new BehaviorTree[] {
                    new AbilityReadyQuery("permaBuff"),
                    new NearbyEnemiesQuery(1, 5f),
                    new PermaBuff()
                }),
                // Temporary strength buff if player is in range
                new Sequence(new BehaviorTree[] {
                    new IsPlayerInRangeQuery(10f),
                    new AbilityReadyQuery("buff"),
                    new NearbyEnemiesQuery(1, 5f),
                    new Buff()
                }),
                //  Attack if player in range
                new Sequence(new BehaviorTree[] {
                    new IsPlayerInRangeQuery(agent.GetAction("attack").range),
                    new MoveToPlayer(agent.GetAction("attack").range),
                    new Attack()
                }),
                //Move and attack if there are more than 14 enemies in range
                new Sequence(new BehaviorTree[] {
                    new CountAlliesQuery(14, 15f),
                    new MoveToPlayer(agent.GetAction("attack").range),
                    new Attack()
                }),
                //If player gets close and there are 6 or more allies, attack
                new Sequence(new BehaviorTree[] {
                    new IsPlayerInRangeQuery(agent.GetAction("attack").range + 3),
                    new CountAlliesQuery(6, 20f),
                    new MoveToPlayer(agent.GetAction("attack").range),
                    new Attack()
                }),
                //Stay put if there are 4 or more allies near you
                new Sequence(new BehaviorTree[]{
                    new CountAlliesQuery (4, 3f)
                }),
                // Fallback move to congregation point, lowest range to keep most protected
                new GoTowards(AIWaypointManager.Instance.GetCongregationPoint(), 1, 1)
            );
        }
        // front line meat shield
        else if (agent.monster == "zombie")
        {
            result = new Selector(
                //  Rush & attack if very close
                new Sequence(new BehaviorTree[] {
                    new IsPlayerInRangeQuery(agent.GetAction("attack").range + 3),
                    new MoveToPlayer(agent.GetAction("attack").range),
                    new Attack()
                }),
                //  Attack if 14 or more allies nearby
                new Sequence(new BehaviorTree[] {
                    new CountAlliesQuery(14, 15f),
                    new MoveToPlayer(agent.GetAction("attack").range),
                    new Attack()
                }),
                //If player gets close and there are 6 or more allies, attack
                new Sequence(new BehaviorTree[] {
                    new CountAlliesQuery(6, 20f),
                    new IsPlayerInRangeQuery(agent.GetAction("attack").range),
                    new MoveToPlayer(agent.GetAction("attack").range),
                    new Attack()
                }),
                //Stay put if there are 4 or more allies near you
                new Sequence(new BehaviorTree[]{
                    new CountAlliesQuery (4, 3f)
                }),
                //  Congregate further out than other units as wall
                new GoTowards(AIWaypointManager.Instance.GetCongregationPoint(), 1, 4)
            );
        }
        // hit and run striker
        else if (agent.monster == "skeleton")
        {
            result = new Selector(
                //  Attack if player is in range
                new Sequence(new BehaviorTree[] {
                    new IsPlayerInRangeQuery(agent.GetAction("attack").range + 4),
                    new Attack()
                }),
                //  Strike when backed by at least 14 allies
                new Sequence(new BehaviorTree[] {
                    new CountAlliesQuery(14, 15f),
                    new MoveToPlayer(agent.GetAction("attack").range),
                    new Attack()
                }),
                //If player gets close and there are 6 or more allies, attack
                new Sequence(new BehaviorTree[] {
                    new IsPlayerInRangeQuery(agent.GetAction("attack").range + 1),
                    new CountAlliesQuery(6, 20f),
                    new MoveToPlayer(agent.GetAction("attack").range),
                    new Attack()
                }),
                //Stay put if there are 4 or more allies near you
                new Sequence(new BehaviorTree[]{
                    new CountAlliesQuery (4, 3f)
                }),
                //  Fallback 
                new GoTowards(AIWaypointManager.Instance.GetCongregationPoint(), 1, 2)
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

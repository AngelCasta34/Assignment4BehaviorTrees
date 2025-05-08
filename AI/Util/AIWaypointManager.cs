using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AIWaypointManager
{
    private List<AIWaypoint> waypoints;
    private static AIWaypointManager theInstance;

    public static AIWaypointManager Instance
    {
        get
        {
            if (theInstance == null)
                theInstance = new AIWaypointManager();
            return theInstance;
        }
    }

    private AIWaypointManager()
    {
        waypoints = new List<AIWaypoint>();
    }

   
    public void AddWaypoint(AIWaypoint wp)
    {
        waypoints.Add(wp);
    }

  
    public AIWaypoint GetClosest(Vector3 point)
    {
        return waypoints
            .Aggregate((a, b) =>
                (a.position - point).sqrMagnitude < (b.position - point).sqrMagnitude ? a : b);
    }

    
    public AIWaypoint GetClosestByType(Vector3 point, AIWaypoint.Type type)
    {
        var ofType = waypoints.Where(w => w.type == type).ToList();
        if (ofType.Count == 0) return null;
        return ofType
            .Aggregate((a, b) =>
                (a.position - point).sqrMagnitude < (b.position - point).sqrMagnitude ? a : b);
    }

 
    public AIWaypoint Get(int i)
    {
        if (i < 0 || i >= waypoints.Count) return null;
        return waypoints[i];
    }

    
    public Transform GetCongregationPoint()
    {
        var safeWaypoints = waypoints.Where(w => w.type == AIWaypoint.Type.SAFE).ToList();
        if (safeWaypoints.Count > 0)
            return safeWaypoints[Random.Range(0, safeWaypoints.Count)].transform;

        if (waypoints.Count > 0)
            return waypoints[Random.Range(0, waypoints.Count)].transform;

        return null;
    }
}

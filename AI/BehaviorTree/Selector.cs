using System.Collections.Generic;

public class Selector : InteriorNode
{
    // ← NEW: allow new Selector(a,b,c…)
    public Selector(params BehaviorTree[] children) 
        : base(children)
    { }

    // existing constructor
    public Selector(IEnumerable<BehaviorTree> children) 
        : base(children) 
    { }

    public override Result Run()
    {
        while (current_child < children.Count)
        {
            var res = children[current_child].Run();
            if (res == Result.SUCCESS)
            {
                current_child = 0;
                return Result.SUCCESS;
            }
            if (res == Result.IN_PROGRESS)
                return Result.IN_PROGRESS;

            current_child++;
        }
        current_child = 0;
        return Result.FAILURE;
    }

    public override BehaviorTree Copy()
    {
        return new Selector(CopyChildren());
    }
}

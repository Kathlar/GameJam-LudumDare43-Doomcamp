using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AiTask
{
    protected Worker parent;
    protected Action onComplete;

    protected AiTask(Worker parent, Action onComplete)
    {
        this.parent = parent;
        this.onComplete = onComplete;
    }

    public virtual void Update()
    {

    }
}

public class TaskMove : AiTask
{
    Vector3 destination;
    NavMeshAgent agent;

    public TaskMove(
        Worker parent,
        Action onComplete, 
        Vector3 destination) : base(parent, onComplete)
    {
        this.destination = destination;
        agent = parent.GetComponent<NavMeshAgent>();
    }

    public override void Update()
    {
        if (agent.destination.x != destination.x || agent.destination.z != destination.z)
        {
            agent.isStopped = false;
            agent.SetDestination(destination);
            return;
        }

        if (agent.remainingDistance < 0.25f)
        {
            agent.isStopped = true;
            onComplete();
        }
    }
}

public class TaskLookAt : AiTask
{
    Quaternion startRot;
    Quaternion endRot;
    float progress;

    public TaskLookAt(
        Worker parent,
        Action onComplete,
        Vector3 worldSpaceDirection) : base(parent, onComplete)
    {
        startRot = parent.transform.rotation;
        endRot = Quaternion.LookRotation(worldSpaceDirection, Vector3.up);
    }

    public override void Update()
    {
        progress = Mathf.MoveTowards(progress, 1.0f, Time.deltaTime);
        parent.transform.rotation = Quaternion.Slerp(startRot, endRot, progress);

        if (progress == 1)
            onComplete();
    }
}

public class TaskPlayAnimation : AiTask
{
    CharacterAnimations animator;
    string animationName;

    public TaskPlayAnimation(
        Worker parent,
        Action onComplete,
        CharacterAnimations animator,
        string animationName) : base(parent, onComplete)
    {
        this.animator = animator;
        this.animationName = animationName;
    }

    public override void Update()
    {
        animator.TriggerAnimation(animationName);
        onComplete();
    }
}

public class TaskWaiForSeconds : AiTask
{
    float seconds;
    float endTime;

    public TaskWaiForSeconds(
        Worker parent,
        Action onComplete,
        float seconds) : base(parent, onComplete)
    {
        this.seconds = seconds;
        endTime = Time.time + seconds;
    }

    public override void Update()
    {
        if (Time.time > seconds)
            onComplete();
    }
}

public class TaskWaitUntilTrue : AiTask
{
    Func<bool> condition;

    public TaskWaitUntilTrue(
        Worker parent, 
        Action onComplete,
        Func<bool> condition) : base(parent, onComplete)
    {
        this.condition = condition;
    }

    public override void Update()
    {
        if (condition())
            onComplete();
    }
}

public class TaskPerformCustomAction : AiTask
{
    Action action;

    public TaskPerformCustomAction(
        Worker parent,
        Action onComplete,
        Action action) : base(parent, onComplete)
    {
        this.action = action;
    }

    public override void Update()
    {
        action();
        onComplete();
    }
}
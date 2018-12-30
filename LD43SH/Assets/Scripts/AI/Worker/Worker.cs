using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Random = UnityEngine.Random;

public class Worker : MonoBehaviour
{
    public float health = 1.0f;
    public Workplace currentWorkplace;
    public ActivityInfo currentActivity;
    private Dictionary<string, Action<object>> taskGenerators;

    private NavMeshAgent agent;
    private CharacterAnimations animations;
    private WorkerManager manager; // will be used later
    private Freezer freezer;
    
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animations = GetComponent<CharacterAnimations>();

        taskGenerators = new Dictionary<string, Action<object>>
        {
            { "GoTo",               TaskGoTo },
            { "LookAt",             TaskLookAt },
            { "PlayAnimation",      TaskPlayAnimation },
            { "Wait",               TaskWait },
            { "WaitForCondition",   TaskWaitCondition },
            { "CustomAction",       TaskCustomAction },
        };
    }

    private void Update()
    {
        if (currentActivity == null)
        {
            if (currentWorkplace)
            {
                currentActivity = currentWorkplace.GetActivity();
            }
            else
            {
                BeginActivityIdle();
            }
        }

        if (currentActivity.currentTask == null)
        {
            string actionName = currentActivity.tasks[currentActivity.currentTaskID];
            object variable = currentActivity.arguments[currentActivity.currentTaskID];
            taskGenerators[actionName](variable);
        }

        currentActivity.currentTask.Update();
    }

    public void Init(WorkerManager manager)
    {
        this.manager = manager;
        health = Random.Range(0.75f, 1.25f);
        freezer = GetComponentInChildren<Freezer>();
        InvokeRepeating("UpdateState", Random.Range(0.0f, 1.0f), 1.0f); // <remove me later
    }

    // called once an in-game hour
    void UpdateState()
    {
        // food, death(12 days to die with no food)
        float reduction = (1.0f / 120);

        if (CampResources.instance.food.value > 0)
        {
            float taken = (1.0f / 120) * CampResources.instance.foodRationsRate;
            reduction -= taken;

            CampResources.instance.food.value = Mathf.Clamp(
                CampResources.instance.food.value - taken, 0, 1000);
        }

        health = Mathf.MoveTowards(health, 0, reduction);
        if (health == 0)
            DieSilent();
        
        if (false) // TODO: this part will handle working with no tools
        {
            CampResources.instance.morale.value = Mathf.Clamp(
                CampResources.instance.morale.value - 0.65f, 0, 100);
            health = Mathf.Clamp(health - 0.25f, 0, 10);
        }

        float slackChance = (1 - (CampResources.instance.morale.value / 100)) / 100;
        if (Random.Range(0.0f, 1.0f) < slackChance
            && currentActivity.workState == WorkState.busy)
        {
            if (Random.Range(0.0f, 1.0f) < 0.25f)
                BeginActivityRunAway();
            else
                BeginActivitySlack();
        }

        if (health == 0)
        {
            health = 1.0f; // przedsmiertna pasja
            Invoke("DieWork", Random.Range(0.0f, 20.0f));
        }

        // TODO: REWRITE THIS SO IT GOES LIKE THAT:
        // consume some food with worker manager
        // get any state changes from worker manager (like tools)
        // send myself to some morale manager
        // update current task from worker manager
        // update health

        // if no health, die
    }

    public void SetWorkplace(Workplace workplace)
    {
        currentWorkplace = workplace;
        currentActivity = null;
    }

    void OnActionComplete()
    {
        currentActivity.currentTaskID++;
        currentActivity.currentTask = null;

        if (currentActivity.currentTaskID >= currentActivity.tasks.Count)
            currentActivity = null;
    }

    #region DEATH HANDLING
    public void DieCold()
    {
        CampResources.instance.morale.value = Mathf.Clamp(
            CampResources.instance.morale.value - 2, 0, 100);
        freezer.Enable(0.0F);
        Cleanup();
    }

    public void DieHunger()
    {
        CampResources.instance.morale.value = Mathf.Clamp(
            CampResources.instance.morale.value - 1, 0, 100);
        freezer.Enable(0.0F);
        Cleanup();
    }

    public void DieWork()
    {
        CampResources.instance.morale.value = Mathf.Clamp(
            CampResources.instance.morale.value - 1, 0, 100);
        freezer.Enable(0.0F);
        Cleanup();
    }

    public void DieShot()
    {
        CampResources.instance.morale.value = Mathf.Clamp(
            CampResources.instance.morale.value + 10, 0, 100);
        animations.Die();
        freezer.Enable(2.0F);
        Cleanup();
    }
        
    public void DieSilent()
    {
        WorkerManager.WorkerDied(this);
        Destroy(gameObject);
    }

    void Cleanup()
    {
        WorkerManager.WorkerDied(this);
        animations.Unparent();
        Destroy(gameObject);
    }
    #endregion

    #region ATOMIC ACTIONS
    void TaskGoTo(object worldPosition)
    {
        currentActivity.currentTask = new TaskMove(
            this, 
            OnActionComplete, 
            (Vector3)worldPosition);
    }

    void TaskLookAt(object worldDirection)
    {
        currentActivity.currentTask = new TaskLookAt(
            this,
            OnActionComplete,
            (Vector3)worldDirection);
    }

    void TaskPlayAnimation(object animationName)
    {
        currentActivity.currentTask = new TaskPlayAnimation(
            this,
            OnActionComplete,
            animations,
            (string)animationName);
    }

    void TaskWait(object time)
    {
        currentActivity.currentTask = new TaskWaiForSeconds(
            this,
            OnActionComplete,
            (float)time);
    }

    void TaskWaitCondition(object condition)
    {
        currentActivity.currentTask = new TaskWaitUntilTrue(
            this,
            OnActionComplete,
            (Func<bool>)condition);
    }

    void TaskCustomAction(object action)
    {
        currentActivity.currentTask = new TaskPerformCustomAction(
            this,
            OnActionComplete,
            (Action)action);
    }
    #endregion

    #region STANDALONE ACTIVITIES
    public void BeginActivityGetBeaten()
    {
        currentActivity = new ActivityInfo();
        currentActivity.name = "Receiving Punishment";
        currentActivity.AddAction("PlayAnimation", "GetBeaten");
        currentActivity.AddAction("Wait", 5.0f);
    }

    public void BeginActivityIdle()
    {
        currentActivity = new ActivityInfo();
        currentActivity.name = "Idle";
        currentActivity.workState = WorkState.available;

        Vector3 pos = transform.position + Random.insideUnitSphere * 5;
        pos.y = transform.position.y;
        currentActivity.AddAction("GoTo", pos);
        currentActivity.AddAction("Wait", 5.0f);
    }

    public void BeginActivitySlack()
    {
        currentActivity = new ActivityInfo();
        currentActivity.name = "Slacking";
        currentActivity.punishmentCriteria = PunishmentCriteria.beating;

        Vector3 pos = transform.position + Random.insideUnitSphere * 5;
        pos.y = transform.position.y;
        currentActivity.AddAction("GoTo", pos);
        currentActivity.AddAction("Wait", 5.0f);
    }

    public void BeginActivityRunAway()
    {        
        currentActivity = new ActivityInfo();
        currentActivity.name = "Attempting escape";
        currentActivity.punishmentCriteria = PunishmentCriteria.execution;

        currentActivity.AddAction(
            "CustomAction",
            (Action)(()=>
            {
                animations.Escape();
                GetComponent<Renderer>().material.color = Color.cyan;
                agent.SetDestination(EscapingManager.GetEscapePoint(this));
                agent.speed = 1;
            }
        ));

        currentActivity.AddAction(
            "WaitForCondition", 
            (Func<bool>)(() => transform.position.magnitude < 60));

        currentActivity.AddAction( "Wait", Random.Range(5.0f, 20.0f));
        currentActivity.AddAction( "CustomAction", (Action)DieCold);
    }    
    #endregion
}

public class ActivityInfo
{
    public object source;
    public AiTask currentTask;
    public int currentTaskID = 0;

    public List<string> tasks = new List<string>();
    public List<object> arguments = new List<object>();

    public string name;

    public PunishmentCriteria punishmentCriteria;
    public ToolType toolRequiredID;
    public WorkState workState;

    public float moraleEffect = 0;
    public float hungerRate = 1;
    public float tireRate = 0;

    public void AddAction(string name, object argument)
    {
        tasks.Add(name);
        arguments.Add(argument);
    }
}

public enum PunishmentCriteria
{
    nothing,
    beating,
    execution,
}

public enum ToolType
{
    none,
    axe,
    hammer,
    pick,
}

public enum WorkState
{
    unavailable,
    available,
    busy,
}

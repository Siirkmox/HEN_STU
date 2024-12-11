using FSM;
using Steerings;
using UnityEngine;

[RequireComponent(typeof(WanderAround))]
[RequireComponent(typeof(Arrive))]
[RequireComponent(typeof(HEN_BLACKBOARD))]
[RequireComponent(typeof(AudioSource))]
public class FSM_HEN_EATS : FiniteStateMachine
{
    public enum State {INITIAL, WANDER, GOTO_WORM, EAT};
    public State currentState = State.INITIAL;

    private WanderAround wanderAround;
    private Arrive arrive;
    private HEN_BLACKBOARD blackboard;
    private AudioSource audioSource;

    private float elapsedTime;

    void Start()
    {
        blackboard = GetComponent<HEN_BLACKBOARD>();
        wanderAround = GetComponent<WanderAround>();
        arrive = GetComponent<Arrive>();
        audioSource = GetComponent<AudioSource>();

        wanderAround.seekWeight = 0.0f;
        wanderAround.attractor = blackboard.attractor;

        wanderAround.enabled = false;
        arrive.enabled = false;
    }

    public override void Exit()
    {
        arrive.enabled = false;
        wanderAround.enabled = false;
        base.Exit();
    }

    public override void ReEnter()
    {
        currentState = State.INITIAL;
        base.ReEnter();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.INITIAL:
                ChangeState(State.WANDER);
                break;
            case State.WANDER:
                blackboard.worm = SensingUtils.FindInstanceWithinRadius(gameObject, "WORM", blackboard.wormDetectableRadius);
                if (blackboard.worm != null)
                {
                    ChangeState(State.GOTO_WORM);
                }
                break;

            case State.GOTO_WORM:
                if(blackboard.worm == null)
                {
                    ChangeState(State.WANDER);
                }
                else if(SensingUtils.DistanceToTarget(gameObject, blackboard.worm) <= blackboard.wormReachedRadius)
                {
                    ChangeState(State.EAT);
                }
                break;

            case State.EAT:
                elapsedTime += Time.deltaTime;
                if (elapsedTime > blackboard.timeToEatWorm)
                {
                    ChangeState(State.WANDER);
                }
                break;
        }
    }

    void ChangeState (State newState)
    {
        switch (currentState)
        {
            case State.WANDER:
                wanderAround.enabled = false;  
                audioSource.Stop();             
                break;
                
            case State.GOTO_WORM:
                arrive.enabled = false;
                audioSource.Stop();
                break;

            case State.EAT:
                audioSource.Stop();
                Destroy(blackboard.worm);
                break;
        }

        switch (newState)
        {
            case State.WANDER:
                wanderAround.enabled = true;
                audioSource.clip = blackboard.cluckingSound;
                audioSource.loop = true;
                audioSource.Play();
                break;
            case State.GOTO_WORM:
                arrive.enabled = true;
                arrive.target = blackboard.worm;
                audioSource.clip = blackboard.angrySound;
                audioSource.loop = true;
                audioSource.Play();
                break;
            case State.EAT:
                audioSource.clip = blackboard.eatingSound;
                audioSource.loop = true;
                audioSource.Play();
                elapsedTime = 0;
                break;
        }

        currentState = newState;
    }
}

using FSM;
using Steerings;
using UnityEngine;

[RequireComponent(typeof(FSM_HEN_EATS))]
[RequireComponent(typeof(Seek))]
[RequireComponent(typeof(HEN_BLACKBOARD))]
public class FSM_HEN_EATS_ALONE : FiniteStateMachine
{
    public enum State { INITIAL, CALM, ANGRY };
    public State currentState;

    private HEN_BLACKBOARD blackboard;
    private FSM_HEN_EATS fsmeating;
    private Seek seek;
    private KinematicState kinematicState;

    private GameObject chick;
    private AudioSource audioSource;
    
    void Start()
    {
        blackboard = GetComponent<HEN_BLACKBOARD>();
        fsmeating = GetComponent<FSM_HEN_EATS>();
        seek = GetComponent<Seek>();
        kinematicState = GetComponent<KinematicState>();
        audioSource = GetComponent<AudioSource>();

        fsmeating.enabled = false;
        seek.enabled = false;
    }

    /* It is not necessary to override the Exit and ReEnter methods */


    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.INITIAL:
                ChangeState(State.CALM);
                break;
            case State.CALM:
                chick = SensingUtils.FindInstanceWithinRadius(gameObject, "CHICK", blackboard.chickDetectionRadius);

                if (chick != null)
                {
            
                    ChangeState(State.ANGRY);

                    gameObject.transform.localScale *= 1.2f;
                    kinematicState.maxAcceleration *= 2.0f;
                    kinematicState.maxSpeed *= 2.0f;
                }
                break;

            case State.ANGRY:
                if(SensingUtils.DistanceToTarget(gameObject, chick) >= blackboard.chickFarEnoughRadius)
                {
                    ChangeState(State.CALM);

                    gameObject.transform.localScale /= 1.2f;
                    kinematicState.maxAcceleration /= 2.0f;
                    kinematicState.maxSpeed /= 2.0f;
                }
                break;
        }
    }

    void ChangeState(State newState)
    {
        switch (currentState)
        {
            case State.CALM:
                fsmeating.Exit();
                fsmeating.enabled = false;
                break;

            case State.ANGRY:
                seek.enabled = false;
                break;
        }

        switch (newState)
        {

            case State.CALM:
                fsmeating.ReEnter();
                fsmeating.enabled = true;

                break;
            case State.ANGRY:
                seek.target = chick;
                seek.enabled = true;
                break;
        }

        currentState = newState;
    }
}
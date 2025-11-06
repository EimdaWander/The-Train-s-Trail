using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerEngineControls : MonoBehaviour
{
    //Components of the Player
    private Rigidbody2D rb;

    //Inputs of the Player
    InputAction steerAction;
    private float steerValue;

    //Private Variables
    private float targetSteerSpeed;
    private float steerVelocity = 0.0f;
    private float steerModifier = 1f;
    private float currentForwardSpeed;
    private float forwardVelocity = 0.0f;
    private float forwardModifier = 1f;

    //Exposed Private Variables
    [Header("Steering")]
    [SerializeField] private float steerBaseSpeed = 2f;
    [SerializeField] private float steerSmoothing = 0.5f;
    [Header("Train Speed")]
    [SerializeField] private float forwardBaseSpeed = 5.0f;
    [SerializeField] private float forwardSmoothing = 0.6f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        steerAction = InputSystem.actions.FindAction("Steer");

        // Set the base target speed of the train. Mostly here to set things at the start (like with a modifier or something like that)
        //forwardTargetSpeed = forwardBaseSpeed; 
    }

    // Update is called once per frame
    void Update()
    {
        //Allow the Player to Steer the Train.
        steerValue = steerAction.ReadValue<float>();
        targetSteerSpeed = steerValue * steerBaseSpeed;
        //Set the Train Speed
        float targetForwardSpeed = forwardBaseSpeed;
        currentForwardSpeed = Mathf.SmoothDamp(currentForwardSpeed, targetForwardSpeed, ref forwardVelocity, forwardSmoothing);
    }

    void FixedUpdate()
    {
        //Sets the forward Velocity for the train. With some smoothing so it's not abrupt
        rb.MovePosition(transform.position + transform.right * currentForwardSpeed * Time.deltaTime);
        //Sets the angular Velocity for the train. With some smoothing so it's not abrupt
        rb.angularVelocity = Mathf.SmoothDamp(rb.angularVelocity, targetSteerSpeed, ref steerVelocity, steerSmoothing);
    }

    //Those two should be called when an item is added or removed (with the given value inverted when removed), when an upgrade is added or when a wagon is added (if I decide to go that way)
    public void TrainSpeedModifier(float speedPercentil)
    {
        //Modify the value of the forward modifier. A percentage is given ex : 10(%). It's then divided by 100 and added to the modifier)
        forwardModifier = speedPercentil / 100 + forwardModifier;
    }
    
    public void TrainSteerModifier(float steerPercentil)
    {
        //Modify the value of the steer modifier. A percentage is given ex : 10(%). It's then divided by 100 and added to the modifier) 
        steerModifier = steerPercentil / 100 + steerModifier;        
    }
}

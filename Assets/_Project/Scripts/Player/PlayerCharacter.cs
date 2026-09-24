using KinematicCharacterController;
using UnityEngine;

public struct CharacterInput
{
    public Vector2 Move;
    public Quaternion Rotation;
    public bool Jump;
    public bool Dash;
}

public class PlayerCharacter : MonoBehaviour, ICharacterController
{
    [field: SerializeField] public KinematicCharacterMotor Motor {get; private set; }

    #region Character Variables
    [Space]
    [Header("Walk Variables")]
    public float walkSpeed = 15f;
    public float walkAcceleration = 20f;
    public float walkDeceleration = 30f;

    [Space]
    public float turnSpeed = 20f;
    #endregion

    public Vector3 RequestedMovement { get; private set; }
    public bool RequestedJump { get; set; }
    public bool RequestedDash { get; set; }

    #region State Machine Variables
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerWalkState WalkState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerFallState FallState { get; private set; }
    #endregion

    public void Initialize()
    {
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine);
        WalkState = new PlayerWalkState(this, StateMachine);
        DashState = new PlayerDashState(this, StateMachine);
        JumpState = new PlayerJumpState(this, StateMachine);
        FallState = new PlayerFallState(this, StateMachine);

        Motor.CharacterController = this;

        StateMachine.Initialize(IdleState);
    }

    public void UpdateInput(CharacterInput input)
    {
        Vector3 requestedMovement = new Vector3(input.Move.x, 0f, input.Move.y);
        requestedMovement = Vector3.ClampMagnitude(requestedMovement, 1f);
        RequestedMovement = input.Rotation * requestedMovement;

        RequestedJump = RequestedJump || input.Jump;
        RequestedDash = RequestedDash || input.Dash;
    }

    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        if (StateMachine.CurrentState != null)
        {
            StateMachine.CurrentState.UpdateVelocity(ref currentVelocity, deltaTime);
        }
    }

    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        if (StateMachine.CurrentState != null)
        {
            StateMachine.CurrentState.UpdateRotation(ref currentRotation, deltaTime);
        }
    }

    public void AfterCharacterUpdate(float deltaTime){}
    public void BeforeCharacterUpdate(float deltaTime){}
    public bool IsColliderValidForCollisions(Collider coll) => true;
    public void OnDiscreteCollisionDetected(Collider hitCollider){}
    public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport){}
    public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport){}
    public void PostGroundingUpdate(float deltaTime){}
    public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport){}
}

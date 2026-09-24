using UnityEngine;

public abstract class PlayerState
{
    protected PlayerCharacter character;
    protected PlayerStateMachine stateMachine;

    public PlayerState(PlayerCharacter character, PlayerStateMachine stateMachine)
    {
        this.character = character;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }

    public virtual void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) { }

    public virtual void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        Vector3 lookDirection = Vector3.ProjectOnPlane(character.RequestedMovement, character.Motor.CharacterUp);

        if (lookDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection, character.Motor.CharacterUp);
            float angleDiff = Quaternion.Angle(currentRotation, targetRotation);
            float dynamicTurnSpeed = character.turnSpeed * (1f + (angleDiff / 180f));

            currentRotation = Quaternion.Slerp(currentRotation, targetRotation, dynamicTurnSpeed * deltaTime);
        }
    }
}

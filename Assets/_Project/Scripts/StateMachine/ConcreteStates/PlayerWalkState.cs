using UnityEngine;

public class PlayerWalkState : PlayerState
{
    public PlayerWalkState(PlayerCharacter character, PlayerStateMachine stateMachine) 
        : base(character, stateMachine) { }

    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        if (character.RequestedMovement.sqrMagnitude <= 0.001f)
        {
            stateMachine.ChangeState(character.IdleState);
            return;
        }

        var groundMovement = character.Motor.GetDirectionTangentToSurface(
            direction: character.RequestedMovement,
            surfaceNormal: character.Motor.GroundingStatus.GroundNormal
        ) * character.RequestedMovement.magnitude;

        var targetVelocity = groundMovement * character.walkSpeed;
        currentVelocity = ExponentialDamp(currentVelocity, targetVelocity, character.walkAcceleration, deltaTime);
    }

    private static Vector3 ExponentialDamp(Vector3 current, Vector3 target, float rate, float deltaTime)
    {
        return Vector3.Lerp(current, target, 1f - Mathf.Exp(-rate * deltaTime));
    }
}

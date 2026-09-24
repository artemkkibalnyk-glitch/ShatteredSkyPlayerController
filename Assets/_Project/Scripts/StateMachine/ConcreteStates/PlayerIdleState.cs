using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerCharacter character, PlayerStateMachine stateMachine) 
        : base(character, stateMachine) { }

    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        if (character.RequestedMovement.sqrMagnitude > 0.001f)
        {
            stateMachine.ChangeState(character.WalkState);
            return;
        }

        currentVelocity = ExponentialDamp(currentVelocity, Vector3.zero, character.walkDeceleration, deltaTime);
    }

    private static Vector3 ExponentialDamp(Vector3 current, Vector3 target, float rate, float deltaTime)
    {
        return Vector3.Lerp(current, target, 1f - Mathf.Exp(-rate * deltaTime));
    }
}

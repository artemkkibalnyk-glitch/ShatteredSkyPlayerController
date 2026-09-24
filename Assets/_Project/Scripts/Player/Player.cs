using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerCharacter character;
    [SerializeField] private PlayerCamera camera;

    private InputSystem inputActions;

    void Start()
    {
        inputActions = new InputSystem();
        inputActions.Enable();

        character.Initialize();
    }

    void OnDestroy()
    {
        inputActions.Dispose();
    }

    void Update()
    {
        var input = inputActions.Gameplay;

        var characterInput = new CharacterInput
        {
            Move = input.Move.ReadValue<Vector2>(),
            Rotation = camera.GetCameraTransfomr().rotation,
            Jump = input.Jump.WasPressedThisFrame(),
            Dash = input.Dash.WasPressedThisFrame()
        };

        character.UpdateInput(characterInput);
    }
}

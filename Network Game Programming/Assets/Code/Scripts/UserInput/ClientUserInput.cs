using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using Labs.Characters;

namespace Labs.UserInput
{
    public class ClientUserInput : NetworkBehaviour
    {
        private Vector2 _previousMovementInput;
        private bool _hasMovementInputChanged;


        [SerializeField] private ServerCharacter _serverCharacter;


        private PlayerInputActions _inputActions;
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (NetworkManager.Singleton.LocalClientId != this.OwnerClientId)
            {
                Destroy(this);
                return;
            }

            CreateInputActions();
        }
        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            DisposeInputActions();
        }


        private void Update()
        {
            Vector2 newMovementInput = _inputActions.General.Movement.ReadValue<Vector2>();
            if (newMovementInput != _previousMovementInput)
            {
                _hasMovementInputChanged = true;
                _previousMovementInput = newMovementInput;
            }
        }
        private void FixedUpdate()
        {
            if (_hasMovementInputChanged)
            {
                _hasMovementInputChanged = false;
                _serverCharacter.SetCharacterMovementServerRpc(_previousMovementInput);
            }
        }


        private void CreateInputActions()
        {
            _inputActions = new PlayerInputActions();


            // Subscribe to Input Events.
            _inputActions.General.Jump.performed += Jump_performed;
            _inputActions.General.Shoot.performed += Shoot_performed;


            // Enable the Input Actions Map.
            _inputActions.Enable();
        }
        private void DisposeInputActions()
        {
            // Unsubscribe from Input Events.
            _inputActions.General.Jump.performed -= Jump_performed;
            _inputActions.General.Shoot.performed -= Shoot_performed;


            // Dispose of the Input Acitons Map.
            _inputActions.Dispose();
        }


        private void Jump_performed(InputAction.CallbackContext ctx) { }
        private void Shoot_performed(InputAction.CallbackContext ctx) => _serverCharacter.FireProjectileServerRpc(transform.position);
    }
}
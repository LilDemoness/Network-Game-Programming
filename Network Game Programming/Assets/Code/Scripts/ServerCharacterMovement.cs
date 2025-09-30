using System.Globalization;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using Labs.Characters;

namespace Labs
{
    public class ServerCharacterMovement : NetworkBehaviour
    {
        private Vector2 _movementInput;
        [SerializeField] private ServerCharacter _serverCharacter;


        [SerializeField] private float _speed = 5.0f;
        [SerializeField] private LayerMask _groundLayers;
        


        // This section will be triggered when a player enters/spawns into the game.
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (!IsServer)
            {
                this.enabled = false;
                return;
            }
        }

        private void FixedUpdate()
        {
            if (_movementInput != Vector2.zero)
                transform.position += (Vector3)_movementInput * _speed * Time.deltaTime;
        }

        public void SetMovementInput(Vector2 movementInput) => _movementInput = movementInput;


        private void OnCollisionEnter2D(Collision2D collision)
        {
            if ((_groundLayers & (1 << collision.gameObject.layer)) != 0)
            {
                _serverCharacter.ClientCharacter.SetClientTouchedGroundClientRpc();
            }
        }
    }
}
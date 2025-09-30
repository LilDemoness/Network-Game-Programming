using Unity.Netcode;
using UnityEngine;

namespace Labs.Characters
{
    public class ServerCharacter : NetworkBehaviour
    {
        [SerializeField] private ClientCharacter _clientCharacter;
        public ClientCharacter ClientCharacter => _clientCharacter;


        [SerializeField] private ServerCharacterMovement _movement; 
        public ServerCharacterMovement Movement => _movement;


        public NetworkVariable<int> CurrentScore;


        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (!this.IsServer)
            {
                this.enabled = false;
                return;
            }

            CurrentScore.Value = 0;
        }


        [ServerRpc]
        public void SetCharacterMovementServerRpc(Vector2 movementInput, ServerRpcParams serverRpcParams = default)
        {
            _movement.SetMovementInput(movementInput);
            _clientCharacter.SetMovementInputClientRpc(movementInput);
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            IncrementScore();
        }
        private void IncrementScore()
        {
            ++CurrentScore.Value;
            Debug.Log($"Player {this.OwnerClientId} gained a point (New Value: {CurrentScore.Value})");
        }
        public int GetScore() => CurrentScore.Value;
    }
}
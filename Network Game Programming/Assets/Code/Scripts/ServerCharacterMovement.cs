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
        private int _currentScore;

        


        // This section will be triggered when a player enters/spawns into the game.
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (!IsServer)
                this.enabled = false;

            _currentScore = 0;
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
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (NetworkManager.Singleton.LocalClientId == this.OwnerClientId)
            {
                IncrementScoreServerRpc(this.OwnerClientId);
            }
        }
        [ServerRpc(RequireOwnership = false)]
        private void IncrementScoreServerRpc(ulong clientID)
        {
            // Request all players to update their scores.
            IncrementScoreClientRpc(clientID);
        }
        [ClientRpc]
        private void IncrementScoreClientRpc(ulong targetClientID)
        {
            // If we are the owner of this object, increment our score.
            if (targetClientID == this.OwnerClientId)
            {
                NetworkManager.Singleton.ConnectedClients[this.OwnerClientId].PlayerObject.GetComponent<ServerCharacterMovement>().IncrementScore();
            }

            Debug.Log($"Score of Player '{targetClientID}' is {NetworkManager.Singleton.ConnectedClients[this.OwnerClientId].PlayerObject.GetComponent<ServerCharacterMovement>().GetScore()}");
        }
        public void IncrementScore() => ++_currentScore;
        public int GetScore() => _currentScore;
    }
}
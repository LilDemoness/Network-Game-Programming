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

        [SerializeField] private SpriteRenderer _playerGFX;


        [Header("Scoring")]
        public NetworkVariable<int> CurrentScore;


        [Header("Shooting")]
        [SerializeField] private GameObject _projectilePrefab;


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
        [ServerRpc]
        public void FireProjectileServerRpc(Vector2 origin, ServerRpcParams serverRpcParams = default)
        {
            GameObject projectileInstance = Instantiate<GameObject>(
                _projectilePrefab,
                origin,
                Quaternion.LookRotation(Vector3.forward, (_playerGFX.flipX ? -transform.right : transform.right)));
            projectileInstance.GetComponent<NetworkObject>().Spawn(true);
            projectileInstance.GetComponent<Projectile>().Initialise();
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
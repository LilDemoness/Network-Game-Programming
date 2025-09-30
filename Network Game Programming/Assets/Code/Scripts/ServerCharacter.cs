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


        [ServerRpc]
        public void SetCharacterMovementServerRpc(Vector2 movementInput, ServerRpcParams serverRpcParams = default)
        {
            _movement.SetMovementInput(movementInput);
            _clientCharacter.SetMovementInputClientRpc(movementInput);
        }
    }
}
using Unity.Netcode;
using UnityEngine;

namespace Labs.Characters
{
    public class ClientCharacter : NetworkBehaviour
    {
        [Header("References")]
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _playerSprite;

        // Temp.
        private Vector2 _movementInput;


        // Animation Hashes.
        private readonly int IS_JUMPING_HASH = Animator.StringToHash("IsJumping");
        private readonly int IS_MOVING_HASH = Animator.StringToHash("IsMoving");


        [ClientRpc]
        public void SetMovementInputClientRpc(Vector2 movementInput)
        {
            SetCharacterMovementAnimation(movementInput);
        }
        [ClientRpc]
        public void SetClientTouchedGroundClientRpc() => _animator.SetBool(IS_JUMPING_HASH, false);


        private void SetCharacterMovementAnimation(Vector2 movementInput) => _movementInput = movementInput;

        private void Update()
        {
            if (_movementInput != Vector2.zero)
            {
                if (_movementInput.y > 0.0f)
                {
                    _animator.SetBool(IS_JUMPING_HASH, true);
                }
                _animator.SetBool(IS_MOVING_HASH, _movementInput.x != 0.0f);
                _playerSprite.flipX = _movementInput.x < 0.0f;
            }
            else
            {
                _animator.SetBool(IS_MOVING_HASH, false);
            }
        }
    }
}
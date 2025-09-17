using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private LayerMask _groundLayers;


    [Header("References")]
    [SerializeField] private PlayerInput _playerInput;


    [Header("Animation")]
    [SerializeField] private Animator _movementAnimator;

    private readonly int IS_JUMPING_HASH = Animator.StringToHash("IsJumping");


    // This section will be triggered when a player enters/spawns into the game.
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    private void FixedUpdate()
    {
        if (!IsOwner)
            return;


        Vector2 movementInput = _playerInput.NormalizedMovementInput;
        Vector2 movement = movementInput * _speed * Time.deltaTime;
        transform.position += (Vector3)movement;


        Debug.Log(transform.position);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((_groundLayers & (1 << collision.gameObject.layer)) != 0)
        {
            _movementAnimator.SetBool(IS_JUMPING_HASH, false);
        }
    }
}

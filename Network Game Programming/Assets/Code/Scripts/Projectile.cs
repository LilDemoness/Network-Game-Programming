using UnityEngine;
using Unity.Netcode;

namespace Labs
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : NetworkBehaviour
    {
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private float _projectileSpeed = 5.0f;

        [SerializeField] private LayerMask _targetableLayers;


#if UNITY_EDITOR

        private void Reset()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

#endif


        public void Initialise()
        {
            _rb.AddForce(transform.up * _projectileSpeed, ForceMode2D.Impulse);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if ((_targetableLayers & (1 << other.gameObject.layer)) != 0)
            {
                GetComponent<NetworkObject>().Despawn(true);    // Destroy on the server.
                Destroy(this);  // Destroy on the client.
            }
        }
    }
}
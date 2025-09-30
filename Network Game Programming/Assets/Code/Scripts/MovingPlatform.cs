using UnityEngine;
using Unity.Netcode;

namespace Labs
{
    public class MovingPlatform : NetworkBehaviour
    {
        [SerializeField] private float _movingDistance = 4.0f;
        [SerializeField] private float _movingSpeed = 1.0f;
        private float _leftTargetX;
        float _currentTime;


        private void Awake()
        {
            _leftTargetX = transform.position.x - _movingDistance / 2.0f;
        }
        public override void OnNetworkSpawn()
        {
            _currentTime = (float)NetworkManager.Singleton.ServerTime.Time;
        }

        private void FixedUpdate()
        {
            _currentTime += Time.fixedDeltaTime;

            // Perform Movement.
            float targetX = _leftTargetX + Mathf.PingPong(_currentTime * _movingSpeed, _movingDistance);
            transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Vector2 leftPosition = transform.position - (Vector3.right * _movingDistance / 2.0f);
            Vector2 rightPosition = transform.position + (Vector3.right * _movingDistance / 2.0f);

            Gizmos.DrawLine(leftPosition, rightPosition);
            Gizmos.DrawSphere(leftPosition, 0.1f);
            Gizmos.DrawSphere(rightPosition, 0.1f);
        }
    }
}
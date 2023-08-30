using System;
using UnityEngine;

namespace GameObjects.Characters
{
    public class Character : MonoBehaviour
    {
        [Header("[GameObjects]")]
        public GameObject body;
        public GameObject head;
        public GameObject cannon;
        
        [Header("[RigidBody]")]
        [SerializeField]
        private Rigidbody _rBody;
        [SerializeField]
        private Rigidbody _rHead;
        [SerializeField]
        private Rigidbody _rCannon;

        private float moveSpeed = 1.0f;
        private float rotateSpeed = 1.0f;

        private float maxHeartBeatTimeout = 5.0f;
        private float currentHeartBeatTimeout = 0f;

        private void FixedUpdate()
        {
            currentHeartBeatTimeout += Time.deltaTime;

            if (currentHeartBeatTimeout >= maxHeartBeatTimeout)
            {
                Destroy(this);
            }
        }

        public void MoveCharacter(Vector2 movement)
        {
            currentHeartBeatTimeout = 0;
            
            this._rBody.velocity += this._rBody.transform.forward * (movement.y * this.moveSpeed);
            this._rBody.rotation *= Quaternion.Euler(Vector3.up * (movement.x * rotateSpeed));
        }
    }
}

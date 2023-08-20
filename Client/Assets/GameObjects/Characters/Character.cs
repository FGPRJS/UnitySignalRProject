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

        private float moveSpeed = 2.0f;
        private float rotateSpeed = 1.0f;

        public void MoveCharacter(Vector2 movement)
        {
            this._rBody.velocity += this._rBody.transform.forward * (movement.y * this.moveSpeed);
            this._rBody.rotation *= Quaternion.Euler(Vector3.up * (movement.x * rotateSpeed));
        }
    }
}

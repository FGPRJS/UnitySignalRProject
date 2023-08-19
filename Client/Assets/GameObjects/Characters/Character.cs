using UnityEngine;

namespace GameObjects.Characters
{
    public class Character : MonoBehaviour
    {
        private PlayerInputActions _playerInputActions;
        
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
        private Vector2 _moveInput;

        private float moveSpeed = 2.0f;
        private float rotateSpeed = 1.0f;

        private void Awake()
        {
            this._playerInputActions = new PlayerInputActions();
        }

        private void OnEnable()
        {
            this._playerInputActions.Character.Enable();
        }

        private void OnDisable()
        {
            this._playerInputActions.Character.Disable();
        }

        private void FixedUpdate()
        {
            this._moveInput = this._playerInputActions.Character.Move.ReadValue<Vector2>();
            this._rBody.velocity += this._rBody.transform.forward * (this._moveInput.y * this.moveSpeed);
            this._rBody.rotation *= Quaternion.Euler(Vector3.up * (this._moveInput.x * rotateSpeed));
        }
    }
}

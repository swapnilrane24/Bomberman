using UnityEngine;
using Common;

namespace PlayerSystem
{
    public class PlayerView : MonoBehaviour , IDamagable
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private Rigidbody2D myBody;
        [SerializeField] private Transform playerSprite;

        private PlayerController playerController;
        private float horizontalVal, verticalVal;

        private void Update()
        {
            MoveDirection();

            if (Input.GetKeyDown(KeyCode.Space)) SpawnBomb();
        }

        private void MoveDirection()
        {
            horizontalVal = Input.GetAxis("Horizontal");
            verticalVal = Input.GetAxis("Vertical");

            if (Mathf.Abs(horizontalVal) > Mathf.Abs(verticalVal))
            {
                if (horizontalVal > 0 && playerSprite.rotation.z != 90)
                    playerSprite.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                else if (horizontalVal < 0 && playerSprite.rotation.z != 270)
                    playerSprite.rotation = Quaternion.Euler(new Vector3(0, 0, 270));

                verticalVal = 0;
            }

            if (Mathf.Abs(verticalVal) > Mathf.Abs(horizontalVal))
            {
                if (verticalVal > 0 && playerSprite.rotation.z != 180)
                    playerSprite.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                else if (verticalVal < 0 && playerSprite.rotation.z != 0)
                    playerSprite.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

                horizontalVal = 0;
            }
        }

        void SpawnBomb() => playerController.SpawnBomb();

        void FixedUpdate()
        {
            myBody.velocity = new Vector2(horizontalVal, verticalVal) * moveSpeed;
        }

        public void Damage() => playerController.PlayerKilled();

        public void SetController(PlayerController playerController) =>
            this.playerController = playerController;

        void OnCollisionEnter2D(Collision2D other)
        {
            if(other.gameObject.GetComponent<IKillable>() != null)
            {
                Damage(); 
            }
        }
    }
}
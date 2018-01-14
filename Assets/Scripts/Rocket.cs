using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class Rocket : MonoBehaviour
    {
        public float ThrusterPower = 1;
        public float RotationSpeed = 1;

        private Rigidbody rigidBody;
        private AudioSource thrusterSound;

        // Use this for initialization
        void Start ()
        {
            rigidBody = GetComponent<Rigidbody>();
            thrusterSound = GetComponent<AudioSource>();
        }
	
        // Update is called once per frame
        void Update ()
        {
            ProcessInput();
        }

        void OnCollisionEnter(Collision collision)
        {
            switch (collision.gameObject.tag)
            {
                case "Friendly":
                    // do nothing
                    break;
                case "Finish":
                    print("WIN");
                    SceneManager.LoadScene(1);
                    break;
                case "PowerUp":
                    print("COLLECT");
                    break;
                default:
                    print("DEAD");
                    SceneManager.LoadScene(0);
                    break;
            }
        }

        private void ProcessInput()
        {
            Thrust();
            Rotate();
        }

        private void Thrust()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                rigidBody.AddRelativeForce(Vector3.up * ThrusterPower * Time.deltaTime);
                if (!thrusterSound.isPlaying)
                {
                    thrusterSound.Play();
                }
            }
            else
            {
                if (thrusterSound.isPlaying)
                {
                    thrusterSound.Stop();
                }
            }
        }

        private void Rotate()
        {
            rigidBody.freezeRotation = true;  // take manual control of rotation

            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
            {
                // Do nothing
            }
            else if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(Vector3.forward * RotationSpeed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(-Vector3.forward * RotationSpeed * Time.deltaTime);
            }

            rigidBody.freezeRotation = false;  // resume physics control of rotation 
        }
    }
}

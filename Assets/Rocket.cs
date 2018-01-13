using UnityEngine;

namespace Assets
{
    public class Rocket : MonoBehaviour
    {
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

        private void ProcessInput()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                rigidBody.AddRelativeForce(Vector3.up);
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

            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
            {
                // Do nothing
            }
            else if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(Vector3.forward);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(-Vector3.forward);
            }
        }
    }
}

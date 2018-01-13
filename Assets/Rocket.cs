using UnityEngine;

namespace Assets
{
    public class Rocket : MonoBehaviour {

        // Use this for initialization
        void Start ()
        {
		
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
                print("Space pressed");
            }

            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
            {
                // Do nothing
            }
            else if (Input.GetKey(KeyCode.A))
            {
                print("Left");
            }
            else if (Input.GetKey(KeyCode.D))
            {
                print("Right");
            }
        }
    }
}

using UnityEngine;

namespace UnityTutorial.Manager
{
    public class InputManager : MonoBehaviour
    {
        public Vector2 Move { get; private set; }
        public bool Run { get; private set; }
        public bool Jump { get; private set; }
        public bool Crouch { get; private set; }
        public Vector2 Look { get; private set; }

        private void Update()
        {
            // Get movement input (WASD or Arrow keys)
            Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            // Get sprint input (Shift key)
            Run = Input.GetKey(KeyCode.LeftShift);

            // Get jump input (Space bar)
            Jump = Input.GetKeyDown(KeyCode.Space);

            // Get crouch input (Control key)
            Crouch = Input.GetKey(KeyCode.LeftControl);

            // Get look input (Mouse movement)
            Look = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }
    }
}

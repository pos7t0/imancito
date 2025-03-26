using UnityEngine.InputSystem;

namespace Core.Input
{
    public sealed class InputButton
    {
        private float m_value;
        private bool m_triggered;
        private bool m_changed;

        public InputButton()
        {
            m_value = 0f;
            m_triggered = false;
            m_changed = false;
        }

        public void SetValues(InputAction.CallbackContext context)
        {
            m_value = context.ReadValue<float>();
            m_triggered = context.action.triggered;
        }

        public InputState State
        {
            get
            {
                if (m_triggered && !m_changed && m_value == 1)
                {
                    m_changed = true;
                    return InputState.Pressed;
                }

                if (m_triggered && m_changed && m_value == 1)
                {
                    return InputState.Holding;
                }

                if (!m_triggered && m_changed && m_value == 0)
                {
                    m_changed = false;
                    return InputState.Release;
                }

                return InputState.Free;
            }
        }

        public bool Free => State == InputState.Free;
        public bool Pressed => State == InputState.Pressed;
        public bool Holding => State == InputState.Holding;
        public bool Released => State == InputState.Release;
    }
}


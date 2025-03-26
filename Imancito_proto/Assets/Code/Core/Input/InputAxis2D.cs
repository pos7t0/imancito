using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Input
{
    public sealed class InputAxis2D
    {
        private float m_lastX;
        private float m_x;
        public float X
        {
            get { return m_x; }
            private set
            {
                if (value != 0f)
                    m_lastX = value;
                m_x = value;
            }
        }

        private float m_lastY;
        private float m_y;
        public float Y
        {
            get { return m_y; }
            private set
            {
                if (value != 0f)
                    m_lastY = value;
                m_y = value;
            }
        }

        public Vector2 Vector { get { return new Vector2(X, Y); } }

        public Direction DirectionOnRelease
        {
            get
            {
                float absX = Mathf.Abs(m_lastX);
                float absY = Mathf.Abs(m_lastY);

                if (absX == absY)
                    return Direction.Null;

                if (absX > absY)
                {
                    if (m_lastX < 0f)
                        return Direction.Left;
                    return Direction.Right;
                }

                if (m_lastY < 0f)
                    return Direction.Bottom;

                return Direction.Top;
            }
        }

        public bool IsHorizontalActive { get { return X != 0f; } }
        public bool IsVerticalActive { get { return Y != 0f; } }
        public bool IsActive { get { return IsHorizontalActive || IsVerticalActive; } }

        public bool m_triggered;
        public bool Released
        {
            get
            {
                if (m_triggered)
                {
                    m_triggered = false;
                    return true;
                }
                return false;
            }
        }
        public bool IsHorizontalActiveOnRelease { get { return m_lastX != 0f; } }
        public bool IsVerticalActiveOnRelease { get { return m_lastY != 0f; } }
        public bool IsActiveOnRelease { get { return IsHorizontalActiveOnRelease || IsVerticalActiveOnRelease; } }

        public InputAxis2D()
        {
            m_lastX = 0f;
            m_lastY = 0f;
            X = 0f;
            Y = 0f;
            m_triggered = false;
        }

        public void SetValues(InputAction.CallbackContext context)
        {
            Vector2 axis = context.ReadValue<Vector2>();
            X = axis.x;
            Y = axis.y;
            if (context.action.triggered && axis == Vector2.zero)
                m_triggered = true;
        }

        public bool TryIsHorizontalActiveOnRelease(out float direction)
        {
            direction = Mathf.Sign(m_lastX);
            return IsHorizontalActiveOnRelease;
        }

        public bool TryIsVerticalActiveOnRelease(out float direction)
        {
            direction = Mathf.Sign(m_lastY);
            return IsVerticalActiveOnRelease;
        }

        public void ResetOnReleaseValues()
        {
            m_lastX = 0f;
            m_lastY = 0f;
        }

        public override string ToString()
        {
            return $"InputAxis2D X:{X}, Y:{Y}";
        }
    }
}


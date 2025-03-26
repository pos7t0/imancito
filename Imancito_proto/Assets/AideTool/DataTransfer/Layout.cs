using System;
using UnityEngine;

namespace AideTool.DataTransfer
{
    [Serializable]
    public struct Layout
    {
        [SerializeField] private float m_left;
        public float Left { get { return m_left; } }
        [SerializeField] private float m_top;
        public float Top { get { return m_top; } }
        [SerializeField] private float m_right;
        public float Right { get { return m_right; } }
        [SerializeField] private float m_bottom;
        public float Bottom { get { return m_bottom; } }
    }
}

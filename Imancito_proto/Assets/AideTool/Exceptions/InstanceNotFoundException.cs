using System;

namespace AideTool.Exceptions
{
    internal sealed class InstanceNotFoundException : Exception
    {
        private readonly string m_referenceName;
        public override string Message => $"Instance not found in {m_referenceName}.";

        public InstanceNotFoundException(string referenceName)
        {
            m_referenceName = referenceName;
        }
    }
}

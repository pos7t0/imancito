using System;

namespace AideTool.ExtendedEditor
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.ReturnValue, Inherited = false, AllowMultiple = false)]
    public sealed class HideFieldAttribute : Attribute
    {
        public string[] FieldNames { get; private set; }

        public HideFieldAttribute(params string[] fieldNames)
        {
            FieldNames= fieldNames;
        }
    }
}
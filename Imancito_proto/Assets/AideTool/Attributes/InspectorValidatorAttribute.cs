using System;
using UnityEngine;

namespace AideTool.ExtendedEditor
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class InspectorValidatorAttribute : PropertyAttribute { }
}

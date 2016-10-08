using System;

namespace <%=baseName%>.Utilities
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MapperAttribute : Attribute
    {
        public const string DEFAULT_METHOD_NAME = "ResgiterMapping";
    }
}
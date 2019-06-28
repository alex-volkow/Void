using System;
using System.Collections.Generic;
using System.Text;

namespace Void.Reflection
{
    public interface IDefinitions : IReadOnlyDictionary<Type, object>
    {
        //object Create(Type type);
        //object Create(Type type, params object[] args);
        //T Create<T>();
        //T Create<T>(params object[] args);
        object Get(Type type);
        object GetRequired(Type type);
        T Get<T>();
        T GetRequired<T>();
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Void.Collections
{
    public interface IDefinitions : IReadOnlyDictionary<Type, object>
    {
        object Get(Type type);
        object GetRequired(Type type);
        T Get<T>();
        T GetRequired<T>();
    }
}

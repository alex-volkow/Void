using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.Json
{
    public partial class JOption
    {
        //object IReadOnlyList<object>.this[int index] => this[index];

        //object IList<object>.this[int index] {
        //    set => throw new NotSupportedException("Object is read-only");
        //    get {
        //        return this[index];
        //        //lock (this.locker) {
        //        //    var json = GetElement();
        //        //    if (json is JArray array) {
        //        //        if (index > 0 && index < (array.Count - 1)) {
        //        //            return array[index];
        //        //        }
        //        //    }
        //        //    else if (json is JValue value && value.Type == JTokenType.String) {
        //        //        var text = (string)value;
        //        //        if (index > 0 && index < (text.Length - 1)) {
        //        //            return text[index];
        //        //        }
        //        //    }
        //        //    return new JOption(this);
        //        //}
        //    }
        //}


        //int IList<object>.IndexOf(object item) {
        //    //lock (this.locker) {
        //    //    var json = GetElement();
        //    //    if (json is IList<JToken> list) {
        //    //        if (item == null) {
        //    //            return list.IndexOf(null);
        //    //        }
        //    //        if (item is JToken token) {
        //    //            return list.IndexOf(token);
        //    //        }
        //    //        for (var i = 0; i < list.Count; i++) {
        //    //            var value = list[i]?.ToObject(item.GetType());
        //    //            if (object.Equals(value, item)) {
        //    //                return i;
        //    //            }
        //    //        }
        //    //        return -1;
        //    //    }
        //    //    if (json is JObject map) {

        //    //    }
        //    //}
        //    throw new NotImplementedException();
        //}

        //void IList<object>.Insert(int index, object item) {
        //    throw new NotImplementedException();
        //}

        //void IList<object>.RemoveAt(int index) {
        //    throw new NotImplementedException();
        //}

        //void ICollection<object>.Add(object item) {
        //    throw new NotImplementedException();
        //}

        //void ICollection<object>.Clear() {
        //    throw new NotImplementedException();
        //}

        //bool ICollection<object>.Contains(object item) {
        //    throw new NotImplementedException();
        //}

        //void ICollection<object>.CopyTo(object[] array, int arrayIndex) {
        //    throw new NotImplementedException();
        //}

        //bool ICollection<object>.Remove(object item) {
        //    throw new NotImplementedException();
        //}

        //IEnumerator<object> IEnumerable<object>.GetEnumerator() {
        //    throw new NotImplementedException();
        //}

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}

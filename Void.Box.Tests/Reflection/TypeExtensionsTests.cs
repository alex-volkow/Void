using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Void.Reflection
{
    public class TypeExtensionsTests
    {
        [Fact]
        public void IsTest() {
            Assert.True(typeof(object).Is<object>());
            Assert.True(typeof(Item).Is<object>());
            Assert.True(typeof(Item).Is<IItem>());
            Assert.False(typeof(IItem).Is<Item>());
            //IList<Item> items1 = new List<Item>();
            Assert.True(typeof(List<Item>).Is<IList<Item>>());
            Assert.True(typeof(List<Item>).Is<List<Item>>());
            Assert.False(typeof(IList<Item>).Is<List<Item>>());
            //IList<IItem> items2 = new List<IItem>();
            Assert.True(typeof(List<IItem>).Is<IList<IItem>>());
            Assert.True(typeof(List<IItem>).Is<List<IItem>>());
            Assert.False(typeof(IList<IItem>).Is<List<IItem>>());
            //IItems<IItem> items3 = new Items<Item>();
            Assert.True(typeof(Items<Item>).Is<IOutItems<IItem>>());
            Assert.False(typeof(IOutItems<IItem>).Is<Items<Item>>());
            Assert.False(typeof(Items<Item>).Is<IStdItems<IItem>>());
            Assert.False(typeof(IStdItems<IItem>).Is<Items<Item>>());
            ////
            Assert.True(typeof(List<Item>).Is(typeof(List<>)));
            Assert.True(typeof(List<IItem>).Is(typeof(List<>)));
        }

        private class Item : IItem { }

        private interface IItem { }

        private class Items<T> : IOutItems<T> { }

        private interface IOutItems<out T> { }

        private interface IStdItems<T> { }

    }
}

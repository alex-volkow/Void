using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Void.Reflection
{
#pragma warning disable 0649
    [Parallelizable]
    public class TypeExtensionsTests
    {
        [Test]
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

        [Test]
        public void GetAllFields() {
            var item1 = new Item();
            var item2 = new ChildItem();
            var item3 = new ChildChildItem();
            var fields1 = item1.GetType()
                .GetAllFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .ToArray();
            var fields2 = item2.GetType()
                .GetAllFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .ToArray();
            var fields3 = item3.GetType()
                .GetAllFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .ToArray();
            Assert.AreEqual(2, fields1.Length);
            Assert.AreEqual(5, fields2.Length);
            Assert.AreEqual(8, fields3.Length);
        }

        [Test]
        public void GetTopFields() {
            var item1 = new Item();
            var item2 = new ChildItem();
            var item3 = new ChildChildItem();
            var fields1 = item1.GetType()
                .GetTopFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .ToArray();
            var fields2 = item2.GetType()
                .GetTopFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .ToArray();
            var fields3 = item3.GetType()
                .GetTopFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .ToArray();
            Assert.AreEqual(2, fields1.Length);
            Assert.AreEqual(4, fields2.Length);
            Assert.AreEqual(5, fields3.Length);
        }

        [Test]
        public void GetAllProperties() {
            var item1 = new Item();
            var item2 = new ChildItem();
            var item3 = new ChildChildItem();
            var itemProperties1 = item1.GetType()
                .GetAllProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .ToArray();
            var itemProperties2 = item2.GetType()
                .GetAllProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .ToArray();
            var itemProperties3 = item3.GetType()
                .GetAllProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .ToArray();
            Assert.AreEqual(2, itemProperties1.Length);
            Assert.AreEqual(5, itemProperties2.Length);
            Assert.AreEqual(7, itemProperties3.Length);
        }

        [Test]
        public void GetTopProperties() {
            var item1 = new Item();
            var item2 = new ChildItem();
            var item3 = new ChildChildItem();
            var itemProperties1 = item1.GetType()
                .GetTopProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .ToArray();
            var itemProperties2 = item2.GetType()
                .GetTopProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .ToArray();
            var itemProperties3 = item3.GetType()
                .GetTopProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .ToArray();
            Assert.AreEqual(2, itemProperties1.Length);
            Assert.AreEqual(3, itemProperties2.Length);
            Assert.AreEqual(3, itemProperties3.Length);
        }

        private class Item : IItem
        {
            protected string text;
            protected virtual string Value => "1";
            public string Fixed { get; }
        }

        private interface IItem { }

        private class Items<T> : IOutItems<T> { }

        private interface IOutItems<out T> { }

        private interface IStdItems<T> { }

        private class ChildItem : Item
        {
            private readonly double f;
            public object obj;
            protected override string Value => "2";

            private int Numer => 4;
        }

        private class ChildChildItem : ChildItem
        {
            public int n;
            protected override string Value => "3";
        }
    }
#pragma warning restore 0649
}

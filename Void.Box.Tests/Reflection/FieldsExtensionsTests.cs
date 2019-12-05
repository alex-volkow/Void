using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Void.Reflection
{
    [Parallelizable]
    public class FieldsExtensionsTests
    {
        private class Item
        {
            private readonly int number = 1;

            public string Text { get; }

            int GetNumer() => this.number;
        }

        [Test]
        public void CheckAutoField() {
            var fields = typeof(Item)
                .GetAllFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .ToArray();
            Assert.AreEqual(2, fields.Length);
            var autofield = fields.First(e => e.Name.Contains(nameof(Item.Text)));
            Assert.True(autofield.IsAuto());
        }

        [Test]
        public void CheckNonAutoField() {
            var fields = typeof(Item)
                .GetAllFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .ToArray();
            Assert.AreEqual(2, fields.Length);
            var standardField = fields.First(e => !e.Name.Contains(nameof(Item.Text)));
            Assert.False(standardField.IsAuto());
        }

        [Test]
        public void GetAutoPropertyName() {
            var fields = typeof(Item)
                .GetAllFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .ToArray();
            Assert.AreEqual(2, fields.Length);
            var autofield = fields.First(e => e.Name.Contains(nameof(Item.Text)));
            Assert.AreEqual(nameof(Item.Text), autofield.GetAutoPropertyName());
        }
    }
}

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Void.Reflection
{
    [Parallelizable]
    public class PropertiesExtensionsTests
    {
        private class Item
        {
            private readonly int number = 1;

            public string Text { get; }

            public int IntValue => 123;

            int GetNumer() => this.number;
        }

        [Test]
        public void GetExistingAutoField() {
            var fields = typeof(Item)
                .GetAllProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .ToArray();
            Assert.AreEqual(2, fields.Length);
            var text = fields.First(e => e.Name == nameof(Item.Text));
            Assert.NotNull(text.GetAutoField());
        }

        [Test]
        public void GetNonExistingAutoField() {
            var fields = typeof(Item)
                .GetAllProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .ToArray();
            Assert.AreEqual(2, fields.Length);
            var number = fields.First(e => e.Name == nameof(Item.IntValue));
            Assert.Null(number.GetAutoField());
        }

        [Test]
        public void IsAutoProperty() {
            var fields = typeof(Item)
                .GetAllProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .ToArray();
            Assert.AreEqual(2, fields.Length);
            var text = fields.First(e => e.Name == nameof(Item.Text));
            Assert.True(text.IsAuto());
        }

        [Test]
        public void IsNonAutoProperty() {
            var fields = typeof(Item)
                .GetAllProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .ToArray();
            Assert.AreEqual(2, fields.Length);
            var number = fields.First(e => e.Name == nameof(Item.IntValue));
            Assert.False(number.IsAuto());
        }

        [Test]
        public void SetForceSuccess() {
            var fields = typeof(Item)
                .GetAllProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .ToArray();
            Assert.AreEqual(2, fields.Length);
            var text = fields.First(e => e.Name == nameof(Item.Text));
            var instance = new Item();
            text.SetForce(instance, "pew pew");
            Assert.AreEqual("pew pew", instance.Text);
        }

        [Test]
        public void SetForceFail() {
            var fields = typeof(Item)
                .GetAllProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .ToArray();
            Assert.AreEqual(2, fields.Length);
            var number = fields.First(e => e.Name == nameof(Item.IntValue));
            var instance = new Item();
            Assert.That(() => number.SetForce(instance, 4), Throws.Exception);
        }
    }
}

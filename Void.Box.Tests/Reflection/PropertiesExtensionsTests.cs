using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace Void.Reflection
{
    public class PropertiesExtensionsTests
    {
        private class Item
        {
            private readonly int number = 1;

            public string Text { get; }

            public int IntValue => 123;

            int GetNumer() => this.number;
        }

        [Fact]
        public void GetExistingAutoField() {
            var fields = typeof(Item)
                .GetAllProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .ToArray();
            Assert.Equal(2, fields.Length);
            var text = fields.First(e => e.Name == nameof(Item.Text));
            Assert.NotNull(text.GetAutoField());
        }

        [Fact]
        public void GetNonExistingAutoField() {
            var fields = typeof(Item)
                .GetAllProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .ToArray();
            Assert.Equal(2, fields.Length);
            var number = fields.First(e => e.Name == nameof(Item.IntValue));
            Assert.Null(number.GetAutoField());
        }

        [Fact]
        public void IsAutoProperty() {
            var fields = typeof(Item)
                .GetAllProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .ToArray();
            Assert.Equal(2, fields.Length);
            var text = fields.First(e => e.Name == nameof(Item.Text));
            Assert.True(text.IsAuto());
        }

        [Fact]
        public void IsNonAutoProperty() {
            var fields = typeof(Item)
                .GetAllProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .ToArray();
            Assert.Equal(2, fields.Length);
            var number = fields.First(e => e.Name == nameof(Item.IntValue));
            Assert.False(number.IsAuto());
        }

        [Fact]
        public void SetForceSuccess() {
            var fields = typeof(Item)
                .GetAllProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .ToArray();
            Assert.Equal(2, fields.Length);
            var text = fields.First(e => e.Name == nameof(Item.Text));
            var instance = new Item();
            text.SetForce(instance, "pew pew");
            Assert.Equal("pew pew", instance.Text);
        }

        [Fact]
        public void SetForceFail() {
            var fields = typeof(Item)
                .GetAllProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .ToArray();
            Assert.Equal(2, fields.Length);
            var number = fields.First(e => e.Name == nameof(Item.IntValue));
            var instance = new Item();
            Assert.ThrowsAny<Exception>(() => number.SetForce(instance, 4));
        }
    }
}

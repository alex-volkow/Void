using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace Void.Reflection
{
    public class FieldsExtensionsTests
    {
        private class Item
        {
            private readonly int number = 1;

            public string Text { get; }

            int GetNumer() => this.number;
        }

        [Fact]
        public void CheckAutoField() {
            var fields = typeof(Item)
                .GetAllFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .ToArray();
            Assert.Equal(2, fields.Length);
            var autofield = fields.First(e => e.Name.Contains(nameof(Item.Text)));
            Assert.True(autofield.IsAuto());
        }

        [Fact]
        public void CheckNonAutoField() {
            var fields = typeof(Item)
                .GetAllFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .ToArray();
            Assert.Equal(2, fields.Length);
            var standardField = fields.First(e => !e.Name.Contains(nameof(Item.Text)));
            Assert.False(standardField.IsAuto());
        }

        [Fact]
        public void GetAutoPropertyName() {
            var fields = typeof(Item)
                .GetAllFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .ToArray();
            Assert.Equal(2, fields.Length);
            var autofield = fields.First(e => e.Name.Contains(nameof(Item.Text)));
            Assert.Equal(nameof(Item.Text), autofield.GetAutoPropertyName());
        }
    }
}

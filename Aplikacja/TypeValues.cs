using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplikacja
{
    internal class TypeValues
    {
        private double minValue;
        private double maxValue;
        private string family;

        public TypeValues(double minValue, double maxValue, string family)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.family = family;
        }

        public double getMinValue() { return this.minValue; }

        public double getMaxValue() { return this.maxValue; }

        public string getFamily() { return this.family; }
    }
}

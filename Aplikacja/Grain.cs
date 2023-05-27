using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplikacja
{
    internal class Grain
    {
        private double width;
        private double height;
        private double coeff;
        private string type;

        public void setValues(double width, double height, double coeff, string type)
        {
            this.width = width;
            this.height = height;
            this.coeff = coeff;
            this.type = type;
        }

        public Grain(double width, double height, double coeff, string type)
        {
            this.setValues(width, height, coeff, type);
        }

        public double getWidth() { return this.width; }
        public double getHeight() { return this.height; }
        public double getCoeff() { return this.coeff; }
        public string getType() { return this.type; }

    }
}

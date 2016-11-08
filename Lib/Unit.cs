using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    public class Unit
    {
        protected Unit U0;
        protected Unit U1;

        public double Value { get; set; } = 0;

        public double Grad { get; set; } = 0;

        public Unit()
        {
        }

        public Unit(double value, double grad)
        {
            Value = value;
            Grad = grad;
        }

        public override string ToString()
        {
            return string.Format("Value: " + Value + ". Grad: " + Grad + ".");
        }
    }
}

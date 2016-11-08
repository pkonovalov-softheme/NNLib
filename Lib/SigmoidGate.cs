using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    public sealed class SigmoidGate : Unit
    {
        private static double Sigmoid(double value)
        {
            double k = Math.Exp(value);
            return k / (1.0d + k);
        }

        private double SigmoidDerivative(double x)
        {
            double s = Sigmoid(x);
            return s * (1 - s);
        }

        public Unit Forward(Unit u0)
        {
            U0 = u0;
            Value = Sigmoid(u0.Value);
            return this;
        }

        public void Backward()
        {
            U0.Grad += SigmoidDerivative(U0.Value);
        }
    }
}

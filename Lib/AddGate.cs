using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    public sealed class AddGate : Unit
    {
        public Unit Forward(Unit u0, Unit u1)
        {
            U0 = u0;
            U1 = u1;

            Value = u0.Value + u1.Value;
            return this;
        }

        public void Backward()
        {
            U0.Grad += 1 * Grad;
            U1.Grad += 1 * Grad;
        }
    }
}

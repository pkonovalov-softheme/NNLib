using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    public class SVM : Unit
    {
        Unit a = new Unit(1.0, 0.0);
        Unit b = new Unit(-2.0, 0.0);
        Unit c = new Unit(-1.0, 0.0);
        Unit unit_out;

        Circuit circuit = new Circuit();

        public Unit Forward(Unit x, Unit y)
        {
            unit_out = this.circuit.Forward(x, y, this.a, this.b, this.c);
            return this.unit_out;
        }

        public void Backward(int label) // label is +1 or -1
        {
            // reset pulls on a,b,c
            a.Grad = 0.0;
            b.Grad = 0.0;
            c.Grad = 0.0;

            // compute the pull based on what the circuit output was
            var pull = 0.0;
            if (label == 1 && this.unit_out.Value < 1)
            {
                pull = 1; // the score was too low: pull up
            }
            if (label == -1 && this.unit_out.Value > -1)
            {
                pull = -1; // the score was too high for a positive example, pull down
            }

            this.circuit.Backward(pull); // writes gradient into x,y,a,b,c

            // add regularization pull for parameters: towards zero and proportional to value
            this.a.Grad += -this.a.Value;
            this.b.Grad += -this.b.Value;
        }

        public void LearnFrom(Unit x, Unit y, int label)
        {
            this.Forward(x, y); // forward pass (set .value in all Units)
            this.Backward(label); // backward pass (set .grad in all Units)
            this.ParameterUpdate(); // parameters respond to tug
        }

        public void ParameterUpdate()
        {
            var step_size = 0.01;
            this.a.Value += step_size * this.a.Grad;
            this.b.Value += step_size * this.b.Grad;
            this.c.Value += step_size * this.c.Grad;
        }
    }
}

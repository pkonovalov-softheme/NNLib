using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    class Circuit : Unit
    {
        readonly MultiplyGate _mulg0 = new MultiplyGate();
        readonly MultiplyGate _mulg1 = new MultiplyGate();
        readonly AddGate _addg0 = new AddGate();
        readonly AddGate _addg1 = new AddGate();
        Unit _ax;
        Unit _by;
        Unit _axpby;
        Unit _axpbypc;

        public Unit Forward(Unit x, Unit y, Unit a, Unit b, Unit c)
        {
            _ax = _mulg0.Forward(a, x); // a*x
            _by = _mulg1.Forward(b, y); // b*y
            _axpby = _addg0.Forward(_ax, _by); // a*x + b*y
            _axpbypc = _addg1.Forward(_axpby, c); // a*x + b*y + c
            return _axpbypc;
        }

        public void Backward(double gradientTop)
        {
            _axpbypc.Grad = gradientTop;
            _addg1.Backward(); // sets gradient in axpby and c
            _addg0.Backward(); // sets gradient in _ax and by
            _mulg1.Backward(); // sets gradient in b and y
            _mulg0.Backward(); // sets gradient in a and x
        }
    }
}

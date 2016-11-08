using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib;

namespace NnLib
{
    class Program
    {
        static Random _rnd = new Random();

        static void TraintSVM()
        {
            List<double[]> data = new List<double[]>(); 
            List<int> labels = new List<int>() ;

            data.Add(new[] {1.2, 0.7}); labels.Add(1);
            data.Add(new[] {-0.3, -0.5}); labels.Add(-1);
            data.Add(new[] {3.0, 0.1}); labels.Add(1);
            data.Add(new[] {-0.1, -1.0}); labels.Add(-1);
            data.Add(new[] {-1.0, 1.1}); labels.Add(-1);
            data.Add(new[] {2.1, -3}); labels.Add(1);
            var svm = new SVM();

            double initAcc = EvalTrainingAccuracy(data, labels, svm);

            // the learning loop
            for (var iter = 0; iter < 400; iter++)
            {
                // pick a random data point
                int i = _rnd.Next(data.Count);
                var x = new Unit(data[i][0], 0.0);
                var y = new Unit(data[i][1], 0.0);
                var label = labels[i];
                svm.LearnFrom(x, y, label);

                if (iter % 25 == 0)
                {
                    double acc = EvalTrainingAccuracy(data, labels, svm);
                    Console.WriteLine("Training accuracy at iter " + iter + ": " + acc);
                }
            }
        }

        private static double EvalTrainingAccuracy(List<double[]> data, List<int> labels, SVM svm)
        {
            var num_correct = 0;
            for (var i = 0; i < data.Count; i++)
            {
                var x = new Unit(data[i][0], 0.0);
                var y = new Unit(data[i][1], 0.0);
                var true_label = labels[i];

                // see if the prediction matches the provided label
                var predicted_label = svm.Forward(x, y).Value > 0 ? 1 : -1;
                if (predicted_label == true_label)
                {
                    num_correct++;
                }
            }
            double acc = (double)num_correct/data.Count;

            return acc;
        }

        static void Main(string[] args)
        {
            //var a = new Unit(1.0, 0.0);
            //var b = new Unit(2.0, 0.0);
            //var c = new Unit(-3.0, 0.0);
            //var x = new Unit(-1.0, 0.0);
            //var y = new Unit(3.0, 0.0);

            //// create the gates
            //var mulg0 = new MultiplyGate();
            //var mulg1 = new MultiplyGate();
            //var addg0 = new AddGate();
            //var addg1 = new AddGate();
            //var sg0 = new SigmoidGate();

            //Unit s = ForwardNeuron(mulg0, a, x, mulg1, b, y, addg0, addg1, c, sg0);
            //Console.WriteLine("circuit output: " + s.Value);

            //s.Grad = 1.0;
            //sg0.Backward(); // writes gradient into axpbypc
            //addg1.Backward(); // writes gradients into axpby and c
            //addg0.Backward(); // writes gradients into ax and by
            //mulg1.Backward(); // writes gradients into b and y
            //mulg0.Backward(); // writes gradients into a and x

            //var step_size = 0.01;
            //a.Value += step_size * a.Grad; // a.grad is -0.105
            //b.Value += step_size * b.Grad; // b.grad is 0.315
            //c.Value += step_size * c.Grad; // c.grad is 0.105
            //x.Value += step_size * x.Grad; // x.grad is 0.105
            //y.Value += step_size * y.Grad; // y.grad is 0.210

            //s = ForwardNeuron(mulg0, a, x, mulg1, b, y, addg0, addg1, c, sg0);
            //Console.WriteLine("circuit output: " + s.Value);// prints 0.8825
            //Test();


            TraintSVM();
        }

        private static Unit ForwardNeuron(MultiplyGate mulg0, Unit a, Unit x, MultiplyGate mulg1, Unit b, Unit y, AddGate addg0,
            AddGate addg1, Unit c, SigmoidGate sg0)
        {
            var ax = mulg0.Forward(a, x); // a*x = -1
            var by = mulg1.Forward(b, y); // b*y = 6
            var axpby = addg0.Forward(ax, by); // a*x + b*y = 5
            var axpbypc = addg1.Forward(axpby, c); // a*x + b*y + c = 2
            var s = sg0.Forward(axpbypc); // sig(a*x + b*y + c) = 0.8808

            return s;
        }

        private static  void Test()
        {
            double a = 1, b = 2, c = -3, x = -1, y = 3;
            var h = 0.0001;
            var a_grad = (ForwardCircuitFast(a + h, b, c, x, y) - ForwardCircuitFast(a, b, c, x, y)) / h;
            var b_grad = (ForwardCircuitFast(a, b + h, c, x, y) - ForwardCircuitFast(a, b, c, x, y)) / h;
            var c_grad = (ForwardCircuitFast(a, b, c + h, x, y) - ForwardCircuitFast(a, b, c, x, y)) / h;
            var x_grad = (ForwardCircuitFast(a, b, c, x + h, y) - ForwardCircuitFast(a, b, c, x, y)) / h;
            var y_grad = (ForwardCircuitFast(a, b, c, x, y + h) - ForwardCircuitFast(a, b, c, x, y)) / h;
            IEnumerable<double> grads = new[] {a_grad, b_grad, c_grad, x_grad, y_grad};
            Console.WriteLine(String.Join(",", grads));
        }

        private static double ForwardCircuitFast(double a, double b, double c, double x, double y)
        {
            return 1 / (1 + Math.Exp(-(a * x + b * y + c)));
        }
    }
}

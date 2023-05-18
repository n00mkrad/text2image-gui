using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StableDiffusionGui.MiscUtils
{
    public class RollingAverage
    {
        private Queue<double> _etas;
        private int _size;

        public RollingAverage(int size)
        {
            this._etas = new Queue<double>(size);
            this._size = size;
        }

        public void AddDataPoint(double dataPoint)
        {
            if (_etas.Count >= _size)
            {
                _etas.Dequeue();
            }

            _etas.Enqueue(dataPoint);
        }

        public double GetAverage()
        {
            return _etas.Average();
        }

        public void Reset()
        {
            _etas.Clear();
        }
    }
}

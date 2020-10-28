using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPhotoshop.Filters
{
    public class GrayscaleParameters : IParameters
    {
        public ParameterInfo[] GetDesсription()
        {
            return new ParameterInfo[0];
        }

        public void SetValues(double[] parameters)
        {
        }
    }
}

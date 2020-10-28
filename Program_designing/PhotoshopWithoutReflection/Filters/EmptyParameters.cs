using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhotoshopWithoutReflection.Filters
{
    public class EmptyParameters : IParameters
    {
        public ParameterInfo[] GetDesсription()
        {
            return new ParameterInfo[0];
        }

        public void SetValues(double[] values)
        {
            
        }
    }
}

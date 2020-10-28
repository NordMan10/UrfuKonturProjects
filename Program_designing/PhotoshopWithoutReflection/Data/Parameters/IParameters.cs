using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhotoshopWithoutReflection.Filters
{
    public interface IParameters
    {
        ParameterInfo[] GetDesсription();
        void SetValues(double[] values);
    }
}

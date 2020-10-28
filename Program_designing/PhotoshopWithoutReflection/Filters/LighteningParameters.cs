

namespace PhotoshopWithoutReflection.Filters
{
    public class LightningParameters : IParameters
    {
        public double Coefficient { get; set; }

        public ParameterInfo[] GetDesсription()
        {
            return new[]
            {
                new ParameterInfo { Name = "Коэффициент", MaxValue = 10, MinValue = 0, Increment = 0.1, DefaultValue = 1 }
            };
        }

        public void SetValues(double[] values)
        {
            Coefficient = values[0];
        }
    }
}

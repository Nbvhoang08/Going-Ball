using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hapiga.Core.Runtime.WeightedRandomization
{
    public class WeightedChance<T>
    {
        public T Value { get; set; }

        public int Weight { get; set; }

        public float AdjustedWeight { get; set; }

        public WeightedChance(T value, int weight)
        {
            Value = value;
            Weight = weight;
        }
    }
}

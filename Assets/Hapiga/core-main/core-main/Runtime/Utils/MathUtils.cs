using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Hapiga.Core.Runtime.Utils
{
    public static class MathUtils
    {
        public static BigInteger PowBigInt(int f, int p)
        {
            BigInteger result = 1;
            for (int i = 0; i < p; i++)
            {
                result *= f;
            }

            return result;
        }
    }
}

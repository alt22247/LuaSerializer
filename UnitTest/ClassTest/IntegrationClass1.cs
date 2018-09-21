using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1.IntegrationTest
{
    public class IntegrationClass1 : IEquatable<IntegrationClass1>
    {
        public int IntProp { get; set; }
        public int IntField;
        public bool BoolProp { get; set; }
        public bool BoolField;
        public double DoubleProp { get; set; }
        public double DoubleField;



        public bool Equals(IntegrationClass1 other)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Caching
{
    internal class SampleEntity : ICloneable
    {
        /// <summary>
        /// Gets ot sets the primary key.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets ot sets the unique key.
        /// </summary>
        public string? Code { get; set; }

        public string? Name { get; set; }

        public override string ToString()
        {
            return $"{{ID={ID}, Code={Code}, Name={Name}}}";
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}

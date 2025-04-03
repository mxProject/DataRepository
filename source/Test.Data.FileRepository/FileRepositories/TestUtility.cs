using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.FileRepositories
{
    internal static class TestUtility
    {
        static TestUtility()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        internal static Encoding DefaultEncoding => Encoding.UTF8;
    }
}

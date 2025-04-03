using mxProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Test.FileRepositories
{
    public class TestPathEx
    {
        public TestPathEx(ITestOutputHelper output)
        {
            m_Output = output;
        }

        private readonly ITestOutputHelper m_Output;

        #region ToAbsolutePath

        [Theory]
        [MemberData(nameof(ToAbsolutePathArguments))]
        public void ToAbsolutePath(string basePath, string relativePath, string expected)
        {
            var result = PathEx.ToAbsolutePath(relativePath, basePath);

            m_Output.WriteLine($"{nameof(basePath)} = {basePath}");
            m_Output.WriteLine($"{nameof(relativePath)} = {relativePath}");
            m_Output.WriteLine($"{nameof(result)} = {result}");
            m_Output.WriteLine($"{nameof(expected)} = {expected}");

            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> ToAbsolutePathArguments()
        {
            yield return new object[] { @"c:\temp\", "a.txt", @"c:\temp\a.txt" };
            yield return new object[] { @"c:\temp\", @".\a.txt", @"c:\temp\a.txt" };
            yield return new object[] { @"c:\temp\", @"..\a.txt", @"c:\a.txt" };
            yield return new object[] { @"c:\temp\", @"dir\a.txt", @"c:\temp\dir\a.txt" };
            yield return new object[] { @"c:\temp\", @".\dir\a.txt", @"c:\temp\dir\a.txt" };
            yield return new object[] { @"c:\temp\", @"..\dir\a.txt", @"c:\dir\a.txt" };

            yield return new object[] { @"c:\temp\", "%.txt", @"c:\temp\%.txt" };

            yield return new object[] { @"c:\temp\", @"d:\a.txt", @"d:\a.txt" };

            yield return new object[] { @"\\host\temp\", "a.txt", @"\\host\temp\a.txt" };

            yield return new object[] { @"c:\temp", "a.txt", @"c:\temp\a.txt" };
            yield return new object[] { @"c:\temp", @".\a.txt", @"c:\temp\a.txt" };
            yield return new object[] { @"c:\temp", @"..\a.txt", @"c:\a.txt" };
            yield return new object[] { @"c:\temp", @"dir\a.txt", @"c:\temp\dir\a.txt" };
            yield return new object[] { @"c:\temp", @".\dir\a.txt", @"c:\temp\dir\a.txt" };
            yield return new object[] { @"c:\temp", @"..\dir\a.txt", @"c:\dir\a.txt" };

            yield return new object[] { @"c:\temp", "%.txt", @"c:\temp\%.txt" };

            yield return new object[] { @"c:\temp", @"d:\a.txt", @"d:\a.txt" };

            yield return new object[] { @"\\host\temp", "a.txt", @"\\host\temp\a.txt" };
        }

        #endregion

        #region ToRalativePath

        [Theory]
        [MemberData(nameof(ToAbsolutePathArguments))]
        public void ToRalativePath(string basePath, string expected, string absolutePath)
        {
            var result = PathEx.ToRalativePath(absolutePath, basePath);

            m_Output.WriteLine($"{nameof(basePath)} = {basePath}");
            m_Output.WriteLine($"{nameof(absolutePath)} = {absolutePath}");
            m_Output.WriteLine($"{nameof(result)} = {result}");
            m_Output.WriteLine($"{nameof(expected)} = {expected}");

            if (expected.StartsWith(@".\"))
            {
                Assert.Equal(expected.Substring(2), result);
            }
            else
            {
                Assert.Equal(expected, result);
            }
        }

        #endregion
    }
}

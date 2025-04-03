using mxProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Test.FileRepositories
{
    internal class SampleJsonSerializer : IEntityFileSerializer
    {
        internal static readonly SampleJsonSerializer Instance = new SampleJsonSerializer();

        public TEntity Deserialize<TEntity>(Stream stream, Encoding encoding)
        {
            using var reader = new StreamReader(stream, encoding, leaveOpen: true);

            var json = reader.ReadToEnd();

            return System.Text.Json.JsonSerializer.Deserialize<TEntity>(json)!;
        }

        public void Serialize<TEntity>(TEntity entity, Stream stream, Encoding encoding)
        {
            var options = new System.Text.Json.JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };

            var json = System.Text.Json.JsonSerializer.Serialize(entity, options);

            using (var writer = new StreamWriter(stream, encoding, leaveOpen: true))
            {
                writer.Write(json);
                writer.Flush();
            }
        }

        //internal TEntity Deserialize<TEntity>(string filePath, Encoding? encoding = null)
        //{
        //    if (encoding == null)
        //    {
        //        using var stream = File.OpenRead(filePath);

        //        return System.Text.Json.JsonSerializer.Deserialize<TEntity>(stream)!;
        //    }
        //    else
        //    {
        //        var json = File.ReadAllText(filePath, encoding);

        //        return System.Text.Json.JsonSerializer.Deserialize<TEntity>(json)!;
        //    }
        //}

        //internal string Serialize<TEntity>(TEntity entity)
        //{
        //    var options = new System.Text.Json.JsonSerializerOptions()
        //    {
        //        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        //        WriteIndented = true
        //    };

        //    return System.Text.Json.JsonSerializer.Serialize(entity, options);
        //}
    }
}

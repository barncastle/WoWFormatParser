using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using Newtonsoft.Json;
using WoWFormatParser.Helpers;
using WoWFormatParser.Serializer.Converters;
using WoWFormatParser.Structures;

namespace WoWFormatParser.Serializer
{
    internal class Serializer
    {
        public SerializerOptions Options { get; private set; }

        private readonly JsonSerializerSettings _serializerSettings;

        public Serializer(SerializerOptions options = null)
        {
            Options = options ?? new SerializerOptions();
            Options.ValidateAndCheck();

            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = Options.RenameIgnoreResolver,
                Converters = new List<JsonConverter>()
                {
                    new ByteArrayConverter(),
                    new MultiArrayConverter(),
                    new StringDescriptionConverter(),
                    new Newtonsoft.Json.Converters.StringEnumConverter()
                },
                NullValueHandling = Options.IgnoreNullOrEmpty ? NullValueHandling.Ignore : NullValueHandling.Include
            };
        }


        #region Processing
        public IEnumerable<string> Serialize(IEnumerable<IFormat> objs)
        {
            foreach (var obj in objs)
                yield return Serialize(obj);
        }

        public string Serialize(IFormat obj)
        {
            Formatting formatting = Options.Indent ? Formatting.Indented : Formatting.None;
            string json = JsonConvert.SerializeObject(obj, formatting, _serializerSettings);

            switch (Options.Compression)
            {
                case CompressionFormat.None:
                    return json;
                case CompressionFormat.GZip:
                    return Convert.ToBase64String(GZipCompress(json));
                case CompressionFormat.Brotli:
                    return Convert.ToBase64String(BrotliCompress(json));
                default:
                    return string.Empty;
            }
        }

        public void Export(IEnumerable<IFormat> objs)
        {
            foreach (var obj in objs)
                Export(Utils.GetExportFileName(obj.FileName, Options.Compression), obj);
        }

        public void Export(string filename, IFormat obj)
        {
            var formatting = Options.Indent ? Formatting.Indented : Formatting.None;
            string json = JsonConvert.SerializeObject(obj, formatting, _serializerSettings);

            switch (Options.Compression)
            {
                case CompressionFormat.None:
                    Save(json, filename);
                    break;
                case CompressionFormat.GZip:
                    GZipCompress(json, filename, true);
                    break;
                case CompressionFormat.Brotli:
                    BrotliCompress(json, filename, true);
                    break;
            }
        }
        #endregion

        #region Saving
        private void Save(string json, string name)
        {
            string filename = Path.Combine(Options.OutputDirectory, name + ".json");
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(json);
                fs.Write(buffer, 0, buffer.Length);
                fs.Flush();
            }
        }

        private void Save(MemoryStream ms, string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                ms.CopyTo(fs);
                fs.Flush();
            }
        }
        #endregion

        #region Compression
        private byte[] GZipCompress(string json, string name = "", bool export = false)
        {
            string filename = Path.Combine(Options.OutputDirectory, name + ".gz");

            using (MemoryStream ms = new MemoryStream())
            using (GZipStream gzip = new GZipStream(ms, CompressionLevel.Fastest))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(json);
                gzip.Write(buffer, 0, buffer.Length);

                if (export)
                    Save(ms, filename);

                return ms.ToArray();
            }
        }

        private byte[] BrotliCompress(string json, string name = "", bool export = false)
        {
            string filename = Path.Combine(Options.OutputDirectory, name + ".brot");

            using (MemoryStream ms = new MemoryStream())
            using (BrotliStream brotli = new BrotliStream(ms, CompressionLevel.Fastest))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(json);
                brotli.Write(buffer, 0, buffer.Length);

                if (export)
                    Save(ms, filename);

                return ms.ToArray();
            }
        }
        #endregion
    }
}

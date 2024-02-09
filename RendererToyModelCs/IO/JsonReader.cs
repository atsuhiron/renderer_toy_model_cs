using System.Text;
using System.Text.Json;
using RendererToyModelCs.WorldObject;

namespace RendererToyModelCs.IO
{
    public class JsonReader
    {
        private static readonly Encoding encoding_s = Encoding.GetEncoding("UTF-8");
        
        public static JsonElement ReadFile(string filePath)
        {
            var jsonStr = File.ReadAllText(filePath);
            var bytes = encoding_s.GetBytes(jsonStr);
            var reader = new Utf8JsonReader(new ReadOnlySpan<byte>(bytes));
            return JsonElement.ParseValue(ref reader);
        }
    }
}

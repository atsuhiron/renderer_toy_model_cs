using System.Text;
using System.Text.Json;

namespace RendererToyModelCs.IO
{
    public class JsonReader
    {
        private static readonly Encoding encoding_s = Encoding.GetEncoding("UTF-8");
        
        public static Dictionary<string, dynamic?> ReadFile(string filePath)
        {
            var jsonStr = File.ReadAllText(filePath);
            return ParseJson(jsonStr);
        }

        // 参考文献 : https://yaspage.com/cs-json-to-dictionary/
        // JSON文字列をDictionary<string, dynamic>型に変換するメソッド
        public static Dictionary<string, dynamic?> ParseJson(string json)
        {
            // とりあえずJSON文字列をDictionary<string, JsonElement>型に変換
            var dic = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);
            if (dic == null)
            {
                return [];
            }

            // JsonElementから値を取り出してdynamic型に入れてDictionary<string, dynamic>型で返す
            return dic
                .Select(d => new { key = d.Key, value = ParseJsonElement(d.Value) })
                .ToDictionary(a => a.key, a => a.value);
        }

        // JsonElementの型を調べて変換するメソッド
        private static dynamic? ParseJsonElement(JsonElement elem) =>
            // データの種類によって値を取得する処理を変える
            elem.ValueKind switch
            {
                JsonValueKind.String => elem.GetString(),
                JsonValueKind.Number => elem.GetDecimal(),
                JsonValueKind.False => false,
                JsonValueKind.True => true,
                JsonValueKind.Array => elem.EnumerateArray().Select(ParseJsonElement).ToList(),
                JsonValueKind.Null => null,
                JsonValueKind.Object => ParseJson(elem.GetRawText()),
                JsonValueKind.Undefined => throw new NotImplementedException(),
                _ => throw new NotSupportedException(),
            };
    }
}

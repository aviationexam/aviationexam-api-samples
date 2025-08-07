using System.Buffers.Text;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Aviationexam.Api.Common.Helpers;

internal sealed class BigIntJsonConverter : JsonConverter<long>
{
    public override long Read(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options
    )
    {
        if (reader.TokenType is JsonTokenType.Number)
        {
            return reader.GetInt64();
        }

        var value = reader.ValueSpan;

        if (long.TryParse(value, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out var result))
        {
            return result;
        }

        throw new JsonException("Unable to parse long value from JSON.");
    }

    public override void Write(
        Utf8JsonWriter writer, long value, JsonSerializerOptions options
    )
    {
        Span<byte> buffer = stackalloc byte[20];
        if (Utf8Formatter.TryFormat(value, buffer, out var bytesWritten))
        {
            writer.WriteStringValue(buffer[..bytesWritten]);
            return;
        }

        throw new JsonException("Unable to serialize long value to JSON.");
    }
}

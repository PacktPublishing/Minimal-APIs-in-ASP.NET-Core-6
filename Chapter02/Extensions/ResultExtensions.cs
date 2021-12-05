using System.Net.Mime;
using System.Text;
using System.Xml.Serialization;

namespace Chapter02.Extensions;

public static class ResultExtensions
{
    public static IResult Xml(this IResultExtensions resultExtensions, object value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new XmlResult(value);
    }
}

public class XmlResult : IResult
{
    private readonly object value;

    public XmlResult(object value)
    {
        this.value = value;
    }

    public Task ExecuteAsync(HttpContext httpContext)
    {
        using var writer = new StringWriter();
        var serializer = new XmlSerializer(value.GetType());
        serializer.Serialize(writer, value);

        var xml = writer.ToString();
        httpContext.Response.ContentType = MediaTypeNames.Application.Xml;
        httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(xml);

        return httpContext.Response.WriteAsync(xml);
    }
}
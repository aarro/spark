﻿#if NETSTANDARD2_0
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Spark.Core;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Engine.Formatters
{
    public class ResourceXmlInputFormatter : TextInputFormatter
    {
        public ResourceXmlInputFormatter()
        {
            SupportedEncodings.Clear();
            SupportedEncodings.Add(Encoding.UTF8);

            SupportedMediaTypes.Add("application/xml");
            SupportedMediaTypes.Add("application/fhir+xml");
            SupportedMediaTypes.Add("application/xml+fhir");
            SupportedMediaTypes.Add("text/xml");
            SupportedMediaTypes.Add("text/xml+fhir");
        }

        protected override bool CanReadType(Type type)
        {
            return typeof(Resource).IsAssignableFrom(type);
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));
            if (encoding != Encoding.UTF8)
                throw Error.BadRequest("FHIR supports UTF-8 encoding exclusively, not " + encoding.WebName);

            try
            {
                using (TextReader reader = context.ReaderFactory(context.HttpContext.Request.Body, encoding))
                {
                    FhirXmlParser parser = context.HttpContext.RequestServices.GetRequiredService<FhirXmlParser>();
                    return await InputFormatterResult.SuccessAsync(parser.Parse(await reader.ReadToEndAsync()));
                }
            }
            catch (FormatException exception)
            {
                throw Error.BadRequest($"Body parsing failed: {exception.Message}");
            }
        }
    }
}
#endif
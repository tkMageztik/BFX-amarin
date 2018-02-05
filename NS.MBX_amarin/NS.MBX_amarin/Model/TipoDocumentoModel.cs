// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using QuickType;
//
//    var data = TipoDocumentoModel.FromJson(jsonString);

namespace NS.MBX_amarin.Model
{
    using System;
    using System.Net;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public partial class TipoDocumentoModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("tipDoc")]
        public string TipDoc { get; set; }

        [JsonProperty("desDoc")]
        public string DesDoc { get; set; }
    }

    public partial class TipoDocumentoModel
    {
        public static TipoDocumentoModel[] FromJson(string json) => JsonConvert.DeserializeObject<TipoDocumentoModel[]>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this TipoDocumentoModel[] self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    public class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}

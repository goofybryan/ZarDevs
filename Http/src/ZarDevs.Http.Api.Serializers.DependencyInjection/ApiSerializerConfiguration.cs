using ZarDevs.DependencyInjection;

namespace ZarDevs.Http.Api.Serializers
{
    /// <summary>
    /// Api serialization configuration.
    /// </summary>
    public static class ApiSerializerConfiguration
    {
        #region Methods

        /// <summary>
        /// Configure the <see cref="IApiCommandContentSerializer"/> serializers and <see cref="IApiCommandContentDeserializer"/> deserializers.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IDependencyBuilder ConfigureApi(this IDependencyBuilder builder)
        {
            builder.Bind<ApiCommandContentSerializerFormUrlEncoded>().Resolve<IApiCommandContentSerializer>().WithKey(ApiSerializer.FormUrlEncoded);
            builder.Bind<ApiCommandContentSerializerJson>().Resolve<IApiCommandContentSerializer>().WithKey(ApiSerializer.Json);
            builder.Bind<ApiCommandContentSerializerString>().Resolve<IApiCommandContentSerializer>().WithKey(ApiSerializer.String);
            builder.Bind<ApiCommandContentSerializerXml>().Resolve<IApiCommandContentSerializer>().WithKey(ApiSerializer.Xml);

            builder.Bind<ApiCommandContentSerializerFormUrlEncoded>().Resolve<IApiCommandContentDeserializer>().WithKey(ApiSerializer.FormUrlEncoded);
            builder.Bind<ApiCommandContentSerializerJson>().Resolve<IApiCommandContentDeserializer>().WithKey(ApiSerializer.Json);
            builder.Bind<ApiCommandContentSerializerString>().Resolve<IApiCommandContentDeserializer>().WithKey(ApiSerializer.String);
            builder.Bind<ApiCommandContentSerializerXml>().Resolve<IApiCommandContentDeserializer>().WithKey(ApiSerializer.Xml);

            return builder;
        }

        #endregion Methods

        #region Enums

        /// <summary>
        /// Api serializers binded.
        /// </summary>
        public enum ApiSerializer
        {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            Json,
            Xml,
            FormUrlEncoded,
            String
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        }

        #endregion Enums
    }
}
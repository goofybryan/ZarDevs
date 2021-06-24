using ZarDevs.DependencyInjection;

namespace ZarDevs.Http.Api.Serializers
{
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
            builder.Bind<IApiCommandContentSerializer>().To<ApiCommandContentSerializerFormUrlEncoded>().WithKey(ApiSerializer.FormUrlEncoded);
            builder.Bind<IApiCommandContentSerializer>().To<ApiCommandContentSerializerJson>().WithKey(ApiSerializer.Json);
            builder.Bind<IApiCommandContentSerializer>().To<ApiCommandContentSerializerString>().WithKey(ApiSerializer.String);
            builder.Bind<IApiCommandContentSerializer>().To<ApiCommandContentSerializerXml>().WithKey(ApiSerializer.Xml);

            builder.Bind<IApiCommandContentDeserializer>().To<ApiCommandContentSerializerFormUrlEncoded>().WithKey(ApiSerializer.FormUrlEncoded);
            builder.Bind<IApiCommandContentDeserializer>().To<ApiCommandContentSerializerJson>().WithKey(ApiSerializer.Json);
            builder.Bind<IApiCommandContentDeserializer>().To<ApiCommandContentSerializerString>().WithKey(ApiSerializer.String);
            builder.Bind<IApiCommandContentDeserializer>().To<ApiCommandContentSerializerXml>().WithKey(ApiSerializer.Xml);

            return builder;
        }

        #endregion Methods

        #region Enums

        /// <summary>
        /// Api serializers binded.
        /// </summary>
        public enum ApiSerializer
        {
            Json,
            Xml,
            FormUrlEncoded,
            String
        }

        #endregion Enums
    }
}
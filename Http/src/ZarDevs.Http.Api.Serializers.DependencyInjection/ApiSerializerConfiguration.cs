﻿using ZarDevs.DependencyInjection;

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
        public static IDependencyBuilder ConfigureApiSerializers(this IDependencyBuilder builder)
        {
            builder.Bind<DefaultFormUrlEncodedContentParser>().Resolve<IDefaultFormUrlEncodedContentParser>().WithKey(ApiSerializer.FormUrlEncoded);
            builder.Bind<ApiCommandContentSerializerFormUrlEncoded>().Resolve<IApiCommandContentSerializer>().WithKey(ApiSerializer.FormUrlEncoded);
            builder.Bind<ApiCommandContentSerializerJson>().Resolve<IApiCommandContentSerializer>().WithKey(ApiSerializer.Json);
            builder.Bind<ApiCommandContentSerializerString>().Resolve<IApiCommandContentSerializer>().WithKey(ApiSerializer.String);
            builder.Bind<ApiCommandContentSerializerXml>().Resolve<IApiCommandContentSerializer>().WithKey(ApiSerializer.Xml);

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
            /// <summary>
            /// Key for <see cref="ApiCommandContentSerializerJson"/> serializer
            /// </summary>
            Json,
            /// <summary>
            /// Key for <see cref="ApiCommandContentSerializerXml"/> serializer
            /// </summary>
            Xml,
            /// <summary>
            /// Key for <see cref="ApiCommandContentSerializerFormUrlEncoded"/> serializer
            /// </summary>
            FormUrlEncoded,
            /// <summary>
            /// Key for <see cref="ApiCommandContentSerializerString"/> serializer
            /// </summary>
            String
        }

        #endregion Enums
    }
}
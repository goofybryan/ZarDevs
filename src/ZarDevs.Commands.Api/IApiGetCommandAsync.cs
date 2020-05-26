using System;
using System.Collections.Generic;
using System.Text;

namespace ZarDevs.Commands.Api
{
    public interface IApiGetCommandAsync : ICommandAsync<ApiCommandRequest, ApiCommandJsonResponse>
    {
    }

    /*
     * 
            Bind<ICommandAsync<ApiCommandRequest, ApiCommandJsonResponse>>().To<ApiGetCommandAsync>().Named(HttpRequestType.Get.GetBindingName());
            Bind<ICommandAsync<ApiCommandRequest, ApiCommandJsonResponse>>().To<ApiPutCommandAsync>().Named(HttpRequestType.Put.GetBindingName());
            Bind<ICommandAsync<ApiCommandRequest, ApiCommandJsonResponse>>().To<ApiPostCommandAsync>().Named(HttpRequestType.Post.GetBindingName());
            */
}

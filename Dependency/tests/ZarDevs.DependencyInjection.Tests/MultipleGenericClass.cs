using System;
#nullable enable

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZarDevs.DependencyInjection.Tests
{
    public interface IGeneric
    { }

    public interface IMultipleGenericClass<TGeneric,TNonGeneric> where TGeneric : IGeneric where TNonGeneric : NonGenericClass?
    { }

    public class MultipleGenericWithUnboundConstraintsClass<TGeneric, TNonGeneric> : IMultipleGenericClass<TGeneric, TNonGeneric> where TGeneric : IGeneric where TNonGeneric : NonGenericClass?, new()
    {
    }

    public class MultipleGenericWithConstrantsClass : IMultipleGenericClass<IGeneric, NonGenericClass>
    {
    }

    public class GenericClass : IGeneric
    { }

    public class NonGenericClass
    {

    }
}

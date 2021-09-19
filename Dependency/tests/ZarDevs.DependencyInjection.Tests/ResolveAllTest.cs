using System;
using System.Collections.Generic;
using System.Text;

namespace ZarDevs.DependencyInjection.Tests
{
    public interface IResolveAllTest { }
    public interface IResolveAllTest2 { }
    public interface IResolveAllTestBase { }

    internal abstract class ResolveAllTestBase : IResolveAllTestBase { }

    internal class ResolveAllTest : ResolveAllTestBase, IResolveAllTest, IResolveAllTest2 { }

    internal class ResolveAllTest2 : ResolveAllTestBase, IResolveAllTest, IResolveAllTest2 { }
}

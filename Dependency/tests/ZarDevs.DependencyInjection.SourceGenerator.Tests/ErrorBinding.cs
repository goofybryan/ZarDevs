using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZarDevs.DependencyInjection.SourceGenerator.Tests
{
    internal class ErrorBinding
    {
        public void Method1() { }

        public void Method2(IDependencyBuilder builder, int value) { }
    }
}

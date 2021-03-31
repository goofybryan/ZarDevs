using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace ZarDevs.Http.Tests.WebServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        #region Fields

        private readonly ILogger<TestController> _logger;
        private readonly ITestFactory _testFactory;

        #endregion Fields

        #region Constructors

        public TestController(ITestFactory testFactory, ILogger<TestController> logger)
        {
            _testFactory = testFactory ?? throw new ArgumentNullException(nameof(testFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion Constructors

        #region Methods

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _logger.LogDebug($"Delete called, deleting test object id={id}.");
            _testFactory.Delete(id);
        }

        [HttpGet("{id}")]
        public Test Get(int id)
        {
            var test = _testFactory.Create(id);
            _logger.LogDebug($"Get called, returning new test object id={test?.Id}.");
            return test;
        }

        [HttpGet]
        public IEnumerable<Test> Index()
        {
            for (int i = 0; i < 10; i++)
            {
                yield return _testFactory.Create();
            }
        }

        [HttpPatch("{id}")]
        public void Patch(int id, [FromBody] Test item)
        {
            _logger.LogDebug($"Patch called, changing test object id={id}.");
            _testFactory.Change(id, item);
        }

        [HttpPut]
        public void Put([FromBody] Test item)
        {
            _logger.LogDebug($"Put called, adding test object id={item.Id}.");
            _testFactory.Add(item);
        }

        #endregion Methods
    }
}
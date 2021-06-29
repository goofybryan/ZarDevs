using System;
using System.Collections.Generic;

namespace ZarDevs.Http.Tests.WebServer
{
    public interface ITestFactory
    {
        #region Methods

        void Add(Test value);

        void Change(int id, Test value);

        Test Create();

        Test Create(int id);

        void Delete(int id);

        Test GetAdded(int id);

        Test GetChanged(int id);

        Test GetCreated(int id);

        Test GetUpdated(int id);

        bool IsDeleted(int id);

        void Update(Test test);

        #endregion Methods
    }

    public class TestFactory : ITestFactory
    {
        #region Fields

        private readonly IDictionary<int, Test> _added;
        private readonly IDictionary<int, Test> _changed;
        private readonly IDictionary<int, Test> _created;
        private readonly IList<int> _deleted;
        private readonly IDictionary<int, Test> _update;

        #endregion Fields

        #region Constructors

        private TestFactory()
        {
            _added = new Dictionary<int, Test>();
            _created = new Dictionary<int, Test>();
            _deleted = new List<int>();
            _changed = new Dictionary<int, Test>();
            _update = new Dictionary<int, Test>();
        }

        #endregion Constructors

        #region Properties

        public static ITestFactory Instance { get; } = new TestFactory();

        #endregion Properties

        #region Methods

        public void Add(Test value)
        {
            _added.Add(value.Id, value);
        }

        public void Change(int id, Test value)
        {
            _changed.Add(id, value);
        }

        public Test Create()
        {
            int id = new Random().Next(0, 100000);

            if (_created.ContainsKey(id))
                return Create();

            var newItem = new Test { Id = id };
            _created.Add(newItem.Id, newItem);
            return newItem;
        }

        public Test Create(int id)
        {
            var test = GetCreated(id);

            if (test == null)
            {
                test = new Test { Id = id };
                _created.Add(id, test);
            }

            return test;
        }

        public void Delete(int id)
        {
            _deleted.Add(id);
        }

        public Test GetAdded(int id) => _added.TryGetValue(id, out var value) ? value : null;

        public Test GetChanged(int id) => _changed.TryGetValue(id, out var value) ? value : null;

        public Test GetCreated(int id) => _created.TryGetValue(id, out var value) ? value : null;

        public Test GetUpdated(int id) => _update.TryGetValue(id, out var value) ? value : null;

        public bool IsDeleted(int id) => _deleted.Contains(id);

        public void Update(Test test)
        {
            _update.Add(test.Id, test);
        }

        #endregion Methods
    }
}
using ZarDevs.Core.Models;
using System.Collections.ObjectModel;

namespace ZarDevs.Core.Models
{
    public class ListViewModelBase<T> : ModelBase
    {
        #region Fields

        private ObservableCollection<T> _items;

        #endregion Fields

        #region Properties

        public ObservableCollection<T> Items
        {
            get { return _items; }
            set { SetValue(ref _items, value); }
        }

        #endregion Properties
    }
}
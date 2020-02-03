using System;

namespace ZarDevs.Core.Models
{
    public class ViewModelBase<T> : ModelBase
    {
        #region Fields

        private T _model;

        #endregion Fields

        #region Properties

        public T Model
        {
            get { return _model; }
            set { SetValue(ref _model, value); }
        }

        #endregion Properties
    }
}
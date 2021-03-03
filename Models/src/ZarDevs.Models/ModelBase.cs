using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ZarDevs.Core.Models
{
    public abstract class ModelBase : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Methods

        protected void OpPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetValue<T>(ref T oldValue, T newValue, [CallerMemberName] string propertyName = "")
        {
            oldValue = newValue;
            OpPropertyChanged(propertyName);
        }

        #endregion Methods
    }
}
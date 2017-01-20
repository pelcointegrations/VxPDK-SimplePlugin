using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverlayDrawings.Model
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// The display name of the view
        /// </summary>
        public abstract string DisplayName { get; set; }

        #region Property Debugging Aids

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propName)
        {
            // Verify if the provided property name matches the real
            // instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propName] == null)
            {
                string msg = String.Format("Invalid property name: {0}", propName);
                if (ThrowOnInvalidPropertyName)
                {
                    throw new ArgumentException(msg);
                }
                else
                {
                    Debug.Fail(msg);
                }
            }
        }

        /// <summary>
        /// Returns weather an exception should be thrown, or if a Debug.Fail() is used
        /// when an invald property name is passed to the VerifyPropertyName method.
        /// The default value is false but subclasses can override this functionality.
        /// </summary>
        protected virtual bool ThrowOnInvalidPropertyName { get; private set; }

        #endregion

        #region INotifyProperyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChangedEvent.
        /// </summary>
        /// <param name="propName"></param>
        protected virtual void OnPropertyChanged(string propName)
        {
            VerifyPropertyName(propName);

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propName);
                handler(this, e);
            }
        }

        #endregion
    }
}

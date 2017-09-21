using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostCore.Collections
{
    public class ExternalReferenceAwareObservableCollection<TItems, TReference> : ObservableCollection<TItems> where TReference : class
    {
        #region Fields

        private TReference _externalReference;

        #endregion

        #region Events

        #region ExternalReferenceChanged

        public virtual event EventHandler ExternalReferenceChanged;

        protected void OnExternalReferenceChanged()
        {
            if (ExternalReferenceChanged == null)
                return;

            ExternalReferenceChanged(this, EventArgs.Empty);
        }

        #endregion

        #endregion

        #region Properties

        public TReference ExternalReference
        {
            get { return _externalReference; }
            set
            {
                _externalReference = value;
                OnExternalReferenceChanged();
            }
        }

        #endregion

        #region Constructors and Initialization

        public ExternalReferenceAwareObservableCollection(TReference externalReference = null)
            : base()
        {
            _externalReference = externalReference;
        }

        public ExternalReferenceAwareObservableCollection(IEnumerable<TItems> collection, TReference externalReference = null)
            : base(collection)
        {
            _externalReference = externalReference;
        }

        public ExternalReferenceAwareObservableCollection(List<TItems> list, TReference externalReference = null)
            : base(list)
        {
            _externalReference = externalReference;
        }

        #endregion
    }

}

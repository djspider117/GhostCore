using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostCore.Collections
{
    public class LookupTable<TColumnType, TRowType, TValueType>
    {
        private Dictionary<TColumnType, Dictionary<TRowType, TValueType>> _internalCollection;

        public Dictionary<TRowType, TValueType> this[TColumnType type]
        {
            get
            {
                if (!_internalCollection.ContainsKey(type))
                    throw new ArgumentException("A column of this type is not registered.", "column");

                return _internalCollection[type];
            }
        }

        public LookupTable()
        {
            _internalCollection = new Dictionary<TColumnType, Dictionary<TRowType, TValueType>>();
        }

        public void RegisterColumn(TColumnType column)
        {
            if (_internalCollection == null)
                throw new MemberAccessException("Lookup table has been corupted, internal collections are invalid", new ArgumentNullException("_internalCollection"));

            if (_internalCollection.ContainsKey(column))
                throw new ArgumentException("A column of the same type already exists", "column");

            _internalCollection.Add(column, new Dictionary<TRowType, TValueType>());
        }

        public void RegisterRow(TColumnType column, TRowType row, TValueType optionalValue = default(TValueType))
        {
            if (!_internalCollection.ContainsKey(column))
                throw new ArgumentException("A column of this type is not registered.", "column");

            _internalCollection[column].Add(row, optionalValue);
        }

    }

}

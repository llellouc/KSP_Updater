﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace KSPUpdater.Common
{
    public class ObservableDictionary<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue>,
        INotifyCollectionChanged, INotifyPropertyChanged
    {
		#region Fields
		private readonly IDictionary<TKey, TValue> _dictionary;
		#endregion

		#region Events
		public event NotifyCollectionChangedEventHandler CollectionChanged = (sender, args) => { };
        public event PropertyChangedEventHandler PropertyChanged = (sender, args) => { };
		#endregion

		public ObservableDictionary() : this(new Dictionary<TKey, TValue>())
		{
		}

		public ObservableDictionary(IDictionary<TKey, TValue> dictionary)
		{
			this._dictionary = dictionary;
		}

		private void AddWithNotification(KeyValuePair<TKey, TValue> item)
		{
			AddWithNotification(item.Key, item.Value);
		}

        private void AddWithNotification(TKey key, TValue value)
		{
			_dictionary.Add(key, value);

			CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, value)));
			PropertyChanged(this, new PropertyChangedEventArgs("Count"));
			PropertyChanged(this, new PropertyChangedEventArgs("Keys"));
			PropertyChanged(this, new PropertyChangedEventArgs("Values"));
		}

        private bool RemoveWithNotification(TKey key)
		{
			TValue value;
			if (_dictionary.TryGetValue(key, out value) && _dictionary.Remove(key))
			{
				CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new KeyValuePair<TKey, TValue>(key, value)));
				PropertyChanged(this, new PropertyChangedEventArgs("Count"));
				PropertyChanged(this, new PropertyChangedEventArgs("Keys"));
				PropertyChanged(this, new PropertyChangedEventArgs("Values"));

				return true;
			}
            return false;
		}

        private void UpdateWithNotification(TKey key, TValue value)
		{
			TValue existing;
			if (_dictionary.TryGetValue(key, out existing))
			{
				_dictionary[key] = value;

				CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace,
					new KeyValuePair<TKey, TValue>(key, value),
					new KeyValuePair<TKey, TValue>(key, existing)));
				PropertyChanged(this, new PropertyChangedEventArgs("Values"));
			}
			else
			{
				AddWithNotification(key, value);
			}
		}

		protected void RaisePropertyChanged(PropertyChangedEventArgs args)
		{
			PropertyChanged(this, args);
		}

		#region IDictionary<TKey,TValue> Members

		public void Add(TKey key, TValue value)
		{
			AddWithNotification(key, value);
		}

		public bool ContainsKey(TKey key)
		{
			return _dictionary.ContainsKey(key);
		}

		public ICollection<TKey> Keys
		{
			get { return _dictionary.Keys; }
		}

		public bool Remove(TKey key)
		{
			return RemoveWithNotification(key);
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return _dictionary.TryGetValue(key, out value);
		}

		public ICollection<TValue> Values
		{
			get { return _dictionary.Values; }
		}

		public TValue this[TKey key]
		{
			get { return _dictionary[key]; }
			set { UpdateWithNotification(key, value); }
		}

		#endregion

		#region ICollection<KeyValuePair<TKey,TValue>> Members

		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
		{
			AddWithNotification(item);
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Clear()
		{
			((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).Clear();

			CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			PropertyChanged(this, new PropertyChangedEventArgs("Count"));
			PropertyChanged(this, new PropertyChangedEventArgs("Keys"));
			PropertyChanged(this, new PropertyChangedEventArgs("Values"));
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
		{
			return ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).Contains(item);
		}

		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).CopyTo(array, arrayIndex);
		}

		int ICollection<KeyValuePair<TKey, TValue>>.Count
		{
			get { return ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).Count; }
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
		{
			get { return ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).IsReadOnly; }
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
		{
			return RemoveWithNotification(item.Key);
		}

		#endregion

		#region IEnumerable<KeyValuePair<TKey,TValue>> Members

		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			return ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).GetEnumerator();
		}

		#endregion
	}
}

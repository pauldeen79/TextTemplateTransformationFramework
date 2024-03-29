﻿using System.Collections;
using System.Collections.Generic;

namespace TextTemplateTransformationFramework.Common.Default
{
    public abstract class ContextBase : IDictionary<string, object>
    {
        private readonly IDictionary<string, object> _state = new Dictionary<string, object>();

        /// <summary>
        /// Gets an generic collection containing the keys of the generic dictionary.
        /// </summary>
        public ICollection<string> Keys => _state.Keys;

        /// <summary>
        /// Gets an generic collection containing the values in the generic dictionary.
        /// </summary>
        public ICollection<object> Values => _state.Values;

        /// <summary>
        /// Gets the number of elements contained in the generic collection.
        /// </summary>
        public int Count => _state.Count;

        /// <summary>
        /// Gets a value indicating whether the generic collection is read-only.
        /// </summary>
        public bool IsReadOnly => _state.IsReadOnly;

        /// <summary>
        /// Gets or sets the <see cref="object"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="object"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public object this[string key]
        {
            get => _state[key];
            set => _state[key] = value;
        }

        /// <summary>
        /// Determines whether the generic dictionary contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the generic dictionary.</param>
        /// <returns>
        /// true if the generic dictionary contains an element with the key; otherwise, false.
        /// </returns>
        public bool ContainsKey(string key) => _state.ContainsKey(key);

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns>
        /// true if the object that implements generic dictionary contains an element with the specified key; otherwise, false.
        /// </returns>
        public bool TryGetValue(string key, out object value) => _state.TryGetValue(key, out value);

        /// <summary>
        /// Adds an element with the provided key and value to the generic dictionary.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        public void Add(string key, object value) => _state.Add(key, value);

        /// <summary>
        /// Adds an item to the generic collection.
        /// </summary>
        /// <param name="item">The object to add to the generic collection.</param>
        public void Add(KeyValuePair<string, object> item) => _state.Add(item);

        /// <summary>
        /// Removes all items from the generic collection.
        /// </summary>
        public void Clear() => _state.Clear();

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="item">The object to locate in the generic collection.</param>
        /// <returns>
        /// true if <paramref name="item">item</paramref> is found in the generic collection; otherwise, false.
        /// </returns>
        public bool Contains(KeyValuePair<string, object> item) => _state.Contains(item);

        /// <summary>
        /// Copies the elements of the generic collection to an <see cref="Array"></see>, starting at a particular <see cref="Array"></see> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"></see> that is the destination of the elements copied from generic collection. The <see cref="Array"></see> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) => _state.CopyTo(array, arrayIndex);

        /// <summary>
        /// Removes the first occurrence of a specific object from the generic collection.
        /// </summary>
        /// <param name="item">The object to remove from the generic collection.</param>
        /// <returns>
        /// true if <paramref name="item">item</paramref> was successfully removed from the generic collection; otherwise, false. This method also returns false if <paramref name="item">item</paramref> is not found in the original generic collection.
        /// </returns>
        public bool Remove(KeyValuePair<string, object> item) => _state.Remove(item);

        /// <summary>
        /// Removes the element with the specified key from the generic dictionary.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>
        /// true if the element is successfully removed; otherwise, false.  This method also returns false if <paramref name="key">key</paramref> was not found in the original generic dictionary.
        /// </returns>
        public bool Remove(string key) => _state.Remove(key);

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => _state.GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_state).GetEnumerator();
    }
}

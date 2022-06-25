﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses
{
    public class DynamicDictionary<TKey, TValue> : Dictionary<TKey, List<TValue>>
    {

        public DynamicDictionary() : base()
        {
        }

        public DynamicDictionary(int capacity) : base(capacity)
        {
        }

        /// <summary>
        /// Adds an element with the specified key and value into the MultiMap. 
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add.</param>
        public void Add(TKey key, TValue value)
        {
            List<TValue> valueList;

            if (TryGetValue(key, out valueList))
            {
                valueList.Add(value);
            }
            else
            {
                valueList = new List<TValue>();
                valueList.Add(value);
                Add(key, valueList);
            }
        }

        /// <summary>
        /// Removes first occurence of an element with a specified key and value.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <param name="value">The value of the element to remove.</param>
        /// <returns>true if the an element is removed;
        /// false if the key or the value were not found.</returns>
        public bool Remove(TKey key, TValue value)
        {
            List<TValue> valueList;

            if (TryGetValue(key, out valueList))
            {
                if (valueList.Remove(value))
                {
                    if (valueList.Count == 0)
                    {
                        Remove(key);
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Removes all occurences of elements with a specified key and value.
        /// </summary>
        /// <param name="key">The key of the elements to remove.</param>
        /// <param name="value">The value of the elements to remove.</param>
        /// <returns>Number of elements removed.</returns>
        public int RemoveAll(TKey key, TValue value)
        {
            List<TValue> valueList;
            int n = 0;

            if (TryGetValue(key, out valueList))
            {
                while (valueList.Remove(value))
                {
                    n++;
                }
                if (valueList.Count == 0)
                {
                    Remove(key);
                }
            }
            return n;
        }

        /// <summary>
        /// Gets the total number of values contained in the MultiMap.
        /// </summary>
        public int CountAll
        {
            get
            {
                int n = 0;

                foreach (List<TValue> valueList in Values)
                {
                    n += valueList.Count;
                }
                return n;
            }
        }

        /// <summary>
        /// Determines whether the MultiMap contains an element with a specific
        /// key / value pair.
        /// </summary>
        /// <param name="key">Key of the element to search for.</param>
        /// <param name="value">Value of the element to search for.</param>
        /// <returns>true if the element was found; otherwise false.</returns>
        public bool Contains(TKey key, TValue value)
        {
            List<TValue> valueList;

            if (TryGetValue(key, out valueList))
            {
                return valueList.Contains(value);
            }
            return false;
        }

        /// <summary>
        /// Determines whether the MultiMap contains an element with a specific value.
        /// </summary>
        /// <param name="value">Value of the element to search for.</param
        /// <returns>true if the element was found; otherwise false.</returns>
        public bool Contains(TValue value)
        {
            foreach (List<TValue> valueList in Values)
            {
                if (valueList.Contains(value))
                {
                    return true;
                }
            }
            return false;
        }
    }
}

﻿using System;

namespace PracticeExercise4
{
	public class HashTableChaining<K,V> : IHashTable<K,V>
    {

        LinkedList< Bucket<K, V> >[] bucketListsArray;
        private int initialCapacity = 16; 

		public HashTableChaining()
		{
            bucketListsArray = new LinkedList<Bucket<K, V>>[initialCapacity];

            for(int i=0; i < bucketListsArray.Length; i++)
            {
                bucketListsArray[i] = new LinkedList<Bucket<K, V>>();
            }
		}

        private int count;
        public int Count => count;

        public double LoadFactor
        {
            get
            {
                int filled = 0;
                foreach(var list in bucketListsArray)
                {
                    if( list.Count > 0 )
                    {
                        filled++;
                    }
                }

                int totalSlots = bucketListsArray.Length;

                return filled / (double)totalSlots;
            }
        }

        public bool Add(K key, V value)
        {
            // find the hash of the key
            int hash = Hash(key);

            // find the bucket index
            int index = hash % bucketListsArray.Length;

            // find the list
            var list = bucketListsArray[index];

            // find the bucket in the bucket list at the index
            foreach( var bucket in list)
            {
                // if the bucket list contains the key,
                // then update the value
                if ( bucket.Key.Equals(key) )
                {
                    bucket.Value = value;
                    return true;
                }
            }

            // else (the bucket list does not contain the key)
            // then append a new bucket to that list

            var newBucket = new Bucket<K, V>(key, value);
            list.AddFirst(newBucket);
            count++;

            return false;
        }

        public bool ContainsKey(K key)
        {
            // compute the hash
            int hash = Hash(key);

            // compute the index
            int index = hash % bucketListsArray.Length;

            //find the list
            var list = bucketListsArray[index];

            // search the list for the key
            foreach(var bucket in list)
            {
                if( bucket.Key.Equals( key))
                {
                    return true;
                }
            }

            return false;

        }

        // TODO
        public bool ContainsValue(V value)
        {
            //Loop through the table, looking for the provided value
            foreach(var list in bucketListsArray)
            {
                //Loop through each chain for the value
                foreach(var index in list)
                {
                    if(index.Value.Equals(value))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // TODO
        public V Get(K key)
        {
            //Hash the key
            int hash = Hash(key);

            //Find the index at which the key SHOULD be
            int index = hash % bucketListsArray.Length;

            //Check the list at that index, return the corresponding value if the key is found
            foreach(var item in bucketListsArray[index])
            {
                if(item.Key.Equals(key))
                {
                    return item.Value;
                }
            }

            return default;
        }

        public List<K> GetKeys()
        {
            List<K> keys = new List<K>();

            foreach(LinkedList< Bucket<K,V>> list in bucketListsArray)
            {
                foreach( var bucket in list)
                {
                    keys.Add(bucket.Key);
                }
            }

            return keys;
        }

        // TODO
        public List<V> GetValues()
        {
            List<V> values = new List<V>();

            foreach (LinkedList<Bucket<K, V>> list in bucketListsArray)
            {
                foreach (var bucket in list)
                {
                    values.Add(bucket.Value);
                }
            }

            return values;
        }

        public bool Remove(K key)
        {
            // find the hash of the key
            int hash = Hash(key);

            // find the bucket index
            int index = hash % bucketListsArray.Length;

            // find the list
            var list = bucketListsArray[index];

            // find the key in the list
            foreach( var bucket in list)
            {
                if( bucket.Key.Equals(key))
                {
                    list.Remove(bucket);
                    count--;
                    return true;
                }
            }

            return false;

        }


        private int Hash(K key)
        {
            int hash = key.GetHashCode();

            return hash < 0 ? -hash : hash;
        }
    }
}


using System;
using System.ComponentModel;

namespace PracticeExercise4
{
	public class HashTableLinearProbing<K,V>: IHashTable<K,V>
	{

        private Bucket<K, V>[] buckets;
        private int initialCapacity = 16;


		public HashTableLinearProbing()
		{
            buckets = new Bucket<K, V>[initialCapacity];

            for(int i= 0; i < buckets.Length; i++)
            {
                buckets[i] = new Bucket<K, V>();
            }

		}
        private int count = 0;
        private readonly double MAX_LOAD_FACTOR = 0.6;

        public int Count => count;

        public double LoadFactor => count / (double)buckets.Length;

        // O(1) - average case
        // O(n) - worst case
        public bool Add(K key, V value)
        {
            if( LoadFactor > MAX_LOAD_FACTOR)
            {
                Resize();
            }

            // find the hash
            int hash = Hash(key);

            // find the starting index
            int startingIndex = hash % buckets.Length;
            int bucketIndex = startingIndex;

            while (buckets[bucketIndex].State == BucketState.Full)
            {
                // if the key already exists, then update it.
                if(buckets[bucketIndex].Key.Equals( key ))
                {
                    buckets[bucketIndex].Value = value;
                    return true;
                }


                bucketIndex = (bucketIndex + 1) % buckets.Length;

                if( bucketIndex == startingIndex)
                {
                    throw new OutOfMemoryException();
                }
            }

            // if the key doesn't exist, then add it.
            buckets[bucketIndex].Key = key;
            buckets[bucketIndex].Value = value;
            buckets[bucketIndex].State = BucketState.Full;
            count++;
            return false;

        }

        // O(1) - average case
        // O(n) - worst case
        public bool ContainsKey(K key)
        {
            //Find the hashcode of the key
            int hash = Hash(key);

            //Identify where the key SHOULD have been placed originally
            int index = hash % buckets.Length;

            //If the key is located at the expected index, return true, if the index is empty return false
            if (buckets[index].State.Equals(BucketState.Full))
            {
                if (buckets[index].Key.Equals(key))
                    return true;
            }
            else
            {
                return false;
            }

            //Starting after the ideal index, iterate through the table till the key is found or a slot is found empty since creation
            int currentIndex = index + 1;

            while(currentIndex != index)
            {
                if (buckets[currentIndex].Key.Equals(key))
                {
                    return true;
                }
                else if(buckets[currentIndex].State.Equals(BucketState.EmptySinceStart))
                {
                    return false;
                }
                
                currentIndex++;
                currentIndex = currentIndex % buckets.Length;
            }
            return false;
        }

        // O(n) - average case
        // O(n) - worst case
        public bool ContainsValue(V value)
        {
            //Loop through the list, return true if the value is found
            foreach(var index in buckets)
            {
                if(index.State.Equals(BucketState.Full))
                {
                    if(index.Value.Equals(value))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // O(1) - average case
        // O(n) - worst case
        public V Get(K key)
        {
            //Hash the key
            int hash = Hash(key);

            //Find the index where it ought to be
            int index = hash % buckets.Length;

            //Check that index, if the key matches return the value
            if (buckets[index].Key.Equals(key))
            {
                return buckets[index].Value;
            }

            //Search the following indices until the matching key is found or an empty since start
            int currentIndex = index + 1;

            while (currentIndex != index)
            {
                if (buckets[currentIndex].Key.Equals(key))
                {
                    return buckets[currentIndex].Value;
                }

                currentIndex++;
                currentIndex = currentIndex % buckets.Length;
            }

            return default;
        }

        // O(n) - average case
        // O(n) - worst case
        public List<K> GetKeys()
        {
            List<K> keys = new List<K>();

            foreach(var bucket in buckets)
            {
                keys.Add(bucket.Key);
            }
            return keys;
        }

        // O(n) - average case
        // O(n) - worst case
        public List<V> GetValues()
        {
            List<V> values = new List<V>();

            foreach (var bucket in buckets)
            {
                if(bucket.State.Equals(BucketState.Full))
                {
                    values.Add(bucket.Value);
                }
            }
            return values;
        }

        // O(1) - average case
        // O(n) - worst case
        public bool Remove(K key)
        {
            //Hash the key
            int hash = Hash(key);

            //Find the index of the key
            int index = hash % buckets.Length;

            //Check the expected index first--get O(n) complexity in best case
            if (buckets[index].State.Equals(BucketState.Full))
            {
                if (buckets[index].Key.Equals(key))
                {
                    buckets[index] = new Bucket<K, V>();
                    buckets[index].State = BucketState.EmptyAfterRemoval;
                    count--;
                    return true;
                }
            }

            //Iterate through and check all, remove if found
            int currentIndex = index + 1;

            while (currentIndex != index)
            {
                if (buckets[index].State.Equals(BucketState.Full))
                {
                    if (buckets[currentIndex].Key.Equals(key))
                    {
                        buckets[index] = new Bucket<K, V>();
                        buckets[index].State = BucketState.EmptyAfterRemoval;
                        count--;
                        return true;
                    }
                }

                currentIndex++;
                currentIndex = currentIndex % buckets.Length;
            }
            return false;
        }

        private void Resize()
        {
            var newBuckets = new Bucket<K, V>[2 * buckets.Length];
            var oldBuckets = buckets;

            buckets = newBuckets;
            for (int i = 0; i < buckets.Length; i++)
            {
                buckets[i] = new Bucket<K, V>();
            }

            count = 0;

            // rehash all the old/existing buckets into the new array/hashtable
            foreach (var bucket in oldBuckets)
            {
                if (bucket.State == BucketState.Full)
                {
                    Add(bucket.Key, bucket.Value);
                }
            }
        }


        private int Hash(K key)
        {
            int hash = key.GetHashCode();

            return hash < 0 ? -hash : hash;
        }
    }
}


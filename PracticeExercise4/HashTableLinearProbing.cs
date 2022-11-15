using System;

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

        public double LoadFactor => Count / (double)buckets.Length;

        public bool Add(K key, V value)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(K key)
        {
            throw new NotImplementedException();
        }

        public bool ContainsValue(V value)
        {
            throw new NotImplementedException();
        }

        public V Get(K key)
        {
            throw new NotImplementedException();
        }

        public List<K> GetKeys()
        {
            throw new NotImplementedException();
        }

        public List<V> GetValues()
        {
            throw new NotImplementedException();
        }

        public bool Remove(K key)
        {
            throw new NotImplementedException();
        }

        private int Hash(K key)
        {
            int hash = key.GetHashCode();

            return hash < 0 ? -hash : hash;
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
            //Rehash all existing buckets to new hashtable
            foreach(var bucket in oldBuckets)
            {
                Add(bucket.Key, bucket.Value);
            }
        }
    }
}


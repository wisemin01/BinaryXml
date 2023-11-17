namespace BinaryXml
{
    internal sealed class BXStringIndexer : IBinarySerializable
    {
        private readonly Dictionary<string, int> _table = new Dictionary<string, int>();
        private readonly Dictionary<int, string> _rtable = new Dictionary<int, string>();
        private int _indexGenerator;

        public int GetIndex(string key)
        {
            if (_table.TryGetValue(key, out var index))
            {
                return index;
            }

            return -1;
        }

        public int GetOrCreateIndex(string key)
        {
            if (_table.TryGetValue(key, out var index))
            {
                return index;
            }

            index = _indexGenerator++;

            _table.Add(key, index);
            _rtable.Add(index, key);

            return index;
        }

        public string GetString(int index)
        {
            if (_rtable.TryGetValue(index, out var str))
            {
                return str;
            }
            return null;
        }

        public void Clear()
        {
            _table.Clear();
            _rtable.Clear();
            _indexGenerator = 0;
        }

        public void ReadFrom(BinaryReader reader)
        {
            int count = reader.ReadInt32();

            _table.Clear();
            _table.EnsureCapacity(count);

            _rtable.Clear();
            _rtable.EnsureCapacity(count);

            for (int i = 0; i < count; ++i)
            {
                var key = reader.ReadString();
                _table.Add(key, i);
                _rtable.Add(i, key);
            }
        }

        public void WriteTo(BinaryWriter writer)
        {
            int count = _table.Count;

            writer.Write(count);

            // Sort by index.
            var keys = new string[count];
            foreach (var kv in _table)
            {
                keys[kv.Value] = kv.Key;
            }

            for (int i = 0; i < count; ++i)
            {
                writer.Write(keys[i]);
            }
        }
    }
}
/**
 * Taken from https://gist.github.com/GustavoHennig/b46997e293971bf8fd4f6f96056d4256
 * Copyright (2016-2017) Gustavo Augusto Hennig
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 * Changes by Viktor Nemeth
 *  - added Clear();
 *  - added ContainsKey(item);
 *  - update to handle dict output and T
**/

using System.Collections.Specialized;
using System.IO.Compression;
using System.Text;
using System.Text.Json;

namespace HLWebScraper.Net.Model;

public class CompressedMemoryCache<T>
{
    private readonly object _sync = new();
    private readonly Dictionary<string, byte[]> cacheDic = [];
    private readonly OrderedDictionary itemPriorityDic = [];

    public int MaxItemsToHold { get; set; }
    public int MaxBytesSizeLimit { get; set; }
    private int BytesSize { get; set; }

    public int Count
    {
        get
        {
            lock (_sync)
            {
                return cacheDic.Count;
            }
        }
    }

    public bool ContainsKey(string key)
    {
        lock (_sync)
        {
            return cacheDic.ContainsKey(key);
        }
    }

    public void Clear()
    {
        lock (_sync)
        {
            itemPriorityDic.Clear();
            cacheDic.Clear();
        }
    }

    public void AddOrUpdate(string key, T value)
    {
        lock (_sync)
        {
            // Serialize object T to JSON string before compression
            string jsonString = JsonSerializer.Serialize(value);
            byte[] compressed = Compress(jsonString);

            if (itemPriorityDic.Contains(key))
            {
                int lastbytes = cacheDic[key].Length;
                BytesSize += compressed.Length - lastbytes;
                cacheDic[key] = compressed;
                RePriorityItem(key);
            }
            else
            {
                itemPriorityDic.Add(key, key);
                if (MaxItemsToHold != 0 && cacheDic.Count >= MaxItemsToHold && itemPriorityDic.Count > 0)
                {
                    RemoveLowerPriorityItem();
                }

                BytesSize += compressed.Length;
                cacheDic.Add(key, compressed);

                while (MaxBytesSizeLimit > 0 && BytesSize > MaxBytesSizeLimit)
                {
                    RemoveLowerPriorityItem();
                    if (cacheDic.Count == 0)
                        throw new Exception("The value size is higher than the MaxBytesSizeLimit: " + MaxBytesSizeLimit);
                }
            }
        }
    }

    public T? Get(string key)
    {
        lock (_sync)
        {
            if (cacheDic.TryGetValue(key, out byte[]? compressedBytes))
            {
                RePriorityItem(key);
                string jsonString = Decompress(compressedBytes);
                return JsonSerializer.Deserialize<T>(jsonString);
            }

            return default;
        }
    }

    private void RemoveLowerPriorityItem()
    {
        object? kremove = itemPriorityDic[0];
        if (kremove == null) return;

        string keyStr = kremove.ToString()!;
        int sizeremoved = cacheDic[keyStr].Length;
        itemPriorityDic.RemoveAt(0);
        cacheDic.Remove(keyStr);
        BytesSize -= sizeremoved;
    }

    private void RePriorityItem(string key)
    {
        itemPriorityDic.Remove(key);
        itemPriorityDic.Add(key, key);
    }

    private static byte[] Compress(string data)
    {
        using MemoryStream inMemStream = new(Encoding.UTF8.GetBytes(data));
        using MemoryStream outMemStream = new();
        using (DeflateStream zipStream = new(outMemStream, CompressionMode.Compress, leaveOpen: true))
        {
            inMemStream.CopyTo(zipStream);
        }
        return outMemStream.ToArray();
    }

    private static string Decompress(byte[] data)
    {
        using MemoryStream inMemStream = new(data);
        using DeflateStream decompressionStream = new(inMemStream, CompressionMode.Decompress);
        using StreamReader streamReader = new(decompressionStream, Encoding.UTF8);
        return streamReader.ReadToEnd();
    }
}
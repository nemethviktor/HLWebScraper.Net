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
 *  - applied code cleanup (via Resharper)
**/

using System.Collections.Specialized;
using System.IO.Compression;
using System.Text;

namespace HLWebScraper.Net.Model;

public class CompressedMemoryCache
{
    private readonly object _sync = new();
    private readonly Dictionary<string, byte[]> cacheDic = new();
    private readonly OrderedDictionary itemPriorityDic = new();

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
            return cacheDic.ContainsKey(key: key);
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

    public void AddOrUpdate(string key, string value)
    {
        lock (_sync)
        {
            byte[] compressed = Compress(data: value);

            if (itemPriorityDic.Contains(key: key))
            {
                int lastbytes = cacheDic[key: key].Length;
                BytesSize += compressed.Length - lastbytes;
                itemPriorityDic[key: key] = compressed;
                RePriorityItem(key: key);
            }
            else
            {
                itemPriorityDic.Add(key: key, value: key);
                if (MaxItemsToHold != 0 &&
                    cacheDic.Count >= MaxItemsToHold &&
                    itemPriorityDic.Count > 0) RemoveLowerPriorityItem();

                BytesSize += compressed.Length;
                cacheDic.Add(key: key, value: compressed);

                while (MaxBytesSizeLimit > 0 &&
                       BytesSize > MaxBytesSizeLimit)
                {
                    RemoveLowerPriorityItem();
                    if (cacheDic.Count == 0)
                        throw new Exception(message: "The value size is higher that the MaxBytesSizeLimit: " +
                                                     MaxBytesSizeLimit);
                }
            }
        }
    }

    private void RemoveLowerPriorityItem()
    {
        object kremove = itemPriorityDic[index: 0];
        int sizeremoved = cacheDic[key: kremove.ToString()].Length;
        itemPriorityDic.RemoveAt(index: 0);
        cacheDic.Remove(key: kremove.ToString());
        BytesSize -= sizeremoved;
    }

    private void RePriorityItem(string key)
    {
        //Move the key to the end of queue
        itemPriorityDic.Remove(key: key);
        itemPriorityDic.Add(key: key, value: key);
    }

    public string Get(string key)
    {
        lock (_sync)
        {
            if (cacheDic.ContainsKey(key: key))
            {
                RePriorityItem(key: key);
                return Decompress(data: cacheDic[key: key]);
            }

            return null;
        }
    }

    private byte[] Compress(string data)
    {
        // convert the source string into a memory stream
        using (MemoryStream inMemStream = new(buffer: Encoding.UTF8.GetBytes(s: data)), outMemStream = new())
        {
            // create a compression stream with the output stream
            using (DeflateStream zipStream = new(stream: outMemStream, mode: CompressionMode.Compress, leaveOpen: true))
                // copy the source string into the compression stream
            {
                inMemStream.WriteTo(stream: zipStream);
            }

            // return the compressed bytes in the output stream
            return outMemStream.ToArray();
        }
    }

    private string Decompress(byte[] data)
    {
        // load the byte array into a memory stream
        using (MemoryStream inMemStream = new(buffer: data))
            // and decompress the memory stream into the original string
        {
            using (DeflateStream decompressionStream = new(stream: inMemStream, mode: CompressionMode.Decompress))
            {
                using (StreamReader streamReader = new(stream: decompressionStream, encoding: Encoding.UTF8))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
    }
}
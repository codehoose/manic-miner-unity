using System;
using System.IO;
using System.Text;
using UnityEngine;

public class SnapshotImporter : IDisposable
{
    MemoryStream _ms;
    BinaryWriter _writer;
    BinaryReader _reader;
    
    public SnapshotImporter(string resourceFile)
    {
        TextAsset asset = Resources.Load<TextAsset>(resourceFile);

        _ms = new MemoryStream();
        _writer = new BinaryWriter(_ms);
        _reader = new BinaryReader(_ms);

        // Cull the first 27 bytes of the .SNA header
        _writer.Write(asset.bytes, 27, 49152);
        _writer.Flush();

        _ms.Seek(0, SeekOrigin.Begin);
    }

    public void Dispose()
    {
        Dispose(true);
    }

    public byte Read()
    {
        return _reader.ReadByte();
    }

    public byte[] ReadBytes (int length)
    {
        return _reader.ReadBytes(length);
    }

    public string ReadString(int length)
    {
        byte[] buf = _reader.ReadBytes(length);
        return Encoding.ASCII.GetString(buf, 0, length);
    }

    public short ReadShort()
    {
        return _reader.ReadInt16();
    }

    public int ReadInt()
    {
        return _reader.ReadInt32();
    }

    public void Seek(int offset)
    {
        offset = offset - 16384; // Why did we do this again????
        _ms.Seek(offset, SeekOrigin.Begin);
    }

    protected void Dispose(bool disposed)
    {
        // Clean up everything
        _writer.Close();
        _reader.Close();
        _ms.Dispose();
    }
}

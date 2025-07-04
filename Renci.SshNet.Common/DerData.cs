using System;
using System.Collections.Generic;
using System.Linq;

namespace Renci.SshNet.Common;

public class DerData
{
	private const byte CONSTRUCTED = 32;

	private const byte BOOLEAN = 1;

	private const byte INTEGER = 2;

	private const byte OCTETSTRING = 4;

	private const byte NULL = 5;

	private const byte OBJECTIDENTIFIER = 6;

	private const byte SEQUENCE = 16;

	private readonly List<byte> _data;

	private int _readerIndex;

	private readonly int _lastIndex;

	public bool IsEndOfData => _readerIndex >= _lastIndex;

	public DerData()
	{
		_data = new List<byte>();
	}

	public DerData(byte[] data)
	{
		_data = new List<byte>(data);
		int num = ReadLength();
		_lastIndex = _readerIndex + num;
	}

	public byte[] Encode()
	{
		int length = _data.Count();
		byte[] length2 = GetLength(length);
		_data.InsertRange(0, length2);
		_data.Insert(0, 48);
		return _data.ToArray();
	}

	public BigInteger ReadBigInteger()
	{
		byte b = ReadByte();
		if (b != 2)
		{
			throw new InvalidOperationException("Invalid data type, INTEGER(02) is expected.");
		}
		int length = ReadLength();
		byte[] source = ReadBytes(length);
		return new BigInteger(source.Reverse().ToArray());
	}

	public int ReadInteger()
	{
		byte b = ReadByte();
		if (b != 2)
		{
			throw new InvalidOperationException("Invalid data type, INTEGER(02) is expected.");
		}
		int num = ReadLength();
		byte[] array = ReadBytes(num);
		if (num > 4)
		{
			throw new InvalidOperationException("Integer type cannot occupy more then 4 bytes");
		}
		int num2 = 0;
		int num3 = (num - 1) * 8;
		for (int i = 0; i < num; i++)
		{
			num2 |= array[i] << num3;
			num3 -= 8;
		}
		return num2;
	}

	public void Write(bool data)
	{
		_data.Add(1);
		_data.Add(1);
		_data.Add((byte)(data ? 1u : 0u));
	}

	public void Write(uint data)
	{
		byte[] bytes = BitConverter.GetBytes(data);
		_data.Add(2);
		byte[] length = GetLength(bytes.Length);
		WriteBytes(length);
		WriteBytes(bytes);
	}

	public void Write(BigInteger data)
	{
		List<byte> list = data.ToByteArray().Reverse().ToList();
		_data.Add(2);
		byte[] length = GetLength(list.Count);
		WriteBytes(length);
		WriteBytes(list);
	}

	public void Write(byte[] data)
	{
		_data.Add(4);
		byte[] length = GetLength(data.Length);
		WriteBytes(length);
		WriteBytes(data);
	}

	public void Write(ObjectIdentifier identifier)
	{
		ulong[] array = new ulong[identifier.Identifiers.Length - 1];
		array[0] = identifier.Identifiers[0] * 40 + identifier.Identifiers[1];
		Buffer.BlockCopy(identifier.Identifiers, 16, array, 8, (identifier.Identifiers.Length - 2) * 8);
		List<byte> list = new List<byte>();
		ulong[] array2 = array;
		foreach (ulong num in array2)
		{
			ulong num2 = num;
			byte[] array3 = new byte[8];
			int num3 = array3.Length - 1;
			byte b = (byte)(num2 & 0x7F);
			do
			{
				array3[num3] = b;
				if (num3 < array3.Length - 1)
				{
					array3[num3] |= 128;
				}
				num2 >>= 7;
				b = (byte)(num2 & 0x7F);
				num3--;
			}
			while (b > 0);
			for (int j = num3 + 1; j < array3.Length; j++)
			{
				list.Add(array3[j]);
			}
		}
		_data.Add(6);
		byte[] length = GetLength(list.Count);
		WriteBytes(length);
		WriteBytes(list);
	}

	public void WriteNull()
	{
		_data.Add(5);
		_data.Add(0);
	}

	public void Write(DerData data)
	{
		byte[] collection = data.Encode();
		_data.AddRange(collection);
	}

	private byte[] GetLength(int length)
	{
		if (length > 127)
		{
			int num = 1;
			int num2 = length;
			while ((num2 >>= 8) != 0)
			{
				num++;
			}
			byte[] array = new byte[num];
			array[0] = (byte)((uint)num | 0x80u);
			int num3 = (num - 1) * 8;
			int num4 = 1;
			while (num3 >= 0)
			{
				array[num4] = (byte)(length >> num3);
				num3 -= 8;
				num4++;
			}
			return array;
		}
		return new byte[1] { (byte)length };
	}

	private int ReadLength()
	{
		int num = ReadByte();
		if (num == 128)
		{
			throw new NotSupportedException("Indefinite-length encoding is not supported.");
		}
		if (num > 127)
		{
			int num2 = num & 0x7F;
			if (num2 > 4)
			{
				throw new InvalidOperationException($"DER length is '{num2}' and cannot be more than 4 bytes.");
			}
			num = 0;
			for (int i = 0; i < num2; i++)
			{
				int num3 = ReadByte();
				num = (num << 8) + num3;
			}
			if (num < 0)
			{
				throw new InvalidOperationException("Corrupted data - negative length found");
			}
		}
		return num;
	}

	private void WriteBytes(IEnumerable<byte> data)
	{
		_data.AddRange(data);
	}

	private byte ReadByte()
	{
		if (_readerIndex > _data.Count)
		{
			throw new InvalidOperationException("Read out of boundaries.");
		}
		return _data[_readerIndex++];
	}

	private byte[] ReadBytes(int length)
	{
		if (_readerIndex + length > _data.Count)
		{
			throw new InvalidOperationException("Read out of boundaries.");
		}
		byte[] array = new byte[length];
		_data.CopyTo(_readerIndex, array, 0, length);
		_readerIndex += length;
		return array;
	}
}

using System;
using System.IO;
using SevenZip.Compression.LZMA;

namespace SevenZip;

internal abstract class LzmaBench
{
	private class CRandomGenerator
	{
		private uint A1;

		private uint A2;

		public CRandomGenerator()
		{
			Init();
		}

		public void Init()
		{
			A1 = 362436069u;
			A2 = 521288629u;
		}

		public uint GetRnd()
		{
			return ((A1 = 36969 * (A1 & 0xFFFF) + (A1 >> 16)) << 16) ^ (A2 = 18000 * (A2 & 0xFFFF) + (A2 >> 16));
		}
	}

	private class CBitRandomGenerator
	{
		private CRandomGenerator RG = new CRandomGenerator();

		private uint Value;

		private int NumBits;

		public void Init()
		{
			Value = 0u;
			NumBits = 0;
		}

		public uint GetRnd(int numBits)
		{
			uint result;
			if (NumBits > numBits)
			{
				result = Value & (uint)((1 << numBits) - 1);
				Value >>= numBits;
				NumBits -= numBits;
				return result;
			}
			numBits -= NumBits;
			result = Value << numBits;
			Value = RG.GetRnd();
			result |= Value & (uint)((1 << numBits) - 1);
			Value >>= numBits;
			NumBits = 32 - numBits;
			return result;
		}
	}

	private class CBenchRandomGenerator
	{
		private CBitRandomGenerator RG = new CBitRandomGenerator();

		private uint Pos;

		private uint Rep0;

		public uint BufferSize;

		public byte[] Buffer;

		public void Set(uint bufferSize)
		{
			Buffer = new byte[bufferSize];
			Pos = 0u;
			BufferSize = bufferSize;
		}

		private uint GetRndBit()
		{
			return RG.GetRnd(1);
		}

		private uint GetLogRandBits(int numBits)
		{
			uint rnd = RG.GetRnd(numBits);
			return RG.GetRnd((int)rnd);
		}

		private uint GetOffset()
		{
			if (GetRndBit() == 0)
			{
				return GetLogRandBits(4);
			}
			return (GetLogRandBits(4) << 10) | RG.GetRnd(10);
		}

		private uint GetLen1()
		{
			return RG.GetRnd((int)(1 + RG.GetRnd(2)));
		}

		private uint GetLen2()
		{
			return RG.GetRnd((int)(2 + RG.GetRnd(2)));
		}

		public void Generate()
		{
			RG.Init();
			Rep0 = 1u;
			while (Pos < BufferSize)
			{
				if (GetRndBit() == 0 || Pos < 1)
				{
					Buffer[Pos++] = (byte)RG.GetRnd(8);
					continue;
				}
				uint num;
				if (RG.GetRnd(3) == 0)
				{
					num = 1 + GetLen1();
				}
				else
				{
					do
					{
						Rep0 = GetOffset();
					}
					while (Rep0 >= Pos);
					Rep0++;
					num = 2 + GetLen2();
				}
				uint num2 = 0u;
				while (num2 < num && Pos < BufferSize)
				{
					Buffer[Pos] = Buffer[Pos - Rep0];
					num2++;
					Pos++;
				}
			}
		}
	}

	private class CrcOutStream : Stream
	{
		public CRC CRC = new CRC();

		public override bool CanRead => false;

		public override bool CanSeek => false;

		public override bool CanWrite => true;

		public override long Length => 0L;

		public override long Position
		{
			get
			{
				return 0L;
			}
			set
			{
			}
		}

		public void Init()
		{
			CRC.Init();
		}

		public uint GetDigest()
		{
			return CRC.GetDigest();
		}

		public override void Flush()
		{
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return 0L;
		}

		public override void SetLength(long value)
		{
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return 0;
		}

		public override void WriteByte(byte b)
		{
			CRC.UpdateByte(b);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			CRC.Update(buffer, (uint)offset, (uint)count);
		}
	}

	private class CProgressInfo : ICodeProgress
	{
		public long ApprovedStart;

		public long InSize;

		public DateTime Time;

		public void Init()
		{
			InSize = 0L;
		}

		public void SetProgress(long inSize, long outSize)
		{
			if (inSize >= ApprovedStart && InSize == 0L)
			{
				Time = DateTime.UtcNow;
				InSize = inSize;
			}
		}
	}

	private const uint kAdditionalSize = 6291456u;

	private const uint kCompressedAdditionalSize = 1024u;

	private const uint kMaxLzmaPropSize = 10u;

	private const int kSubBits = 8;

	private static uint GetLogSize(uint size)
	{
		for (int i = 8; i < 32; i++)
		{
			for (uint num = 0u; num < 256; num++)
			{
				if (size <= (uint)((1 << i) + (int)(num << i - 8)))
				{
					return (uint)(i << 8) + num;
				}
			}
		}
		return 8192u;
	}

	private static ulong MyMultDiv64(ulong value, ulong elapsedTime)
	{
		ulong num = 10000000uL;
		ulong num2 = elapsedTime;
		while (num > 1000000)
		{
			num >>= 1;
			num2 >>= 1;
		}
		if (num2 == 0L)
		{
			num2 = 1uL;
		}
		return value * num / num2;
	}

	private static ulong GetCompressRating(uint dictionarySize, ulong elapsedTime, ulong size)
	{
		ulong num = GetLogSize(dictionarySize) - 4608;
		ulong num2 = 1060 + (num * num * 10 >> 16);
		ulong value = size * num2;
		return MyMultDiv64(value, elapsedTime);
	}

	private static ulong GetDecompressRating(ulong elapsedTime, ulong outSize, ulong inSize)
	{
		ulong value = inSize * 220 + outSize * 20;
		return MyMultDiv64(value, elapsedTime);
	}

	private static ulong GetTotalRating(uint dictionarySize, ulong elapsedTimeEn, ulong sizeEn, ulong elapsedTimeDe, ulong inSizeDe, ulong outSizeDe)
	{
		return (GetCompressRating(dictionarySize, elapsedTimeEn, sizeEn) + GetDecompressRating(elapsedTimeDe, inSizeDe, outSizeDe)) / 2uL;
	}

	private static void PrintValue(ulong v)
	{
		string text = v.ToString();
		for (int i = 0; i + text.Length < 6; i++)
		{
			Console.Write(" ");
		}
		Console.Write(text);
	}

	private static void PrintRating(ulong rating)
	{
		PrintValue(rating / 1000000uL);
		Console.Write(" MIPS");
	}

	private static void PrintResults(uint dictionarySize, ulong elapsedTime, ulong size, bool decompressMode, ulong secondSize)
	{
		ulong num = MyMultDiv64(size, elapsedTime);
		PrintValue(num / 1024uL);
		Console.Write(" KB/s  ");
		ulong rating = ((!decompressMode) ? GetCompressRating(dictionarySize, elapsedTime, size) : GetDecompressRating(elapsedTime, size, secondSize));
		PrintRating(rating);
	}

	public static int LzmaBenchmark(int numIterations, uint dictionarySize)
	{
		if (numIterations <= 0)
		{
			return 0;
		}
		if (dictionarySize < 262144)
		{
			Console.WriteLine("\nError: dictionary size for benchmark must be >= 19 (512 KB)");
			return 1;
		}
		Console.Write("\n       Compressing                Decompressing\n\n");
		Encoder encoder = new Encoder();
		Decoder decoder = new Decoder();
		CoderPropID[] propIDs = new CoderPropID[1] { CoderPropID.DictionarySize };
		object[] properties = new object[1] { (int)dictionarySize };
		uint num = dictionarySize + 6291456;
		uint capacity = num / 2u + 1024;
		encoder.SetCoderProperties(propIDs, properties);
		MemoryStream memoryStream = new MemoryStream();
		encoder.WriteCoderProperties(memoryStream);
		byte[] decoderProperties = memoryStream.ToArray();
		CBenchRandomGenerator cBenchRandomGenerator = new CBenchRandomGenerator();
		cBenchRandomGenerator.Set(num);
		cBenchRandomGenerator.Generate();
		CRC cRC = new CRC();
		cRC.Init();
		cRC.Update(cBenchRandomGenerator.Buffer, 0u, cBenchRandomGenerator.BufferSize);
		CProgressInfo cProgressInfo = new CProgressInfo();
		cProgressInfo.ApprovedStart = dictionarySize;
		ulong num2 = 0uL;
		ulong num3 = 0uL;
		ulong num4 = 0uL;
		ulong num5 = 0uL;
		MemoryStream memoryStream2 = new MemoryStream(cBenchRandomGenerator.Buffer, 0, (int)cBenchRandomGenerator.BufferSize);
		MemoryStream memoryStream3 = new MemoryStream((int)capacity);
		CrcOutStream crcOutStream = new CrcOutStream();
		for (int i = 0; i < numIterations; i++)
		{
			cProgressInfo.Init();
			memoryStream2.Seek(0L, SeekOrigin.Begin);
			memoryStream3.Seek(0L, SeekOrigin.Begin);
			encoder.Code(memoryStream2, memoryStream3, -1L, -1L, cProgressInfo);
			ulong ticks = (ulong)(DateTime.UtcNow - cProgressInfo.Time).Ticks;
			long position = memoryStream3.Position;
			if (cProgressInfo.InSize == 0L)
			{
				throw new Exception("Internal ERROR 1282");
			}
			ulong num6 = 0uL;
			for (int j = 0; j < 2; j++)
			{
				memoryStream3.Seek(0L, SeekOrigin.Begin);
				crcOutStream.Init();
				decoder.SetDecoderProperties(decoderProperties);
				ulong outSize = num;
				DateTime utcNow = DateTime.UtcNow;
				decoder.Code(memoryStream3, crcOutStream, 0L, (long)outSize, null);
				num6 = (ulong)(DateTime.UtcNow - utcNow).Ticks;
				if (crcOutStream.GetDigest() != cRC.GetDigest())
				{
					throw new Exception("CRC Error");
				}
			}
			ulong num7 = (ulong)(num - cProgressInfo.InSize);
			PrintResults(dictionarySize, ticks, num7, decompressMode: false, 0uL);
			Console.Write("     ");
			PrintResults(dictionarySize, num6, num, decompressMode: true, (ulong)position);
			Console.WriteLine();
			num2 += num7;
			num3 += ticks;
			num4 += num6;
			num5 += (ulong)position;
		}
		Console.WriteLine("---------------------------------------------------");
		PrintResults(dictionarySize, num3, num2, decompressMode: false, 0uL);
		Console.Write("     ");
		PrintResults(dictionarySize, num4, (ulong)(num * numIterations), decompressMode: true, num5);
		Console.WriteLine("    Average");
		return 0;
	}
}

using System;
using System.Collections;
using System.IO;
using SevenZip.CommandLineParser;
using SevenZip.Compression.LZMA;

namespace SevenZip;

internal class LzmaAlone
{
	private enum Key
	{
		Help1,
		Help2,
		Mode,
		Dictionary,
		FastBytes,
		LitContext,
		LitPos,
		PosBits,
		MatchFinder,
		EOS,
		StdIn,
		StdOut,
		Train
	}

	private static void PrintHelp()
	{
		Console.WriteLine("\nUsage:  LZMA <e|d> [<switches>...] inputFile outputFile\n  e: encode file\n  d: decode file\n  b: Benchmark\n<Switches>\n  -d{N}:  set dictionary - [0, 29], default: 23 (8MB)\n  -fb{N}: set number of fast bytes - [5, 273], default: 128\n  -lc{N}: set number of literal context bits - [0, 8], default: 3\n  -lp{N}: set number of literal pos bits - [0, 4], default: 0\n  -pb{N}: set number of pos bits - [0, 4], default: 2\n  -mf{MF_ID}: set Match Finder: [bt2, bt4], default: bt4\n  -eos:   write End Of Stream marker\n");
	}

	private static bool GetNumber(string s, out int v)
	{
		v = 0;
		foreach (char c in s)
		{
			if (c < '0' || c > '9')
			{
				return false;
			}
			v *= 10;
			v += c - 48;
		}
		return true;
	}

	private static int IncorrectCommand()
	{
		throw new Exception("Command line error");
	}

	private static int Main2(string[] args)
	{
		//Discarded unreachable code: IL_0143
		Console.WriteLine("\nLZMA# 4.61  2008-11-23\n");
		if (args.Length == 0)
		{
			PrintHelp();
			return 0;
		}
		SwitchForm[] array = new SwitchForm[13];
		int numSwitches = 0;
		array[numSwitches++] = new SwitchForm("?", SwitchType.Simple, multi: false);
		array[numSwitches++] = new SwitchForm("H", SwitchType.Simple, multi: false);
		array[numSwitches++] = new SwitchForm("A", SwitchType.UnLimitedPostString, multi: false, 1);
		array[numSwitches++] = new SwitchForm("D", SwitchType.UnLimitedPostString, multi: false, 1);
		array[numSwitches++] = new SwitchForm("FB", SwitchType.UnLimitedPostString, multi: false, 1);
		array[numSwitches++] = new SwitchForm("LC", SwitchType.UnLimitedPostString, multi: false, 1);
		array[numSwitches++] = new SwitchForm("LP", SwitchType.UnLimitedPostString, multi: false, 1);
		array[numSwitches++] = new SwitchForm("PB", SwitchType.UnLimitedPostString, multi: false, 1);
		array[numSwitches++] = new SwitchForm("MF", SwitchType.UnLimitedPostString, multi: false, 1);
		array[numSwitches++] = new SwitchForm("EOS", SwitchType.Simple, multi: false);
		array[numSwitches++] = new SwitchForm("SI", SwitchType.Simple, multi: false);
		array[numSwitches++] = new SwitchForm("SO", SwitchType.Simple, multi: false);
		array[numSwitches++] = new SwitchForm("T", SwitchType.UnLimitedPostString, multi: false, 1);
		Parser parser = new Parser(numSwitches);
		try
		{
			parser.ParseStrings(array, args);
		}
		catch
		{
			return IncorrectCommand();
		}
		if (parser[0].ThereIs || parser[1].ThereIs)
		{
			PrintHelp();
			return 0;
		}
		ArrayList nonSwitchStrings = parser.NonSwitchStrings;
		int num = 0;
		if (num >= nonSwitchStrings.Count)
		{
			return IncorrectCommand();
		}
		string text = (string)nonSwitchStrings[num++];
		text = text.ToLower();
		bool flag = false;
		int num2 = 2097152;
		if (parser[3].ThereIs)
		{
			if (!GetNumber((string)parser[3].PostStrings[0], out var v))
			{
				IncorrectCommand();
			}
			num2 = 1 << v;
			flag = true;
		}
		string text2 = "bt4";
		if (parser[8].ThereIs)
		{
			text2 = (string)parser[8].PostStrings[0];
		}
		text2 = text2.ToLower();
		if (text == "b")
		{
			int v2 = 10;
			if (num < nonSwitchStrings.Count && !GetNumber((string)nonSwitchStrings[num++], out v2))
			{
				v2 = 10;
			}
			return LzmaBench.LzmaBenchmark(v2, (uint)num2);
		}
		string text3 = string.Empty;
		if (parser[12].ThereIs)
		{
			text3 = (string)parser[12].PostStrings[0];
		}
		bool flag2 = false;
		if (text == "e")
		{
			flag2 = true;
		}
		else if (text == "d")
		{
			flag2 = false;
		}
		else
		{
			IncorrectCommand();
		}
		bool thereIs = parser[10].ThereIs;
		bool thereIs2 = parser[11].ThereIs;
		Stream stream = null;
		if (thereIs)
		{
			throw new Exception("Not implemeted");
		}
		if (num >= nonSwitchStrings.Count)
		{
			IncorrectCommand();
		}
		string path = (string)nonSwitchStrings[num++];
		stream = new FileStream(path, FileMode.Open, FileAccess.Read);
		FileStream fileStream = null;
		if (thereIs2)
		{
			throw new Exception("Not implemeted");
		}
		if (num >= nonSwitchStrings.Count)
		{
			IncorrectCommand();
		}
		string path2 = (string)nonSwitchStrings[num++];
		fileStream = new FileStream(path2, FileMode.Create, FileAccess.Write);
		FileStream fileStream2 = null;
		if (text3.Length != 0)
		{
			fileStream2 = new FileStream(text3, FileMode.Open, FileAccess.Read);
		}
		if (flag2)
		{
			if (!flag)
			{
				num2 = 8388608;
			}
			int v3 = 2;
			int v4 = 3;
			int v5 = 0;
			int v6 = 2;
			int v7 = 128;
			bool flag3 = parser[9].ThereIs || thereIs;
			if (parser[2].ThereIs && !GetNumber((string)parser[2].PostStrings[0], out v6))
			{
				IncorrectCommand();
			}
			if (parser[4].ThereIs && !GetNumber((string)parser[4].PostStrings[0], out v7))
			{
				IncorrectCommand();
			}
			if (parser[5].ThereIs && !GetNumber((string)parser[5].PostStrings[0], out v4))
			{
				IncorrectCommand();
			}
			if (parser[6].ThereIs && !GetNumber((string)parser[6].PostStrings[0], out v5))
			{
				IncorrectCommand();
			}
			if (parser[7].ThereIs && !GetNumber((string)parser[7].PostStrings[0], out v3))
			{
				IncorrectCommand();
			}
			CoderPropID[] propIDs = new CoderPropID[8]
			{
				CoderPropID.DictionarySize,
				CoderPropID.PosStateBits,
				CoderPropID.LitContextBits,
				CoderPropID.LitPosBits,
				CoderPropID.Algorithm,
				CoderPropID.NumFastBytes,
				CoderPropID.MatchFinder,
				CoderPropID.EndMarker
			};
			object[] properties = new object[8] { num2, v3, v4, v5, v6, v7, text2, flag3 };
			Encoder encoder = new Encoder();
			encoder.SetCoderProperties(propIDs, properties);
			encoder.WriteCoderProperties(fileStream);
			long num3 = ((!flag3 && !thereIs) ? stream.Length : (-1));
			for (int i = 0; i < 8; i++)
			{
				fileStream.WriteByte((byte)(num3 >> 8 * i));
			}
			if (fileStream2 != null)
			{
				CDoubleStream cDoubleStream = new CDoubleStream();
				cDoubleStream.s1 = fileStream2;
				cDoubleStream.s2 = stream;
				cDoubleStream.fileIndex = 0;
				stream = cDoubleStream;
				long length = fileStream2.Length;
				cDoubleStream.skipSize = 0L;
				if (length > num2)
				{
					cDoubleStream.skipSize = length - num2;
				}
				fileStream2.Seek(cDoubleStream.skipSize, SeekOrigin.Begin);
				encoder.SetTrainSize((uint)(length - cDoubleStream.skipSize));
			}
			encoder.Code(stream, fileStream, -1L, -1L, null);
		}
		else
		{
			if (!(text == "d"))
			{
				throw new Exception("Command Error");
			}
			byte[] array2 = new byte[5];
			if (stream.Read(array2, 0, 5) != 5)
			{
				throw new Exception("input .lzma is too short");
			}
			Decoder decoder = new Decoder();
			decoder.SetDecoderProperties(array2);
			if (fileStream2 != null && !decoder.Train(fileStream2))
			{
				throw new Exception("can't train");
			}
			long num4 = 0L;
			for (int j = 0; j < 8; j++)
			{
				int num5 = stream.ReadByte();
				if (num5 < 0)
				{
					throw new Exception("Can't Read 1");
				}
				num4 |= (long)(int)(byte)num5 << 8 * j;
			}
			long inSize = stream.Length - stream.Position;
			decoder.Code(stream, fileStream, inSize, num4, null);
		}
		return 0;
	}

	[STAThread]
	private static int Main(string[] args)
	{
		//Discarded unreachable code: IL_000c, IL_0024
		try
		{
			return Main2(args);
		}
		catch (Exception arg)
		{
			Console.WriteLine("{0} Caught exception #1.", arg);
			return 1;
		}
	}
}

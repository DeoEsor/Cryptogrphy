﻿using NumberTheory;

namespace CryptographyLib.Symmetric;

public sealed unsafe partial class Magenta
{
	#region Magenta Functions

	private static byte F(byte x) => Sbox[x];

	private static byte A(byte x, byte y) => F((byte)(x ^ F(y)));

	private static byte* Pe(byte x, byte y)
	{
		var result = stackalloc byte[2];
		result[0] = A(x, y);
		result[1] = A(y, x);

		return result;
	}

	private static  byte* P(byte* x)
	{
		var res16 = stackalloc byte[16];

		for(var i = 0; i < 8; i++)
			res16[i] = Pe(x[i], x[8 + i])[i % 2];

		return res16;
	}

	private static byte* T(byte* x) => P(P(P(P(x))));

	private static byte* Xe(byte* x)
	{
		var result = stackalloc byte[8];

		for (var i = 0; i < 8; i++)
			result[i] = x[2 * i];

		return result;
	}

	private static byte* Xo(byte* x)
	{
		var result = stackalloc byte[8];

		for (var i = 0; i < 8; i++)
			result[i] = x[2 * i + 1];

		return result;
	}

	private static byte* C1(byte* x) => T(x);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="x"></param>
	/// <returns></returns>
	private static byte* C2(byte* x)
	{
		var y = stackalloc byte[16];

		for (var i = 0; i < 16; i++)
		{
			y[i] = i > 7 
				? (byte)(x[i] ^ Xo(C1(x))[i - 8])
				:(byte)(x[i] ^ Xe(C1(x))[i]);	
		}

		return T(y);
	}

	private static byte* C3(byte* x)
	{
		var y = stackalloc byte[16];

		for (var i = 0; i < 8; i++)
			y[i] = (byte)(x[i] ^ Xe(C2(x))[i]);

		for (var i = 8; i < 16; i++)
			y[i] = (byte)(x[i] ^ Xo(C2(x))[i - 8]);

		return T(y);
	}

	private static byte* E(byte* x) => Xe(C3(x));

	private static byte[,] F(ReadOnlySpan<byte> xLeft, ReadOnlySpan<byte> xRight, ReadOnlySpan<byte> y8)
	{
		var z = new byte[2, 8];
		var xy = stackalloc byte[16];

		for (var i = 0; i < 8; i++)
			(xy[i], xy[i + 8]) = (xRight[i], y8[i]);

		for (var i = 0; i < 8; i++)
			(z[0, i], z[1, i]) = (xRight[i], (byte)(xLeft[i] ^ E(xy)[i]));

		return z;
	}

	#endregion
}
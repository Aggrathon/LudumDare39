
using UnityEngine;

public static class Utils
{

	public static string GetRomanNumeral(int number)
	{
		if (number < 1)
			return "";
		if (number < 4)
			return new string('I', number);
		if (number == 4)
			return "IV";
		if (number < 9)
			return "V" + new string('I', number - 5);
		if (number == 9)
			return "IX";
		if (number < 40)
			return new string('X', number/10) + GetRomanNumeral(number%10);
		if (number < 50)
			return "XL" + GetRomanNumeral(number - 40);
		if (number < 90)
			return "L" + GetRomanNumeral(number - 50);
		if (number < 100)
			return "XC" + GetRomanNumeral(number - 90);
		if (number < 400)
			return new string('C', number / 100) + GetRomanNumeral(number % 100);
		return number.ToString();
	}
}

using System;

namespace GreenKaleAndYamsApi.TestUtilities {
	public static class RandomValues {
		public static Random random = new Random();

		private static string numbers = "0123456789";
		private static string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
		private static string vowels = "aeiouy";
		private static string consonants = "bcdfghjklmnpqrstvwxz";


		public static bool GetBoolean() {
			return random.Next(10) < 5;
		}

		public static DateTime GetDate(DateTime start, DateTime? end = null) {
			if (end == null) {
				end = DateTime.Now;
			}
			return new DateTime(start.Ticks + random.NextInt64(end.Value.Ticks - start.Ticks));
		}

		public static string GetPhoneNumber() {
			return GetNumberString(10);
		}

		public static int GetInt(int min = 1, int max = 999999999) {
			return random.Next(min, max);
		}

		public static long GetLong(long min = 1, long max = -1) {
			return random.NextInt64(min, max);
		}

		public static double GetDouble(int min = 1, int max = 999999999, int floatPrecisions = 0) {
			double num = random.NextDouble() * (max - min) + min;
			if (floatPrecisions > 0) {
				num *= 10 * floatPrecisions;
			}
			num = Math.Floor(num);
			if (floatPrecisions > 0) {
				num /= 10 * floatPrecisions;
			}
			return num;
		}

		public static string GetNumberString(int length = 5) {
			return GenerateString(length, numbers);
		}

		public static string GetString(int length, bool useNumbers = false) {
			string charSet = letters;
			if (useNumbers) {
				charSet += numbers;
			}
			return GenerateString(length, charSet);
		}

		public static string GetSentence(int words) {
			string str = "";
			if (words <= 1) {
				return str;
			}
			for (int i = 0; i < words; i++) {
				str += " " + GenerateString(random.Next(3, 15), letters);
			}
			return str.Substring(1);
		}

		private static string GenerateString(int length, string charSet) {
			string str = "";
			for (int i = 0; i < length; i++) {
				str += charSet[random.Next(charSet.Length)];
			}
			return str;
		}

		public static Guid GetGuid() {
			return Guid.NewGuid();
		}

		// Alternates between vowels and consonants
		public static string GetName(int length) {
			string str = "";

			// Starts with
			bool firstCharIsVowel = GetBoolean();
			string firstPart = "";
			string secondPart = "";
			if (firstCharIsVowel) {
				firstPart = vowels;
				secondPart = consonants;
			} else {
				firstPart = consonants;
				secondPart = vowels;
			}

			// Assemble name
			for (int i = 1; i < length + 1; i++) {
				if (i % 2 == 0) {
					str += secondPart[random.Next(secondPart.Length)];
				} else {
					str += firstPart[random.Next(firstPart.Length)];
				}
			}

			// Capitalize first letter
			if (str.Length > 0) {
				str = str.Substring(0, 1).ToUpper() + str.Substring(1);
			}
			return str;
		}
	}
}

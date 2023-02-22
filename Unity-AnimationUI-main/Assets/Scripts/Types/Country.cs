using UnityEngine;

namespace Types
{
	[System.Serializable]
	public class Country
	{
		public string name;
		public string name_long; // alternate
		public string name_sort; // alternate 2 (not sure what the _sort attribute means?)

		public string nameOfficial;

		public string abbreviation;

		public string continent;
		public string alpha2Code;
		public string alpha3Code;
		public int population;

		public City[] cities;
		public Shape shape;

		public string GetPreferredDisplayName(int maxDesiredLength = int.MaxValue, bool debug = false)
		{
			const int abbreviatedIndex = 4;
			string[] rankedNames = { name, name_long, nameOfficial, name_sort, abbreviation };
			int[] scores = new int[rankedNames.Length];
			for (int i = 0; i < scores.Length; i++)
			{
				string currentName = rankedNames[i];

				int penalty = i;
				if (currentName.Length > 0)
				{
					penalty += currentName.Length;
					if (currentName.Length > maxDesiredLength)
					{
						penalty += 1000;
					}

					if (i == abbreviatedIndex || currentName.Contains("."))
					{
						penalty += 100;
						penalty -= currentName.Length * 2;
					}
				}
				else
				{
					penalty = int.MaxValue;
				}
				scores[i] = -penalty;
			}

			Seb.Sorting.SortByScores(rankedNames, scores);
			if (debug)
			{
				for (int i = 0; i < rankedNames.Length; i++)
				{
					Debug.Log($"{rankedNames[i]}  (score = {scores[i]})");
				}
			}
			return rankedNames[0];
		}
	}
}

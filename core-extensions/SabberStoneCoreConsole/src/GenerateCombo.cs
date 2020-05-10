﻿using System;
using System.Collections.Generic;
using System.Linq;
using SabberStoneCore.Model;

namespace SabberStoneCoreConsole.src
{
	public static class GenerateCombo
	{
		static public List<CardCombo> comboList = new List<CardCombo>();
		static int CountLimit = 200;
		static Random rand = new Random();

	

		public static HashSet<CardCombo> RandomComboGenerator(List<CardInfo> CardWithtag)
		{
			HashSet<CardCombo> result = new HashSet<CardCombo>();
			int count = 0;
			while(count < CountLimit)
			{
				int index1 = rand.Next(CardWithtag.Count);
				int index2 = rand.Next(CardWithtag.Count);
				HashSet<int[]> randomSet = new HashSet<int[]>();
				if(index1 > index2) //always index1 <= index2
				{
					int tmp = index1;
					index1 = index2;
					index2 = tmp;
				}
				int[] indexSet = new int[] { index1, index2 };
				if (index1 != index2 && !randomSet.Contains(indexSet) && CanReact(CardWithtag[index1], CardWithtag[index2]))
				{
					result.Add(new CardCombo(CardWithtag[index1], CardWithtag[index2]));
					randomSet.Add(indexSet);
					count++;
				}
			}
			return result;
		}
		///
		/*public List<T> GetRandomElements<T>(this IEnumerable<T> list, int elementsCount)
		{
			return list.OrderBy(arg => Guid.NewGuid()).Take(elementsCount).ToList();
		}*/


		public static bool CanReact(CardInfo card1, CardInfo card2)
		{
			// Rule 0 : not belongs to different classes and total cost less than 10
			if (card1.card.Cost + card2.card.Cost > 10)
				return false;
			if (card1.card.Class.ToString() != "NEUTRAL" && card1.card.Class.ToString() != "NEUTRAL"
				&& card1.card.Class != card1.card.Class)
				return false;
			// Rule 1 : has same tag
			HashSet<string> intersect = new HashSet<string>(card1.tags);
			intersect.IntersectWith(card2.tags);
			if (intersect.Count > 0)
				return true;
			// Rule 2 : charge + buff (= Remove)
			if ((card1.tags.Contains("Charge") && card2.tags.Contains("Buff"))
				|| (card1.tags.Contains("Buff") && card2.tags.Contains("Charge")))
				return true;
			// Rule 3 : AOE + Taunt
			if ((card1.tags.Contains("AOE") && card2.tags.Contains("Taunt"))
				|| (card1.tags.Contains("Taunt") && card2.tags.Contains("AOE")))
				return true;

			return false;
		}

		public static HashSet<CardCombo> Evo(List<CardInfo> CardWithtag)
		{
			HashSet<CardCombo> result = new HashSet<CardCombo>();
			//while(result.Count < CountLimit)
			{
				HashSet<CardCombo> comboTemp = RandomComboGenerator(CardWithtag);
				foreach (CardCombo combo in comboTemp)
				{
					Console.WriteLine(combo.Score);
					if (combo.Score < 0 || combo.Score > 15)
						continue;
					result.Add(combo);
				}
			}
			return result;
		}
	}
}
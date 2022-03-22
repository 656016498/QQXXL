﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EasyExcel.
//     Runtime Version: 4.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using UnityEngine;
using EasyExcel;

namespace EasyExcelGenerated
{
	[Serializable]
	public class PiggyBankSection : EERowData
	{
		[EEKeyField]
		[SerializeField]
		private int _type;
		public int type { get { return _type; } }

		[SerializeField]
		private int[] _pigIcon;
		public int[] pigIcon { get { return _pigIcon; } }

		[SerializeField]
		private int[] _min;
		public int[] min { get { return _min; } }

		[SerializeField]
		private int[] _max;
		public int[] max { get { return _max; } }


		public PiggyBankSection()
		{
		}

#if UNITY_EDITOR
		public PiggyBankSection(List<List<string>> sheet, int row, int column)
		{
			TryParse(sheet[row][column++], out _type);
			string[] _pigIconArray = sheet[row][column++].Split(',');
			int _pigIconCount = _pigIconArray.Length;
			_pigIcon = new int[_pigIconCount];
			for(int i = 0; i < _pigIconCount; i++)
				TryParse(_pigIconArray[i], out _pigIcon[i]);
			string[] _minArray = sheet[row][column++].Split(',');
			int _minCount = _minArray.Length;
			_min = new int[_minCount];
			for(int i = 0; i < _minCount; i++)
				TryParse(_minArray[i], out _min[i]);
			string[] _maxArray = sheet[row][column++].Split(',');
			int _maxCount = _maxArray.Length;
			_max = new int[_maxCount];
			for(int i = 0; i < _maxCount; i++)
				TryParse(_maxArray[i], out _max[i]);
		}
#endif
		public override void OnAfterSerialized()
		{
		}
	}

	public class PiggyBankConfig_PiggyBankSection_Sheet : EERowDataCollection
	{
		[SerializeField]
		private List<PiggyBankSection> elements = new List<PiggyBankSection>();

		public override void AddData(EERowData data)
		{
			elements.Add(data as PiggyBankSection);
		}

		public override int GetDataCount()
		{
			return elements.Count;
		}

		public override EERowData GetData(int index)
		{
			return elements[index];
		}

		public override void OnAfterSerialized()
		{
			foreach (var element in elements)
				element.OnAfterSerialized();
		}
	}
}
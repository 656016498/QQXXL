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
	public class LoteryFillConfig : EERowData
	{
		[EEKeyField]
		[SerializeField]
		private int _ID;
		public int ID { get { return _ID; } }

		[SerializeField]
		private int _WxMin;
		public int WxMin { get { return _WxMin; } }

		[SerializeField]
		private int _WxMax;
		public int WxMax { get { return _WxMax; } }

		[SerializeField]
		private float _GetNum;
		public float GetNum { get { return _GetNum; } }


		public LoteryFillConfig()
		{
		}

#if UNITY_EDITOR
		public LoteryFillConfig(List<List<string>> sheet, int row, int column)
		{
			TryParse(sheet[row][column++], out _ID);
			TryParse(sheet[row][column++], out _WxMin);
			TryParse(sheet[row][column++], out _WxMax);
			TryParse(sheet[row][column++], out _GetNum);
		}
#endif
		public override void OnAfterSerialized()
		{
		}
	}

	public class MainConfig_LoteryFillConfig_Sheet : EERowDataCollection
	{
		[SerializeField]
		private List<LoteryFillConfig> elements = new List<LoteryFillConfig>();

		public override void AddData(EERowData data)
		{
			elements.Add(data as LoteryFillConfig);
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

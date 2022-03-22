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
	public class VedioConfig : EERowData
	{
		[EEKeyField]
		[SerializeField]
		private int _ID;
		public int ID { get { return _ID; } }

		[SerializeField]
		private int _VedioMin;
		public int VedioMin { get { return _VedioMin; } }

		[SerializeField]
		private int _VedioMax;
		public int VedioMax { get { return _VedioMax; } }

		[SerializeField]
		private float _GetNum;
		public float GetNum { get { return _GetNum; } }


		public VedioConfig()
		{
		}

#if UNITY_EDITOR
		public VedioConfig(List<List<string>> sheet, int row, int column)
		{
			TryParse(sheet[row][column++], out _ID);
			TryParse(sheet[row][column++], out _VedioMin);
			TryParse(sheet[row][column++], out _VedioMax);
			TryParse(sheet[row][column++], out _GetNum);
		}
#endif
		public override void OnAfterSerialized()
		{
		}
	}

	public class MainConfig_VedioConfig_Sheet : EERowDataCollection
	{
		[SerializeField]
		private List<VedioConfig> elements = new List<VedioConfig>();

		public override void AddData(EERowData data)
		{
			elements.Add(data as VedioConfig);
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

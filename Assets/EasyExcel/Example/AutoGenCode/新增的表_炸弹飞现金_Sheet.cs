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
	public class 炸弹飞现金 : EERowData
	{
		[EEKeyField]
		[SerializeField]
		private int _ID;
		public int ID { get { return _ID; } }

		[SerializeField]
		private float _Min;
		public float Min { get { return _Min; } }

		[SerializeField]
		private float _Max;
		public float Max { get { return _Max; } }

		[SerializeField]
		private float _AD;
		public float AD { get { return _AD; } }


		public 炸弹飞现金()
		{
		}

#if UNITY_EDITOR
		public 炸弹飞现金(List<List<string>> sheet, int row, int column)
		{
			TryParse(sheet[row][column++], out _ID);
			TryParse(sheet[row][column++], out _Min);
			TryParse(sheet[row][column++], out _Max);
			TryParse(sheet[row][column++], out _AD);
		}
#endif
		public override void OnAfterSerialized()
		{
		}
	}

	public class 新增的表_炸弹飞现金_Sheet : EERowDataCollection
	{
		[SerializeField]
		private List<炸弹飞现金> elements = new List<炸弹飞现金>();

		public override void AddData(EERowData data)
		{
			elements.Add(data as 炸弹飞现金);
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

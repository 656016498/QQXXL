//------------------------------------------------------------------------------
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
	public class SevenWithDrawConfig : EERowData
	{
		[EEKeyField]
		[SerializeField]
		private int _Day;
		public int Day { get { return _Day; } }

		[SerializeField]
		private string _withdraw;
		public string withdraw { get { return _withdraw; } }

		[SerializeField]
		private float _value;
		public float value { get { return _value; } }

		[SerializeField]
		private int _awardType;
		public int awardType { get { return _awardType; } }

		[SerializeField]
		private int _awardNum;
		public int awardNum { get { return _awardNum; } }

		[SerializeField]
		private int _taskType;
		public int taskType { get { return _taskType; } }

		[SerializeField]
		private int _target;
		public int target { get { return _target; } }

		[SerializeField]
		private string _taskDir;
		public string taskDir { get { return _taskDir; } }


		public SevenWithDrawConfig()
		{
		}

#if UNITY_EDITOR
		public SevenWithDrawConfig(List<List<string>> sheet, int row, int column)
		{
			TryParse(sheet[row][column++], out _Day);
			TryParse(sheet[row][column++], out _withdraw);
			TryParse(sheet[row][column++], out _value);
			TryParse(sheet[row][column++], out _awardType);
			TryParse(sheet[row][column++], out _awardNum);
			TryParse(sheet[row][column++], out _taskType);
			TryParse(sheet[row][column++], out _target);
			TryParse(sheet[row][column++], out _taskDir);
		}
#endif
		public override void OnAfterSerialized()
		{
		}
	}

	public class MainConfig_SevenWithDrawConfig_Sheet : EERowDataCollection
	{
		[SerializeField]
		private List<SevenWithDrawConfig> elements = new List<SevenWithDrawConfig>();

		public override void AddData(EERowData data)
		{
			elements.Add(data as SevenWithDrawConfig);
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

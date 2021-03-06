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
	public class PiggBankKeys : EERowData
	{
		[EEKeyField]
		[SerializeField]
		private int _id;
		public int id { get { return _id; } }

		[EEKeyField]
		[SerializeField]
		private string _money;
		public string money { get { return _money; } }

		[SerializeField]
		private string _key;
		public string key { get { return _key; } }


		public PiggBankKeys()
		{
		}

#if UNITY_EDITOR
		public PiggBankKeys(List<List<string>> sheet, int row, int column)
		{
			TryParse(sheet[row][column++], out _id);
			TryParse(sheet[row][column++], out _money);
			TryParse(sheet[row][column++], out _key);
		}
#endif
		public override void OnAfterSerialized()
		{
		}
	}

	public class PiggyBankConfig_PiggBankKeys_Sheet : EERowDataCollection
	{
		[SerializeField]
		private List<PiggBankKeys> elements = new List<PiggBankKeys>();

		public override void AddData(EERowData data)
		{
			elements.Add(data as PiggBankKeys);
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

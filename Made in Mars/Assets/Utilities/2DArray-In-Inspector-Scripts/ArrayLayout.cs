﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;

/// <summary>
/// A helper script making the fancy 2D bool array for the BuildingData script
/// </summary>
[System.Serializable]
public class ArrayLayout  {

	public ArrayLayout() {
		column = new rowData[7];
		for (int i = 0; i < column.Length; i++) {
			column[i] = new rowData();
			column[i].row = new bool[7];
		}
	}

	[System.Serializable]
	public class rowData {
		public bool[] row = new bool[7];
	}

	[FormerlySerializedAs("rows")]
	public rowData[] column = new rowData[7]; //Grid of 7x7

	public int height {
		get {
			int max = 0;
			int min = 7;
			for (int y = 0; y < column.Length; y++) {
				for (int x = 0; x < column[y].row.Length; x++) {
					if (column[y].row[x]) {
						min = Mathf.Min(y, min);
						max = Mathf.Max(y, max);
					}
				}
			}

			return max-min +1;
		}
	}
	
	public int width {
		get {
			int max = 0;
			int min = 7;
			for (int y = 0; y < column.Length; y++) {
				for (int x = 0; x < column[y].row.Length; x++) {
					if (column[y].row[x]) {
						min = Mathf.Min(x, min);
						max = Mathf.Max(x, max);
					}
				}
			}

			return max-min +1;
		}
	}


	public int maxHeightFromCenter {
		get {
			int max = 0;
			int min = 7;
			for (int y = 0; y < column.Length; y++) {
				for (int x = 0; x < column[y].row.Length; x++) {
					if (column[y].row[x]) {
						min = Mathf.Min(y, min);
						max = Mathf.Max(y, max);
					}
				}
			}

			return max-3 + 1;
		}
	}
}

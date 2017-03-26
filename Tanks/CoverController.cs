using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Tanks
{
	class CoverController
	{
		private TanksModel tanksModel;

		public CoverController(TanksModel tanksModel)
		{
			this.tanksModel = tanksModel;
		}

		public void addCover(Cover cover)
		{
			//TODO: Merge cover here.
			tanksModel.coverList.Add(cover);
		}

		public List<Cover> getCoverList()
		{
			return tanksModel.coverList;
		}

		public void setCoverList(List<Cover> coverList)
		{
			this.tanksModel.coverList = coverList;
		}

		public void removeLastCover()
		{
			if (tanksModel.coverList.Count > 0)
			{
				tanksModel.coverList.RemoveAt(tanksModel.coverList.Count - 1);
			}
		}

	}
}
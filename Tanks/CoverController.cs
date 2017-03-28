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

using Path = System.Collections.Generic.List<ClipperLib.IntPoint>;
using Paths = System.Collections.Generic.List<System.Collections.Generic.List<ClipperLib.IntPoint>>;
using ClipperLib;

namespace Tanks
{
	class CoverController
	{
		private TanksModel tanksModel;

		public CoverController(TanksModel tanksModel)
		{
			this.tanksModel = tanksModel;
		}

		public void addCover(Cover coverToAdd)
		{
			//TODO: Merge cover here. Clipper union op

			List<Cover> newCoverList = new List<Cover>();
			Clipper clipper = new Clipper();
			Paths solution = new Paths();

			clipper.AddPath(coverToAdd.getAssignedLine().getIntPointsPath(), PolyType.ptClip, true);

			tanksModel.coverList.ForEach(delegate (Cover cover)
			{

				clipper.AddPath(cover.getAssignedLine().getIntPointsPath(), PolyType.ptSubject, true);

			});

			clipper.Execute(ClipType.ctUnion, solution);

			if (solution.Count > 0)
			{
				solution.ForEach(delegate (List<IntPoint> coverItem)
				{
					Cover newCover = convertIntPointsToCover(coverItem);

					newCoverList.Add(newCover);
				});

			}


			//tanksModel.coverList.Remove(coverToAdd);
			tanksModel.coverList = newCoverList;


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

		public Cover convertIntPointsToCover(Path coverItem)
		{
			Cover newCover = new Tanks.Cover();

			for (int index = 0; index < coverItem.Count; index++)
			{
				newCover.getAssignedLine().addPoint(Vector2Ext.ToVector2(coverItem[index]));
			}

			//One more line to connect it all up...
			newCover.getAssignedLine().addPoint(newCover.getAssignedLine().getPoints()[0]);
			return newCover;
		}

	}
}
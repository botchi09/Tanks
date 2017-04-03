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
	class CoverCollisionResult
	{
		private Line line;
		private bool wasModified;

		public Line getLine()
		{
			return line;
		}

		public bool getWasModified()
		{
			return wasModified;
		}

		public CoverCollisionResult(Line line, bool wasModified)
		{
			this.line = line;
			this.wasModified = wasModified;
		}
	}
}
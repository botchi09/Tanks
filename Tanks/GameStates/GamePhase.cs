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
	enum GamePhase
	{
		P1_DRAW,
		P2_DRAW,
		P1_FIGHT,
		P2_FIGHT,
		MATCH_COMPLETE
	}
}
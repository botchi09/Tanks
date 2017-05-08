using ClipperLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks;

namespace TanksUnitTesting
{
	//This class is crucial, thorough testing is important here
	[TestClass]
	public class Vector2ExtTest
	{
		[TestMethod]
		public void ConversionBetweenVectorTypes()
		{
			Assert.AreEqual(Vector2Ext.ToIntPoint(new Vector2(100, 100)), new IntPoint(100, 100));
			Assert.AreEqual(Vector2Ext.ToVector2(new IntPoint(100, 100)), new Vector2(100, 100));

		}
	}
}

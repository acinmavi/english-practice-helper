/*

	Generated: 07/13/2013 19:59:21
*/



namespace DBHelperTest
{
    using DBHelper;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    [TestClass]
	public class EntityBaseTest : DataAccessTestBase
	{
		[TestMethod]
		public void CreateCommandTest()
		{
			Assert.IsNotNull(EntityBase.CreateCommand());
		}

        [TestMethod]
		public void ConnectionIsOpenTest()
		{
			var expected = System.Data.ConnectionState.Open;
			var actual = EntityBase.Connection.State;
			Assert.AreEqual(expected, actual);
		}
	}
}


/*

	Generated: 07/13/2013 19:59:21
*/


using DBHelper;
namespace DBHelperTest
{
	public partial class MockDataRepository : IDataRepository
	{

		public MockDataRepository()
		{
			EnglishSentences = new MockEnglishSentencesRepository();
		}

		public IEnglishSentencesRepository EnglishSentences { get; private set; }

		public System.Data.SqlServerCe.SqlCeTransaction BeginTransaction()
		{
			return null;
		}

		public void Commit()
		{
		}

		public void Rollback()
		{
		}

		public void Dispose()
		{
		}

	}
}


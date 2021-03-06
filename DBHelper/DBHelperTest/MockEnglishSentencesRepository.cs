/*
	This code was generated by SQL Compact Code Generator version 1.3.0.7

	SQL Compact Code Generator was written by Christian Resma Helle (http://sqlcecodegen.codeplex.com)
	and is under the GNU General Public License version 2 (GPLv2)

	Generated: 07/13/2013 19:59:21
*/



namespace DBHelperTest
{
    using DBHelper;
    using System.Linq;

	public partial class MockEnglishSentencesRepository : IEnglishSentencesRepository
	{

		private readonly System.Collections.Generic.List<EnglishSentences> mockDataSource = new System.Collections.Generic.List<EnglishSentences>();

		public System.Data.SqlServerCe.SqlCeTransaction Transaction { get; set; }

		public System.Collections.Generic.List<EnglishSentences> ToList()
		{
			return mockDataSource;
		}

		public EnglishSentences[] ToArray()
		{
			var list = ToList();
			return list != null ? list.ToArray() : null;
		}

		public System.Collections.Generic.List<EnglishSentences> ToList(int count)
		{
			return mockDataSource.Take(count).ToList();
		}

		public EnglishSentences[] ToArray(int count)
		{
			var list = ToList(count);
			return list != null ? list.ToArray() : null;
		}

		public System.Collections.Generic.List<EnglishSentences> SelectByID(System.Int32? ID)
		{
			return mockDataSource.Where(c => c.ID == ID).ToList();
		}

		public System.Collections.Generic.List<EnglishSentences> SelectByContent(System.String Content)
		{
			return mockDataSource.Where(c => c.Content == Content).ToList();
		}

		public System.Collections.Generic.List<EnglishSentences> SelectByLevel(System.Int32? Level)
		{
			return mockDataSource.Where(c => c.Level == Level).ToList();
		}

		public System.Collections.Generic.List<EnglishSentences> SelectByID(System.Int32? ID, int count)
		{
			return mockDataSource.Where(c => c.ID == ID).Take(count).ToList();
		}

		public System.Collections.Generic.List<EnglishSentences> SelectByContent(System.String Content, int count)
		{
			return mockDataSource.Where(c => c.Content == Content).Take(count).ToList();
		}

		public System.Collections.Generic.List<EnglishSentences> SelectByLevel(System.Int32? Level, int count)
		{
			return mockDataSource.Where(c => c.Level == Level).Take(count).ToList();
		}

		public void Create(EnglishSentences item)
		{
			mockDataSource.Add(item);
		}

		public void Create(System.String Content, System.Int32? Level)
		{
			Create(new EnglishSentences
			{
				Content = Content, 
				Level = Level, 
			});
		}

		public void Create(System.Int32? ID, System.String Content, System.Int32? Level)
		{
			Create(new EnglishSentences
			{
				ID = ID, 
				Content = Content, 
				Level = Level, 
			});
		}

		public void Create(System.Collections.Generic.IEnumerable<EnglishSentences> items)
		{
			mockDataSource.AddRange(items);
		}

		public void Delete(EnglishSentences item)
		{
			mockDataSource.Remove(item);
		}

		public void Delete(System.Collections.Generic.IEnumerable<EnglishSentences> items)
		{
			foreach (var item in new System.Collections.Generic.List<EnglishSentences>(items)) mockDataSource.Remove(item);
		}

		public int DeleteByID(System.Int32? ID)
		{
			var items = mockDataSource.Where(c => c.ID == ID);
			var count = 0;
			foreach (var item in new System.Collections.Generic.List<EnglishSentences>(items))
			{
				mockDataSource.Remove(item);
				count++;
			}
			return count;
		}

		public int DeleteByContent(System.String Content)
		{
			var items = mockDataSource.Where(c => c.Content == Content);
			var count = 0;
			foreach (var item in new System.Collections.Generic.List<EnglishSentences>(items))
			{
				mockDataSource.Remove(item);
				count++;
			}
			return count;
		}

		public int DeleteByLevel(System.Int32? Level)
		{
			var items = mockDataSource.Where(c => c.Level == Level);
			var count = 0;
			foreach (var item in new System.Collections.Generic.List<EnglishSentences>(items))
			{
				mockDataSource.Remove(item);
				count++;
			}
			return count;
		}

		public int Purge()
		{
			var returnValue = mockDataSource.Count;
			mockDataSource.Clear();
			return returnValue;
		}

		public void Update(EnglishSentences item)
		{
			for (int i = 0; i < mockDataSource.Count; i++)
			{
				if (mockDataSource[i].ID == item.ID)
					mockDataSource[i] = item;
			}
		}

		public void Update(System.Collections.Generic.IEnumerable<EnglishSentences> items)
		{
			foreach (var item in items) Update(item);
		}

		public int Count()
		{
			return mockDataSource.Count;
		}

	}
}


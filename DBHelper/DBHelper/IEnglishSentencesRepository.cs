/*

	Generated: 07/13/2013 19:59:21
*/



namespace DBHelper
{
	/// <summary>
	/// Represents the EnglishSentences repository
	/// </summary>
	public partial interface IEnglishSentencesRepository : IRepository<EnglishSentences>
	{
		/// <summary>
		/// Transaction instance created from <see cref="IDataRepository" />
		/// </summary>
		System.Data.SqlServerCe.SqlCeTransaction Transaction { get; set; }

		/// <summary>
		/// Retrieves a collection of items by ID
		/// </summary>
		/// <param name="ID">ID value</param>
		System.Collections.Generic.List<EnglishSentences> SelectByID(System.Int32? ID);

		/// <summary>
		/// Retrieves the first set of items specified by count by ID
		/// </summary>
		/// <param name="ID">ID value</param>
		/// <param name="count">the number of records to be retrieved</param>
		System.Collections.Generic.List<EnglishSentences> SelectByID(System.Int32? ID, int count);

		/// <summary>
		/// Retrieves a collection of items by Content
		/// </summary>
		/// <param name="Content">Content value</param>
		System.Collections.Generic.List<EnglishSentences> SelectByContent(System.String Content);

		/// <summary>
		/// Retrieves the first set of items specified by count by Content
		/// </summary>
		/// <param name="Content">Content value</param>
		/// <param name="count">the number of records to be retrieved</param>
		System.Collections.Generic.List<EnglishSentences> SelectByContent(System.String Content, int count);

		/// <summary>
		/// Retrieves a collection of items by Level
		/// </summary>
		/// <param name="Level">Level value</param>
		System.Collections.Generic.List<EnglishSentences> SelectByLevel(System.Int32? Level);

		/// <summary>
		/// Retrieves the first set of items specified by count by Level
		/// </summary>
		/// <param name="Level">Level value</param>
		/// <param name="count">the number of records to be retrieved</param>
		System.Collections.Generic.List<EnglishSentences> SelectByLevel(System.Int32? Level, int count);

		/// <summary>
		/// Delete records by ID
		/// </summary>
		/// <param name="ID">ID value</param>
		int DeleteByID(System.Int32? ID);

		/// <summary>
		/// Delete records by Content
		/// </summary>
		/// <param name="Content">Content value</param>
		int DeleteByContent(System.String Content);

		/// <summary>
		/// Delete records by Level
		/// </summary>
		/// <param name="Level">Level value</param>
		int DeleteByLevel(System.Int32? Level);

		/// <summary>
		/// Create new record without specifying a primary key
		/// </summary>
		void Create(System.String Content, System.Int32? Level);

		/// <summary>
		/// Create new record specifying all fields
		/// </summary>
		void Create(System.Int32? ID, System.String Content, System.Int32? Level);
	}
}


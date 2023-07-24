using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;

namespace InvestorsApp.Pages.Investors
{
	public class IndexModel : PageModel
	{
		public List<InvestorInfo> listInvestors = new List<InvestorInfo>();
		public void OnGet()
		{
			try
			{

				string ConnectionString = "Data Source=.\\sqlexpress;Initial Catalog=downing;Integrated Security=True";

				using (SqlConnection connection = new SqlConnection(ConnectionString))

				{
					connection.Open();
					string sql = "SELECT * FROM Investors ORDER BY company_name";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								InvestorInfo InvestorInfo = new InvestorInfo();
								InvestorInfo.id = "" + reader.GetInt32(0);
								InvestorInfo.company_name = reader.GetString(1);
								InvestorInfo.company_code = reader.GetString(2);
								InvestorInfo.share_price = "" + reader.GetSqlDecimal(3);
								InvestorInfo.create_date = reader.GetDateTime(4).ToString("dd'-'MMM'-'yyyy");
								listInvestors.Add(InvestorInfo);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception: " + ex.ToString());

			}
		}
	}

	public class InvestorInfo
	{
		public string id;
		public string company_name;
		public string company_code;
		public string share_price;
		public string create_date;
	}
}

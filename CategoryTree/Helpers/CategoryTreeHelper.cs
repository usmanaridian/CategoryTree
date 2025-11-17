using CategoryTree.Core;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace CategoryTree.Helpers
{
    public static class CategoryTreeHelper
    {
        public static void GetCategoryTreeWithEF(IList<Category> categories)
        {
            Console.WriteLine("Running LINQ and EF Test");

            var sw = Stopwatch.StartNew();            

            var tree = BuildTree(categories);

            sw.Stop();

            Console.WriteLine($"EF Execution Time: {sw.ElapsedMilliseconds} ms");
            Console.WriteLine("EF Tree Output:");
            PrintTree(tree);
        }

        public static void PrintTree(IList<Category>? categories, int indent = 0)
        {
            if (categories == null || categories.Count == 0)
                return;

            foreach (var cat in categories)
            {
                Console.WriteLine($"{new string(' ', indent * 2)}- {cat.Name}");
                PrintTree(cat.SubCategories, indent + 1);
            }
        }

        public static IList<Category> BuildTree(IList<Category>? flat)
        {
            if(flat == null)
                return new List<Category>();

            var categoriesGroup = flat.ToLookup(x => x.ParentId);            

            return GetSubCategories(null, categoriesGroup);
            
        }

        public static IList<Category> GetSubCategories(Guid? parentId, ILookup<Guid?, Category> categoriesGroup)
        {
            return categoriesGroup[parentId]
                    .Select(c => {
                        c.SubCategories = GetSubCategories(c.Id, categoriesGroup);
                        return c;
                    })
                    .ToList();
        }

        public static async Task GetCategoryTreeWithSP(string connectionString)
        {
            Console.WriteLine("Running Stored Procedure...");

            var sw = Stopwatch.StartNew();

            var result = new List<Category>();

            using var conn = new SqlConnection(connectionString);
            using var cmd = new SqlCommand("GetCategoryTree", conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new Category
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    ParentId = reader.IsDBNull(2) ? null : reader.GetGuid(2)
                });
            }

            var tree = BuildTree(result);

            sw.Stop();

            Console.WriteLine($"Stored Procedure Execution Time: {sw.ElapsedMilliseconds} ms");
            Console.WriteLine("Stored Procedure Tree Output:");
            PrintTree(tree);
        }
    }
}

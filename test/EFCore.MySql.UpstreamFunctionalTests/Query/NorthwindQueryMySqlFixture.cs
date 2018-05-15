using EFCore.MySql.UpstreamFunctionalTests.TestUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.TestModels.Northwind;
using Microsoft.EntityFrameworkCore.TestUtilities;

namespace EFCore.MySql.UpstreamFunctionalTests.Query
{
    public class NorthwindQueryMySqlFixture<TModelCustomizer> : NorthwindQueryRelationalFixture<TModelCustomizer>
        where TModelCustomizer : IModelCustomizer, new()
    {
        // TODO: Consider using an existing database/initialize from script
        protected override ITestStoreFactory TestStoreFactory => MySqlTestStoreFactory.Instance;

        protected override void Seed(NorthwindContext context)
        {
            base.Seed(context);
            context.Database.ExecuteSqlCommand(@"DROP PROCEDURE IF EXISTS `Ten Most Expensive Products`;
CREATE PROCEDURE `Ten Most Expensive Products` ()
BEGIN
  SELECT `ProductName` AS `TenMostExpensiveProducts`, `UnitPrice`
  FROM `Products`
  ORDER BY `UnitPrice` DESC
  LIMIT 10;
END;");
            context.Database.ExecuteSqlCommand(@"DROP PROCEDURE IF EXISTS `CustOrderHist`;
CREATE PROCEDURE `CustOrderHist` (IN CustomerID VARCHAR(768))
BEGIN
  SELECT `ProductName`, SUM(`Quantity`) AS `Total`
  FROM `Products` `p`, `Order Details` `od`, `Orders` `o`, `Customers` `c`
  WHERE `c`.`CustomerId` = `CustomerId`
  AND `c`.`CustomerId` = `o`.`CustomerId`
  AND `o`.`OrderId` = `od`.`OrderId`
  AND `od`.`ProductId` = `p`.`ProductId`
  GROUP BY `ProductName`;
END;");
        }
    }
}
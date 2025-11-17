CREATE OR ALTER PROCEDURE GetCategoryTree
AS
BEGIN
    ;WITH CTE AS
    (
        SELECT 
            Id, Name, ParentId, 0 AS Level
        FROM Category
        WHERE ParentId IS NULL
        
        UNION ALL
        
        SELECT 
            c.Id, c.Name, c.ParentId, CTE.Level + 1
        FROM Category c
        INNER JOIN CTE ON c.ParentId = CTE.Id
    )
    SELECT * FROM CTE ORDER BY Level, ParentId, Id;
END
GO
CREATE TABLE [financial].[Symbol]
(
	[Symbol] NVARCHAR(32) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(254) NOT NULL,
    [Type] NVARCHAR(64) NOT NULL,
    [Region] NVARCHAR(32) NOT NULL,
    [MarketOpen] NVARCHAR(8) NOT NULL,
    [MarketClose] NVARCHAR(8) NOT NULL,
    [Timezone] NVARCHAR(32) NOT NULL,
    [Currency] NVARCHAR(8) NOT NULL,
    [MatchScore] DECIMAL(5,4) NOT NULL
);

--TEST DATA
--"1. symbol": "SAIC",
--"2. name": "Science Applications International Corp",
-- "3. type": "Equity",
--"4. region": "United States",
--"5. marketOpen": "09:30",
--"6. marketClose": "16:00",
--"7. timezone": "UTC-04",
--"8. currency": "USD",
-- "9. matchScore": "1.0000"
GO
CREATE TABLE [financial].[MetaData]
(
    [Symbol] NVARCHAR(32) NOT NULL PRIMARY KEY,
    [Information] NVARCHAR(254) NOT NULL,
    [Type] NVARCHAR(64) NOT NULL,
    [LastRefreshed] DATE NOT NULL,
    [TimeZone] NVARCHAR(32) NOT NULL,
    CONSTRAINT [FK_MetaData_Symbol] FOREIGN KEY ([Symbol]) REFERENCES [financial].[Symbol]([Symbol])
);

--TEST DATA
--"1. Information": "Daily Prices (open, high, low, close) and Volumes",
--"2. Symbol": "IBM",
--"3. Last Refreshed": "2024-12-13",
--"4. Output Size": "Compact",
--"5. Time Zone": "US/Eastern"